// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "BrowserSettings.h"
#include "MouseButtonType.h"
#include "IRenderWebBrowser.h"
#include "Internals/RenderClientAdapterInternal.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class RenderClientAdapter
    {
    private:
        RenderClientAdapterInternal* _renderClientAdapterInternal;

    public:
        RenderClientAdapter(IRenderWebBrowser^ offscreenBrowserControl)
        {
            _renderClientAdapterInternal = new RenderClientAdapterInternal(offscreenBrowserControl);
        }

        ~RenderClientAdapter()
        {
            delete _renderClientAdapterInternal;
        }

        void CreateOffscreenBrowser(BrowserSettings^ browserSettings, IntPtr^ sourceHandle, Uri^ uri)
        {
            HWND hwnd = static_cast<HWND>(sourceHandle->ToPointer());
            CefWindowInfo window;
            window.SetAsOffScreen(hwnd);
            CefString url = StringUtils::ToNative(uri->ToString());

            CefBrowserHost::CreateBrowser(window, _renderClientAdapterInternal, url,
                *(CefBrowserSettings*) browserSettings->_internalBrowserSettings);
        }

        void WasResized()
        {
            auto cefHost = _renderClientAdapterInternal->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->WasResized();
            }
        }

        void SendFocusEvent(bool isFocused)
        {
            auto cefHost = _renderClientAdapterInternal->TryGetCefHost();

            if (cefHost != nullptr)
            {
                cefHost->SendFocusEvent(isFocused);
            }
        }

        void OnMouseMove(int x, int y, bool mouseLeave)
        {
            auto cefHost = _renderClientAdapterInternal->TryGetCefHost();

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
            auto cefHost = _renderClientAdapterInternal->TryGetCefHost();

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
            auto cefHost = _renderClientAdapterInternal->TryGetCefHost();

            if (cefHost != nullptr)
            {
                CefMouseEvent mouseEvent;
                mouseEvent.x = x;
                mouseEvent.y = y;

                cefHost->SendMouseWheelEvent(mouseEvent, deltaX, deltaY);
            }
        }
    };
}
