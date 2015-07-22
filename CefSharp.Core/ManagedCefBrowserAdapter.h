// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <include/cef_runnable.h>

#include "BrowserSettings.h"
#include "Internals/ClientAdapter.h"
#include "Internals/CefDragDataWrapper.h"
#include "Internals/RenderClientAdapter.h"
#include "Internals/MCefRefPtr.h"
#include "Internals/StringVisitor.h"
#include "Internals/CefFrameWrapper.h"
#include "Internals/CefSharpBrowserWrapper.h"
#include "Internals/JavascriptCallbackFactory.h"

using namespace CefSharp::Internals;
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
            _javaScriptObjectRepository = gcnew JavascriptObjectRepository();
            _javascriptCallbackFactory = gcnew CefSharp::Internals::JavascriptCallbackFactory(_clientAdapter->GetPendingTaskRepository());
        }

        !ManagedCefBrowserAdapter()
        {
            _clientAdapter = nullptr;
        }

        ~ManagedCefBrowserAdapter()
        {
            // Release the MCefRefPtr<ClientAdapter> reference
            // before calling _browserWrapper->CloseBrowser(true)
            this->!ManagedCefBrowserAdapter();
            if (_browserWrapper != nullptr)
            {
                _browserWrapper->CloseBrowser(true);

                delete _browserWrapper;
                _browserWrapper = nullptr;
            }

            if (CefSharpSettings::WcfEnabled && _browserProcessServiceHost != nullptr)
            {
                _browserProcessServiceHost->Close();
                _browserProcessServiceHost = nullptr;
            }

            _webBrowserInternal = nullptr;
            _javaScriptObjectRepository = nullptr;
            _isDisposed = true;
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
        void OnDragTargetDragEnter(CefDragDataWrapper^ dragData, MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragOver(MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragLeave();
        void OnDragTargetDragDrop(MouseEvent^ mouseEvent);

        /// <summary>
        /// Gets the CefBrowserWrapper instance
        /// </summary>
        /// <returns>Gets the current instance or null</returns>
        virtual IBrowser^ GetBrowser();

        virtual property IJavascriptCallbackFactory^ JavascriptCallbackFactory
        {
            CefSharp::Internals::IJavascriptCallbackFactory^ get();
        }
    };
}
