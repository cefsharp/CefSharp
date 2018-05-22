// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include <msclr/lock.h>

#include "ClientAdapter.h"

using namespace msclr;
using namespace CefSharp::Structs;

namespace CefSharp
{
    namespace Internals
    {
        private class RenderClientAdapter : public ClientAdapter,
            public CefRenderHandler
        {
        private:
            gcroot<IRenderWebBrowser^> _renderWebBrowser;

        public:
            RenderClientAdapter(IWebBrowserInternal^ webBrowserInternal, IBrowserAdapter^ browserAdapter):
                ClientAdapter(webBrowserInternal, browserAdapter)
            {
                _renderWebBrowser = dynamic_cast<IRenderWebBrowser^>(webBrowserInternal);
            }

            ~RenderClientAdapter()
            {
                _renderWebBrowser = nullptr;
            }

            // CefClient
            virtual DECL CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE{ return this; };

            // CefRenderHandler
            virtual DECL bool GetScreenInfo(CefRefPtr<CefBrowser> browser, CefScreenInfo& screen_info) OVERRIDE
            {
                if ((IRenderWebBrowser^)_renderWebBrowser == nullptr)
                {
                    return false;
                }

                auto screenInfo = _renderWebBrowser->GetScreenInfo();

                if (screenInfo.HasValue == false || screen_info.device_scale_factor == screenInfo.Value.ScaleFactor)
                {
                    return false;
                }

                //NOTE: We're relying on a call to GetViewRect to populate the view rectangle
                //https://bitbucket.org/chromiumembedded/cef/src/47e6d4bf84444eb6cb4d4509231a8c9ee878a584/include/cef_render_handler.h?at=2357#cef_render_handler.h-90
                screen_info.device_scale_factor = screenInfo.Value.ScaleFactor;
                return true;
            }

            // CefRenderHandler
            virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE
            {
                if ((IRenderWebBrowser^)_renderWebBrowser == nullptr)
                {
                    return false;
                }

                auto viewRect = _renderWebBrowser->GetViewRect();

                if (viewRect.HasValue == false)
                {
                    return false;
                }

                rect = CefRect(0, 0, viewRect.Value.Width, viewRect.Value.Height);

                return true;
            };

            ///
            // Called to retrieve the translation from view coordinates to actual screen
            // coordinates. Return true if the screen coordinates were provided.
            ///
            /*--cef()--*/
            virtual DECL bool GetScreenPoint(CefRefPtr<CefBrowser> browser, int viewX, int viewY, int& screenX, int& screenY) OVERRIDE
            {
                return _renderWebBrowser->GetScreenPoint(viewX, viewY, screenX, screenY);
            }

            ///
            // Called when the browser wants to show or hide the popup widget. The popup
            // should be shown if |show| is true and hidden if |show| is false.
            ///
            /*--cef()--*/
            virtual DECL void OnPopupShow(CefRefPtr<CefBrowser> browser, bool show) OVERRIDE
            {
                _renderWebBrowser->OnPopupShow(show);
            };

            ///
            // Called when the browser wants to move or resize the popup widget. |rect|
            // contains the new location and size.
            ///
            /*--cef()--*/
            virtual DECL void OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect) OVERRIDE
            {
                _renderWebBrowser->OnPopupSize(Rect(rect.x, rect.y, rect.width, rect.height));
            };

            virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects,
                const void* buffer, int width, int height) OVERRIDE
            {
                CefRect r = dirtyRects.front();
                _renderWebBrowser->OnPaint((CefSharp::PaintElementType)type, CefSharp::Structs::Rect(r.x, r.y, r.width, r.height), IntPtr((void *)buffer), width, height);
            };

            virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor, CursorType type,
                const CefCursorInfo& custom_cursor_info) OVERRIDE
            {
                CursorInfo customCursorInfo;

                //Only create the struct when we actually have a custom cursor
                if (type == CursorType::CT_CUSTOM)
                {
                    Point hotspot = Point(custom_cursor_info.hotspot.x, custom_cursor_info.hotspot.y);
                    Size size = Size(custom_cursor_info.size.width, custom_cursor_info.size.height);
                    customCursorInfo = CursorInfo(IntPtr((void *)custom_cursor_info.buffer), hotspot, custom_cursor_info.image_scale_factor, size);
                }

                _renderWebBrowser->OnCursorChange((IntPtr)cursor, (CefSharp::Enums::CursorType)type, customCursorInfo);
            };

            virtual DECL bool StartDragging(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData,
                CefRenderHandler::DragOperationsMask allowedOps, int x, int y)
            {
                CefDragDataWrapper dragDataWrapper(dragData);
                return _renderWebBrowser->StartDragging(%dragDataWrapper, (CefSharp::Enums::DragOperationsMask)allowedOps, x, y);
            }

            ///
            // Called when the web view wants to update the mouse cursor during a
            // drag & drop operation. |operation| describes the allowed operation
            // (none, move, copy, link).
            ///
            /*--cef()--*/
            virtual DECL void UpdateDragCursor(CefRefPtr<CefBrowser> browser, CefRenderHandler::DragOperation operation)
            {
                return _renderWebBrowser->UpdateDragCursor((CefSharp::Enums::DragOperationsMask)operation);
            }

            ///
            // Called when the IME composition range has changed. |selected_range| is the
            // range of characters that have been selected. |character_bounds| is the
            // bounds of each character in view coordinates.
            ///
            /*--cef()--*/
            virtual DECL void OnImeCompositionRangeChanged(CefRefPtr<CefBrowser> browser, const CefRange& selectedRange, const RectList& characterBounds)
            {
                auto charBounds = gcnew cli::array<Rect>((int)characterBounds.size());

                std::vector<CefRect>::const_iterator it = characterBounds.begin();
                for (int index = 0; it != characterBounds.end(); ++it, index++)
                {
                    charBounds[index] = Rect((*it).x, (*it).y, (*it).width, (*it).height);
                }

                _renderWebBrowser->OnImeCompositionRangeChanged(Range(selectedRange.from, selectedRange.to), charBounds);
            }

            IMPLEMENT_REFCOUNTING(RenderClientAdapter)
        };
    }
}
