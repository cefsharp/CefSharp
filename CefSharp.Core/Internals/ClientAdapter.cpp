﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "include/wrapper/cef_stream_resource_handler.h"
#include "ClientAdapter.h"
#include "CefRequestWrapper.h"
#include "CefContextMenuParamsWrapper.h"
#include "CefDragDataWrapper.h"
#include "TypeConversion.h"
#include "CefSharpBrowserWrapper.h"
#include "CefDownloadItemCallbackWrapper.h"
#include "CefBeforeDownloadCallbackWrapper.h"
#include "CefGeolocationCallbackWrapper.h"
#include "CefFileDialogCallbackWrapper.h"
#include "CefAuthCallbackWrapper.h"
#include "CefJSDialogCallbackWrapper.h"
#include "CefRequestCallbackWrapper.h"
#include "Messaging/Messages.h"
#include "Serialization/ObjectsSerialization.h"

namespace CefSharp
{
    namespace Internals
    {
        using namespace Messaging;

        IBrowser^ ClientAdapter::GetBrowserWrapper(int browserId, bool isPopup)
        {
            if(isPopup)
            {
                IBrowser^ browserWrapper;
                if (_popupBrowsers->TryGetValue(browserId, browserWrapper))
                {
                    return browserWrapper;
                }

                auto stackFrame = gcnew StackFrame(1);
                auto callingMethodName = stackFrame->GetMethod()->Name;

                ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::" + callingMethodName));
            }

            return _browserAdapter->GetBrowser();
        }

        void ClientAdapter::CloseAllPopups(bool forceClose)
        {
            if (_popupBrowsers->Count > 0)
            {
                for each (IBrowser^ browser in _popupBrowsers->Values)
                {
                    browser->GetHost()->CloseBrowser(forceClose);
                    // NOTE: We don't dispose the IBrowsers here
                    // because ->CloseBrowser() will invoke
                    // ->OnBeforeClose() for the browser.
                    // OnBeforeClose() disposes the IBrowser there.
                }
            }
        }

        bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& target_url,
            const CefString& target_frame_name, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo,
            CefRefPtr<CefClient>& client, CefBrowserSettings& settings, bool* no_javascript_access)
        {
            auto handler = _browserControl->LifeSpanHandler;

            if (handler == nullptr)
            {
                return false;
            }
            
            bool createdWrapper = false;
            IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            CefFrameWrapper frameWrapper(frame, _browserAdapter);
            auto result = handler->OnBeforePopup(
                _browserControl, browserWrapper,
                %frameWrapper, StringUtils::ToClr(target_url),
                windowInfo.x, windowInfo.y, windowInfo.width, windowInfo.height, *no_javascript_access);
            return result;
        }

        void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
        {
            auto browserAdapter = static_cast<IBrowserAdapter^>(_browserAdapter);
            if (browser->IsPopup())
            {
                auto browserWrapper = gcnew CefSharpBrowserWrapper(browser, _browserAdapter);
                // Add to the list of popup browsers.
                _popupBrowsers->Add(browser->GetIdentifier(), browserWrapper);
                auto handler = _browserControl->PopupHandler;
                if (handler != nullptr)
                {
                    handler->OnAfterCreated(_browserControl, browserWrapper);
                }
            }
            else
            {
                _browserHwnd = browser->GetHost()->GetWindowHandle();
                _cefBrowser = browser;
                
                if (browserAdapter != nullptr)
                {
					_javascriptCallbackFactories->Add(browser->GetIdentifier(), _browserAdapter->JavascriptCallbackFactory);
                    _pendingTaskRepositories->Add(browser->GetIdentifier(), _browserAdapter->PendingTaskRepository);
                    browserAdapter->OnAfterBrowserCreated(browser->GetIdentifier());
                }
            }

            if (browserAdapter != nullptr)
            {
                auto rootObject = browserAdapter->GetObjectRepository()->RootObject;
                SendJavascriptRootObject(browser, rootObject);
            }
        }

        void ClientAdapter::OnBeforeClose(CefRefPtr<CefBrowser> browser)
        {
            if (browser->IsPopup())
            {
                // Remove from the browser popup list.
                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                auto handler = _browserControl->PopupHandler;
                if (handler != nullptr)
                {
                    handler->OnBeforeClose(_browserControl, browserWrapper);
                }
                _popupBrowsers->Remove(browser->GetIdentifier());
                // Dispose the CefSharpBrowserWrapper
                delete browserWrapper;
            }
            else if (_browserHwnd == browser->GetHost()->GetWindowHandle())
            {
                _pendingTaskRepositories->Remove(browser->GetIdentifier());
				_javascriptCallbackFactories->Remove(browser->GetIdentifier());
                auto handler = _browserControl->LifeSpanHandler;
                if (handler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), false);

                    handler->OnBeforeClose(_browserControl, browserWrapper);
                }
                _cefBrowser = NULL;
            }
        }

        void ClientAdapter::OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            if (browser->IsPopup())
            {
                auto handler = _browserControl->PopupHandler;
                if (handler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                    handler->OnLoadingStateChange(_browserControl, browserWrapper, isLoading, canGoBack, canGoForward);
                }
            }
            else
            {
                _browserControl->SetLoadingStateChange(canGoBack, canGoForward, isLoading);
            }
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
            if (browser->IsPopup())
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

        void ClientAdapter::OnFaviconURLChange(CefRefPtr<CefBrowser> browser, const std::vector<CefString>& iconUrls)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                    popupHandler->OnFaviconUrlChange(_browserControl, browserWrapper, StringUtils::ToClr(iconUrls));
                }
            }
            else
            {
                auto handler = _browserControl->RequestHandler;
                if (handler != nullptr)
                {
                    handler->OnFaviconUrlChange(_browserControl, StringUtils::ToClr(iconUrls));
                }
            }
        }

        bool ClientAdapter::OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text)
        {
            String^ tooltip = StringUtils::ToClr(text);

            // TODO: Deal with popuup browsers properly...
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
            auto statusMessage = StringUtils::ToClr(value);

            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                    popupHandler->OnStatusMessage(_browserControl, browserWrapper, statusMessage);
                }
            }
            else
            {
                _browserControl->OnStatusMessage(statusMessage);
            }
        }

        bool ClientAdapter::OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                    return popupHandler->OnKeyEvent(
                        _browserControl, browserWrapper, (KeyType)event.type, 
                        event.windows_key_code, event.native_key_code, 
                        (CefEventFlags)event.modifiers, event.is_system_key == 1);
                }
            }
            else
            {
                auto handler = _browserControl->KeyboardHandler;

                if (handler == nullptr)
                {
                    return false;
                }

                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), false);

                return handler->OnKeyEvent(
                    _browserControl, browserWrapper, (KeyType)event.type, event.windows_key_code, 
                    event.native_key_code,
                    (CefEventFlags)event.modifiers, event.is_system_key == 1);
            }
            return false;
        }

        bool ClientAdapter::OnPreKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event, bool* is_keyboard_shortcut)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                    popupHandler->OnPreKeyEvent(_browserControl, browserWrapper, (KeyType)event.type, event.windows_key_code, event.native_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1, *is_keyboard_shortcut);
                }
            }
            else
            {
                auto handler = _browserControl->KeyboardHandler;

                if (handler == nullptr)
                {
                    return false;
                }

                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), false);

                return handler->OnPreKeyEvent(
                    _browserControl, browserWrapper, (KeyType)event.type, event.windows_key_code,
                    event.native_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1,
                    *is_keyboard_shortcut);
            }
            return false;
        }

        void ClientAdapter::OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);
                    CefFrameWrapper frameWrapper(frame, _browserAdapter);
                    popupHandler->OnFrameLoadStart(_browserControl, gcnew FrameLoadStartEventArgs(browserWrapper, %frameWrapper));
                }
            }
            else
            {
                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), false);
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                _browserControl->OnFrameLoadStart(gcnew FrameLoadStartEventArgs(browserWrapper, %frameWrapper));
            }
        }

        void ClientAdapter::OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                    CefFrameWrapper frameWrapper(frame, _browserAdapter);
                    popupHandler->OnFrameLoadEnd(_browserControl, gcnew FrameLoadEndEventArgs(browserWrapper, %frameWrapper, httpStatusCode));
                }
            }
            else
            {
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), false);
                _browserControl->OnFrameLoadEnd(gcnew FrameLoadEndEventArgs(browserWrapper, %frameWrapper, httpStatusCode));
            }
        }

        void ClientAdapter::OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& errorText, const CefString& failedUrl)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);
                    CefFrameWrapper frameWrapper(frame, _browserAdapter);
                    popupHandler->OnLoadError(_browserControl, browserWrapper,
                        gcnew LoadErrorEventArgs(%frameWrapper, static_cast<CefErrorCode>(errorCode), StringUtils::ToClr(errorText), StringUtils::ToClr(failedUrl)));
                }
            }
            else
            {
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                _browserControl->OnLoadError(%frameWrapper, (CefErrorCode)errorCode, StringUtils::ToClr(errorText), StringUtils::ToClr(failedUrl));
            }
        }

        bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isRedirect)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler == nullptr)
                {
                    return false;
                }

                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                CefRequestWrapper requestWrapper(request);
                return popupHandler->OnBeforeBrowse(_browserControl, browserWrapper, %requestWrapper, isRedirect, %frameWrapper);
            }
            else
            {
                auto handler = _browserControl->RequestHandler;
                if (handler == nullptr)
                {
                    return false;
                }

                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), false);
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                CefRequestWrapper requestWrapper(request);
                
                return handler->OnBeforeBrowse(_browserControl, browserWrapper, %frameWrapper, %requestWrapper, isRedirect);
            }

            return false;
        }

        bool ClientAdapter::OnCertificateError(CefRefPtr<CefBrowser> browser, cef_errorcode_t cert_error, const CefString& request_url, CefRefPtr<CefSSLInfo> ssl_info, CefRefPtr<CefRequestCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            // If callback is empty the error cannot be recovered from and the request will be canceled automatically.
            // Still notify the user of the certificate error just don't provide a callback.
            auto requestCallback = callback == NULL ? nullptr : gcnew CefRequestCallbackWrapper(callback);

            IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnCertificateError(_browserControl, browserWrapper, (CefErrorCode)cert_error, StringUtils::ToClr(request_url), requestCallback);
        }

        bool ClientAdapter::OnQuotaRequest(CefRefPtr<CefBrowser> browser, const CefString& originUrl, int64 newSize, CefRefPtr<CefRequestCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }
            
            auto requestCallback = gcnew CefRequestCallbackWrapper(callback);

            IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnQuotaRequest(_browserControl, browserWrapper, StringUtils::ToClr(originUrl), newSize, requestCallback);
        }

        // CEF3 API: public virtual bool OnBeforePluginLoad( CefRefPtr< CefBrowser > browser, const CefString& url, const CefString& policy_url, CefRefPtr< CefWebPluginInfo > info );
        // ---
        // return value:
        //     false: Load Plugin (do not block it)
        //     true:  Ignore Plugin (Block it)
        bool ClientAdapter::OnBeforePluginLoad(CefRefPtr<CefBrowser> browser, const CefString& url, const CefString& policy_url, CefRefPtr<CefWebPluginInfo> info)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto pluginInfo = TypeConversion::FromNative(info);

            IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnBeforePluginLoad(_browserControl, browserWrapper, StringUtils::ToClr(url), StringUtils::ToClr(policy_url), pluginInfo);
        }

        void ClientAdapter::OnPluginCrashed(CefRefPtr<CefBrowser> browser, const CefString& plugin_path)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnPluginCrashed(_browserControl, browserWrapper, StringUtils::ToClr(plugin_path));
            }
        }

        void ClientAdapter::OnRenderProcessTerminated(CefRefPtr<CefBrowser> browser, TerminationStatus status)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnRenderProcessTerminated(_browserControl, browserWrapper, (CefTerminationStatus)status);
            }
        }

        void ClientAdapter::OnResourceRedirect(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& oldUrl, CefString& newUrl)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto managedNewUrl = StringUtils::ToClr(newUrl);
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);;
                    CefFrameWrapper frameWrapper(frame, _browserAdapter);

                    popupHandler->OnResourceRedirect(_browserControl, browserWrapper, %frameWrapper, managedNewUrl);

                    newUrl = StringUtils::ToNative(managedNewUrl);
                }
            }
            else
            {
                auto handler = _browserControl->RequestHandler;
                if (handler != nullptr)
                {
                    auto managedNewUrl = StringUtils::ToClr(newUrl);
                    CefFrameWrapper frameWrapper(frame, _browserAdapter);

                    handler->OnResourceRedirect(_browserControl, %frameWrapper, managedNewUrl);

                    newUrl = StringUtils::ToNative(managedNewUrl);
                }
            }
        }

        void ClientAdapter::OnProtocolExecution(CefRefPtr<CefBrowser> browser, const CefString& url, bool& allowOSExecution)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                allowOSExecution = handler->OnProtocolExecution(_browserControl, browserWrapper, StringUtils::ToClr(url));
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

            IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto frameWrapper = gcnew CefFrameWrapper(frame, _browserAdapter);
            auto requestWrapper = gcnew CefRequestWrapper(request);

            auto resourceHandler = factory->GetResourceHandler(_browserControl, browserWrapper, frameWrapper, requestWrapper);

            if (resourceHandler == nullptr)
            {
                // Clean up our disposables if our factory doesn't want
                // this request.
                delete frameWrapper;
                delete requestWrapper;
                return NULL;
            }

            // No need to pass browserWrapper for disposable lifetime management here
            // because GetBrowserWrapper returned IBrowser^s are already properly
            // managed.
            return new ResourceHandlerWrapper(resourceHandler, nullptr, frameWrapper, requestWrapper);
        }

        cef_return_value_t ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefRequestCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return cef_return_value_t::RV_CONTINUE;
            }

            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto frameWrapper = gcnew CefFrameWrapper(frame, _browserAdapter);
                    auto requestWrapper = gcnew CefRequestWrapper(request);
                    IBrowser^ browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);

                    auto requestCallback = gcnew CefRequestCallbackWrapper(callback, frameWrapper, requestWrapper);

                    return (cef_return_value_t)popupHandler->OnBeforeResourceLoad(_browserControl, browserWrapper, frameWrapper, requestWrapper, requestCallback);
                }
            }
            else
            {
                auto frameWrapper = gcnew CefFrameWrapper(frame, _browserAdapter);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), false);
                auto requestWrapper = gcnew CefRequestWrapper(request);
                auto requestCallback = gcnew CefRequestCallbackWrapper(callback, frameWrapper, requestWrapper);

                return (cef_return_value_t)handler->OnBeforeResourceLoad(_browserControl, browserWrapper, frameWrapper, requestWrapper, requestCallback);
            }
            return cef_return_value_t::RV_CONTINUE;
        }

        bool ClientAdapter::GetAuthCredentials(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, bool isProxy,
            const CefString& host, int port, const CefString& realm, const CefString& scheme, CefRefPtr<CefAuthCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto frameWrapper = gcnew CefFrameWrapper(frame, _browserAdapter);
            auto callbackWrapper = gcnew CefAuthCallbackWrapper(callback, frameWrapper);

            return handler->GetAuthCredentials(
                _browserControl, browserWrapper, frameWrapper, isProxy, 
                StringUtils::ToClr(host), port, StringUtils::ToClr(realm), 
                StringUtils::ToClr(scheme), callbackWrapper);
        }

        void ClientAdapter::OnBeforeContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
            CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model)
        {
            auto handler = _browserControl->MenuHandler;
            if (handler == nullptr) return;

            // Context menu params
            CefContextMenuParamsWrapper contextMenuParamsWrapper(params);
            CefFrameWrapper frameWrapper(frame, _browserAdapter);
            auto result = handler->OnBeforeContextMenu(_browserControl, %frameWrapper, %contextMenuParamsWrapper);
            if (!result)
            {
                model->Clear();
            }
        }

        void ClientAdapter::OnGotFocus(CefRefPtr<CefBrowser> browser)
        {
            auto handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                return;
            }

            // NOTE: a popup handler for OnGotFocus doesn't make sense yet because
            // non-offscreen windows don't wrap popup browser's yet.
            if (!browser->IsPopup())
            {
                handler->OnGotFocus();
            }
        }

        bool ClientAdapter::OnSetFocus(CefRefPtr<CefBrowser> browser, FocusSource source)
        {
            auto handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                // Allow the focus to be set by default.
                return false;
            }

            // NOTE: a popup handler for OnGotFocus doesn't make sense yet because
            // non-offscreen windows don't wrap popup browser's yet.
            if (!browser->IsPopup())
            {
                return handler->OnSetFocus((CefFocusSource)source);
            }
            // Allow the focus to be set by default.
            return false;
        }

        void ClientAdapter::OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next)
        {
            auto handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                return;
            }

            // NOTE: a popup handler for OnGotFocus doesn't make sense yet because
            // non-offscreen windows don't wrap popup browser's yet.
            if (!browser->IsPopup())
            {
                handler->OnTakeFocus(next);
            }
        }

        bool ClientAdapter::OnJSDialog(CefRefPtr<CefBrowser> browser, const CefString& origin_url, const CefString& accept_lang,
            JSDialogType dialog_type, const CefString& message_text, const CefString& default_prompt_text,
            CefRefPtr<CefJSDialogCallback> callback, bool& suppress_message)
        {
            auto handler = _browserControl->JsDialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto callbackWrapper = gcnew CefJSDialogCallbackWrapper(callback);
            return handler->OnJSDialog(_browserControl, browserWrapper,
                                       StringUtils::ToClr(origin_url), StringUtils::ToClr(accept_lang), (CefJsDialogType)dialog_type, 
                                       StringUtils::ToClr(message_text), StringUtils::ToClr(default_prompt_text), callbackWrapper, suppress_message);
        }

        bool ClientAdapter::OnBeforeUnloadDialog(CefRefPtr<CefBrowser> browser, const CefString& message_text, bool is_reload, CefRefPtr<CefJSDialogCallback> callback)
        {
            auto handler = _browserControl->JsDialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto callbackWrapper = gcnew CefJSDialogCallbackWrapper(callback);

            return handler->OnJSBeforeUnload(_browserControl, browserWrapper, StringUtils::ToClr(message_text), is_reload, callbackWrapper);
        }

        bool ClientAdapter::OnFileDialog(CefRefPtr<CefBrowser> browser, FileDialogMode mode, const CefString& title,
                const CefString& default_file_path, const std::vector<CefString>& accept_filters, int selected_accept_filter,
                CefRefPtr<CefFileDialogCallback> callback)
        {
            auto handler = _browserControl->DialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto callbackWrapper = gcnew CefFileDialogCallbackWrapper(callback);

            return handler->OnFileDialog(_browserControl, browserWrapper, (CefFileDialogMode)mode, StringUtils::ToClr(title), StringUtils::ToClr(default_file_path), StringUtils::ToClr(accept_filters), selected_accept_filter, callbackWrapper);
        }

        bool ClientAdapter::OnDragEnter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData, DragOperationsMask mask)
        {
            auto handler = _browserControl->DragHandler;

            if (handler == nullptr)
            {
                return false;
            }

            CefDragDataWrapper dragDataWrapper(dragData);
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnDragEnter(_browserControl, browserWrapper, %dragDataWrapper, (CefSharp::DragOperationsMask)mask);
        }

        bool ClientAdapter::OnRequestGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id, CefRefPtr<CefGeolocationCallback> callback)
        {
            auto handler = _browserControl->GeolocationHandler;
            if (handler == nullptr)
            {
                // Default deny, as CEF does.
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto callbackWrapper = gcnew CefGeolocationCallbackWrapper(callback);

            return handler->OnRequestGeolocationPermission(_browserControl, browserWrapper, StringUtils::ToClr(requesting_url), request_id, callbackWrapper);
        }

        void ClientAdapter::OnCancelGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id)
        {
            IGeolocationHandler^ handler = _browserControl->GeolocationHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                handler->OnCancelGeolocationPermission(_browserControl, browserWrapper, StringUtils::ToClr(requesting_url), request_id);
            }
        }

        void ClientAdapter::OnBeforeDownload(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback)
        {
            auto handler = _browserControl->DownloadHandler;
            
            if(handler != nullptr)
            {
                auto downloadItem = TypeConversion::FromNative(download_item);
                downloadItem->SuggestedFileName = StringUtils::ToClr(suggested_name);

                auto callbackWrapper = gcnew CefBeforeDownloadCallbackWrapper(callback);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnBeforeDownload(browserWrapper, downloadItem, callbackWrapper);
            }
        };

        void ClientAdapter::OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            CefRefPtr<CefDownloadItemCallback> callback)
        {
            auto handler = _browserControl->DownloadHandler;

            if(handler != nullptr)
            {
                auto callbackWrapper = gcnew CefDownloadItemCallbackWrapper(callback);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnDownloadUpdated(browserWrapper, TypeConversion::FromNative(download_item), callbackWrapper);
            }
        }

        void ClientAdapter::SendJavascriptRootObject(CefRefPtr<CefBrowser> browser, JavascriptRootObject^ rootObject)
        {
            auto message = CefProcessMessage::Create(Messaging::kJsRootObject);
            auto argumentList = message->GetArgumentList();
            Serialization::SerializeJsRootObject(rootObject, argumentList);

            browser->SendProcessMessage(CefProcessId::PID_RENDERER, message);
        }

        bool ClientAdapter::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message)
        {
            bool handled = false;

            ProcessMessageDelegateSet::iterator it = _processMessageDelegates.begin();
            for (; it != _processMessageDelegates.end() && !handled; ++it) {
                handled = (*it)->OnProcessMessageReceived(browser, source_process, message);
            }

            return handled;
        }

        Task<JavascriptResponse^>^ ClientAdapter::EvaluateScriptAsync(int browserId, int frameId, String^ script, Nullable<TimeSpan> timeout)
        {
            Task<JavascriptResponse^>^ result = nullptr;
            if (_cefBrowser.get())
            {
                result = _evalScriptDoneDelegate->EvaluateScriptAsync(_cefBrowser, frameId, script, timeout);
            }
            return result;
        }

        void ClientAdapter::AddProcessMessageDelegate(CefRefPtr<ProcessMessageDelegate> processMessageDelegate)
        {
            _processMessageDelegates.insert(processMessageDelegate);
        }

    }
}
