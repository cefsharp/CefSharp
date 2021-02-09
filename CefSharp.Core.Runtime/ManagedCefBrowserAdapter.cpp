// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "ManagedCefBrowserAdapter.h"
#include "WindowInfo.h"
#include "Internals\Messaging\Messages.h"
#include "Internals\CefFrameWrapper.h"
#include "Internals\CefBrowserWrapper.h"
#include "Internals\Serialization\Primitives.h"
#include "Internals\Serialization\JsObjectsSerialization.h"

using namespace CefSharp::Internals::Serialization;
using namespace CefSharp::Internals::Messaging;

namespace CefSharp
{
    namespace Core
    {

        bool ManagedCefBrowserAdapter::IsDisposed::get()
        {
            return _isDisposed;
        }

        void ManagedCefBrowserAdapter::CreateBrowser(IWindowInfo^ windowInfo, IBrowserSettings^ browserSettings, IRequestContext^ requestContext, String^ address)
        {
            CefString addressNative = StringUtils::ToNative(address);

            if (browserSettings == nullptr)
            {
                throw gcnew ArgumentNullException("browserSettings", "cannot be null");
            }

            if (browserSettings->IsDisposed)
            {
                throw gcnew ObjectDisposedException("browserSettings", "browser settings has already been disposed. " +
                    "BrowserSettings created by CefSharp are automatically disposed, to control the lifecycle create and set your own instance.");
            }

            auto objectRepository = _javaScriptObjectRepository;

            //It's no longer possible to change these settings
            objectRepository->Settings->Freeze();

            CefRefPtr<CefDictionaryValue> extraInfo = CefDictionaryValue::Create();
            auto legacyBindingEnabled = false;

            if (objectRepository->Settings->LegacyBindingEnabled)
            {
                auto legacyBoundObjects = objectRepository->GetLegacyBoundObjects();

                legacyBindingEnabled = objectRepository->HasBoundObjects;

                //For legacy binding we only add values if we have bond objects.
                if (legacyBindingEnabled)
                {
                    auto listValue = CefListValue::Create();

                    SerializeJsObjects(legacyBoundObjects, listValue, 0);

                    extraInfo->SetList("LegacyBindingObjects", listValue);
                }
            }

            extraInfo->SetBool("LegacyBindingEnabled", legacyBindingEnabled);

            if (!String::IsNullOrEmpty(objectRepository->Settings->JavascriptBindingApiGlobalObjectName))
            {
                auto globalObjName = objectRepository->Settings->JavascriptBindingApiGlobalObjectName;

                if (StringCheck::IsFirstCharacterLowercase(globalObjName))
                {
                    extraInfo->SetString("JsBindingPropertyNameCamelCase", StringUtils::ToNative(globalObjName));
                }
                else
                {
                    extraInfo->SetString("JsBindingPropertyName", StringUtils::ToNative(globalObjName));
                }
            }

            CefRefPtr<CefRequestContext> requestCtx;

            if (requestContext != nullptr)
            {
                auto managedRequestCtx = (RequestContext^)requestContext->UnWrap();
                requestCtx = static_cast<CefRefPtr<CefRequestContext>>(managedRequestCtx);
            }

            auto winInfo = (WindowInfo^)windowInfo->UnWrap();
            auto bSettings = (BrowserSettings^)browserSettings->UnWrap();

            if (!CefBrowserHost::CreateBrowser(*winInfo->GetWindowInfo(), _clientAdapter.get(), addressNative,
                *bSettings->_browserSettings, extraInfo, requestCtx))
            {
                throw gcnew InvalidOperationException("CefBrowserHost::CreateBrowser call failed, review the CEF log file for more details.");
            }

            delete windowInfo;
        }

#ifndef NETCOREAPP
        // NOTE: This was moved out of OnAfterBrowserCreated to prevent the System.ServiceModel assembly from being loaded when WCF is not enabled.
        __declspec(noinline) void ManagedCefBrowserAdapter::InitializeBrowserProcessServiceHost(IBrowser^ browser)
        {
            _browserProcessServiceHost = gcnew BrowserProcessServiceHost(_javaScriptObjectRepository, Process::GetCurrentProcess()->Id, browser->Identifier, _javascriptCallbackFactory);
            //NOTE: Attempt to solve timing issue where browser is opened and rapidly disposed. In some cases a call to Open throws
            // an exception about the process already being closed. Two relevant issues are #862 and #804.
            if (_browserProcessServiceHost->State == CommunicationState::Created)
            {
                try
                {
                    _browserProcessServiceHost->Open();
                }
                catch (Exception^)
                {
                    //Ignore exception as it's likely cause when the browser is closing
                }
            }
        }

        // NOTE: This was moved out of ~ManagedCefBrowserAdapter to prevent the System.ServiceModel assembly from being loaded when WCF is not enabled.
        __declspec(noinline) void ManagedCefBrowserAdapter::DisposeBrowserProcessServiceHost()
        {
            if (_browserProcessServiceHost != nullptr)
            {
                if (CefSharpSettings::WcfTimeout > TimeSpan::Zero)
                {
                    _browserProcessServiceHost->Close(CefSharpSettings::WcfTimeout);
                }
                else
                {
                    _browserProcessServiceHost->Abort();
                }
                _browserProcessServiceHost = nullptr;
            }
        }
#endif

        void ManagedCefBrowserAdapter::OnAfterBrowserCreated(IBrowser^ browser)
        {
            if (!_isDisposed)
            {
                _browserWrapper = browser;
                _javascriptCallbackFactory->BrowserAdapter = gcnew WeakReference<IBrowserAdapter^>(this);

                //Browser has been initialized, it's now too late to register a sync JSB object if Wcf wasn't enabled
                _javaScriptObjectRepository->IsBrowserInitialized = true;

#ifndef NETCOREAPP
                if (CefSharpSettings::WcfEnabled)
                {
                    InitializeBrowserProcessServiceHost(browser);
                }
#endif

                if (_webBrowserInternal != nullptr)
                {
                    _webBrowserInternal->OnAfterBrowserCreated(browser);
                }
            }
        }

        void ManagedCefBrowserAdapter::Resize(int width, int height)
        {
            HWND browserHwnd = _clientAdapter->GetBrowserHwnd();
            if (browserHwnd)
            {
                if (width == 0 && height == 0)
                {
                    // For windowed browsers when the frame window is minimized set the
                    // browser window size to 0x0 to reduce resource usage.
                    SetWindowPos(browserHwnd, NULL, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOMOVE | SWP_NOACTIVATE);
                }
                else
                {
                    SetWindowPos(browserHwnd, NULL, 0, 0, width, height, SWP_NOZORDER);
                }
            }
        }

        IBrowser^ ManagedCefBrowserAdapter::GetBrowser(int browserId)
        {
            return _clientAdapter->GetBrowserWrapper(browserId);
        }

        IJavascriptCallbackFactory^ ManagedCefBrowserAdapter::JavascriptCallbackFactory::get()
        {
            return _javascriptCallbackFactory;
        }

        IJavascriptObjectRepositoryInternal^ ManagedCefBrowserAdapter::JavascriptObjectRepository::get()
        {
            return _javaScriptObjectRepository;
        }

        IMethodRunnerQueue^ ManagedCefBrowserAdapter::MethodRunnerQueue::get()
        {
            return _methodRunnerQueue;
        }

        void ManagedCefBrowserAdapter::MethodInvocationComplete(Object^ sender, MethodInvocationCompleteArgs^ e)
        {
            auto result = e->Result;
            if (result->CallbackId.HasValue)
            {
                _clientAdapter->MethodInvocationComplete(result);
            }
        }

        MCefRefPtr<ClientAdapter> ManagedCefBrowserAdapter::GetClientAdapter()
        {
            return _clientAdapter;
        }
    }
}
