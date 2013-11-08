// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "BrowserSettings.h"
#include "MouseButtonType.h"
#include "ScriptCore.h"
#include "Internals/JavascriptBinding/IJavascriptProxy.h"
#include "Internals/IRenderWebBrowser.h"
#include "Internals/RenderClientAdapter.h"

using namespace CefSharp::Internals;
using namespace CefSharp::Internals::JavascriptBinding;
using namespace System::ServiceModel;

namespace CefSharp
{
    private ref class CefBrowserWrapper
    {
    private:
        RenderClientAdapter* _renderClientAdapter;
        ScriptCore* _scriptCore;
        IJavascriptProxy^ _javaScriptProxy;

    public:
        property Object^ BitmapLock
        {
            Object^ get() { return _renderClientAdapter->BitmapLock; }
        }

        property int BitmapWidth
        {
            int get() { return _renderClientAdapter->BitmapWidth; }
        }

        property int BitmapHeight
        {
            int get() { return _renderClientAdapter->BitmapHeight; }
        }

        property String^ DevToolsUrl
        {
            String^ get()
            {
                auto cefHost = _renderClientAdapter->TryGetCefHost();

                if (cefHost != nullptr)
                {
                    return StringUtils::ToClr(cefHost->GetDevToolsURL(true));
                }
                else
                {
                    return nullptr;
                }
            }
        }

        CefBrowserWrapper(IRenderWebBrowser^ offscreenBrowserControl)
        {
            _renderClientAdapter = new RenderClientAdapter(offscreenBrowserControl);
            _scriptCore = new ScriptCore();
        }

        ~CefBrowserWrapper()
        {
            _renderClientAdapter = nullptr;
        }

        void CreateOffscreenBrowser(BrowserSettings^ browserSettings, IntPtr^ sourceHandle, String^ address)
        {
            HWND hwnd = static_cast<HWND>(sourceHandle->ToPointer());
            CefWindowInfo window;
            window.SetAsOffScreen(hwnd);
            window.SetTransparentPainting(true);
            CefString addressNative = StringUtils::ToNative(address);

            CefBrowserHost::CreateBrowser(window, _renderClientAdapter, addressNative,
                *(CefBrowserSettings*) browserSettings->_internalBrowserSettings);
        }

        void Close()
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->CloseBrowser(true);
            }
        }

        void LoadUrl(String^ address)
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->LoadURL(StringUtils::ToNative(address));
            }
        }

        void LoadHtml(String^ html, String^ url)
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->LoadString(StringUtils::ToNative(html), StringUtils::ToNative(url));
            }
        }

        void WasResized()
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->WasResized();
            }
        }

        void SendFocusEvent(bool isFocused)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->SendFocusEvent(isFocused);
            }
        }

        bool SendKeyEvent(int message, int wParam)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost == nullptr)
            {
                return false;
            }
            else
            {
                CefKeyEvent keyEvent;
                if (message == WM_CHAR)
                    keyEvent.type = KEYEVENT_CHAR;
                else if (message == WM_KEYDOWN || message == WM_SYSKEYDOWN)
                    keyEvent.type = KEYEVENT_KEYDOWN;
                else if (message == WM_KEYUP || message == WM_SYSKEYUP)
                    keyEvent.type = KEYEVENT_KEYUP;

                keyEvent.windows_key_code = keyEvent.native_key_code = wParam;
                keyEvent.is_system_key = 
                    message == WM_SYSKEYDOWN ||
                    message == WM_SYSKEYUP ||
                    message == WM_SYSCHAR;

                cefHost->SendKeyEvent(keyEvent);
                return true;
            }
        }

        void OnMouseMove(int x, int y, bool mouseLeave)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                CefMouseEvent mouseEvent;
                mouseEvent.x = x;
                mouseEvent.y = y;

                cefHost->SendMouseMoveEvent(mouseEvent, mouseLeave);
            }
        }

        void OnMouseButton(int x, int y, MouseButtonType mouseButtonType, bool mouseUp, int clickCount)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                CefMouseEvent mouseEvent;
                mouseEvent.x = x;
                mouseEvent.y = y;

                cefHost->SendMouseClickEvent(mouseEvent, (CefBrowserHost::MouseButtonType) mouseButtonType, mouseUp, clickCount);
            }
        }

        void OnMouseWheel(int x, int y, int deltaX, int deltaY)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                CefMouseEvent mouseEvent;
                mouseEvent.x = x;
                mouseEvent.y = y;

                cefHost->SendMouseWheelEvent(mouseEvent, deltaX, deltaY);
            }
        }

        void GoBack()
        {
            auto cefBrowser = _renderClientAdapter->GetCefBrowser();

            if (cefBrowser != nullptr)
            {
                cefBrowser->GoBack();
            }
        }

        void GoForward()
        {
            auto cefBrowser = _renderClientAdapter->GetCefBrowser();

            if (cefBrowser != nullptr)
            {
                cefBrowser->GoForward();
            }
        }

        void Reload()
        {
            auto cefBrowser = _renderClientAdapter->GetCefBrowser();

            if (cefBrowser != nullptr)
            {
                cefBrowser->Reload();
            }
        }

        void ViewSource()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->ViewSource();
            }
        }

        void ExecuteScript(String^ script)
        {
            auto browser = _renderClientAdapter->GetCefBrowser();

            if (browser != nullptr)
            {
                _scriptCore->Execute(browser, StringUtils::ToNative(script));
            }
        }

        Object^ EvaluateScript(String^ script, TimeSpan timeout)
        {
            auto browser = _renderClientAdapter->GetCefBrowser();
            auto frame = _renderClientAdapter->TryGetCefMainFrame();

            if (browser != nullptr &&
                frame != nullptr)
            {
				// TODO: Make the address be shared between the Subprocess and the CefSharp projects.
				// TODO: Don't instantiate this on every request. The problem is that the CefBrowser is not set in our constructor.
				auto channelFactory = gcnew ChannelFactory<IJavascriptProxy^>(
					gcnew NetNamedPipeBinding(),
					gcnew EndpointAddress("net.pipe://localhost/JavaScriptProxy_" + _renderClientAdapter->GetCefBrowser()->GetIdentifier())
				);

				_javaScriptProxy = channelFactory->CreateChannel();


                return _javaScriptProxy->EvaluateScript(frame->GetIdentifier(), script, timeout.TotalMilliseconds);
            }
            else
            {
                return nullptr;
            }
        }

		void CreateBrowser(BrowserSettings^ browserSettings, IntPtr^ sourceHandle, String^ address)
        {
            HWND hwnd = static_cast<HWND>(sourceHandle->ToPointer());
			RECT rect;
			GetClientRect(hwnd, &rect);
            CefWindowInfo window;
            window.SetAsChild(hwnd, rect);
            CefString addressNative = StringUtils::ToNative(address);

            CefBrowserHost::CreateBrowser(window, _renderClientAdapter, addressNative,
                *(CefBrowserSettings*) browserSettings->_internalBrowserSettings);
        }

		void OnSizeChanged(IntPtr^ sourceHandle)
		{
			HWND hWnd = static_cast<HWND>(sourceHandle->ToPointer());
			RECT rect;
			GetClientRect(hWnd, &rect);
			HDWP hdwp = BeginDeferWindowPos(1);

			HWND browserHwnd = _renderClientAdapter->GetBrowserHwnd();
			hdwp = DeferWindowPos(hdwp, browserHwnd, NULL, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, SWP_NOZORDER);
			EndDeferWindowPos(hdwp);
		}
    };
}
