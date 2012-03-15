#include "stdafx.h"

#include "IRenderWebBrowser.h"
#include "RenderClientAdapter.h"

namespace CefSharp
{
    void RenderClientAdapter::OnPopupShow(CefRefPtr<CefBrowser> browser, bool show)
    {

    }

    void RenderClientAdapter::OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect)
    {

    }

    void RenderClientAdapter::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer)
    {
        if (type == PET_VIEW)
        {
            int width, height;
            browser->GetSize(type, width, height);

            _renderBrowserControl->SetBuffer(width, height, buffer);
        }
        else if (type == PET_POPUP)
        {
            // XXX
        }
    }

    void RenderClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        _renderBrowserControl->SetCursor(cursor);
    }
}