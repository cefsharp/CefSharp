﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "Internals/CefRequestWrapper.h"
#include "Internals/CefContextMenuParamsWrapper.h"
#include "Internals/CefDragDataWrapper.h"
#include "ClientAdapter.h"
#include "DownloadAdapter.h"
#include "StreamAdapter.h"
#include "JsDialogCallback.h"
#include "RequestCallback.h"
#include "Internals/TypeConversion.h"
#include "include/wrapper/cef_stream_resource_handler.h"
#include "include/internal/cef_types.h"
#include "Internals/CefSharpBrowserWrapper.h"

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
                    
                    (dynamic_cast<CefSharpBrowserWrapper^>(browser))->GetHost()->CloseBrowser(forceClose);
                }
            }
        }

        bool ClientAdapter::OnBeforePopup(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& target_url,
            const CefString& target_frame_name, const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo,
            CefRefPtr<CefClient>& client, CefBrowserSettings& settings, bool* no_javascript_access)
        {
            ILifeSpanHandler^ handler = _browserControl->LifeSpanHandler;
            bool createdWrapper = false;

            if (handler == nullptr)
            {
                return false;
            }
            IBrowser^ browserWrapper;
            if (!_popupBrowsers->TryGetValue(browser->GetIdentifier(), browserWrapper))
            {
                browserWrapper = gcnew CefSharpBrowserWrapper(browser, _browserAdapter);
                createdWrapper = true;
            }
            CefFrameWrapper frameWrapper(frame, _browserAdapter);
            auto result = handler->OnBeforePopup(_browserControl, browserWrapper,
                %frameWrapper, StringUtils::ToClr(target_url),
                windowInfo.x, windowInfo.y, windowInfo.width, windowInfo.height, *no_javascript_access);
            if (createdWrapper)
            {
                delete browserWrapper;
            }
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
                else
                {
                    ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnBeforeClose"));
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
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnLoadingStateChange"));
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
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnFaviconUrlChange"));
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
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnStatusMessage"));
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
            // TODO: windows_key_code could possibly be the wrong choice here (the OnKeyEvent signature has changed since CEF1). The
            // other option would be native_key_code.
            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        return popupHandler->OnKeyEvent(_browserControl, entry, (KeyType)event.type, event.windows_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1);
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnKeyEvent"));
                    }
                }
            }
            else
            {
                IKeyboardHandler^ handler = _browserControl->KeyboardHandler;

                if (handler == nullptr)
                {
                    return false;
                }
                return handler->OnKeyEvent(_browserControl, (KeyType)event.type, event.windows_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1);
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
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        popupHandler->OnPreKeyEvent(_browserControl, entry, (KeyType)event.type, event.windows_key_code, event.native_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1, *is_keyboard_shortcut);
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnPreKeyEvent"));
                    }
                }
            }
            else
            {
                IKeyboardHandler^ handler = _browserControl->KeyboardHandler;

                if (handler == nullptr)
                {
                    return false;
                }

                return handler->OnPreKeyEvent(_browserControl, (KeyType)event.type, event.windows_key_code, event.native_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1, *is_keyboard_shortcut);
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
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        CefFrameWrapper frameWrapper(frame, _browserAdapter);
                        popupHandler->OnFrameLoadStart(_browserControl, gcnew FrameLoadStartEventArgs(entry, %frameWrapper));
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnLoadStart"));
                    }
                }
            }
            else
            {
                CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                _browserControl->OnFrameLoadStart(gcnew FrameLoadStartEventArgs(%browserWrapper, %frameWrapper));
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
                        CefFrameWrapper frameWrapper(frame, _browserAdapter);
                        popupHandler->OnFrameLoadEnd(_browserControl, gcnew FrameLoadEndEventArgs(entry, %frameWrapper, httpStatusCode));
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnLoadEnd"));
                    }
                }
            }
            else
            {
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);
                _browserControl->OnFrameLoadEnd(gcnew FrameLoadEndEventArgs(%browserWrapper, %frameWrapper, httpStatusCode));
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
                        CefFrameWrapper frameWrapper(frame, _browserAdapter);
                        popupHandler->OnLoadError(_browserControl, entry,
                            gcnew LoadErrorEventArgs(%frameWrapper, static_cast<CefErrorCode>(errorCode), StringUtils::ToClr(errorText), StringUtils::ToClr(failedUrl)));
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnLoadError"));
                    }
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
            bool result;

            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        CefFrameWrapper frameWrapper(frame, _browserAdapter);
                        CefRequestWrapper wrapper(request);
                        result = popupHandler->OnBeforeBrowse(_browserControl, entry, %wrapper, isRedirect, %frameWrapper);
                        return result;
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnBeforeBrowser"));
                    }
                }
            }
            else
            {
                IRequestHandler^ handler = _browserControl->RequestHandler;
                if (handler == nullptr)
                {
                    return false;
                }

                CefRequestWrapper wrapper(request);
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                result = handler->OnBeforeBrowse(_browserControl, %wrapper, isRedirect, %frameWrapper);
                return result;
            }
            return false;
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
            auto requestCallback = callback == NULL ? nullptr : gcnew RequestCallback(callback);

            auto browserId = browser->GetIdentifier();
            IBrowser^ entry;
            if (!_popupBrowsers->TryGetValue(browserId, entry))
            {
                if (browser->IsPopup())
                {
                    ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnCertificateError"));
                }
                CefSharpBrowserWrapper browser(_cefBrowser, _browserAdapter);
                return handler->OnCertificateError(_browserControl, %browser, (CefErrorCode)cert_error, StringUtils::ToClr(request_url), requestCallback);
            }
            else
            {
                return handler->OnCertificateError(_browserControl, entry, (CefErrorCode)cert_error, StringUtils::ToClr(request_url), requestCallback);
            }
        }

        bool ClientAdapter::OnQuotaRequest(CefRefPtr<CefBrowser> browser, const CefString& originUrl, int64 newSize, CefRefPtr<CefRequestCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;
            if (handler == nullptr)
            {
                return false;
            }
            
            auto requestCallback = gcnew RequestCallback(callback);

            auto browserId = browser->GetIdentifier();
            IBrowser^ entry;
            if (!_popupBrowsers->TryGetValue(browserId, entry))
            {
                if (browser->IsPopup())
                {
                    ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnQuotaRequest"));
                }
                CefSharpBrowserWrapper wrapper(_cefBrowser, _browserAdapter);
                return handler->OnQuotaRequest(_browserControl, %wrapper, StringUtils::ToClr(originUrl), newSize, requestCallback);
            }
            else
            {
                return handler->OnQuotaRequest(_browserControl, entry, StringUtils::ToClr(originUrl), newSize, requestCallback);
            }
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

            auto browserId = browser->GetIdentifier();
            IBrowser^ entry;
            if (!_popupBrowsers->TryGetValue(browserId, entry))
            {
                if (browser->IsPopup())
                {
                    ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnBeforePluginLoad"));
                }
                CefSharpBrowserWrapper wrapper(_cefBrowser, _browserAdapter);
                auto result = handler->OnBeforePluginLoad(_browserControl, %wrapper, StringUtils::ToClr(url), StringUtils::ToClr(policy_url), pluginInfo);
                return result;
            }
            return handler->OnBeforePluginLoad(_browserControl, entry, StringUtils::ToClr(url), StringUtils::ToClr(policy_url), pluginInfo);
        }

        void ClientAdapter::OnPluginCrashed(CefRefPtr<CefBrowser> browser, const CefString& plugin_path)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler != nullptr)
            {
                auto browserId = browser->GetIdentifier();
                IBrowser^ entry;
                if (!_popupBrowsers->TryGetValue(browserId, entry))
                {
                    if (browser->IsPopup())
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnPluginCrashed"));
                    }
                    CefSharpBrowserWrapper wrapper(_cefBrowser, _browserAdapter);
                    handler->OnPluginCrashed(_browserControl, %wrapper, StringUtils::ToClr(plugin_path));
                }
                else
                {
                    handler->OnPluginCrashed(_browserControl, entry, StringUtils::ToClr(plugin_path));
                }
            }			
        }

        void ClientAdapter::OnRenderProcessTerminated(CefRefPtr<CefBrowser> browser, TerminationStatus status)
        {
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler != nullptr)
            {
                auto browserId = browser->GetIdentifier();
                IBrowser^ entry;
                if (!_popupBrowsers->TryGetValue(browserId, entry))
                {
                    if (browser->IsPopup())
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnRenderProcessTerminated"));
                    }
                    CefSharpBrowserWrapper wrapper(_cefBrowser, _browserAdapter);
                    handler->OnRenderProcessTerminated(_browserControl, %wrapper, (CefTerminationStatus)status);
                }
                else
                {
                    handler->OnRenderProcessTerminated(_browserControl, entry, (CefTerminationStatus)status);
                }
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
                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        CefFrameWrapper frameWrapper(frame, _browserAdapter);
                        popupHandler->OnResourceRedirect(_browserControl, entry, %frameWrapper, managedNewUrl);

                        newUrl = StringUtils::ToNative(managedNewUrl);
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnResourceRedirect"));
                    }
                }
            }
            else
            {
                IRequestHandler^ handler = _browserControl->RequestHandler;
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
            IRequestHandler^ handler = _browserControl->RequestHandler;
            if (handler != nullptr)
            {
                auto browserId = browser->GetIdentifier();
                IBrowser^ entry;
                if (!_popupBrowsers->TryGetValue(browserId, entry))
                {
                    if (browser->IsPopup())
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnProtocolExecution"));
                    }
                    CefSharpBrowserWrapper wrapper(_cefBrowser, _browserAdapter);
                    allowOSExecution = handler->OnProtocolExecution(_browserControl, %wrapper, StringUtils::ToClr(url));
                }
                else
                {
                    allowOSExecution = handler->OnProtocolExecution(_browserControl, entry, StringUtils::ToClr(url));
                }
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
            CefFrameWrapper frameWrapper(frame, _browserAdapter);
            IResourceHandler^ resourceHandler;

            auto browserId = browser->GetIdentifier();
            IBrowser^ entry;
            if (!_popupBrowsers->TryGetValue(browserId, entry))
            {
                if (browser->IsPopup())
                {
                    ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::GetResourceHandler"));
                }
                CefSharpBrowserWrapper wrapper(_cefBrowser, _browserAdapter);
                resourceHandler = factory->GetResourceHandler(_browserControl, %wrapper, %frameWrapper, requestWrapper);
            }
            else
            {
                resourceHandler = factory->GetResourceHandler(_browserControl, entry, %frameWrapper, requestWrapper);
            }

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

            if (browser->IsPopup())
            {
                auto popupHandler = _browserControl->PopupHandler;
                if (popupHandler != nullptr)
                {
                    auto requestWrapper = gcnew CefRequestWrapper(request);
                    auto requestCallback = gcnew RequestCallback(callback);

                    auto browserId = browser->GetIdentifier();
                    IBrowser^ entry;
                    if (_popupBrowsers->TryGetValue(browserId, entry))
                    {
                        CefFrameWrapper frameWrapper(frame, _browserAdapter);
                        return (cef_return_value_t)popupHandler->OnBeforeResourceLoad(_browserControl, entry, %frameWrapper, requestWrapper, requestCallback);
                    }
                    else
                    {
                        ThrowUnknownPopupBrowser(gcnew String(L"ClientAdapter::OnBeforeResourceLoad"));
                    }
                }
            }
            else
            {
                auto requestWrapper = gcnew CefRequestWrapper(request);
                auto requestCallback = gcnew RequestCallback(callback);
                CefFrameWrapper frameWrapper(frame, _browserAdapter);
                CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);

                return (cef_return_value_t)handler->OnBeforeResourceLoad(_browserControl, %browserWrapper, %frameWrapper, requestWrapper, requestCallback);
            }
            return cef_return_value_t::RV_CONTINUE;
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

            CefSharpBrowserWrapper browserWrapper(_cefBrowser, _browserAdapter);
            CefFrameWrapper frameWrapper(frame, _browserAdapter);

            bool handled = handler->GetAuthCredentials(
                _browserControl, %browserWrapper, %frameWrapper, isProxy, 
                StringUtils::ToClr(host), port, StringUtils::ToClr(realm), 
                StringUtils::ToClr(scheme), usernameString, passwordString);

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
            CefFrameWrapper frameWrapper(frame, _browserAdapter);
            auto result = handler->OnBeforeContextMenu(_browserControl, %frameWrapper, contextMenuParamsWrapper);
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

            // NOTE: a popup handler for OnGotFocus doesn't make sense yet because
            // non-offscreen windows don't wrap popup browser's yet.
            if (!browser->IsPopup())
            {
                handler->OnGotFocus();
            }
        }

        bool ClientAdapter::OnSetFocus(CefRefPtr<CefBrowser> browser, FocusSource source)
        {
            IFocusHandler^ handler = _browserControl->FocusHandler;

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
            IFocusHandler^ handler = _browserControl->FocusHandler;

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

            auto dialogCallback = gcnew JsDialogCallback(callback);
            CefSharpBrowserWrapper iBrowser(browser, _browserAdapter);
            bool result = handler->OnJSDialog(_browserControl, %iBrowser,
                                       StringUtils::ToClr(origin_url), StringUtils::ToClr(accept_lang), (CefJsDialogType)dialog_type, 
                                       StringUtils::ToClr(message_text), StringUtils::ToClr(default_prompt_text), dialogCallback, suppress_message);

            return result;
        }

        bool ClientAdapter::OnBeforeUnloadDialog(CefRefPtr<CefBrowser> browser, const CefString& message_text, bool is_reload, CefRefPtr<CefJSDialogCallback> callback)
        {
            IJsDialogHandler^ handler = _browserControl->JsDialogHandler;

            if (handler == nullptr)
            {
                return false;
            }

            bool allowUnload;
            CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);
            auto handled = handler->OnJSBeforeUnload(_browserControl, %browserWrapper, StringUtils::ToClr(message_text), is_reload, allowUnload);
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
            CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);
            if (handler->OnFileDialog(_browserControl, %browserWrapper, (CefFileDialogMode)mode, StringUtils::ToClr(title), StringUtils::ToClr(default_file_path), StringUtils::ToClr(accept_filters), selected_accept_filter, filePaths))
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
            CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);

            return handler->OnDragEnter(_browserControl, %browserWrapper, dragDataWrapper, (CefSharp::DragOperationsMask)mask);
        }

        bool ClientAdapter::OnRequestGeolocationPermission(CefRefPtr<CefBrowser> browser, const CefString& requesting_url, int request_id, CefRefPtr<CefGeolocationCallback> callback)
        {
            IGeolocationHandler^ handler = _browserControl->GeolocationHandler;
            if (handler == nullptr)
            {
                // Default deny, as CEF does.
                return false;
            }

            CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);
            if (handler->OnRequestGeolocationPermission(_browserControl, %browserWrapper, StringUtils::ToClr(requesting_url), request_id))
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
                CefSharpBrowserWrapper browserWrapper(browser, _browserAdapter);
                handler->OnCancelGeolocationPermission(_browserControl, %browserWrapper, StringUtils::ToClr(requesting_url), request_id);
            }
        }
    }
}
