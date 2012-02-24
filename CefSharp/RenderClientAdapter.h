#include "stdafx.h"
#pragma once

#include "ClientAdapter.h"

namespace CefSharp
{
    using namespace System;

    interface class IRenderWebBrowser;
    
    class RenderClientAdapter : public ClientAdapter,
                                public CefRenderHandler
    {
    private:
        gcroot<IRenderWebBrowser^> _offscreenBrowserControl;

    public:
        ~RenderClientAdapter() { _offscreenBrowserControl = nullptr; }
        RenderClientAdapter(IRenderWebBrowser^ offscreenBrowserControl) : ClientAdapter((IWebBrowser^)offscreenBrowserControl)
        {
            _offscreenBrowserControl = offscreenBrowserControl;
        }

        // CefClient
        virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

        // CefRenderHandler
        virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer) OVERRIDE;
        virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE;

        IMPLEMENT_LOCKING(RenderClientAdapter);
        IMPLEMENT_REFCOUNTING(RenderClientAdapter);
    };
}
