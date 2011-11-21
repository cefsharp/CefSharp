#include "stdafx.h"
#pragma once

#include "ClientAdapter.h"

namespace CefSharp 
{
    using namespace System;

    ref class CefWpfWebBrowser;
    
    class WpfClientAdapter : public ClientAdapter,
                             public CefRenderHandler
    {
    private:
        gcroot<CefWpfWebBrowser^> _wpfBrowserControl;

    public:
        ~WpfClientAdapter() { }
        WpfClientAdapter(CefWpfWebBrowser^ wpfBroserControl) : ClientAdapter((ICefWebBrowser^)wpfBroserControl)
        {
            _wpfBrowserControl = wpfBroserControl;
        }

        // CefClient
        virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

        // CefRenderHandler
        virtual bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE;
        virtual bool GetScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE;
        virtual bool GetScreenPoint(CefRefPtr<CefBrowser> browser, int viewX, int viewY, int& screenX, int& screenY) OVERRIDE;
        //virtual void OnPopupShow(CefRefPtr<CefBrowser> browser, bool show) OVERRIDE;
        //virtual void OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect) OVERRIDE;
        virtual void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer) OVERRIDE;
        virtual void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE;

        IMPLEMENT_LOCKING(WpfClientAdapter);
        IMPLEMENT_REFCOUNTING(WpfClientAdapter);
    };
}