﻿// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefContextMenuParamsWrapper.h"
#include "Internals/CefDragDataWrapper.h"
#include "ClientAdapter.h"
#include "DownloadAdapter.h"
#include "StreamAdapter.h"
#include "Internals/TypeConversion.h"
#include "include/wrapper/cef_stream_resource_handler.h"

namespace CefSharp
{
    namespace Internals
    {
        void ClientAdapter::ShowDevTools()
        {
            auto browser = GetCefBrowser();

            if (browser != nullptr)
            {
                CefWindowInfo windowInfo;
                CefBrowserSettings settings;

                windowInfo.SetAsPopup(browser->GetHost()->GetWindowHandle(), "DevTools");

                browser->GetHost()->ShowDevTools(windowInfo, this, settings, CefPoint());
            }
        }

        void ClientAdapter::CloseDevTools()
        {
            auto browser = GetCefBrowser();

            if (browser != nullptr)
            {
                browser->GetHost()->CloseDevTools();
            }
        }

        void ClientAdapter::CloseAllPopups(bool forceClose)
        {
            if (!_popupBrowsers.empty())
            {
                // Request that any popup browsers close.
                auto it = _popupBrowsers.begin();
                for (; it != _popupBrowsers.end(); ++it)
                {
                    (*it)->GetHost()->CloseBrowser(forceClose);
                }
            }
        }

        bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& target_url,
            const CefString& target_frame_name, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo,
            CefRefPtr<CefClient>& client, CefBrowserSettings& settings, bool* no_javascript_access)
        {
            ILifeSpanHandler^ handler = _browserControl->LifeSpanHandler;

            if (handler == nullptr)
            {
                return false;
            }

            return handler->OnBeforePopup(_browserControl, StringUtils::ToClr(frame->GetURL()), StringUtils::ToClr(target_url),
                windowInfo.x, windowInfo.y, windowInfo.width, windowInfo.height);
        }

        void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
        {
            if (browser->IsPopup())
            {
                // Add to the list of popup browsers.
                _popupBrowsers.push_back(browser);
            }
            else
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
            if (browser->IsPopup())
            {
                // Remove from the browser popup list.
                auto it = _popupBrowsers.begin();
                for (; it != _popupBrowsers.end(); ++it)
                {
                    if ((*it)->IsSame(browser))
                    {
                        _popupBrowsers.erase(it);
                        break;
                    }
                }
            }
            else if (_browserHwnd == browser->GetHost()->GetWindowHandle())
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
            _browserControl->SetLoadingStateChange(canGoBack, canGoForward, isLoading);
        }

        void ClientAdapter::OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& address)
        {
            if (!browser->IsPopup())
            {
                _browserControl->SetAddress(StringUtils::ToClr(address));
            }
        }

        void ClientAdapter::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
        {
            if(browser->IsPopup())
            {
                // Set the popup window title
                auto hwnd = browser->GetHost()->GetWindowHandle();
                SetWindowText(hwnd, std::wstring(title).c_str());
            }
            else
            {
                _browserControl->SetTitle(StringUtils::ToClr(title));
            }
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

            return handler->OnBeforeBrowse(_browserControl, wrapper, isRedirect, frame->IsMain());
        }

        bool ClientAdapter::OnCertificateError(CefRefPtr<CefBrowser> browser, cef_errorcode_t cert_error, const CefString& request_url, CefRefPtr<CefSSLInfo> ssl_info, CefRefPtr<CefAllowCertificateErrorCallback> callback)
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
        bool ClientAdapter::OnBeforePluginLoad(CefRefPtr<CefBrowser> browser, const CefString& url, const CefString& policy_url, CefRefPtr<CefWebPluginInfo> info)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto pluginInfo = TypeConversion::FromNative(info);

            return handler->OnBeforePluginLoad(_browserControl, StringUtils::ToClr(url), StringUtils::ToClr(policy_url), pluginInfo);
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
            auto factory = _browserControl->ResourceHandlerFactory;

            if (factory == nullptr || !factory->HasHandlers)
            {
                return NULL;
            }

            auto requestWrapper = gcnew CefRequestWrapper(request);

            auto resourceHandler = factory->GetResourceHandler(_browserControl, requestWrapper);

            if(resourceHandler != nullptr)
            {
                auto mimeType = StringUtils::ToNative(resourceHandler->MimeType);
                auto statusText = StringUtils::ToNative(resourceHandler->StatusText);

                CefRefPtr<StreamAdapter> streamAdapter = new StreamAdapter(resourceHandler->Stream);

                CefRefPtr<CefStreamReader> stream = CefStreamReader::CreateForHandler(static_cast<CefRefPtr<CefReadHandler>>(streamAdapter));
                if (stream.get())
                {
                    CefResponse::HeaderMap map = SchemeHandlerWrapper::ToHeaderMap(resourceHandler->Headers);

                    //NOTE: This will crash in a debug build due to a CEF bug.
                    return new CefStreamResourceHandler(resourceHandler->StatusCode, statusText, mimeType, map, stream);
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

            return handler->OnBeforeResourceLoad(_browserControl, requestWrapper, frame->IsMain());
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

            // Context menu params
            CefContextMenuParamsWrapper^ contextMenuParamsWrapper = gcnew CefContextMenuParamsWrapper(params);

            auto result = handler->OnBeforeContextMenu(_browserControl, contextMenuParamsWrapper);
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

        bool ClientAdapter::OnBeforeUnloadDialog(CefRefPtr<CefBrowser> browser, const CefString& message_text, bool is_reload, CefRefPtr<CefJSDialogCallback> callback)
        {
            IJsDialogHandler^ handler = _browserControl->JsDialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            bool allowUnload;

            auto handled = handler->OnJSBeforeUnload(_browserControl, StringUtils::ToClr(message_text), is_reload, allowUnload);
            if (handled)
            {
                callback->Continue(allowUnload, CefString());
            }

            return handled;
        }

        bool ClientAdapter::OnFileDialog(CefRefPtr<CefBrowser> browser, FileDialogMode mode, const CefString& title,
                const CefString& default_file_path, const std::vector<CefString>& accept_filters, int selected_accept_filter,
                CefRefPtr<CefFileDialogCallback> callback)
        {
            IDialogHandler^ handler = _browserControl->DialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            List<System::String ^>^ filePaths = nullptr;

            if(handler->OnFileDialog(_browserControl, (CefFileDialogMode)mode, StringUtils::ToClr(title), StringUtils::ToClr(default_file_path), StringUtils::ToClr(accept_filters), selected_accept_filter, filePaths))
            {
                callback->Continue(selected_accept_filter, StringUtils::ToNative(filePaths));

                return true;
            }

            return false;
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

        bool ClientAdapter::OnRequestGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id, CefRefPtr<CefGeolocationCallback> callback)
        {
            IGeolocationHandler^ handler = _browserControl->GeolocationHandler;
            if (handler == nullptr)
            {
                // Default deny, as CEF does.
                return false;
            }

            if (handler->OnRequestGeolocationPermission(_browserControl, StringUtils::ToClr(requesting_url), request_id))
            {
                callback->Continue(true);
                return true;
            }

            return false;
        }

        void ClientAdapter::OnCancelGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id)
        {
            IGeolocationHandler^ handler = _browserControl->GeolocationHandler;
            if (handler != nullptr)
            {
                handler->OnCancelGeolocationPermission(_browserControl, StringUtils::ToClr(requesting_url), request_id);
            }
        }
    }
}
