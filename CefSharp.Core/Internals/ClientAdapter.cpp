// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefWebPluginInfoWrapper.h"
#include "Internals/JavascriptBinding/BindingHandler.h"
#include "Internals/RequestResponse.h"
#include "ClientAdapter.h"
#include "Cef.h"
#include "DownloadAdapter.h"
#include "StreamAdapter.h"

using namespace std;
using namespace CefSharp::Internals::JavascriptBinding;

namespace CefSharp
{
    namespace Internals
    {
        bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& target_url,
            const CefString& target_frame_name, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo,
            CefRefPtr<CefClient>& client, CefBrowserSettings& settings, bool* no_javascript_access)
        {
            ILifeSpanHandler^ handler = _browserControl->LifeSpanHandler;

            if (handler == nullptr)
            {
                return false;
            }

            return handler->OnBeforePopup(_browserControl, StringUtils::ToClr(target_url),
                windowInfo.x, windowInfo.y, windowInfo.width, windowInfo.height);
        }

        void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
        {
            if (!browser->IsPopup())
            {
                _browserHwnd = browser->GetHost()->GetWindowHandle();
                _cefBrowser = browser;

                _managedCefBrowserAdapter->OnInitialized();
            }
        }

        void ClientAdapter::OnBeforeClose(CefRefPtr<CefBrowser> browser)
        {
            if (_browserHwnd == browser->GetHost()->GetWindowHandle())
            {
                ILifeSpanHandler^ handler = _browserControl->LifeSpanHandler;
                if (handler != nullptr)
                {
                    handler->OnBeforeClose(_browserControl);
                }

                _cefBrowser = nullptr;
            }
        }

        void ClientAdapter::OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            _browserControl->SetIsLoading(isLoading);

            auto canReload = !isLoading;
            _browserControl->SetNavState(canGoBack, canGoForward, canReload);
        }

        void ClientAdapter::OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& address)
        {
            if (frame->IsMain())
            {
                _browserControl->SetAddress(StringUtils::ToClr(address));
            }
        }

        void ClientAdapter::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
        {
            _browserControl->SetTitle(StringUtils::ToClr(title));
        }

        bool ClientAdapter::OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text)
        {
            String^ tooltip = StringUtils::ToClr(text);

            if (tooltip != _tooltip)
            {
                _tooltip = tooltip;
                _browserControl->SetTooltipText(_tooltip);
            }

            return true;
        }

        bool ClientAdapter::OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
        {
            String^ messageStr = StringUtils::ToClr(message);
            String^ sourceStr = StringUtils::ToClr(source);
            _browserControl->OnConsoleMessage(messageStr, sourceStr, line);

            return true;
        }

        void ClientAdapter::OnStatusMessage(CefRefPtr<CefBrowser> browser, const CefString& value)
        {
            String^ valueStr = StringUtils::ToClr(value);
            _browserControl->OnStatusMessage(valueStr);
        }

        KeyType KeyTypeToManaged(cef_key_event_type_t keytype)
        {
            switch (keytype)
            {
            case KEYEVENT_RAWKEYDOWN:
                return KeyType::RawKeyDown;
            case KEYEVENT_KEYDOWN:
                return KeyType::KeyDown;
            case KEYEVENT_KEYUP:
                return KeyType::KeyUp;
            case KEYEVENT_CHAR:
                return KeyType::Char;
            default:
                throw gcnew ArgumentOutOfRangeException("keytype", String::Format("'{0}' is not a valid keytype", gcnew array<Object^>(keytype)));
            }
        }

        bool ClientAdapter::OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event)
        {
            IKeyboardHandler^ handler = _browserControl->KeyboardHandler;

            if (handler == nullptr)
            {
                return false;
            }

            // TODO: windows_key_code could possibly be the wrong choice here (the OnKeyEvent signature has changed since CEF1). The
            // other option would be native_key_code.
            return handler->OnKeyEvent(_browserControl, KeyTypeToManaged(event.type), event.windows_key_code, event.modifiers, event.is_system_key);
        }

        bool ClientAdapter::OnPreKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event, bool* is_keyboard_shortcut)
        {
            IKeyboardHandler^ handler = _browserControl->KeyboardHandler;

            if (handler == nullptr)
            {
                return false;
            }

            return handler->OnPreKeyEvent(_browserControl, (KeyType)event.type, event.windows_key_code, event.native_key_code, event.modifiers, event.is_system_key, *is_keyboard_shortcut);
        }

        void ClientAdapter::OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
        {
            if (browser->IsPopup())
            {
                return;
            }

            AutoLock lock_scope(this);
            if (frame->IsMain())
            {
                _browserControl->SetIsLoading(true);
                _browserControl->SetNavState(false, false, false);
            }

            _browserControl->OnFrameLoadStart(StringUtils::ToClr(frame->GetURL()), frame->IsMain());
        }

        void ClientAdapter::OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode)
        {
            if (browser->IsPopup())
            {
                return;
            }

            AutoLock lock_scope(this);
            if (frame->IsMain())
            {
                _browserControl->SetIsLoading(false);
            }

            _browserControl->OnFrameLoadEnd(StringUtils::ToClr(frame->GetURL()), frame->IsMain(), httpStatusCode);
        }

        void ClientAdapter::OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& errorText, const CefString& failedUrl)
        {
            _browserControl->OnLoadError(StringUtils::ToClr(failedUrl), (CefErrorCode)errorCode, StringUtils::ToClr(errorText));
        }

        bool ClientAdapter:: OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isRedirect)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);

            return handler->OnBeforeBrowse(_browserControl, wrapper, isRedirect);
        }

        // CEF3 API: public virtual bool OnBeforePluginLoad( CefRefPtr< CefBrowser > browser, const CefString& url, const CefString& policy_url, CefRefPtr< CefWebPluginInfo > info );
        // ---
        // return value:
        //     false: Load Plugin (do not block it)
        //     true:  Ignore Plugin (Block it)
        bool ClientAdapter::OnBeforePluginLoad( CefRefPtr< CefBrowser > browser, const CefString& url, const CefString& policy_url, CefRefPtr< CefWebPluginInfo > info )
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            CefWebPluginInfoWrapper^ wrapper = gcnew CefWebPluginInfoWrapper(info);

            return handler->OnBeforePluginLoad(_browserControl, StringUtils::ToClr(url), StringUtils::ToClr(policy_url), wrapper);
        }

        void ClientAdapter::OnPluginCrashed(CefRefPtr<CefBrowser> browser, const CefString& plugin_path)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler != nullptr)
            {
                handler->OnPluginCrashed(_browserControl, StringUtils::ToClr(plugin_path));
            }			
        }

        void ClientAdapter::OnRenderProcessTerminated(CefRefPtr<CefBrowser> browser, TerminationStatus status)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler != nullptr)
            {
                handler->OnRenderProcessTerminated(_browserControl, (CefTerminationStatus)status);
            }			
        }

        bool ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request)
        {
            // TOOD: Try to support with CEF3; seems quite difficult because the method signature has changed greatly with many parts
            // seemingly MIA...
            IRequestHandler^ handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);
            RequestResponse^ requestResponse = gcnew RequestResponse(wrapper);

            bool ret = handler->OnBeforeResourceLoad(_browserControl, requestResponse);

            if (requestResponse->Action == ResponseAction::Redirect)
            {
                request->SetURL(StringUtils::ToNative(requestResponse->RedirectUrl));
            }
            else if (requestResponse->Action == ResponseAction::Respond)
            {
                CefRefPtr<StreamAdapter> adapter = new StreamAdapter(requestResponse->ResponseStream);

                throw gcnew NotImplementedException("Respond is not yet supported.");

                //resourceStream = CefStreamReader::CreateForHandler(static_cast<CefRefPtr<CefReadHandler>>(adapter));
                //response->SetMimeType(StringUtils::ToNative(requestResponse->MimeType));
                //response->SetStatus(requestResponse->StatusCode);
                //response->SetStatusText(StringUtils::ToNative(requestResponse->StatusText));

                //CefResponse::HeaderMap map;

                //if (requestResponse->ResponseHeaders != nullptr)
                //{
                //    for each (KeyValuePair<String^, String^>^ kvp in requestResponse->ResponseHeaders)
                //    {
                //        map.insert(pair<CefString,CefString>(StringUtils::ToNative(kvp->Key),StringUtils::ToNative(kvp->Value)));
                //    }
                //}

                //response->SetHeaderMap(map);
            }

            return ret;
        }

        CefRefPtr<CefDownloadHandler> ClientAdapter::GetDownloadHandler()
        {
            IDownloadHandler^ downloadHandler = _browserControl->DownloadHandler;
            if (downloadHandler == nullptr)
            {
                return nullptr;
            }

            return new DownloadAdapter(downloadHandler);
        }

        bool ClientAdapter::GetAuthCredentials(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, bool isProxy,
            const CefString& host, int port, const CefString& realm, const CefString& scheme, CefRefPtr<CefAuthCallback> callback)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            String^ usernameString = nullptr;
            String^ passwordString = nullptr;
            bool handled = handler->GetAuthCredentials(_browserControl, isProxy, StringUtils::ToClr(host), port, StringUtils::ToClr(realm), StringUtils::ToClr(scheme), usernameString, passwordString);

            if (handled)
            {
                CefString username;
                CefString password;

                if (usernameString != nullptr)
                {
                    username = StringUtils::ToNative(usernameString);
                }

                if (passwordString != nullptr)
                {
                    password = StringUtils::ToNative(passwordString);
                }

                callback->Continue(username, password);
            }
            else
            {
                // TOOD: Should we call Cancel() here or not? At first glance, I believe we should since there will otherwise be no
                // way to cancel the auth request from an IRequestHandler.
                callback->Cancel();
            }

            return handled;
        }

        // TODO: Investigate how we can support in CEF3.
        /*
        void ClientAdapter::OnResourceResponse(CefRefPtr<CefBrowser> browser, const CefString& url, CefRefPtr<CefResponse> response, CefRefPtr<CefContentFilter>& filter)
        {
        IRequestHandler^ handler = _browserControl->RequestHandler;
        if (handler == nullptr)
        {
        return;
        }

        WebHeaderCollection^ headers = gcnew WebHeaderCollection();
        CefResponse::HeaderMap map;
        response->GetHeaderMap(map);
        for (CefResponse::HeaderMap::iterator it = map.begin(); it != map.end(); ++it)
        {
        try
        {
        headers->Add(StringUtils::ToClr(it->first), StringUtils::ToClr(it->second));
        }
        catch (Exception ^ex)
        {
        // adding a header with invalid characters can cause an exception to be
        // thrown. we will drop those headers for now.
        // we could eventually use reflection to call headers->AddWithoutValidate().
        }
        }

        handler->OnResourceResponse(
        _browserControl,
        StringUtils::ToClr(url),
        response->GetStatus(),
        StringUtils::ToClr(response->GetStatusText()),
        StringUtils::ToClr(response->GetMimeType()),
        headers);
        }*/

        void ClientAdapter::OnContextCreated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefV8Context> context)
        {
            // TODO: Support the BindingHandler with CEF3.
            /*
            for each(KeyValuePair<String^, Object^>^ kvp in Cef::GetBoundObjects())
            {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
            }

            for each(KeyValuePair<String^, Object^>^ kvp in _browserControl->GetBoundObjects())
            {
            BindingHandler::Bind(kvp->Key, kvp->Value, context->GetGlobal());
            }
            */
        }

        void ClientAdapter::OnBeforeContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
            CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model)
        {
            // Something like this...
            auto winFormsWebBrowserControl = dynamic_cast<IWinFormsWebBrowser^>((IWebBrowserInternal^)_browserControl);
            if (winFormsWebBrowserControl == nullptr) return;

            IMenuHandler^ handler = winFormsWebBrowserControl->MenuHandler;
            if (handler == nullptr) return;

            auto result = handler->OnBeforeContextMenu(_browserControl);
            if (!result) {
                // The only way I found for preventing the context menu to be displayed is by removing all items. :-)
                while (model->GetCount() > 0) {
                    model->RemoveAt(0);
                }
            }
        }

        void ClientAdapter::OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next)
        {
            _browserControl->OnTakeFocus(next);
        }

        bool ClientAdapter::OnJSDialog(CefRefPtr<CefBrowser> browser, const CefString& origin_url, const CefString& accept_lang,
            JSDialogType dialog_type, const CefString& message_text, const CefString& default_prompt_text,
            CefRefPtr<CefJSDialogCallback> callback, bool& suppress_message)
        {
            IJsDialogHandler^ handler = _browserControl->JsDialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            bool result;
            bool handled = false;

            switch (dialog_type)
            {
            case JSDIALOGTYPE_ALERT:
                handled = handler->OnJSAlert(_browserControl, StringUtils::ToClr(origin_url), StringUtils::ToClr(message_text));
                break;

            case JSDIALOGTYPE_CONFIRM:
                handled = handler->OnJSConfirm(_browserControl, StringUtils::ToClr(origin_url), StringUtils::ToClr(message_text), result);
                if(handled)
                {
                    callback->Continue(result, CefString());
                }
                break;

            case JSDIALOGTYPE_PROMPT:
                String^ resultString = nullptr;
                handled = handler->OnJSPrompt(_browserControl, StringUtils::ToClr(origin_url), StringUtils::ToClr(message_text),
                    StringUtils::ToClr(default_prompt_text), result, resultString);
                if(handled)
                {
                    callback->Continue(result, StringUtils::ToNative(resultString));
                }
                break;
            }

            return handled;
        }

        bool ClientAdapter::OnFileDialog(CefRefPtr<CefBrowser> browser, FileDialogMode mode, const CefString& title,
            const CefString& default_file_name, const std::vector<CefString>& accept_types,
            CefRefPtr<CefFileDialogCallback> callback)
        {
            IDialogHandler^ handler = _browserControl->DialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            bool handled;

            List<System::String ^>^ resultString = nullptr;

            handled = handler->OnFileDialog(_browserControl, (CefFileDialogMode)mode, StringUtils::ToClr(title), StringUtils::ToClr(default_file_name), StringUtils::ToClr(accept_types), resultString);
            callback->Continue(StringUtils::ToNative(resultString));

            return handled;
        }
    }
}
