// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "BrowserSettings.h"
#include "MouseButtonType.h"
#include "Internals/RenderClientAdapter.h"
#include "Internals/MCefRefPtr.h"
#include "Internals/StringVisitor.h"

using namespace CefSharp::Internals;
using namespace System::Diagnostics;
using namespace System::ServiceModel;

namespace CefSharp
{
    private ref class ManagedCefBrowserAdapter : public ObjectBase, ISubProcessCallback
    {
        MCefRefPtr<RenderClientAdapter> _renderClientAdapter;
        ISubProcessProxy^ _javaScriptProxy;
        IWebBrowserInternal^ _webBrowserInternal;
        String^ _address;
        
    protected:
        virtual void DoDispose(bool isDisposing) override
        {
            Close();

            _renderClientAdapter = nullptr;
            _javaScriptProxy = nullptr;
            _webBrowserInternal = nullptr;
            _address = nullptr;

            ObjectBase::DoDispose(isDisposing);
        };

    public:
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

        ManagedCefBrowserAdapter(IWebBrowserInternal^ webBrowserInternal)
        {
            _webBrowserInternal = webBrowserInternal;
            _renderClientAdapter = new RenderClientAdapter(webBrowserInternal, this);
        }

        void CreateOffscreenBrowser(BrowserSettings^ browserSettings)
        {
            HWND hwnd = HWND();
            CefWindowInfo window;
            window.SetAsOffScreen(hwnd);
            window.SetTransparentPainting(true);
            CefString addressNative = StringUtils::ToNative("about:blank");

            if (!CefBrowserHost::CreateBrowser(window, _renderClientAdapter.get(), addressNative,
                *(CefBrowserSettings*) browserSettings->_internalBrowserSettings, NULL))
            {
                throw gcnew InvalidOperationException( "Failed to create offscreen browser. Call Cef.Initialize() first." );
            }
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
            _address = address;
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->LoadURL(StringUtils::ToNative(address));
            }
        }


        void OnInitialized()
        {
            _webBrowserInternal->OnInitialized();

            auto address = _address;

            if ( address != nullptr )
            {
                LoadUrl(address);
            }
        };

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

        bool SendKeyEvent(int message, int wParam, CefEventFlags modifiers)
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

                keyEvent.modifiers = (uint32)modifiers;

                cefHost->SendKeyEvent(keyEvent);
                return true;
            }
        }

        void OnMouseMove(int x, int y, bool mouseLeave, CefEventFlags modifiers)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                CefMouseEvent mouseEvent;
                mouseEvent.x = x;
                mouseEvent.y = y;

                mouseEvent.modifiers = (uint32)modifiers;

                cefHost->SendMouseMoveEvent(mouseEvent, mouseLeave);
            }
        }

        void OnMouseButton(int x, int y, MouseButtonType mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                CefMouseEvent mouseEvent;
                mouseEvent.x = x;
                mouseEvent.y = y;
                mouseEvent.modifiers = (uint32)modifiers;

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

        void Stop()
        {
            auto cefBrowser = _renderClientAdapter->GetCefBrowser();

            if (cefBrowser != nullptr)
            {
                cefBrowser->StopLoad();
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

        void Print()
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->Print();
            }
        }

        void Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->Find(identifier, StringUtils::ToNative(searchText), forward, matchCase, findNext);
            }
        }

        void StopFinding(bool clearSelection)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->StopFinding(clearSelection);
            }
        }
        
        void Reload()
        {
            Reload(false);
        }

        void Reload(bool ignoreCache)
        {
            auto cefBrowser = _renderClientAdapter->GetCefBrowser();

            if (cefBrowser != nullptr)
            {
                if (ignoreCache)
                {
                    cefBrowser->ReloadIgnoreCache();
                }
                else
                {
                    cefBrowser->Reload();
                }
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

        void GetSource(IStringVisitor^ visitor)
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                auto stringVisitor = new StringVisitor(visitor);
                cefFrame->GetSource(stringVisitor);
            }
        }

        void GetText(IStringVisitor^ visitor)
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                auto stringVisitor = new StringVisitor(visitor);
                cefFrame->GetText(stringVisitor);
            }
        }

        void Cut()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame(); 
            
            if (cefFrame != nullptr)
            {
                cefFrame->Cut();
            }
        }

        void Copy()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame(); 
            
            if (cefFrame != nullptr)
            {
                cefFrame->Copy();
            }
        }

        void Paste()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->Paste();
            }
        }

        void Delete()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->Delete();
            }
        }

        void SelectAll()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->SelectAll();
            }
        }

        void Undo()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->Undo();
            }
        }

        void Redo()
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->Redo();
            }
        }
        
        void ExecuteScriptAsync(String^ script)
        {
            auto cefFrame = _renderClientAdapter->TryGetCefMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->ExecuteJavaScript(StringUtils::ToNative(script), "about:blank", 0);
            }
        }

        Object^ EvaluateScript(String^ script, TimeSpan timeout)
        {
            auto browser = _renderClientAdapter->GetCefBrowser();
            auto frame = _renderClientAdapter->TryGetCefMainFrame();

            if (browser != nullptr &&
                frame != nullptr)
            {
                // TODO: Don't instantiate this on every request. The problem is that the CefBrowser is not set in our constructor.
                auto serviceName = SubProcessProxySupport::GetServiceName(Process::GetCurrentProcess()->Id, _renderClientAdapter->GetCefBrowser()->GetIdentifier());
                auto channelFactory = gcnew DuplexChannelFactory<ISubProcessProxy^>(
                    this,
                    gcnew NetNamedPipeBinding(),
                    gcnew EndpointAddress(serviceName)
                );

                _javaScriptProxy = channelFactory->CreateChannel();

                return _javaScriptProxy->EvaluateScript(frame->GetIdentifier(), script, timeout.TotalMilliseconds);
            }
            else
            {
                return nullptr;
            }
        }

        double GetZoomLevel()
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                return cefHost->GetZoomLevel();
            }
            
            return 0;
        }

        void SetZoomLevel(double zoomLevel)
        {
            auto cefHost = _renderClientAdapter->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->SetZoomLevel(zoomLevel);
            }
        }

        virtual void Error(Exception^ ex)
        {

        }

        void CreateBrowser(BrowserSettings^ browserSettings, IntPtr^ sourceHandle, String^ address)
        {
            HWND hwnd = static_cast<HWND>(sourceHandle->ToPointer());
            RECT rect;
            GetClientRect(hwnd, &rect);
            CefWindowInfo window;
            window.SetAsChild(hwnd, rect);
            CefString addressNative = StringUtils::ToNative(address);

            CefBrowserHost::CreateBrowser(window, _renderClientAdapter.get(), addressNative,
                *(CefBrowserSettings*) browserSettings->_internalBrowserSettings, NULL);
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
