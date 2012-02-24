#include "stdafx.h"

#include "IRenderWebBrowser.h"
#include "RenderClientAdapter.h"

namespace CefSharp
{
    void RenderClientAdapter::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer)
    {
        int width, height;
        browser->GetSize(type, width, height);

        _offscreenBrowserControl->SetBuffer(width, height, buffer);
    }

    void RenderClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        _offscreenBrowserControl->SetCursor(cursor);
    }
}