// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefContextMenuParamsWrapper.h"
#include "Internals/CefDragDataWrapper.h"
#include "ClientAdapter.h"
#include "CefJSDialogCallbackWrapper.h"
#include "CefRequestCallbackWrapper.h"
#include "Internals/TypeConversion.h"
#include "include/wrapper/cef_stream_resource_handler.h"
#include "include/internal/cef_types.h"
#include "Internals/CefSharpBrowserWrapper.h"
#include "CefDownloadItemCallbackWrapper.h"
#include "CefBeforeDownloadCallbackWrapper.h"
#include "CefGeolocationCallbackWrapper.h"
#include "CefFileDialogCallbackWrapper.h"
#include "CefAuthCallbackWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        void ClientAdapter::CloseAllPopups(bool forceClose)
        {
            if (_popupBrowsers->Count > 0)
            {
                for each (IBrowser^ browser in _popupBrowsers->Values)
                {
                    browser->GetHost()->CloseBrowser(forceClose);
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
            IBrowser^ browserWrapper;
            if (!_popupBrowsers->TryGetValue(browser->GetIdentifier(), browserWrapper))
            {
                browserWrapper = gcnew CefSharpBrowserWrapper(browser, _browserAdapter);
            }
            auto result = handler->OnBeforePopup(_browserControl, browserWrapper,
                gcnew CefFrameWrapper(frame, _browserAdapter), StringUtils::ToClr(target_url),
                windowInfo.x, windowInfo.y, windowInfo.width, windowInfo.height, *no_javascript_access);
            return result;
        }

        void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
        {
            if (browser->IsPopup())
            {
                auto browserWrapper = gcnew CefSharpBrowserWrapper(browser, _browserAdapter);
                // Add to the list of popup browsers.
                _popupBrowsers->Add(browser->GetIdentifier(), browserWrapper);
                IPopupHandler^ handler = _browserControl->PopupHandler;
                if (handler != nullptr)
                {
                    handler->OnAfterCreated(_browserControl, browserWrapper);
                }
            }
            else
            {
                _browserHwnd = browser->GetHost()->GetWindowHandle();
                _cefBrowser = browser;
                auto browserId = browser->GetIdentifier();
                
                if (static_cast<IBrowserAdapter^>(_browserAdapter) != nullptr)
                {
                    _browserAdapter->OnAfterBrowserCreated(browserId);
                }
            }
        }

        void ClientAdapter::OnBeforeClose(CefRefPtr<CefBrowser> browser)
        {
            if (browser->IsPopup())
            {
                // Remove from the browser popup list.
                auto browserId = browser->GetIdentifier();
                IBrowser^ entry;
                if (_popupBrowsers->TryGetValue(browserId, entry))
                {
                    IPopupHandler^ handler = _browserControl->PopupHandler;
                    if (handler != nullptr)
                    {
                        handler->OnBeforeClose(_browserControl, entry);
                    }
                    _popupBrowsers->Remove(browserId);
                    // Dispose the CefSharpBrowserWrapper
                    delete entry;
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
            if (browser->IsPopup())
            {
                auto browserId = browser->GetIdentifier();
                IBrowser^ entry;
                IPopupHandler^ handler = _browserControl->PopupHandler;
                if (handler != nullptr)
                {
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        handler->OnLoadingStateChange(_browserControl, entry, isLoading, canGoBack, canGoForward);
                    }
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
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        popupHandler->OnFaviconUrlChange(_browserControl, entry, StringUtils::ToClr(iconUrls));
                    }
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
            if (browser->IsPopup())
            {
                auto browserId = browser->GetIdentifier();
                IBrowser^ entry;
                if (_popupBrowsers->TryGetValue(browserId, entry))
                {
                    auto popupHandler = _browserControl->PopupHandler;
                    if (popupHandler != nullptr)
                    {
                        popupHandler->OnStatusMessage(_browserControl, entry, valueStr);
                    }
                }
            }
            else
            {
                _browserControl->OnStatusMessage(valueStr);
            }
        }

        bool ClientAdapter::OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event)
        {
            IKeyboardHandler^ handler = _browserControl->KeyboardHandler;

            if (handler == nullptr)
            {
                return false;
            }

            // TODO: This seems wrong, browser might be a popup browser.
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

            // TODO: This seems wrong, browser might be a popup browser.
            return handler->OnPreKeyEvent(_browserControl, (KeyType)event.type, event.windows_key_code, event.native_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1, *is_keyboard_shortcut);
        }

        void ClientAdapter::OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        auto popupHandler = _browserControl->PopupHandler;
                        auto frameWrapper = gcnew CefFrameWrapper(frame, _browserAdapter);
                        popupHandler->OnFrameLoadStart(_browserControl, entry, gcnew FrameLoadStartEventArgs(frameWrapper));
                    }
                }
            }
            else
            {
                _browserControl->OnFrameLoadStart(gcnew CefFrameWrapper(frame, _browserAdapter));
            }
        }

        void ClientAdapter::OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        auto popupHandler = _browserControl->PopupHandler;
                        auto frameWrapper = gcnew CefFrameWrapper(frame, _browserAdapter);
                        popupHandler->OnFrameLoadEnd(_browserControl, entry, gcnew FrameLoadEndEventArgs(frameWrapper, httpStatusCode));
                    }
                }
            }
            else
            {
                _browserControl->OnFrameLoadEnd(gcnew CefFrameWrapper(frame, _browserAdapter), httpStatusCode);
            }
        }

        void ClientAdapter::OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& errorText, const CefString& failedUrl)
        {
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        auto popupHandler = _browserControl->PopupHandler;
                        auto frameWrapper = gcnew CefFrameWrapper(frame, _browserAdapter);
                        popupHandler->OnLoadError(_browserControl, entry, 
                            gcnew LoadErrorEventArgs(frameWrapper, static_cast<CefErrorCode>(errorCode), StringUtils::ToClr(errorText), StringUtils::ToClr(failedUrl)));
                    }
                }
            }
            else
            {
                _browserControl->OnLoadError(gcnew CefFrameWrapper(frame, _browserAdapter), (CefErrorCode)errorCode, StringUtils::ToClr(errorText), StringUtils::ToClr(failedUrl));
            }
        }

        bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isRedirect)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            CefRequestWrapper^ wrapper = gcnew CefRequestWrapper(request);

            return handler->OnBeforeBrowse(_browserControl, wrapper, isRedirect, gcnew CefFrameWrapper(frame, _browserAdapter));
        }

        bool ClientAdapter::OnCertificateError(CefRefPtr<CefBrowser> browser, cef_errorcode_t cert_error, const CefString& request_url, CefRefPtr<CefSSLInfo> ssl_info, CefRefPtr<CefRequestCallback> callback)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            // If callback is empty the error cannot be recovered from and the request will be canceled automatically.
            // Still notify the user of the certificate error just don't provide a callback.
            auto requestCallback = callback == NULL ? nullptr : gcnew CefRequestCallbackWrapper(callback);

            return handler->OnCertificateError(_browserControl, (CefErrorCode)cert_error, StringUtils::ToClr(request_url), requestCallback);
        }

        bool ClientAdapter::OnQuotaRequest(CefRefPtr<CefBrowser> browser, const CefString& originUrl, int64 newSize, CefRefPtr<CefRequestCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }
            
            auto requestCallback = gcnew CefRequestCallbackWrapper(callback);

            return handler->OnQuotaRequest(_browserControl, StringUtils::ToClr(originUrl), newSize, requestCallback);
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

        void ClientAdapter::OnResourceRedirect(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& oldUrl, CefString& newUrl)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler != nullptr)
            {
                auto managedNewUrl = StringUtils::ToClr(newUrl);
                handler->OnResourceRedirect(_browserControl, gcnew CefFrameWrapper(frame, _browserAdapter), managedNewUrl);

                newUrl = StringUtils::ToNative(managedNewUrl);
            }	
        }

        void ClientAdapter::OnProtocolExecution(CefRefPtr<CefBrowser> browser, const CefString& url, bool& allowOSExecution)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler != nullptr)
            {
                allowOSExecution = handler->OnProtocolExecution(_browserControl, StringUtils::ToClr(url));
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

            if (resourceHandler != nullptr)
            {
                return new ResourceHandlerWrapper(resourceHandler);
            }

            return NULL;
        }

        cef_return_value_t ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefRequestCallback> callback)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return cef_return_value_t::RV_CONTINUE;
            }

            auto requestWrapper = gcnew CefRequestWrapper(request);
            auto requestCallback = gcnew CefRequestCallbackWrapper(callback);

            return (cef_return_value_t)handler->OnBeforeResourceLoad(_browserControl, requestWrapper, gcnew CefFrameWrapper(frame, _browserAdapter), requestCallback);
        }

        bool ClientAdapter::GetAuthCredentials(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, bool isProxy,
            const CefString& host, int port, const CefString& realm, const CefString& scheme, CefRefPtr<CefAuthCallback> callback)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }

            auto callbackWrapper = gcnew CefAuthCallbackWrapper(callback);

            return handler->GetAuthCredentials(_browserControl, gcnew CefFrameWrapper(frame, _browserAdapter), isProxy, StringUtils::ToClr(host), port, StringUtils::ToClr(realm), StringUtils::ToClr(scheme), callbackWrapper);
        }

        void ClientAdapter::OnBeforeContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
            CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model)
        {

            IMenuHandler^ handler = _browserControl->MenuHandler;
            if (handler == nullptr) return;

            // Context menu params
            CefContextMenuParamsWrapper^ contextMenuParamsWrapper = gcnew CefContextMenuParamsWrapper(params);

            auto result = handler->OnBeforeContextMenu(_browserControl, gcnew CefFrameWrapper(frame, _browserAdapter), contextMenuParamsWrapper);
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
            auto handler = _browserControl->JsDialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto callbackWrapper = gcnew CefJSDialogCallbackWrapper(callback);

            return handler->OnJSDialog(_browserControl, StringUtils::ToClr(origin_url), StringUtils::ToClr(accept_lang), (CefJsDialogType)dialog_type, 
                                        StringUtils::ToClr(message_text), StringUtils::ToClr(default_prompt_text), callbackWrapper, suppress_message);
        }

        bool ClientAdapter::OnBeforeUnloadDialog(CefRefPtr<CefBrowser> browser, const CefString& message_text, bool is_reload, CefRefPtr<CefJSDialogCallback> callback)
        {
            IJsDialogHandler^ handler = _browserControl->JsDialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto callbackWrapper = gcnew CefJSDialogCallbackWrapper(callback);

            return handler->OnJSBeforeUnload(_browserControl, StringUtils::ToClr(message_text), is_reload, callbackWrapper);
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

            auto callbackWrapper = gcnew CefFileDialogCallbackWrapper(callback);

            return handler->OnFileDialog(_browserControl, (CefFileDialogMode)mode, StringUtils::ToClr(title), StringUtils::ToClr(default_file_path), StringUtils::ToClr(accept_filters), selected_accept_filter, callbackWrapper);
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

            auto callbackWrapper = gcnew CefGeolocationCallbackWrapper(callback);

            return handler->OnRequestGeolocationPermission(_browserControl, StringUtils::ToClr(requesting_url), request_id, callbackWrapper);
        }

        void ClientAdapter::OnCancelGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id)
        {
            IGeolocationHandler^ handler = _browserControl->GeolocationHandler;
            if (handler != nullptr)
            {
                handler->OnCancelGeolocationPermission(_browserControl, StringUtils::ToClr(requesting_url), request_id);
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

                handler->OnBeforeDownload(gcnew CefSharpBrowserWrapper(browser, _browserAdapter), downloadItem, callbackWrapper);
            }
        };

        void ClientAdapter::OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            CefRefPtr<CefDownloadItemCallback> callback)
        {
            auto handler = _browserControl->DownloadHandler;

            if(handler != nullptr)
            {
                handler->OnDownloadUpdated(gcnew CefSharpBrowserWrapper(browser, _browserAdapter), TypeConversion::FromNative(download_item), gcnew CefDownloadItemCallbackWrapper(callback));
            }
        }
    }
}
