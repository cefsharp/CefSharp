#include "Stdafx.h"
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
        static void _SetBuffer(RenderClientAdapter* const _this, int width, int height, const void* buffer);
        static void _SetPopupBuffer(RenderClientAdapter* const _this, int width, int height, const void* buffer);
        static void _SetCursor(RenderClientAdapter* const _this, CefCursorHandle cursor);
        static void _OnPopupShow(RenderClientAdapter* const _this, bool show);
        static void _OnPopupSize(RenderClientAdapter* const _this, const CefRect* const _rect);

    private:
        gcroot<IRenderWebBrowser^> _renderBrowserControl;

    public:
        ~RenderClientAdapter() { _renderBrowserControl = nullptr; }
        RenderClientAdapter(IRenderWebBrowser^ offscreenBrowserControl) : ClientAdapter((IWebBrowser^)offscreenBrowserControl)
        {
            _renderBrowserControl = offscreenBrowserControl;
        }

        // CefClient
        virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

        // CefRenderHandler
        virtual DECL void OnPopupShow(CefRefPtr<CefBrowser> browser, bool show) OVERRIDE;
        virtual DECL void OnPopupSize(CefRefPtr<CefBrowser> browser,const CefRect& rect) OVERRIDE;
        virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer) OVERRIDE;
        virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE;
        virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE;
        virtual DECL bool GetScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE;
    };
}
