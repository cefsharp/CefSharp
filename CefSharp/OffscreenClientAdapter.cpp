#include "stdafx.h"

#include "IOffscreenWebBrowser.h"
#include "OffscreenClientAdapter.h"

namespace CefSharp
{
    bool OffscreenClientAdapter::GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
    {
        return false;
    }

    bool OffscreenClientAdapter::GetScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
    {
        return false;
    }

    bool OffscreenClientAdapter::GetScreenPoint(CefRefPtr<CefBrowser> browser, int viewX, int viewY, int& screenX, int& screenY)
    {
        return false;
    }

    void OffscreenClientAdapter::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer)
    {
        int width, height;
        browser->GetSize(type, width, height);

        _offscreenBrowserControl->SetBuffer(width, height, buffer);
    }

    void OffscreenClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        _offscreenBrowserControl->SetCursor(cursor);
    }
}