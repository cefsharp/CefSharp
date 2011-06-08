#include "stdafx.h"

#include "CefWpfWebBrowser.h"

namespace CefSharp
{
    bool WpfClientAdapter::GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
    {
        return false;
    }

    bool WpfClientAdapter::GetScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
    {
        return false;
    }

    bool WpfClientAdapter::GetScreenPoint(CefRefPtr<CefBrowser> browser, int viewX, int viewY, int& screenX, int& screenY)
    {
        return false;
    }

    void WpfClientAdapter::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const CefRect& dirtyRect, const void* buffer)
    {
        int width, height;
        browser->GetSize(type, width, height);

        _wpfBrowserControl->SetBuffer(width, height, dirtyRect, buffer);
    }

    void WpfClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        _wpfBrowserControl->SetCursor(cursor);
    }
}