// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "IRenderWebBrowser.h"
#include "RenderClientAdapterInternal.h"

namespace CefSharp
{
    namespace Internals
    {
        void RenderClientAdapterInternal::OnPopupShow(CefRefPtr<CefBrowser> browser, bool show)
        {
            _renderBrowserControl->SetPopupIsOpen(show);
        }

        void RenderClientAdapterInternal::OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect)
        {
            _renderBrowserControl->SetPopupSizeAndPosition((IntPtr) (void*) &rect);
        }

        void RenderClientAdapterInternal::OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects,
            const void* buffer, int width, int height)
        {
            auto intPtrBuffer = (IntPtr) (void *) buffer;

            if (type == PET_VIEW)
            {
                _renderBrowserControl->SetBuffer(width, height, intPtrBuffer);
            }
            else if (type == PET_POPUP)
            {
                _renderBrowserControl->SetPopupBuffer(width, height, intPtrBuffer);
            }
        }

        void RenderClientAdapterInternal::OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor)
        {
            _renderBrowserControl->SetCursor((IntPtr)cursor);
        }
    }
}