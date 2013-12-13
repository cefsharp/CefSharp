#include "Stdafx.h"

#include "IRenderWebBrowser.h"
#include "RenderClientAdapter.h"

namespace CefSharp
{
    void RenderClientAdapter::OnPopupShow(CefRefPtr<CefBrowser> browser, bool show)
    {
        _renderBrowserControl->SetPopupIsOpen(show);
    }

    void RenderClientAdapter::OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect)
    {
        _renderBrowserControl->SetPopupSizeAndPosition((void*) &rect);
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
            int width, height;
            browser->GetSize(type, width, height);

            _renderBrowserControl->SetPopupBuffer(width, height, buffer);
        }
    }

    void RenderClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        _renderBrowserControl->SetCursor((IntPtr)cursor);
    }

    bool RenderClientAdapter::GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
    {
        // The simulated screen and view rectangle are the same. This is necessary for popup menus to be located and sized inside
        // the view.
        int width, height;
        browser->GetSize(PET_VIEW, width, height);

        rect.x = rect.y = 0;
        rect.width = width;
        rect.height = height;
        return true;
    }

    bool RenderClientAdapter::GetScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect)
    {
        return GetViewRect(browser, rect);
    }
}