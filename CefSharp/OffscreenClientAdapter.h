#include "stdafx.h"
#pragma once

#include "ClientAdapter.h"

namespace CefSharp
{
    using namespace System;

    interface class IOffscreenWebBrowser;
    
    class OffscreenClientAdapter : public ClientAdapter,
                                   public CefRenderHandler
    {
    private:
        gcroot<IOffscreenWebBrowser^> _offscreenBrowserControl;

    public:
        ~OffscreenClientAdapter() { _offscreenBrowserControl = nullptr; }
        OffscreenClientAdapter(IOffscreenWebBrowser^ offscreenBrowserControl) : ClientAdapter((IWebBrowser^)offscreenBrowserControl)
        {
            _offscreenBrowserControl = offscreenBrowserControl;
        }

        // CefClient
        virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

        // CefRenderHandler
        virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE;
        virtual DECL bool GetScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE;
        virtual DECL bool GetScreenPoint(CefRefPtr<CefBrowser> browser, int viewX, int viewY, int& screenX, int& screenY) OVERRIDE;
        //virtual DECL void OnPopupShow(CefRefPtr<CefBrowser> browser, bool show) OVERRIDE;
        //virtual DECL void OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect) OVERRIDE;
        virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer) OVERRIDE;
        virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE;

        IMPLEMENT_LOCKING(OffscreenClientAdapter);
        IMPLEMENT_REFCOUNTING(OffscreenClientAdapter);
    };
}
