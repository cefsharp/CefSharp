// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "include\cef_client.h"
#include "include\wrapper\cef_stream_resource_handler.h"

#include "ClientAdapter.h"
#include "ManagedCefBrowserAdapter.h"
#include "CefRequestWrapper.h"
#include "CefResponseWrapper.h"
#include "CefContextMenuParamsWrapper.h"
#include "CefMenuModelWrapper.h"
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
#include "CefRunContextMenuCallbackWrapper.h"
#include "WindowInfo.h"
#include "Serialization\Primitives.h"
#include "Serialization\V8Serialization.h"
#include "Serialization\JsObjectsSerialization.h"
#include "Serialization\ObjectsSerialization.h"
#include "Messaging\Messages.h"
#include "CefResponseFilterAdapter.h"
#include "PopupFeatures.h"

using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    namespace Internals
    {
        IBrowser^ ClientAdapter::GetBrowserWrapper(int browserId)
        {
            if (_cefBrowser.get())
            {
                if (_cefBrowser->GetIdentifier() == browserId)
                {
                    return _browser;
                }

                IBrowser^ popupBrowser;
                if (_popupBrowsers->TryGetValue(browserId, popupBrowser))
                {
                    return popupBrowser;
                }
            }

            return nullptr;
        }

        IBrowser^ ClientAdapter::GetBrowserWrapper(int browserId, bool isPopup)
        {
            if (_browserControl->HasParent)
            {
                return _browser;
            }

            if(isPopup)
            {
                IBrowser^ popupBrowser;
                if (_popupBrowsers->TryGetValue(browserId, popupBrowser))
                {
                    return popupBrowser;
                }

                return nullptr;
            }

            return _browser;
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
            const CefString& target_frame_name, CefLifeSpanHandler::WindowOpenDisposition target_disposition, bool user_gesture,
            const CefPopupFeatures& popupFeatures, CefWindowInfo& windowInfo,
            CefRefPtr<CefClient>& client, CefBrowserSettings& settings, bool* no_javascript_access)
        {
            auto handler = _browserControl->LifeSpanHandler;
            
            if (handler == nullptr)
            {
                return false;
            }

            IWebBrowser^ newBrowser = nullptr;
            bool createdWrapper = false;
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            CefFrameWrapper frameWrapper(frame);
            PopupFeatures popupFeaturesWrapper(&popupFeatures);
            BrowserSettings browserSettingsWrapper(&settings);
            WindowInfo windowInfoWrapper(&windowInfo);

            auto result = handler->OnBeforePopup(
                _browserControl, browserWrapper,
                %frameWrapper, StringUtils::ToClr(target_url),
                StringUtils::ToClr(target_frame_name),
                (CefSharp::WindowOpenDisposition)target_disposition,
                user_gesture, %popupFeaturesWrapper,
                %windowInfoWrapper, %browserSettingsWrapper, *no_javascript_access, newBrowser);

            if (newBrowser != nullptr)
            {
                auto newBrowserInternal = dynamic_cast<IWebBrowserInternal^>(newBrowser);

                if (newBrowserInternal != nullptr)
                {
                    //This should already be set using the SetAsPopup extension, just making doubly sure
                    newBrowserInternal->HasParent = true;

                    auto browserAdapter = dynamic_cast<ManagedCefBrowserAdapter^>(newBrowserInternal->BrowserAdapter);
                    if (browserAdapter != nullptr)
                    {
                        client = browserAdapter->GetClientAdapter().get();

                        auto renderClientAdapter = dynamic_cast<RenderClientAdapter*>(client.get());
                        if (renderClientAdapter != nullptr)
                        {
                            renderClientAdapter->CreateBitmapInfo();
                        }
                    }
                }
            }

            return result;
        }

        void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
        {
            auto browserWrapper = gcnew CefSharpBrowserWrapper(browser);

            auto isPopup = browser->IsPopup() && !_browserControl->HasParent;

            if (isPopup)
            {
                // Add to the list of popup browsers.
                _popupBrowsers->Add(browser->GetIdentifier(), browserWrapper);
            }
            else
            {
                _browserHwnd = browser->GetHost()->GetWindowHandle();
                _cefBrowser = browser;

                _browser = browserWrapper;
                
                if (!Object::ReferenceEquals(_browserAdapter, nullptr))
                {
                    _browserAdapter->OnAfterBrowserCreated(browserWrapper);
                }
            }

            auto handler = _browserControl->LifeSpanHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), isPopup);

                handler->OnAfterCreated(_browserControl, browserWrapper);
            }
        }

        bool ClientAdapter::DoClose(CefRefPtr<CefBrowser> browser)
        {
            auto handler = _browserControl->LifeSpanHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                return handler->DoClose(_browserControl, browserWrapper);
            }

            return false;
        }

        void ClientAdapter::OnBeforeClose(CefRefPtr<CefBrowser> browser)
        {
            auto isPopup = browser->IsPopup() && !_browserControl->HasParent;
            auto handler = _browserControl->LifeSpanHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), isPopup);

                handler->OnBeforeClose(_browserControl, browserWrapper);
            }

            if (isPopup)
            {
                // Remove from the browser popup list.
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);
                _popupBrowsers->Remove(browser->GetIdentifier());
                // Dispose the CefSharpBrowserWrapper
                delete browserWrapper;
            }
            //TODO: When creating a new ChromiumWebBrowser and passing in a newBrowser to OnBeforePopup
            //the handles don't match up (at least in WPF), need to investigate further.
            else if (_browserHwnd == browser->GetHost()->GetWindowHandle() || _browserControl->HasParent)
            {
                _cefBrowser = NULL;
            }
        }

        void ClientAdapter::OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto args = gcnew LoadingStateChangedEventArgs(browserWrapper, canGoBack, canGoForward, isLoading);

            if (!browser->IsPopup())
            {
                _browserControl->SetLoadingStateChange(args);
            }

            auto handler = _browserControl->LoadHandler;

            if (handler != nullptr)
            {
                handler->OnLoadingStateChange(_browserControl, args);
            }
        }

        void ClientAdapter::OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& address)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto args = gcnew AddressChangedEventArgs(browserWrapper, StringUtils::ToClr(address));

            if (!browser->IsPopup())
            {
                _browserControl->SetAddress(args);
            }

            auto handler = _browserControl->DisplayHandler;

            if (handler != nullptr)
            {
                handler->OnAddressChanged(_browserControl, args);
            }
        }

        void ClientAdapter::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
        {
            auto args = gcnew TitleChangedEventArgs(StringUtils::ToClr(title));

            if (browser->IsPopup() && !_browserControl->HasParent)
            {
                // Set the popup window title
                auto hwnd = browser->GetHost()->GetWindowHandle();
                SetWindowText(hwnd, std::wstring(title).c_str());
            }
            else
            {
                _browserControl->SetTitle(args);
            }

            auto handler = _browserControl->DisplayHandler;

            if (handler != nullptr)
            {
                handler->OnTitleChanged(_browserControl, args);
            }
        }

        void ClientAdapter::OnFaviconURLChange(CefRefPtr<CefBrowser> browser, const std::vector<CefString>& iconUrls)
        {
            auto handler = _browserControl->DisplayHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnFaviconUrlChange(_browserControl, browserWrapper, StringUtils::ToClr(iconUrls));
            }
        }

        void ClientAdapter::OnFullscreenModeChange(CefRefPtr<CefBrowser> browser, bool fullscreen)
        {
            auto handler = _browserControl->DisplayHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnFullscreenModeChange(_browserControl, browserWrapper, fullscreen);
            }
        }

        bool ClientAdapter::OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text)
        {
            auto tooltip = StringUtils::ToClr(text);
            bool hasChanged = tooltip != _tooltip;

            //NOTE: Only called if tooltip changed otherwise called many times
            // also only appears to be called with OSR rendering at the moment
            if(hasChanged)
            {
                if (!browser->IsPopup())
                {
                    _tooltip = tooltip;
                    _browserControl->SetTooltipText(_tooltip);
                }

                auto handler = _browserControl->DisplayHandler;
                if (handler != nullptr)
                {
                    return handler->OnTooltipChanged(_browserControl, tooltip);
                }
            }

            return true;
        }

        bool ClientAdapter::OnConsoleMessage(CefRefPtr<CefBrowser> browser, const CefString& message, const CefString& source, int line)
        {
            auto args = gcnew ConsoleMessageEventArgs(StringUtils::ToClr(message), StringUtils::ToClr(source), line);

            if (!browser->IsPopup())
            {
                _browserControl->OnConsoleMessage(args);
            }

            auto handler = _browserControl->DisplayHandler;
            if (handler == nullptr)
            {
                return false;
            }

            return handler->OnConsoleMessage(_browserControl, args);
        }

        void ClientAdapter::OnStatusMessage(CefRefPtr<CefBrowser> browser, const CefString& value)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto args = gcnew StatusMessageEventArgs(browserWrapper, StringUtils::ToClr(value));

            if (!browser->IsPopup())
            {
                _browserControl->OnStatusMessage(args);
            }

            auto handler = _browserControl->DisplayHandler;
            if (handler != nullptr)
            {
                handler->OnStatusMessage(_browserControl, args);
            }
        }

        bool ClientAdapter::OnKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event)
        {
            auto handler = _browserControl->KeyboardHandler;

            if (handler == nullptr)
            {
                return false;
            }
            
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnKeyEvent(
                _browserControl, browserWrapper, (KeyType)event.type, event.windows_key_code, 
                event.native_key_code,
                (CefEventFlags)event.modifiers, event.is_system_key == 1);            
        }

        bool ClientAdapter::OnPreKeyEvent(CefRefPtr<CefBrowser> browser, const CefKeyEvent& event, CefEventHandle os_event, bool* is_keyboard_shortcut)
        {
            auto handler = _browserControl->KeyboardHandler;

            if (handler == nullptr)
            {
                return false;
            }            

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnPreKeyEvent(
                _browserControl, browserWrapper, (KeyType)event.type, event.windows_key_code,
                event.native_key_code, (CefEventFlags)event.modifiers, event.is_system_key == 1,
                *is_keyboard_shortcut);
        }

        void ClientAdapter::OnLoadStart(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, TransitionType transitionType)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);

            if (!browser->IsPopup())
            {
                _browserControl->OnFrameLoadStart(gcnew FrameLoadStartEventArgs(browserWrapper, %frameWrapper, (CefSharp::TransitionType)transitionType));
            }

            auto handler = _browserControl->LoadHandler;
            if (handler != nullptr)
            {
                handler->OnFrameLoadStart(_browserControl, gcnew FrameLoadStartEventArgs(browserWrapper, %frameWrapper, (CefSharp::TransitionType)transitionType));
            }
        }

        void ClientAdapter::OnLoadEnd(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, int httpStatusCode)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);

            if (!browser->IsPopup())
            {
                _browserControl->OnFrameLoadEnd(gcnew FrameLoadEndEventArgs(browserWrapper, %frameWrapper, httpStatusCode));
            }

            auto handler = _browserControl->LoadHandler;

            if (handler != nullptr)
            {
                handler->OnFrameLoadEnd(_browserControl, gcnew FrameLoadEndEventArgs(browserWrapper, %frameWrapper, httpStatusCode));
            }
        }

        void ClientAdapter::OnLoadError(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, ErrorCode errorCode, const CefString& errorText, const CefString& failedUrl)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);

            if (!browser->IsPopup())
            {
                _browserControl->OnLoadError(gcnew LoadErrorEventArgs(browserWrapper, %frameWrapper,
                    (CefErrorCode)errorCode, StringUtils::ToClr(errorText), StringUtils::ToClr(failedUrl)));
            }

            auto handler = _browserControl->LoadHandler;

            if (handler != nullptr)
            {
                handler->OnLoadError(_browserControl,
                    gcnew LoadErrorEventArgs(browserWrapper, %frameWrapper, (CefErrorCode)errorCode, StringUtils::ToClr(errorText), StringUtils::ToClr(failedUrl)));
            }
        }

        bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isRedirect)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }           

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);
            CefRequestWrapper requestWrapper(request);

            return handler->OnBeforeBrowse(_browserControl, browserWrapper, %frameWrapper, %requestWrapper, isRedirect);
        }

        bool ClientAdapter::OnOpenURLFromTab(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& targetUrl,
            CefRequestHandler::WindowOpenDisposition targetDisposition, bool userGesture)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);

            return handler->OnOpenUrlFromTab(_browserControl, browserWrapper, %frameWrapper, StringUtils::ToClr(targetUrl), (CefSharp::WindowOpenDisposition)targetDisposition, userGesture);
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
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnCertificateError(_browserControl, browserWrapper, (CefErrorCode)cert_error, StringUtils::ToClr(request_url), nullptr, requestCallback);
        }

        bool ClientAdapter::OnQuotaRequest(CefRefPtr<CefBrowser> browser, const CefString& originUrl, int64 newSize, CefRefPtr<CefRequestCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }
            
            auto requestCallback = gcnew CefRequestCallbackWrapper(callback);
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnQuotaRequest(_browserControl, browserWrapper, StringUtils::ToClr(originUrl), newSize, requestCallback);
        }

        void ClientAdapter::OnPluginCrashed(CefRefPtr<CefBrowser> browser, const CefString& plugin_path)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnPluginCrashed(_browserControl, browserWrapper, StringUtils::ToClr(plugin_path));
            }
        }

        void ClientAdapter::OnRenderViewReady(CefRefPtr<CefBrowser> browser)
        {
            if (!Object::ReferenceEquals(_browserAdapter, nullptr) && !_browserAdapter->IsDisposed && !browser->IsPopup())
            {
                auto objectRepository = _browserAdapter->JavascriptObjectRepository;

                if (objectRepository->HasBoundObjects)
                {
                    //transmit async bound objects
                    auto jsRootObjectMessage = CefProcessMessage::Create(kJavascriptRootObjectRequest);
                    auto argList = jsRootObjectMessage->GetArgumentList();
                    SerializeJsObject(objectRepository->AsyncRootObject, argList, 0);
                    SerializeJsObject(objectRepository->RootObject, argList, 1);
                    browser->SendProcessMessage(CefProcessId::PID_RENDERER, jsRootObjectMessage);
                }
            }

            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnRenderViewReady(_browserControl, browserWrapper);
            }
        }

        void ClientAdapter::OnRenderProcessTerminated(CefRefPtr<CefBrowser> browser, TerminationStatus status)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnRenderProcessTerminated(_browserControl, browserWrapper, (CefTerminationStatus)status);
            }
        }

        void ClientAdapter::OnResourceRedirect(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefString& newUrl)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                auto managedNewUrl = StringUtils::ToClr(newUrl);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                CefFrameWrapper frameWrapper(frame);
                CefRequestWrapper requestWrapper(request);

                handler->OnResourceRedirect(_browserControl, browserWrapper, %frameWrapper, %requestWrapper, managedNewUrl);

                newUrl = StringUtils::ToNative(managedNewUrl);
            }
        }

        bool ClientAdapter::OnResourceResponse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }
                
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);
            CefRequestWrapper requestWrapper(request);
            CefResponseWrapper responseWrapper(response);

            return handler->OnResourceResponse(_browserControl, browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);
        }

        CefRefPtr<CefResponseFilter> ClientAdapter::GetResourceResponseFilter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return NULL;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);
            CefRequestWrapper requestWrapper(request);
            CefResponseWrapper responseWrapper(response);

            auto filter = handler->GetResourceResponseFilter(_browserControl, browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper);

            if (filter == nullptr)
            {
                return NULL;
            }

            return new CefResponseFilterAdapter(filter);
        }

        void ClientAdapter::OnResourceLoadComplete(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefResponse> response, URLRequestStatus status, int64 receivedContentLength)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                CefFrameWrapper frameWrapper(frame);
                CefRequestWrapper requestWrapper(request);
                CefResponseWrapper responseWrapper(response);

                handler->OnResourceLoadComplete(_browserControl, browserWrapper, %frameWrapper, %requestWrapper, %responseWrapper, (UrlRequestStatus)status, receivedContentLength);
            }
        }

        void ClientAdapter::OnProtocolExecution(CefRefPtr<CefBrowser> browser, const CefString& url, bool& allowOSExecution)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

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

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);
            CefRequestWrapper requestWrapper(request);

            auto handler = factory->GetResourceHandler(_browserControl, browserWrapper, %frameWrapper, %requestWrapper);

            if (handler == nullptr)
            {
                return NULL;
            }

            if (handler->GetType() == ResourceHandler::typeid)
            {
                auto resourceHandler = static_cast<ResourceHandler^>(handler);
                if (resourceHandler->Type == ResourceHandlerType::File)
                {
                    return new CefStreamResourceHandler(StringUtils::ToNative(resourceHandler->MimeType), CefStreamReader::CreateForFile(StringUtils::ToNative(resourceHandler->FilePath)));
                }
            }

            return new ResourceHandlerWrapper(handler);
        }

        cef_return_value_t ClientAdapter::OnBeforeResourceLoad(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, CefRefPtr<CefRequestCallback> callback)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return cef_return_value_t::RV_CONTINUE;
            }            

            auto frameWrapper = gcnew CefFrameWrapper(frame);
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto requestWrapper = gcnew CefRequestWrapper(request);
            auto requestCallback = gcnew CefRequestCallbackWrapper(callback, frameWrapper, requestWrapper);

            return (cef_return_value_t)handler->OnBeforeResourceLoad(_browserControl, browserWrapper, frameWrapper, requestWrapper, requestCallback);
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
            auto frameWrapper = gcnew CefFrameWrapper(frame);
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
            
            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                CefFrameWrapper frameWrapper(frame);
                CefContextMenuParamsWrapper contextMenuParamsWrapper(params);
                CefMenuModelWrapper menuModelWrapper(model);
                
                handler->OnBeforeContextMenu(_browserControl, browserWrapper, %frameWrapper, %contextMenuParamsWrapper, %menuModelWrapper);
            }           
        }

        bool ClientAdapter::OnContextMenuCommand(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame,
                CefRefPtr<CefContextMenuParams> params, int commandId, EventFlags eventFlags)
        {
            auto handler = _browserControl->MenuHandler;
            
            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                CefFrameWrapper frameWrapper(frame);
                CefContextMenuParamsWrapper contextMenuParamsWrapper(params);
                
                return handler->OnContextMenuCommand(_browserControl, browserWrapper, %frameWrapper, %contextMenuParamsWrapper,
                    (CefMenuCommand)commandId, (CefEventFlags)eventFlags);
            }

            return false;
        }
        
        void ClientAdapter::OnContextMenuDismissed(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame)
        {
            auto handler = _browserControl->MenuHandler;
            
            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                CefFrameWrapper frameWrapper(frame);
                
                handler->OnContextMenuDismissed(_browserControl, browserWrapper, %frameWrapper);
            }
        }

        bool ClientAdapter::RunContextMenu(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefContextMenuParams> params, CefRefPtr<CefMenuModel> model, CefRefPtr<CefRunContextMenuCallback> callback)
        {
            auto handler = _browserControl->MenuHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);
            CefContextMenuParamsWrapper contextMenuParamsWrapper(params);
            CefMenuModelWrapper menuModelWrapper(model);

            auto callbackWrapper = gcnew CefRunContextMenuCallbackWrapper(callback);

            return handler->RunContextMenu(_browserControl, browserWrapper, %frameWrapper, %contextMenuParamsWrapper, %menuModelWrapper, callbackWrapper);
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

        bool ClientAdapter::OnJSDialog(CefRefPtr<CefBrowser> browser, const CefString& origin_url,
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
                                       StringUtils::ToClr(origin_url), (CefJsDialogType)dialog_type, 
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

        void ClientAdapter::OnResetDialogState(CefRefPtr<CefBrowser> browser)
        {
            auto handler = _browserControl->JsDialogHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnResetDialogState(_browserControl, browserWrapper);
            }
        }

        void ClientAdapter::OnDialogClosed(CefRefPtr<CefBrowser> browser)
        {
            auto handler = _browserControl->JsDialogHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnDialogClosed(_browserControl, browserWrapper);
            }
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

        void ClientAdapter::OnDraggableRegionsChanged(CefRefPtr<CefBrowser> browser, const std::vector<CefDraggableRegion>& regions)
        {
            auto handler = _browserControl->DragHandler;

            if (handler != nullptr)
            {
                auto regionsList = TypeConversion::FromNative(regions);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                return handler->OnDraggableRegionsChanged(_browserControl, browserWrapper, regionsList);
            }
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

        void ClientAdapter::OnCancelGeolocationPermission(CefRefPtr<CefBrowser> browser, int request_id)
        {
            auto handler = _browserControl->GeolocationHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                handler->OnCancelGeolocationPermission(_browserControl, browserWrapper, request_id);
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

        void ClientAdapter::OnFindResult(CefRefPtr<CefBrowser> browser, int identifier, int count, const CefRect& selectionRect, int activeMatchOrdinal, bool finalUpdate)
        {
            auto handler = _browserControl->FindHandler;

            if (handler != nullptr)
            {
                auto rect = Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnFindResult(_browserControl, browserWrapper, identifier, count, rect, activeMatchOrdinal, finalUpdate);
            }
        }

        bool ClientAdapter::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefProcessId source_process, CefRefPtr<CefProcessMessage> message)
        {
            auto handled = false;
            auto name = message->GetName();
            auto argList = message->GetArgumentList();
            IJavascriptCallbackFactory^ callbackFactory = _browserAdapter->JavascriptCallbackFactory;

            if (name == kOnContextCreatedRequest)
            {
                auto handler = _browserControl->RenderProcessMessageHandler;

                if (handler != nullptr)
                {
                    auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                    CefFrameWrapper frameWrapper(browser->GetFrame(GetInt64(argList, 0)));

                    handler->OnContextCreated(_browserControl, browserWrapper, %frameWrapper);
                }

                handled = true;
            }
            else if (name == kOnFocusedNodeChanged)
            {
                auto handler = _browserControl->RenderProcessMessageHandler;
                if (handler != nullptr)
                {
                    IDomNode^ node = nullptr;

                    // 0: frame ID (int 64)
                    // 1: is a node (bool)
                    // 2: tag name (string)
                    // 3: attributes (dictionary)
                    auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                    CefFrameWrapper frameWrapper(browser->GetFrame(GetInt64(argList, 0)));
                    
                    auto notEmpty = argList->GetBool(1);
                    if (notEmpty)
                    {
                        // Node information was passed from the render process.
                        auto tagName = StringUtils::ToClr(argList->GetString(2));
                        auto argAttributes = argList->GetDictionary(3);
                        auto attributes = gcnew System::Collections::Generic::Dictionary<String^, String^>();
                        CefDictionaryValue::KeyList keys;
                        argAttributes->GetKeys(keys);
                        for (auto key : keys)
                        {
                            attributes->Add(StringUtils::ToClr(key), StringUtils::ToClr(argAttributes->GetString(key)));
                        }

                        node = gcnew DomNode(tagName, attributes);
                    }

                    // DomNode will be empty if input focus was cleared
                    handler->OnFocusedNodeChanged(_browserControl, browserWrapper, %frameWrapper, node);
                }
            }
            else if (name == kEvaluateJavascriptResponse || name == kJavascriptCallbackResponse)
            {
                auto success = argList->GetBool(0);
                auto callbackId = GetInt64(argList, 1);

                auto pendingTask = _pendingTaskRepository->RemovePendingTask(callbackId);
                if (pendingTask != nullptr)
                {
                    auto response = gcnew JavascriptResponse();
                    response->Success = success;

                    if (success)
                    {
                        response->Result = DeserializeObject(argList, 2, callbackFactory);
                    }
                    else
                    {
                        response->Message = StringUtils::ToClr(argList->GetString(2));
                    }

                    CefSharp::Internals::TaskExtensions::TrySetResultAsync<JavascriptResponse^>(pendingTask, response);
                }

                handled = true;
            }
            else if (name == kJavascriptAsyncMethodCallRequest && !_browserAdapter->IsDisposed)
            {
                auto frameId = GetInt64(argList, 0);
                auto objectId = GetInt64(argList, 1);
                auto callbackId = GetInt64(argList, 2);
                auto methodName = StringUtils::ToClr(argList->GetString(3));
                auto arguments = argList->GetList(4);
                auto methodInvocation = gcnew MethodInvocation(browser->GetIdentifier(), frameId, objectId, methodName, (callbackId > 0 ? Nullable<int64>(callbackId) : Nullable<int64>()));
                for (auto i = 0; i < static_cast<int>(arguments->GetSize()); i++)
                {
                    methodInvocation->Parameters->Add(DeserializeObject(arguments, i, callbackFactory));
                }

                _browserAdapter->MethodRunnerQueue->Enqueue(methodInvocation);

                handled = true;
            }

            return handled;
        }

        Task<JavascriptResponse^>^ ClientAdapter::EvaluateScriptAsync(int browserId, bool isBrowserPopup, int64 frameId, String^ script, Nullable<TimeSpan> timeout)
        {
            auto browser = GetBrowserWrapper(browserId, isBrowserPopup);

            //If we're unable to get the underlying browser wrapper then return null
            if (browser == nullptr)
            {
                return nullptr;
            }

            auto browserWrapper = static_cast<CefSharpBrowserWrapper^>(browser);

            //create a new taskcompletionsource
            auto idAndComplectionSource = _pendingTaskRepository->CreatePendingTask(timeout);

            auto message = CefProcessMessage::Create(kEvaluateJavascriptRequest);
            auto argList = message->GetArgumentList();
            SetInt64(argList, 0, frameId);
            SetInt64(argList, 1, idAndComplectionSource.Key);
            argList->SetString(2, StringUtils::ToNative(script));

            

            browserWrapper->SendProcessMessage(CefProcessId::PID_RENDERER, message);

            return idAndComplectionSource.Value->Task;
        }

        PendingTaskRepository<JavascriptResponse^>^ ClientAdapter::GetPendingTaskRepository()
        {
            return _pendingTaskRepository;
        }

        void ClientAdapter::MethodInvocationComplete(MethodInvocationResult^ result)
        {
            if (result->CallbackId.HasValue)
            {
                auto message = CefProcessMessage::Create(kJavascriptAsyncMethodCallResponse);
                auto argList = message->GetArgumentList();
                SetInt64(argList, 0, result->FrameId);
                SetInt64(argList, 1, result->CallbackId.Value);
                argList->SetBool(2, result->Success);
                if (result->Success)
                {
                    SerializeV8Object(argList, 3, result->Result);
                }
                else
                {
                    argList->SetString(3, StringUtils::ToNative(result->Message));
                }

                auto browser = GetBrowserWrapper(result->BrowserId);

                if (browser != nullptr)
                {
                    auto wrapper = static_cast<CefSharpBrowserWrapper^>(browser);

                    wrapper->Browser->SendProcessMessage(CefProcessId::PID_RENDERER, message);
                }
            }
        }
    }
}
