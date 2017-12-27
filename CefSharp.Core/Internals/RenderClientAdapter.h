// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include <msclr/lock.h>

#include "ClientAdapter.h"

using namespace msclr;

namespace CefSharp
{
    namespace Internals
    {
        private class RenderClientAdapter : public ClientAdapter,
            public CefRenderHandler
        {
        private:
            gcroot<IWebBrowserInternal^> _webBrowserInternal;
            gcroot<IRenderWebBrowser^> _renderWebBrowser;
            gcroot<BitmapInfo^> _mainBitmapInfo;
            gcroot<BitmapInfo^> _popupBitmapInfo;

        public:
            RenderClientAdapter(IWebBrowserInternal^ webBrowserInternal, IBrowserAdapter^ browserAdapter):
                ClientAdapter(webBrowserInternal, browserAdapter),
                _webBrowserInternal(webBrowserInternal)
            {
                _renderWebBrowser = dynamic_cast<IRenderWebBrowser^>(webBrowserInternal);
            }

            ~RenderClientAdapter()
            {
                _renderWebBrowser = nullptr;
                _webBrowserInternal = nullptr;

                ReleaseBitmapHandlers(_mainBitmapInfo);

                delete _mainBitmapInfo;
                _mainBitmapInfo = nullptr;

                ReleaseBitmapHandlers(_popupBitmapInfo);

                delete _popupBitmapInfo;
                _popupBitmapInfo = nullptr;
            }

            void CreateBitmapInfo()  
            {  
                _mainBitmapInfo = _renderWebBrowser->CreateBitmapInfo(false);  
                _popupBitmapInfo = _renderWebBrowser->CreateBitmapInfo(true);  
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

                if (screen_info.device_scale_factor == screenInfo.ScaleFactor)
                {
                    return false;
                }

                //NOTE: We're relying on a call to GetViewRect to populate the view rectangle
                //https://bitbucket.org/chromiumembedded/cef/src/47e6d4bf84444eb6cb4d4509231a8c9ee878a584/include/cef_render_handler.h?at=2357#cef_render_handler.h-90
                screen_info.device_scale_factor = screenInfo.ScaleFactor;
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

                rect = CefRect(0, 0, viewRect.Width, viewRect.Height);

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
                _renderWebBrowser->SetPopupIsOpen(show);
            };

            ///
            // Called when the browser wants to move or resize the popup widget. |rect|
            // contains the new location and size.
            ///
            /*--cef()--*/
            virtual DECL void OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect) OVERRIDE
            {
                _renderWebBrowser->SetPopupSizeAndPosition(rect.width, rect.height, rect.x, rect.y);
            };

            virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects,
                const void* buffer, int width, int height) OVERRIDE
            {
                auto bitmapInfo = type == PET_VIEW ? _mainBitmapInfo : _popupBitmapInfo;

                lock l(bitmapInfo->BitmapLock);

                if(bitmapInfo->DirtyRectSupport)
                {
                    //NOTE: According to https://bitbucket.org/chromiumembedded/cef/commits/1ddb0ba41d7052eaad50b8d9de959f3b5e05ff21?at=master
                    // There is only one rect now that's a union of all dirty regions. API Still passes in a vector

                    CefRect r = dirtyRects.front();
                    bitmapInfo->DirtyRect = Rect(r.x, r.y, r.width, r.height);
                }

                auto backBufferHandle = (HANDLE)bitmapInfo->BackBufferHandle;

                if (backBufferHandle == NULL || bitmapInfo->Width != width || bitmapInfo->Height != height)
                {
                    int pixels = width * height;
                    int numberOfBytes = pixels * bitmapInfo->BytesPerPixel;
                    auto fileMappingHandle = (HANDLE)bitmapInfo->FileMappingHandle;

                    //Clear the reference to Bitmap so a new one is created by InvokeRenderAsync
                    bitmapInfo->ClearBitmap();

                    //Release the current handles (if not null)
                    ReleaseBitmapHandlers(bitmapInfo);

                    // Create new fileMappingHandle
                    fileMappingHandle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, numberOfBytes, NULL);
                    if (fileMappingHandle == NULL)
                    {
                        // TODO: Consider doing something more sensible here, since the browser will be very badly broken if this
                        // TODO: method call fails.
                        return;
                    }

                    backBufferHandle = MapViewOfFile(fileMappingHandle, FILE_MAP_ALL_ACCESS, 0, 0, numberOfBytes);
                    if (backBufferHandle == NULL)
                    {
                        // TODO: Consider doing something more sensible here, since the browser will be very badly broken if this
                        // TODO: method call fails.
                        return;
                    }

                    bitmapInfo->FileMappingHandle = (IntPtr)fileMappingHandle;
                    bitmapInfo->BackBufferHandle = (IntPtr)backBufferHandle;
                    bitmapInfo->Width = width;
                    bitmapInfo->Height = height;
                    bitmapInfo->NumberOfBytes = numberOfBytes;
                }               

                CopyMemory(backBufferHandle, (void*)buffer, bitmapInfo->NumberOfBytes);

                _renderWebBrowser->OnPaint(bitmapInfo);
            };

            virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor, CursorType type,
                const CefCursorInfo& custom_cursor_info) OVERRIDE
            {
                _renderWebBrowser->SetCursor((IntPtr)cursor, (CefSharp::CursorType)type);
            };

            virtual DECL bool StartDragging(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDragData> dragData,
                CefRenderHandler::DragOperationsMask allowedOps, int x, int y)
            {
                CefDragDataWrapper dragDataWrapper(dragData);
                return _renderWebBrowser->StartDragging(%dragDataWrapper, (CefSharp::DragOperationsMask)allowedOps, x, y);
            }

            ///
            // Called when the web view wants to update the mouse cursor during a
            // drag & drop operation. |operation| describes the allowed operation
            // (none, move, copy, link).
            ///
            /*--cef()--*/
            virtual DECL void UpdateDragCursor(CefRefPtr<CefBrowser> browser, CefRenderHandler::DragOperation operation)
            {
                return _renderWebBrowser->UpdateDragCursor((CefSharp::DragOperationsMask)operation);
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

        private:
            void ReleaseBitmapHandlers(BitmapInfo^ bitmapInfo)
            {
                if(bitmapInfo)
                {
                    auto backBufferHandle = (HANDLE)bitmapInfo->BackBufferHandle;
                    auto fileMappingHandle = (HANDLE)bitmapInfo->FileMappingHandle;

                    if (backBufferHandle != NULL)
                    {
                        UnmapViewOfFile(backBufferHandle);
                        backBufferHandle = NULL;
                        bitmapInfo->BackBufferHandle = IntPtr::Zero;
                    }

                    if (fileMappingHandle != NULL)
                    {
                        CloseHandle(fileMappingHandle);
                        fileMappingHandle = NULL;
                        bitmapInfo->FileMappingHandle = IntPtr::Zero;
                    }
                }
            }

            IMPLEMENT_REFCOUNTING(RenderClientAdapter)
        };
    }
}
