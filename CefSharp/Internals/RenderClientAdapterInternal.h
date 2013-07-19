// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "ClientAdapter.h"

namespace CefSharp
{
    interface class IRenderWebBrowser;

    namespace Internals
    {
        using namespace System;

        public class RenderClientAdapter :
            public ClientAdapter,
            public CefRenderHandler
        {
        private:
            gcroot<IRenderWebBrowser^> _renderBrowserControl;

        public:
            ~RenderClientAdapter() { _renderBrowserControl = nullptr; }
            RenderClientAdapter(IRenderWebBrowser^ offscreenBrowserControl) : 
                ClientAdapter((IWebBrowser^) offscreenBrowserControl)
            {
                _renderBrowserControl = offscreenBrowserControl;
            }

            // CefClient
            virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

            // CefRenderHandler
            virtual DECL void OnPopupShow(CefRefPtr<CefBrowser> browser, bool show) OVERRIDE;
            virtual DECL void OnPopupSize(CefRefPtr<CefBrowser> browser,const CefRect& rect) OVERRIDE;
            virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects,
                const void* buffer, int width, int height) OVERRIDE;
            virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE;
        };
    }
}
