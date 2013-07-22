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

        private class RenderClientAdapterInternal : public ClientAdapter,
                                                    public CefRenderHandler
        {
        private:
            gcroot<IRenderWebBrowser^> _renderBrowserControl;

        public:
            RenderClientAdapterInternal(IRenderWebBrowser^ offscreenBrowserControl) :
                ClientAdapter((IWebBrowser^) offscreenBrowserControl)
            {
                _renderBrowserControl = offscreenBrowserControl;
            }

            ~RenderClientAdapterInternal() { _renderBrowserControl = nullptr; }

            // CefClient
            virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

            // CefRenderHandler
            // TODO: Do we have to do something about GetViewRect?
            virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE { return false; }
            virtual DECL void OnPopupShow(CefRefPtr<CefBrowser> browser, bool show) OVERRIDE;
            virtual DECL void OnPopupSize(CefRefPtr<CefBrowser> browser,const CefRect& rect) OVERRIDE;
            virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects,
                const void* buffer, int width, int height) OVERRIDE;
            virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE;

            IMPLEMENT_REFCOUNTING(RenderClientAdapterInternal)
        };
    }
}
