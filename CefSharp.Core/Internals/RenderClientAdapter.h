// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "ClientAdapter.h"

using namespace msclr;
using namespace System;

namespace CefSharp
{
    namespace Internals
    {
        private class RenderClientAdapter : public ClientAdapter,
            public CefRenderHandler
        {
        private:
            gcroot<IRenderWebBrowser^> _renderWebBrowser;
            gcroot<BitmapInfo^> _mainBitmapInfo;
            gcroot<BitmapInfo^> _popupBitmapInfo;

        public:
            RenderClientAdapter(IWebBrowserInternal^ webBrowserInternal, Action<int>^ onAfterBrowserCreated):
                ClientAdapter(webBrowserInternal, onAfterBrowserCreated)
            {
                _renderWebBrowser = dynamic_cast<IRenderWebBrowser^>(webBrowserInternal);
            }

            ~RenderClientAdapter()
            {
                _renderWebBrowser = nullptr;

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
            virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE{ return this; };

            // CefRenderHandler
            virtual DECL bool GetScreenInfo(CefRefPtr<CefBrowser> browser, CefScreenInfo& screen_info) OVERRIDE
            {
                if ((IRenderWebBrowser^)_renderWebBrowser == nullptr)
                {
                    return false;
                }

                auto screenInfo = _renderWebBrowser->GetScreenInfo();

                /*if (screen_info.device_scale_factor == screenInfo.ScaleFactor)
                {
                    return false;
                }*/

                screen_info.device_scale_factor = screenInfo.ScaleFactor;
                screen_info.rect = CefRect(screenInfo.X, screenInfo.Y, screenInfo.Width, screenInfo.Height);
                screen_info.available_rect = CefRect(screenInfo.AvailableX, screenInfo.AvailableY, screenInfo.AvailableWidth, screenInfo.AvailableHeight);
                return true;
            }


                ///
                // Called to retrieve the root window rectangle in screen coordinates. Return
                // true if the rectangle was provided.
                ///
                /*--cef()--*/
            virtual DECL bool GetRootScreenRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE
            {
                if ((IRenderWebBrowser^)_renderWebBrowser == nullptr)
                {
                    return false;
                }

                auto test = _renderWebBrowser->GetScreenInfo();
                rect = CefRect(test.AvailableX, test.AvailableY, test.AvailableWidth, test.AvailableHeight);
                return true;
            }

            // CefRenderHandler
            virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE
            {
                if ((IRenderWebBrowser^)_renderWebBrowser == nullptr)
                {
                    return false;
                }

                auto test = _renderWebBrowser->GetViewInfo();
                //auto screenInfo = _renderWebBrowser->GetScreenInfo();

                rect = CefRect(test.X, test.Y, test.Width, test.Height);
                return true;
            };

            ///
            // Called to retrieve the translation from view coordinates to actual screen
            // coordinates. Return true if the screen coordinates were provided.
            ///
            /*--cef()--*/
            virtual DECL virtual bool GetScreenPoint(CefRefPtr<CefBrowser> browser,int viewX,int viewY,int& screenX,int& screenY)  OVERRIDE
            {
                if ((IRenderWebBrowser^)_renderWebBrowser == nullptr)
                {
                    return false;
                }

                _renderWebBrowser->GetScreenPoint(viewX, viewY, screenX, screenY);
                return true;
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
            virtual void OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect) OVERRIDE
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
                    //NOTE: According to https://bitbucket.org/chromiumembedded/branches-2171-cef3/commits/ce984ddff3268a50cf9967487327e1257015b98c
                    // There is only one rect now that's a union of all dirty regions. API Still passes in a vector

                    CefRect r = dirtyRects.front();
                    bitmapInfo->DirtyRect = CefDirtyRect(r.x, r.y, r.width, r.height);
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

                _renderWebBrowser->InvokeRenderAsync(bitmapInfo);
            };

            virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor, CursorType type,
                const CefCursorInfo& custom_cursor_info) OVERRIDE
            {
                _renderWebBrowser->SetCursor((IntPtr)cursor);
            };

        private:
            void ReleaseBitmapHandlers(BitmapInfo^ bitmapInfo)
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

            IMPLEMENT_REFCOUNTING(RenderClientAdapter)
        };
    }
}
