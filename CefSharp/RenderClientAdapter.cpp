#include "Stdafx.h"

#include "IRenderWebBrowser.h"
#include "RenderClientAdapter.h"

namespace CefSharp
{
    void RenderClientAdapter::_OnPopupShow(RenderClientAdapter* const _this, bool show)
    {
        _this->_renderBrowserControl->SetPopupIsOpen(show);
    }

    void RenderClientAdapter::OnPopupShow(CefRefPtr<CefBrowser> browser, bool show)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnPopupShow, this, show);
        } else {
            _OnPopupShow(this, show);
        }
    }

    void RenderClientAdapter::_OnPopupSize(RenderClientAdapter* const _this, const CefRect* const _rect)
    {
        _this->_renderBrowserControl->SetPopupSizeAndPosition((void*) _rect);
    }

    void RenderClientAdapter::OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_OnPopupSize, this, &rect);
        } else {
            _OnPopupSize(this, &rect);
        }
    }

    void RenderClientAdapter::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects, const void* buffer)
    {
        if (type == PET_VIEW)
        {
            int width, height;
            browser->GetSize(type, width, height);

            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_SetBuffer, this, width, height, buffer);
            } else {
                _SetBuffer(this, width, height, buffer);
            }
        }
        else if (type == PET_POPUP)
        {
            int width, height;
            browser->GetSize(type, width, height);

            if (IsCrossDomainCallRequired()) {
                msclr::call_in_appdomain(GetAppDomainId(), &_SetPopupBuffer, this, width, height, buffer);
            } else {
                _SetPopupBuffer(this, width, height, buffer);
            }
        }
    }

    void RenderClientAdapter::_SetBuffer(RenderClientAdapter* const _this, int width, int height, const void* buffer)
    {
        _this->_renderBrowserControl->SetBuffer(width, height, buffer);
    }

    void RenderClientAdapter::_SetPopupBuffer(RenderClientAdapter* const _this, int width, int height, const void* buffer)
    {
        _this->_renderBrowserControl->SetPopupBuffer(width, height, buffer);
    }

    void RenderClientAdapter::_SetCursor(RenderClientAdapter* const _this, CefCursorHandle cursor)
    {
        _this->_renderBrowserControl->SetCursor((IntPtr) cursor);
    }

    void RenderClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_SetCursor, this, cursor);
        } else {
            _SetCursor(this, cursor);
        }
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