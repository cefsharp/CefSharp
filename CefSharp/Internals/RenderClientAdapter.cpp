// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "IRenderWebBrowser.h"
#include "RenderClientAdapter.h"

namespace CefSharp
{
    namespace Internals
    {
        void RenderClientAdapter::OnPopupShow(CefRefPtr<CefBrowser> browser, bool show)
        {
            _renderBrowserControl->SetPopupIsOpen(show);
        }

        void RenderClientAdapter::OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect)
        {
            _renderBrowserControl->SetPopupSizeAndPosition((void*) &rect);
        }

        void RenderClientAdapter::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects,
            const void* buffer, int width, int height)
        {
            if (type == PET_VIEW)
            {
                _renderBrowserControl->SetBuffer(width, height, buffer);
            }
            else if (type == PET_POPUP)
            {
                _renderBrowserControl->SetPopupBuffer(width, height, buffer);
            }
        }

        void RenderClientAdapter::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
        {
            _renderBrowserControl->SetCursor((IntPtr)cursor);
        }
    }
}