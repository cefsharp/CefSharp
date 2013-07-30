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
        private class RenderClientAdapter : public ClientAdapter,
            public CefRenderHandler
        {
        private:
            gcroot<IRenderWebBrowser^> _renderWebBrowser;
            gcroot<Action^> _setBitmapDelegate, _setPopupBitmapDelegate;
            HANDLE _backBufferHandle, _popupBackBufferHandle;

        public:
            gcroot<Object^> BitmapLock;
            int BitmapWidth;
            int BitmapHeight;

            RenderClientAdapter(IRenderWebBrowser^ offscreenBrowserControl) :
                ClientAdapter(offscreenBrowserControl),
                BitmapWidth(0), 
                BitmapHeight(0),
                _backBufferHandle(NULL),
                _popupBackBufferHandle(NULL),
                _renderWebBrowser(offscreenBrowserControl)
            {
                BitmapLock = gcnew Object();

                _setBitmapDelegate = gcnew Action(offscreenBrowserControl, &IRenderWebBrowser::SetBitmap);
                _setPopupBitmapDelegate = gcnew Action(offscreenBrowserControl, &IRenderWebBrowser::SetPopupBitmap);
            }

            ~RenderClientAdapter()
            {
                _renderWebBrowser = nullptr;
            }

            // CefClient
            virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE { return this; }

            // CefRenderHandler
            virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE
            {
                if ((IRenderWebBrowser^) _renderWebBrowser == nullptr)
                {
                    return false;
                }
                else
                {
                    rect = CefRect(0, 0, _renderWebBrowser->Width, _renderWebBrowser->Height);
                    return true;
                }
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

            CefRefPtr<CefBrowserHost> TryGetCefHost()
            {
                if (!this->GetCefBrowser().get() ||
                    !this->GetCefBrowser()->GetHost().get())
                {
                    return nullptr;
                }
                else
                {
                    return this->GetCefBrowser()->GetHost();
                }
            }

            CefRefPtr<CefFrame> TryGetCefMainFrame()
            {
                auto cefBrowser = this->GetCefBrowser().get();

                if (!cefBrowser)
                {
                    return nullptr;
                }

                return cefBrowser->GetMainFrame();
            }

        private:

            void SetBuffer(int newWidth, int newHeight, const void* buffer)
            {
                lock l(BitmapLock);

                int currentWidth = BitmapWidth, currentHeight = BitmapHeight;
                auto fileMappingHandle = (HANDLE) _renderWebBrowser->FileMappingHandle, backBufferHandle = _backBufferHandle;

                SetBufferHelper(currentWidth, currentHeight, newWidth, newHeight, fileMappingHandle, backBufferHandle, buffer);

                _renderWebBrowser->FileMappingHandle = (IntPtr) fileMappingHandle;
                _backBufferHandle = backBufferHandle;

                BitmapWidth = currentWidth;
                BitmapHeight = currentHeight;

                _renderWebBrowser->InvokeRenderAsync(_setBitmapDelegate);
            }

            void SetPopupBuffer(int width, int height, const void* buffer)
            {
                // TODO: implement
            }

            void SetBufferHelper(int &currentWidth, int& currentHeight, int width, int height, HANDLE& fileMappingHandle,
                HANDLE& backBufferHandle, const void* buffer)
            {
                int pixels = width * height;
                int numberOfBytes = pixels * _renderWebBrowser->BytesPerPixel;

                if (!backBufferHandle ||
                    currentWidth != width ||
                    currentHeight != height)
                {
                    _renderWebBrowser->ClearBitmap();

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
            }

            IMPLEMENT_REFCOUNTING(RenderClientAdapterInternal)
        };
    }
}
