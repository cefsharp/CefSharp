// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#ifndef CEFSHARP_CORE_MANAGEDCEFBROWSERADAPTER_H_
#define CEFSHARP_CORE_MANAGEDCEFBROWSERADAPTER_H_

#pragma once

#include "Stdafx.h"

#include "include\cef_client.h"

#include "BrowserSettings.h"
#include "RequestContext.h"
#include "Internals/ClientAdapter.h"
#include "Internals/CefDragDataWrapper.h"
#include "Internals/RenderClientAdapter.h"
#include "Internals/JavascriptCallbackFactory.h"

using namespace System::Diagnostics;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;
using namespace CefSharp::ModelBinding;

namespace CefSharp
{
    /// <exclude />
    public ref class ManagedCefBrowserAdapter : public IBrowserAdapter
    {
        MCefRefPtr<ClientAdapter> _clientAdapter;
        BrowserProcessServiceHost^ _browserProcessServiceHost;
        IWebBrowserInternal^ _webBrowserInternal;
        JavascriptObjectRepository^ _javaScriptObjectRepository;
        JavascriptCallbackFactory^ _javascriptCallbackFactory;
        IMethodRunnerQueue^ _methodRunnerQueue;
        IBrowser^ _browserWrapper;
        bool _isDisposed;

    private:
        void MethodInvocationComplete(Object^ sender, MethodInvocationCompleteArgs^ e);
        void InitializeBrowserProcessServiceHost(IBrowser^ browser);
        void DisposeBrowserProcessServiceHost();

    internal:
        MCefRefPtr<ClientAdapter> GetClientAdapter();

    public:
        ManagedCefBrowserAdapter(IWebBrowserInternal^ webBrowserInternal, bool offScreenRendering)
            : _isDisposed(false)
        {
            if (offScreenRendering)
            {
                _clientAdapter = new RenderClientAdapter(webBrowserInternal, this);
            }
            else
            {
                _clientAdapter = new ClientAdapter(webBrowserInternal, this);
            }

            _webBrowserInternal = webBrowserInternal;
            _javaScriptObjectRepository = gcnew CefSharp::Internals::JavascriptObjectRepository();
            _javascriptCallbackFactory = gcnew CefSharp::Internals::JavascriptCallbackFactory(_clientAdapter->GetPendingTaskRepository());

            if (CefSharpSettings::ConcurrentTaskExecution)
            {
                _methodRunnerQueue = gcnew CefSharp::Internals::ConcurrentMethodRunnerQueue(_javaScriptObjectRepository);
            }
            else
            {
                _methodRunnerQueue = gcnew CefSharp::Internals::MethodRunnerQueue(_javaScriptObjectRepository);
            }

            _methodRunnerQueue->MethodInvocationComplete += gcnew EventHandler<MethodInvocationCompleteArgs^>(this, &ManagedCefBrowserAdapter::MethodInvocationComplete);
        }

        !ManagedCefBrowserAdapter()
        {
            _clientAdapter = nullptr;
        }

        ~ManagedCefBrowserAdapter()
        {
            _isDisposed = true;

            // Stop the method runner before releasing browser adapter and browser wrapper (#2529)
            if (_methodRunnerQueue != nullptr)
            {
                _methodRunnerQueue->MethodInvocationComplete -= gcnew EventHandler<MethodInvocationCompleteArgs^>(this, &ManagedCefBrowserAdapter::MethodInvocationComplete);
                delete _methodRunnerQueue;
                _methodRunnerQueue = nullptr;
            }

            // Release the MCefRefPtr<ClientAdapter> reference
            // before calling _browserWrapper->CloseBrowser(true)
            this->!ManagedCefBrowserAdapter();

            if (_browserWrapper != nullptr)
            {
                _browserWrapper->CloseBrowser(true);

                delete _browserWrapper;
                _browserWrapper = nullptr;
            }

            if (CefSharpSettings::WcfEnabled)
            {
                DisposeBrowserProcessServiceHost();
            }

            _webBrowserInternal = nullptr;
            delete _javaScriptObjectRepository;
            _javaScriptObjectRepository = nullptr;
        }

        virtual property bool IsDisposed
        {
            bool get();
        }

        virtual void OnAfterBrowserCreated(IBrowser^ browser);
        void CreateBrowser(IWindowInfo^ windowInfo, BrowserSettings^ browserSettings, RequestContext^ requestContext, String^ address);
        virtual void Resize(int width, int height);

        virtual IBrowser^ GetBrowser(int browserId);

        virtual property IJavascriptCallbackFactory^ JavascriptCallbackFactory
        {
            CefSharp::Internals::IJavascriptCallbackFactory^ get();
        }

        virtual property JavascriptObjectRepository^ JavascriptObjectRepository
        {
            CefSharp::Internals::JavascriptObjectRepository^ get();
        }

        virtual property IMethodRunnerQueue^ MethodRunnerQueue
        {
            CefSharp::Internals::IMethodRunnerQueue^ get();
        }
    };
}
#endif  // CEFSHARP_CORE_INTERNALS_CLIENTADAPTER_H_
