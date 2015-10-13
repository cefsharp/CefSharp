// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_client.h"

#include "BrowserSettings.h"
#include "Internals/ClientAdapter.h"
#include "Internals/CefDragDataWrapper.h"
#include "Internals/RenderClientAdapter.h"
#include "Internals/JavascriptCallbackFactory.h"

using namespace System::Diagnostics;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    public ref class ManagedCefBrowserAdapter : public IBrowserAdapter
    {
        MCefRefPtr<ClientAdapter> _clientAdapter;
        BrowserProcessServiceHost^ _browserProcessServiceHost;
        IWebBrowserInternal^ _webBrowserInternal;
        JavascriptObjectRepository^ _javaScriptObjectRepository;
        JavascriptCallbackFactory^ _javascriptCallbackFactory;
        MethodRunnerQueue^ _methodRunnerQueue;
        IBrowser^ _browserWrapper;
        bool _isDisposed;

    private:
        // Private keyboard functions:
        bool IsKeyDown(WPARAM wparam)
        {
            return (GetKeyState(wparam) & 0x8000) != 0;
        }

        // Misc. private functions:
        int GetCefKeyboardModifiers(WPARAM wparam, LPARAM lparam);
        CefMouseEvent GetCefMouseEvent(MouseEvent^ mouseEvent);

        void MethodInvocationComplete(Object^ sender, MethodInvocationCompleteArgs^ e);

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
            _methodRunnerQueue = gcnew CefSharp::Internals::MethodRunnerQueue(_javaScriptObjectRepository);
            _methodRunnerQueue->MethodInvocationComplete += gcnew EventHandler<MethodInvocationCompleteArgs^>(this, &ManagedCefBrowserAdapter::MethodInvocationComplete);
            _methodRunnerQueue->Start();
        }

        !ManagedCefBrowserAdapter()
        {
            _clientAdapter = nullptr;
        }

        ~ManagedCefBrowserAdapter()
        {
            _isDisposed = true;
            // Release the MCefRefPtr<ClientAdapter> reference
            // before calling _browserWrapper->CloseBrowser(true)
            this->!ManagedCefBrowserAdapter();

            if (_browserWrapper != nullptr)
            {
                _browserWrapper->CloseBrowser(true);

                delete _browserWrapper;
                _browserWrapper = nullptr;
            }

            if (_methodRunnerQueue != nullptr)
            {
                _methodRunnerQueue->MethodInvocationComplete -= gcnew EventHandler<MethodInvocationCompleteArgs^>(this, &ManagedCefBrowserAdapter::MethodInvocationComplete);
                _methodRunnerQueue->Stop();
                _methodRunnerQueue = nullptr;
            }

            if (CefSharpSettings::WcfEnabled && _browserProcessServiceHost != nullptr)
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

            _webBrowserInternal = nullptr;
            _javaScriptObjectRepository = nullptr;
        }

        virtual property bool IsDisposed
        {
            bool get();
        }

        virtual void OnAfterBrowserCreated(int browserId);
        void CreateOffscreenBrowser(IntPtr windowHandle, BrowserSettings^ browserSettings, RequestContext^ requestContext, String^ address);
        void CreateBrowser(BrowserSettings^ browserSettings, RequestContext^ requestContext, IntPtr sourceHandle, String^ address);
        void WasResized();
        void WasHidden(bool hidden);
        void SendFocusEvent(bool isFocused);
        void SetFocus(bool isFocused);
        bool SendKeyEvent(int message, int wParam, int lParam);
        void Resize(int width, int height);
        void NotifyMoveOrResizeStarted();
        void NotifyScreenInfoChanged();
        void RegisterJsObject(String^ name, Object^ object, bool lowerCaseJavascriptNames);
        void RegisterAsyncJsObject(String^ name, Object^ object, bool lowerCaseJavascriptNames);
        void OnDragTargetDragEnter(CefDragDataWrapper^ dragData, MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragOver(MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragLeave();
        void OnDragTargetDragDrop(MouseEvent^ mouseEvent);

        void OnDragSourceEndedAt(int x, int y, DragOperationsMask op);
        void OnDragSourceSystemDragEnded();

        /// <summary>
        /// Gets the CefBrowserWrapper instance
        /// </summary>
        /// <returns>Gets the current instance or null</returns>
        virtual IBrowser^ GetBrowser();

        virtual IBrowser^ GetBrowser(int browserId);

        virtual property IJavascriptCallbackFactory^ JavascriptCallbackFactory
        {
            CefSharp::Internals::IJavascriptCallbackFactory^ get();
        }

        virtual property JavascriptObjectRepository^ JavascriptObjectRepository
        {
            CefSharp::Internals::JavascriptObjectRepository^ get();
        }

        virtual property MethodRunnerQueue^ MethodRunnerQueue
        {
            CefSharp::Internals::MethodRunnerQueue^ get();
        }
    };
}
