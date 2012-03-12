#include "stdafx.h"

#include "IRenderWebBrowser.h"
#include "RenderClientAdapter.h"

namespace CefSharp
{
    void RenderClientAdapter::OnPopupShow(CefRefPtr<CefBrowser> browser, bool show)
    {
        Console::WriteLine("OnPopupShow: {0}", show);
    }

    void RenderClientAdapter::OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect)
    {
        Console::WriteLine("OnPopupSize");
    }

    void RenderClientAdapter::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer)
    {
        Console::WriteLine("PET: {0}", (int)type);

        int width, height;
        browser->GetSize(type, width, height);

        _renderBrowserControl->SetBuffer(width, height, buffer);
    }

    void RenderClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        _renderBrowserControl->SetCursor(cursor);
    }
}