// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefWebPluginInfoWrapper.h"
#include "Internals/CefDragDataWrapper.h"
#include "ClientAdapter.h"
#include "DownloadAdapter.h"
#include "StreamAdapter.h"
#include "include/wrapper/cef_stream_resource_handler.h"

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
                auto browserId = browser->GetIdentifier();
                
                if (static_cast<Action<int>^>(_onAfterBrowserCreated) != nullptr)
                {
                    _onAfterBrowserCreated->Invoke(browserId);
                }
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

                _cefBrowser = NULL;
            }
        }

        void ClientAdapter::OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
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

        bool ClientAdapter::OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event)
        {
            IKeyboardHandler^ handler = _browserControl->KeyboardHandler;

            if (handler == nullptr)
            {
                return false;
            }

            // TODO: windows_key_code could possibly be the wrong choice here (the OnKeyEvent signature has changed since CEF1). The
            // other option would be native_key_code.
            return handler->OnKeyEvent(_browserControl, (KeyType)event.type, event.windows_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1);
        }

        bool ClientAdapter::OnPreKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event, bool* is_keyboard_shortcut)
        {
            IKeyboardHandler^ handler = _browserControl->KeyboardHandler;

            if (handler == nullptr)
            {
                return false;
            }

            return handler->OnPreKeyEvent(_browserControl, (KeyType)event.type, event.windows_key_code, event.native_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1, *is_keyboard_shortcut);
        }

        void ClientAdapter::OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
        {
            if (browser->IsPopup())
            {
                return;
            }

            AutoLock lock_scope(_syncRoot);

            if (frame->IsMain())
            {
                _browserControl->SetIsLoading(true);
            }

            _browserControl->OnFrameLoadStart(StringUtils::ToClr(frame->GetURL()), frame->IsMain());
        }

        void ClientAdapter::OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode)
        {
            if (browser->IsPopup())
            {
                return;
            }

            AutoLock lock_scope(_syncRoot);

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

        bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isRedirect)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);

            return handler->OnBeforeBrowse(_browserControl, wrapper, isRedirect);
        }

        bool ClientAdapter::OnCertificateError(cef_errorcode_t cert_error, const CefString& request_url, CefRefPtr<CefAllowCertificateErrorCallback> callback)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            if (handler->OnCertificateError(_browserControl, (CefErrorCode)cert_error, StringUtils::ToClr(request_url)))
            {
                callback->Continue(true);
                return true;
            }

            return false;
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

        // Called on the IO thread before a resource is loaded. To allow the resource
        // to load normally return NULL. To specify a handler for the resource return
        // a CefResourceHandler object. The |request| object should not be modified in
        // this callback.
        CefRefPtr<CefResourceHandler> ClientAdapter::GetResourceHandler(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request)
        {
            auto handler = _browserControl->ResourceHandler;

            if (handler == nullptr)
            {
                return NULL;
            }

            auto requestWrapper = gcnew CefRequestWrapper(request);

            auto resourceHandler = handler->GetResourceHandler(_browserControl, requestWrapper);

            if(resourceHandler != nullptr)
            {
                auto mimeType = StringUtils::ToNative(resourceHandler->MimeType);
                auto statusText = StringUtils::ToNative(resourceHandler->StatusText);
                
                CefRefPtr<StreamAdapter> streamAdapter = new StreamAdapter(resourceHandler->Stream);

                CefRefPtr<CefStreamReader> stream = CefStreamReader::CreateForHandler(static_cast<CefRefPtr<CefReadHandler>>(streamAdapter));
                if (stream.get())
                {
                    CefResponse::HeaderMap map = SchemeHandlerWrapper::ToHeaderMap(resourceHandler->Headers);

                    //TODO: Investigate crash when using full response
                    //return new CefStreamResourceHandler(resourceHandler->StatusCode, statusText, mimeType, map, stream);
                    return new CefStreamResourceHandler(mimeType, stream);
                }
            }

            return NULL;
        }

        bool ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto requestWrapper = gcnew CefRequestWrapper(request);
            auto response = gcnew Response();

            bool ret = handler->OnBeforeResourceLoad(_browserControl, requestWrapper, response);

            if (response->Action == ResponseAction::Redirect)
            {
                request->SetURL(StringUtils::ToNative(response->RedirectUrl));
            }
            else if (response->Action == ResponseAction::Cancel)
            {
                return true;
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

        void ClientAdapter::OnBeforeContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
            CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model)
        {

            IMenuHandler^ handler = _browserControl->MenuHandler;
            if (handler == nullptr) return;

            auto result = handler->OnBeforeContextMenu(_browserControl);
            if (!result)
            {
                model->Clear();
            }
        }

        void ClientAdapter::OnGotFocus(CefRefPtr<CefBrowser> browser)
        {
            IFocusHandler^ handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                return;
            }

            handler->OnGotFocus();
        }

        bool ClientAdapter::OnSetFocus(CefRefPtr<CefBrowser> browser, FocusSource source)
        {
            IFocusHandler^ handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                // Allow the focus to be set by default.
                return false;
            }

            return handler->OnSetFocus((CefFocusSource)source);
        }

        void ClientAdapter::OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next)
        {
            IFocusHandler^ handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                return;
            }

            handler->OnTakeFocus(next);
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

        bool ClientAdapter::OnDragEnter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData, DragOperationsMask mask)
        {
            IDragHandler^ handler = _browserControl->DragHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto dragDataWrapper = gcnew CefDragDataWrapper(dragData);

            return handler->OnDragEnter(_browserControl, dragDataWrapper, (CefSharp::DragOperationsMask)mask);
        }

    }
}
