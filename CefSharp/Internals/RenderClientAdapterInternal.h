// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "ClientAdapter.h"

using namespace msclr;
using namespace System;

namespace CefSharp
{
    interface class IRenderWebBrowser;

    namespace Internals
    {
        private class RenderClientAdapterInternal : public ClientAdapter,
            public CefRenderHandler
        {
        private:
            gcroot<IRenderWebBrowser^> _renderWebBrowser;
            gcroot<Object^> _sync;
            gcroot<Action^> _setBitmapDelegate, _setPopupBitmapDelegate;
            HANDLE _backBufferHandle, _popupBackBufferHandle;

            int _width;
            int _height;

        public:
            RenderClientAdapterInternal(IRenderWebBrowser^ offscreenBrowserControl) :
                ClientAdapter(offscreenBrowserControl),
            
                // TODO: Get these from the IRenderWebBrowser instead of hardwiring.
                _width(500), _height(500),
                
                _backBufferHandle(NULL),
                _popupBackBufferHandle(NULL)
            {
                _renderWebBrowser = offscreenBrowserControl;
                _sync = gcnew Object();

                _setBitmapDelegate = gcnew Action(offscreenBrowserControl, &IRenderWebBrowser::SetBitmap);
                _setPopupBitmapDelegate = gcnew Action(offscreenBrowserControl, &IRenderWebBrowser::SetPopupBitmap);
            }

            ~RenderClientAdapterInternal() { _renderWebBrowser = nullptr; }

            // CefClient
            virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

            // CefRenderHandler
            virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE
            {
                // TODO: add a real implementation... :)
                rect = CefRect(0, 0, _width, _height);
                return true;
            }

            virtual DECL void OnPopupShow(CefRefPtr<CefBrowser> browser, bool show) OVERRIDE
            {
                _renderWebBrowser->SetPopupIsOpen(show);
            }

            virtual DECL void OnPopupSize(CefRefPtr<CefBrowser> browser, const CefRect& rect) OVERRIDE
            {
                _renderWebBrowser->SetPopupSizeAndPosition((IntPtr) (void*) &rect);
            }

            virtual DECL void OnPaint(CefRefPtr<CefBrowser> browser, PaintElementType type, const RectList& dirtyRects,
                const void* buffer, int width, int height) OVERRIDE
            {
                if (type == PET_VIEW)
                {
                    SetBuffer(width, height, buffer);
                }
                else if (type == PET_POPUP)
                {
                    SetPopupBuffer(width, height, buffer);
                }
            }

            virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE
            {
                _renderWebBrowser->SetCursor((IntPtr) cursor);
            }

        private:

            void SetBuffer(int width, int height, const void* buffer)
            {
                lock l(_sync);

                int currentWidth = _width, currentHeight = _height;
                HANDLE fileMappingHandle = (HANDLE) _renderWebBrowser->FileMappingHandle, backBufferHandle = _backBufferHandle;

                SetBufferHelper(currentWidth, currentHeight, width, height, fileMappingHandle, backBufferHandle,
                    _setBitmapDelegate, buffer);

                _renderWebBrowser->FileMappingHandle = (IntPtr) fileMappingHandle;
                _backBufferHandle = backBufferHandle;

                _width = currentWidth;
                _height = currentHeight;
            }

            void SetPopupBuffer(int width, int height, const void* buffer)
            {
                // TODO: implement
            }

            void SetBufferHelper(int &currentWidth, int& currentHeight, int width, int height, HANDLE& fileMappingHandle,
                HANDLE& backBufferHandle, Action^ paintDelegate, const void* buffer)
            {
                int pixels = width * height;
                int numberOfBytes = pixels * _renderWebBrowser->BytesPerPixel;

                if (!backBufferHandle ||
                    currentWidth != width ||
                    currentHeight != height)
                {
                    if (backBufferHandle)
                    {
                        UnmapViewOfFile(backBufferHandle);
                        backBufferHandle = NULL;
                    }

                    if (fileMappingHandle)
                    {
                        CloseHandle(fileMappingHandle);
                        fileMappingHandle = NULL;
                    }


                    fileMappingHandle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, numberOfBytes, NULL);
                    if (!fileMappingHandle) 
                    {
                        return;
                    }

                    backBufferHandle = MapViewOfFile(fileMappingHandle, FILE_MAP_ALL_ACCESS, 0, 0, numberOfBytes);
                    if (!backBufferHandle) 
                    {
                        return;
                    }

                    currentWidth = width;
                    currentHeight = height;
                }

                CopyMemory(backBufferHandle, (void*) buffer, numberOfBytes);

                _renderWebBrowser->InvokeRenderAsync(paintDelegate);
            }

            IMPLEMENT_REFCOUNTING(RenderClientAdapterInternal)
        };
    }
}
