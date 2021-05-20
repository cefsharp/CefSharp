// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "ClientAdapter.h"

#include "include\cef_client.h"
#include "include\wrapper\cef_stream_resource_handler.h"

#include "CefAuthCallbackWrapper.h"
#include "CefBeforeDownloadCallbackWrapper.h"
#include "CefCertificateCallbackWrapper.h"
#include "CefContextMenuParamsWrapper.h"
#include "CefDownloadItemCallbackWrapper.h"
#include "DragData.h"
#include "CefFileDialogCallbackWrapper.h"
#include "CefFrameWrapper.h"
#include "CefJSDialogCallbackWrapper.h"
#include "CefMenuModelWrapper.h"
#include "Request.h"
#include "CefResourceRequestHandlerAdapter.h"
#include "CefRequestCallbackWrapper.h"
#include "CefRunContextMenuCallbackWrapper.h"
#include "CefSslInfoWrapper.h"
#include "CefBrowserWrapper.h"
#include "ManagedCefBrowserAdapter.h"
#include "Messaging\Messages.h"
#include "PopupFeatures.h"
#include "Serialization\Primitives.h"
#include "Serialization\V8Serialization.h"
#include "Serialization\JsObjectsSerialization.h"
#include "Serialization\ObjectsSerialization.h"
#include "TypeConversion.h"
#include "WindowInfo.h"

using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;
using namespace System::Security::Cryptography::X509Certificates;

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

                //IJavascriptCallbacks that are finalized after the browser has been Disposed
                //but before the IBrowserAdapter.IsDisposed is set might end up here
                //attempting to access _popupBrowsers which has been set to null already.
                auto popupBrowsers = _popupBrowsers;

                if (Object::ReferenceEquals(popupBrowsers, nullptr))
                {
                    return nullptr;
                }

                IBrowser^ popupBrowser;
                if (popupBrowsers->TryGetValue(browserId, popupBrowser))
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

            if (isPopup)
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
            CefRefPtr<CefClient>& client, CefBrowserSettings& settings, CefRefPtr<CefDictionaryValue>& extraInfo, bool* no_javascript_access)
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
                //newBrowser should never be set to _browserControl (I've seen user code where someone attepted to do this).
                //So throw exception to make that perfectly clear.
                if (Object::ReferenceEquals(_browserControl, newBrowser))
                {
                    throw gcnew Exception("newBrowser should be a new instance of ChromiumWebBrowser or null.");
                }

                //newBrowser is not null and result is true, whilst the documentation clearly states returning true will
                //cancel popup creation people keep expecting that newBrowser will do something which it won't
                if (result == true)
                {
                    throw gcnew Exception("returning true cancels popup creation, if you return true newBrowser should be set to null."
                        + "Previously no exception was thrown in this instance, this exception has been added to reduce the number of"
                        + " support requests from people returning true and setting newBrowser and expecting popups to work.");
                }

                auto newBrowserInternal = dynamic_cast<IWebBrowserInternal^>(newBrowser);

                if (newBrowserInternal != nullptr)
                {
                    //This should already be set using the SetAsPopup extension, just making doubly sure
                    newBrowserInternal->HasParent = true;

                    auto browserAdapter = dynamic_cast<ManagedCefBrowserAdapter^>(newBrowserInternal->BrowserAdapter);
                    if (browserAdapter != nullptr)
                    {
                        client = browserAdapter->GetClientAdapter().get();
                    }
                }
            }

            return result;
        }

        void ClientAdapter::OnAfterCreated(CefRefPtr<CefBrowser> browser)
        {
            BrowserRefCounter::Instance->Increment();

            auto browserWrapper = gcnew CefBrowserWrapper(browser);

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
                handler->OnAfterCreated(_browserControl, browserWrapper);
            }
        }

        bool ClientAdapter::DoClose(CefRefPtr<CefBrowser> browser)
        {
            auto handler = _browserControl->LifeSpanHandler;

            if (handler != nullptr)
            {
                //By this point it's possible IBrowser references have been disposed
                //Rather than attempting to rework the rather complex closing logic
                //It's easier to pass in a new wrapper and dispose it straight away
                CefBrowserWrapper browserWrapper(browser);

                return handler->DoClose(_browserControl, %browserWrapper);
            }

            return false;
        }

        void ClientAdapter::OnBeforeClose(CefRefPtr<CefBrowser> browser)
        {
            auto isPopup = browser->IsPopup() && !_browserControl->HasParent;
            auto handler = _browserControl->LifeSpanHandler;

            if (handler != nullptr)
            {
                //By this point it's possible IBrowser references have been disposed
                //Rather than attempting to rework the rather complex closing logic
                //It's easier to pass in a new wrapper and dispose it straight away
                CefBrowserWrapper browserWrapper(browser);

                handler->OnBeforeClose(_browserControl, %browserWrapper);
            }

            if (isPopup)
            {
                // Remove from the browser popup list.
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), true);
                _popupBrowsers->Remove(browser->GetIdentifier());
                // Dispose the CefBrowserWrapper
                delete browserWrapper;
            }
            //TODO: When creating a new ChromiumWebBrowser and passing in a newBrowser to OnBeforePopup
            //the handles don't match up (at least in WPF), need to investigate further.
            else if (_browserHwnd == browser->GetHost()->GetWindowHandle() || _browserControl->HasParent)
            {
                _cefBrowser = NULL;
            }

            BrowserRefCounter::Instance->Decrement();
        }

        void ClientAdapter::OnLoadingStateChange(CefRefPtr<CefBrowser> browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto args = gcnew LoadingStateChangedEventArgs(browserWrapper, canGoBack, canGoForward, isLoading);

            if (!browser->IsPopup() || _browserControl->HasParent)
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

            if (!browser->IsPopup() || _browserControl->HasParent)
            {
                _browserControl->SetAddress(args);
            }

            auto handler = _browserControl->DisplayHandler;

            if (handler != nullptr)
            {
                handler->OnAddressChanged(_browserControl, args);
            }
        }

        bool ClientAdapter::OnAutoResize(CefRefPtr<CefBrowser> browser, const CefSize& new_size)
        {
            auto handler = _browserControl->DisplayHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnAutoResize(_browserControl, browserWrapper, CefSharp::Structs::Size(new_size.width, new_size.height));
        }

        bool ClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor, cef_cursor_type_t type, const CefCursorInfo& custom_cursor_info)
        {
            auto handler = _browserControl->DisplayHandler;

            if (handler == nullptr)
            {
                InternalCursorChange(browser, cursor, type, custom_cursor_info);

                return false;
            }            

            CursorInfo customCursorInfo;

            //TODO: this is duplicated in RenderClientAdapter::InternalCursorChange
            //Only create the struct when we actually have a custom cursor
            if (type == cef_cursor_type_t::CT_CUSTOM)
            {
                Point hotspot = Point(custom_cursor_info.hotspot.x, custom_cursor_info.hotspot.y);
                Size size = Size(custom_cursor_info.size.width, custom_cursor_info.size.height);
                customCursorInfo = CursorInfo(IntPtr((void*)custom_cursor_info.buffer), hotspot, custom_cursor_info.image_scale_factor, size);
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            auto handled = handler->OnCursorChange(_browserControl, browserWrapper, (IntPtr)cursor, (CefSharp::Enums::CursorType)type, customCursorInfo);

            if (handled)
            {
                return true;
            }

            InternalCursorChange(browser, cursor, type, custom_cursor_info);

            return false;
        };

        void ClientAdapter::InternalCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor, cef_cursor_type_t type, const CefCursorInfo& custom_cursor_info)
        {

        }

        void ClientAdapter::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto args = gcnew TitleChangedEventArgs(browserWrapper, StringUtils::ToClr(title));

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

        void ClientAdapter::OnLoadingProgressChange(CefRefPtr<CefBrowser> browser, double progress)
        {
            auto handler = _browserControl->DisplayHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnLoadingProgressChange(_browserControl, browserWrapper, progress);
            }
        }

        bool ClientAdapter::OnTooltip(CefRefPtr<CefBrowser> browser, CefString& text)
        {
            auto tooltip = StringUtils::ToClr(text);
            bool hasChanged = tooltip != _tooltip;
            bool returnFlag = true;

            //NOTE: Only called if tooltip changed otherwise called many times
            // also only called when using OSR, https://bitbucket.org/chromiumembedded/cef/issues/783

            if (hasChanged)
            {
                auto handler = _browserControl->DisplayHandler;
                if (handler != nullptr)
                {
                    returnFlag = handler->OnTooltipChanged(_browserControl, tooltip);
                }

                if (!browser->IsPopup() || _browserControl->HasParent)
                {
                    _tooltip = tooltip;
                    _browserControl->SetTooltipText(_tooltip);
                }
            }

            return returnFlag;
        }

        bool ClientAdapter::OnConsoleMessage(CefRefPtr<CefBrowser> browser, cef_log_severity_t level, const CefString& message, const CefString& source, int line)
        {
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            auto args = gcnew ConsoleMessageEventArgs(browserWrapper, (LogSeverity)level, StringUtils::ToClr(message), StringUtils::ToClr(source), line);

            if (!browser->IsPopup() || _browserControl->HasParent)
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

            if (!browser->IsPopup() || _browserControl->HasParent)
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

            if (!browser->IsPopup() || _browserControl->HasParent)
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

            if (!browser->IsPopup() || _browserControl->HasParent)
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

            if (!browser->IsPopup() || _browserControl->HasParent)
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

        bool ClientAdapter::OnBeforeBrowse(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool userGesture, bool isRedirect)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);
            Request requestWrapper(request);

            return handler->OnBeforeBrowse(_browserControl, browserWrapper, %frameWrapper, %requestWrapper, userGesture, isRedirect);
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

        CefRefPtr<CefResourceRequestHandler> ClientAdapter::GetResourceRequestHandler(CefRefPtr<CefBrowser> browser,
            CefRefPtr<CefFrame> frame, CefRefPtr<CefRequest> request, bool isNavigation, bool isDownload, const CefString& requestInitiator, bool& disableDefaultHandling)
        {
            auto handler = _browserControl->RequestHandler;
            auto resourceRequestHandlerFactory = _browserControl->ResourceRequestHandlerFactory;

            //No handler and no factory, we'll just return null
            if (handler == nullptr && resourceRequestHandlerFactory == nullptr)
            {
                return NULL;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            CefFrameWrapper frameWrapper(frame);
            Request requestWrapper(request);

            if (handler != nullptr)
            {
                auto resourceRequestHandler = handler->GetResourceRequestHandler(_browserControl, browserWrapper, %frameWrapper,
                    %requestWrapper, isNavigation, isDownload, StringUtils::ToClr(requestInitiator), disableDefaultHandling);

                if (resourceRequestHandler != nullptr)
                {
                    return new CefResourceRequestHandlerAdapter(_browserControl, resourceRequestHandler);
                }
            }

            if (resourceRequestHandlerFactory != nullptr && resourceRequestHandlerFactory->HasHandlers)
            {
                auto factoryHandler = resourceRequestHandlerFactory->GetResourceRequestHandler(_browserControl, browserWrapper, %frameWrapper,
                    %requestWrapper, isNavigation, isDownload, StringUtils::ToClr(requestInitiator), disableDefaultHandling);

                if (factoryHandler != nullptr)
                {
                    return new CefResourceRequestHandlerAdapter(_browserControl, factoryHandler);
                }
            }

            return NULL;
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
            auto sslInfoWrapper = gcnew CefSslInfoWrapper(ssl_info);

            return handler->OnCertificateError(_browserControl, browserWrapper, (CefErrorCode)cert_error, StringUtils::ToClr(request_url), sslInfoWrapper, requestCallback);
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

        void ClientAdapter::OnDocumentAvailableInMainFrame(CefRefPtr<CefBrowser> browser)
        {
            auto handler = _browserControl->RequestHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnDocumentAvailableInMainFrame(_browserControl, browserWrapper);
            }
        }

        bool ClientAdapter::GetAuthCredentials(CefRefPtr<CefBrowser> browser, const CefString& originUrl, bool isProxy,
            const CefString& host, int port, const CefString& realm, const CefString& scheme, CefRefPtr<CefAuthCallback> callback)
        {
            if (isProxy && CefSharpSettings::Proxy != nullptr && CefSharpSettings::Proxy->IP == StringUtils::ToClr(host) && CefSharpSettings::Proxy->HasUsernameAndPassword())
            {
                callback->Continue(StringUtils::ToNative(CefSharpSettings::Proxy->Username), StringUtils::ToNative(CefSharpSettings::Proxy->Password));
                return true;
            }

            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto callbackWrapper = gcnew CefAuthCallbackWrapper(callback);

            return handler->GetAuthCredentials(
                _browserControl, browserWrapper, StringUtils::ToClr(originUrl), isProxy,
                StringUtils::ToClr(host), port, StringUtils::ToClr(realm),
                StringUtils::ToClr(scheme), callbackWrapper);
        }

        bool ClientAdapter::OnSelectClientCertificate(CefRefPtr<CefBrowser> browser, bool isProxy, const CefString& host,
            int port, const CefRequestHandler::X509CertificateList& certificates, CefRefPtr<CefSelectClientCertificateCallback> callback)
        {

            auto handler = _browserControl->RequestHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto callbackWrapper = gcnew CefCertificateCallbackWrapper(callback, certificates);

            auto list = gcnew X509Certificate2Collection();

            std::vector<CefRefPtr<CefX509Certificate> >::const_iterator it =
                certificates.begin();
            for (; it != certificates.end(); ++it)
            {
                auto bytes((*it)->GetDEREncoded());
                auto byteSize = bytes->GetSize();

                auto bufferByte = gcnew cli::array<Byte>(byteSize);
                pin_ptr<Byte> src = &bufferByte[0]; // pin pointer to first element in arr

                bytes->GetData(static_cast<void*>(src), byteSize, 0);
                auto cert = gcnew X509Certificate2(bufferByte);
                list->Add(cert);
            }

            return handler->OnSelectClientCertificate(
                _browserControl, browserWrapper, isProxy,
                StringUtils::ToClr(host), port, list, callbackWrapper);
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

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            handler->OnGotFocus(_browserControl, browserWrapper);
        }

        bool ClientAdapter::OnSetFocus(CefRefPtr<CefBrowser> browser, FocusSource source)
        {
            auto handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                // Allow the focus to be set by default.
                return false;
            }

            //For DevTools (which is hosted as a popup) OnSetFocus is called before OnAfterCreated so we don't
            // have a reference to the standard popup IBrowser wrapper, so we just pass a
            // short term reference.
            CefBrowserWrapper browserWrapper(browser);

            return handler->OnSetFocus(_browserControl, %browserWrapper, (CefFocusSource)source);
        }

        void ClientAdapter::OnTakeFocus(CefRefPtr<CefBrowser> browser, bool next)
        {
            auto handler = _browserControl->FocusHandler;

            if (handler == nullptr)
            {
                return;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            handler->OnTakeFocus(_browserControl, browserWrapper, next);
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

            return handler->OnBeforeUnloadDialog(_browserControl, browserWrapper, StringUtils::ToClr(message_text), is_reload, callbackWrapper);
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

            auto dialogMode = mode & FileDialogMode::FILE_DIALOG_TYPE_MASK;
            auto dialogFlags = mode & ~FileDialogMode::FILE_DIALOG_TYPE_MASK;

            return handler->OnFileDialog(
                _browserControl,
                browserWrapper,
                (CefFileDialogMode)dialogMode,
                (CefFileDialogFlags)dialogFlags,
                StringUtils::ToClr(title),
                StringUtils::ToClr(default_file_path),
                StringUtils::ToClr(accept_filters),
                selected_accept_filter,
                callbackWrapper);
        }

        bool ClientAdapter::OnDragEnter(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData, DragOperationsMask mask)
        {
            auto handler = _browserControl->DragHandler;

            if (handler == nullptr)
            {
                return false;
            }

            DragData dragDataWrapper(dragData);
            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

            return handler->OnDragEnter(_browserControl, browserWrapper, %dragDataWrapper, (CefSharp::Enums::DragOperationsMask)mask);
        }

        void ClientAdapter::OnDraggableRegionsChanged(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const std::vector<CefDraggableRegion>& regions)
        {
            auto handler = _browserControl->DragHandler;

            if (handler != nullptr)
            {
                auto regionsList = TypeConversion::FromNative(regions);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                CefFrameWrapper frameWrapper(frame);

                return handler->OnDraggableRegionsChanged(_browserControl, browserWrapper, %frameWrapper, regionsList);
            }
        }

        void ClientAdapter::OnBeforeDownload(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback)
        {
            auto handler = _browserControl->DownloadHandler;

            if (handler != nullptr)
            {
                auto downloadItem = TypeConversion::FromNative(download_item);
                downloadItem->SuggestedFileName = StringUtils::ToClr(suggested_name);

                auto callbackWrapper = gcnew CefBeforeDownloadCallbackWrapper(callback);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnBeforeDownload(_browserControl, browserWrapper, downloadItem, callbackWrapper);
            }
        };

        void ClientAdapter::OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
            CefRefPtr<CefDownloadItemCallback> callback)
        {
            auto handler = _browserControl->DownloadHandler;

            if (handler != nullptr)
            {
                auto callbackWrapper = gcnew CefDownloadItemCallbackWrapper(callback);
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnDownloadUpdated(_browserControl, browserWrapper, TypeConversion::FromNative(download_item), callbackWrapper);
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

        bool ClientAdapter::GetAudioParameters(CefRefPtr<CefBrowser> browser, CefAudioParameters & params)
        {
            auto handler = _browserControl->AudioHandler;

            if (handler == nullptr)
            {
                return false;
            }

            auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
            auto parameters = new AudioParameters((CefSharp::Enums::ChannelLayout)params.channel_layout, params.sample_rate, params.frames_per_buffer);

            auto result = handler->GetAudioParameters(_browserControl, browserWrapper, *parameters);

            if (result)
            {
                params.channel_layout = (cef_channel_layout_t)parameters->ChannelLayout;
                params.sample_rate = parameters->SampleRate;
                params.frames_per_buffer = parameters->FramesPerBuffer;
            }

            return result;
        }

        void ClientAdapter::OnAudioStreamStarted(CefRefPtr<CefBrowser> browser, const CefAudioParameters& params, int channels)
        {
            auto handler = _browserControl->AudioHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                AudioParameters parameters((CefSharp::Enums::ChannelLayout)params.channel_layout, params.sample_rate, params.frames_per_buffer);

                handler->OnAudioStreamStarted(_browserControl, browserWrapper, parameters, channels);
            }
        }

        void ClientAdapter::OnAudioStreamPacket(CefRefPtr<CefBrowser> browser, const float** data, int frames, int64 pts)
        {
            auto handler = _browserControl->AudioHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnAudioStreamPacket(_browserControl, browserWrapper, IntPtr((void *)data), frames, pts);
            }
        }

        void ClientAdapter::OnAudioStreamStopped(CefRefPtr<CefBrowser> browser)
        {
            auto handler = _browserControl->AudioHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnAudioStreamStopped(_browserControl, browserWrapper);
            }
        }

        void ClientAdapter::OnAudioStreamError(CefRefPtr<CefBrowser> browser, const CefString& message)
        {
            auto handler = _browserControl->AudioHandler;

            if (handler != nullptr)
            {
                auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());

                handler->OnAudioStreamError(_browserControl, browserWrapper, StringUtils::ToClr(message));
            }
        }

        bool ClientAdapter::OnProcessMessageReceived(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, CefProcessId source_process, CefRefPtr<CefProcessMessage> message)
        {
            if (_disposed)
            {
                return true;
            }

            auto handled = false;
            auto name = message->GetName();
            auto argList = message->GetArgumentList();

            //TODO: JSB Rename messages (remove Root from name)
            if (name == kJavascriptRootObjectRequest)
            {
                auto browserAdapter = _browserAdapter;
                if (Object::ReferenceEquals(browserAdapter, nullptr) || browserAdapter->IsDisposed)
                {
                    return true;
                }
                    
                auto objectRepository = browserAdapter->JavascriptObjectRepository;

                if (objectRepository == nullptr)
                {
                    return true;
                }

                auto callbackId = GetInt64(argList, 0);
                auto objectNames = argList->GetList(1);

                auto names = gcnew List<String^>(objectNames->GetSize());
                for (size_t i = 0; i < objectNames->GetSize(); i++)
                {
                    names->Add(StringUtils::ToClr(objectNames->GetString(i)));
                }

                //Call GetObjects with the list of names provided (will default to all if the list is empty
                //Previously we only sent a response if there were bound objects, now we always send
                //a response so the promise is resolved.
                auto objs = objectRepository->GetObjects(names);

                auto msg = CefProcessMessage::Create(kJavascriptRootObjectResponse);
                auto responseArgList = msg->GetArgumentList();
                SetInt64(responseArgList, 0, callbackId);
                SerializeJsObjects(objs, responseArgList, 1);
                frame->SendProcessMessage(CefProcessId::PID_RENDERER, msg);

                handled = true;
            }
            else if (name == kJavascriptObjectsBoundInJavascript)
            {
                auto browserAdapter = _browserAdapter;
                if (Object::ReferenceEquals(browserAdapter, nullptr) || browserAdapter->IsDisposed)
                {
                    return true;
                }

                auto objectRepository = browserAdapter->JavascriptObjectRepository;

                if (objectRepository == nullptr)
                {
                    return true;
                }

                auto boundObjects = argList->GetList(0);
                auto objs = gcnew List<Tuple<String^, bool, bool>^>(boundObjects->GetSize());
                for (size_t i = 0; i < boundObjects->GetSize(); i++)
                {
                    auto obj = boundObjects->GetDictionary(i);
                    auto objectName = obj->GetString("Name");
                    auto alreadyBound = obj->GetBool("AlreadyBound");
                    auto isCached = obj->GetBool("IsCached");

                    objs->Add(Tuple::Create(StringUtils::ToClr(objectName), alreadyBound, isCached));
                }

                objectRepository->ObjectsBound(objs);

                handled = true;
            }
            else if (name == kOnContextCreatedRequest)
            {
                //In certain circumstances the frame has already been destroyed by the time
                //we get here, only continue if we have a valid frame reference
                if (frame.get() && frame->IsValid())
                {
                    if (frame->IsMain())
                    {
                        _browserControl->SetCanExecuteJavascriptOnMainFrame(frame->GetIdentifier(), true);
                    }

                    auto handler = _browserControl->RenderProcessMessageHandler;

                    if (handler != nullptr)
                    {
                        auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                        CefFrameWrapper frameWrapper(frame);

                        handler->OnContextCreated(_browserControl, browserWrapper, %frameWrapper);
                    }
                }

                handled = true;
            }
            else if (name == kOnContextReleasedRequest)
            {
                //In certain circumstances the frame has already been destroyed by the time
                //we get here, only continue if we have a valid frame reference
                if (frame.get() && frame->IsValid())
                {
                    if (frame->IsMain())
                    {
                        _browserControl->SetCanExecuteJavascriptOnMainFrame(frame->GetIdentifier(), false);
                    }

                    auto handler = _browserControl->RenderProcessMessageHandler;

                    if (handler != nullptr)
                    {
                        auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                        CefFrameWrapper frameWrapper(frame);

                        handler->OnContextReleased(_browserControl, browserWrapper, %frameWrapper);
                    }
                }

                handled = true;
            }
            else if (name == kOnFocusedNodeChanged)
            {
                auto handler = _browserControl->RenderProcessMessageHandler;
                if (handler != nullptr)
                {
                    IDomNode^ node = nullptr;

                    // 0: is a node (bool)
                    // 1: tag name (string)
                    // 2: attributes (dictionary)
                    auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                    CefFrameWrapper frameWrapper(frame);

                    auto notEmpty = argList->GetBool(0);
                    if (notEmpty)
                    {
                        // Node information was passed from the render process.
                        auto tagName = StringUtils::ToClr(argList->GetString(1));
                        auto argAttributes = argList->GetDictionary(2);
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
            else if (name == kOnUncaughtException)
            {
                auto handler = _browserControl->RenderProcessMessageHandler;
                if (handler != nullptr)
                {
                    auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                    CefFrameWrapper frameWrapper(frame);

                    auto exception = gcnew JavascriptException();
                    exception->Message = StringUtils::ToClr(argList->GetString(0));

                    auto stackTrace = gcnew System::Collections::Generic::List<JavascriptStackFrame^>();

                    auto argFrames = argList->GetList(1);

                    for (auto i = 0; i < static_cast<int>(argFrames->GetSize()); i++)
                    {
                        auto argFrame = argFrames->GetList(i);

                        auto stackFrame = gcnew JavascriptStackFrame();
                        stackFrame->FunctionName = StringUtils::ToClr(argFrame->GetString(0));
                        stackFrame->LineNumber = argFrame->GetInt(1);
                        stackFrame->ColumnNumber = argFrame->GetInt(2);
                        stackFrame->SourceName = StringUtils::ToClr(argFrame->GetString(3));

                        stackTrace->Add(stackFrame);
                    }

                    exception->StackTrace = stackTrace->ToArray();

                    handler->OnUncaughtException(_browserControl, browserWrapper, %frameWrapper, exception);
                }
            }
            else if (name == kEvaluateJavascriptResponse || name == kJavascriptCallbackResponse)
            {
                auto browserAdapter = _browserAdapter;
                if (Object::ReferenceEquals(browserAdapter, nullptr) || browserAdapter->IsDisposed)
                {
                    return true;
                }

                auto callbackFactory = browserAdapter->JavascriptCallbackFactory;

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
            else if (name == kJavascriptAsyncMethodCallRequest)
            {
                auto browserAdapter = _browserAdapter;
                if (Object::ReferenceEquals(browserAdapter, nullptr) || browserAdapter->IsDisposed)
                {
                    return true;
                }

                auto callbackFactory = browserAdapter->JavascriptCallbackFactory;
                auto methodRunnerQueue = browserAdapter->MethodRunnerQueue;

                //Dispose is called on a different thread, so there's a chance
                //dispose is called after our IsDisposed checks, make sure we have
                //actual references.
                if (callbackFactory == nullptr || methodRunnerQueue == nullptr)
                {
                    return true;
                }

                auto frameId = frame->GetIdentifier();
                auto objectId = GetInt64(argList, 0);
                auto callbackId = GetInt64(argList, 1);
                auto methodName = StringUtils::ToClr(argList->GetString(2));
                auto arguments = argList->GetList(3);
                auto methodInvocation = gcnew MethodInvocation(browser->GetIdentifier(), frameId, objectId, methodName, (callbackId > 0 ? Nullable<int64>(callbackId) : Nullable<int64>()));
                for (auto i = 0; i < static_cast<int>(arguments->GetSize()); i++)
                {
                    methodInvocation->Parameters->Add(DeserializeObject(arguments, i, callbackFactory));
                }

                methodRunnerQueue->Enqueue(methodInvocation);

                handled = true;
            }
            else if (name == kJavascriptMessageReceived)
            {
                auto browserAdapter = _browserAdapter;
                if (Object::ReferenceEquals(browserAdapter, nullptr) || browserAdapter->IsDisposed)
                {
                    return true;
                }

                auto callbackFactory = browserAdapter->JavascriptCallbackFactory;
                //In certain circumstances the frame has already been destroyed by the time
                //we get here, only continue if we have a valid frame reference
                if (frame.get() && frame->IsValid())
                {
                    auto browserWrapper = GetBrowserWrapper(browser->GetIdentifier(), browser->IsPopup());
                    CefFrameWrapper frameWrapper(frame);

                    auto deserializedMessage = DeserializeObject(argList, 0, callbackFactory);

                    _browserControl->SetJavascriptMessageReceived(gcnew JavascriptMessageReceivedEventArgs(browserWrapper, %frameWrapper, deserializedMessage));
                }

                handled = true;
            }

            return handled;
        }

        PendingTaskRepository<JavascriptResponse^>^ ClientAdapter::GetPendingTaskRepository()
        {
            return _pendingTaskRepository;
        }

        void ClientAdapter::MethodInvocationComplete(MethodInvocationResult^ result)
        {
            auto browser = GetBrowserWrapper(result->BrowserId);

            if (result->CallbackId.HasValue && browser != nullptr && !browser->IsDisposed)
            {
                auto wrapper = static_cast<CefBrowserWrapper^>(browser);
                if (wrapper == nullptr)
                {
                    return;
                }

                auto cefBrowser = wrapper->Browser;

                if (cefBrowser.get())
                {
                    auto frame = cefBrowser->GetFrame(result->FrameId);

                    if (frame.get() && frame->IsValid())
                    {
                        auto message = CefProcessMessage::Create(kJavascriptAsyncMethodCallResponse);
                        auto argList = message->GetArgumentList();
                        SetInt64(argList, 0, result->CallbackId.Value);
                        argList->SetBool(1, result->Success);
                        if (result->Success)
                        {
                            SerializeV8Object(argList, 2, result->Result, result->NameConverter);
                        }
                        else
                        {
                            argList->SetString(2, StringUtils::ToNative(result->Message));
                        }

                        frame->SendProcessMessage(CefProcessId::PID_RENDERER, message);
                    }
                }
            }
        }
    }
}
