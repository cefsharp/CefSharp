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
            gcroot<IWebBrowserInternal^> _webBrowserInternal;
            gcroot<IRenderWebBrowser^> _renderWebBrowser;

        public:
            gcroot<BitmapInfo^> MainBitmapInfo;
            gcroot<BitmapInfo^> PopupBitmapInfo;

            RenderClientAdapter(IWebBrowserInternal^ webBrowserInternal, Action<int>^ onAfterBrowserCreated):
                ClientAdapter(webBrowserInternal, onAfterBrowserCreated),
                _webBrowserInternal(webBrowserInternal)
            {
                MainBitmapInfo = gcnew BitmapInfo();
                PopupBitmapInfo = gcnew BitmapInfo();
                PopupBitmapInfo->IsPopup = true;

                _renderWebBrowser = dynamic_cast<IRenderWebBrowser^>(webBrowserInternal);
            }

            ~RenderClientAdapter()
            {
                _renderWebBrowser = nullptr;
                _webBrowserInternal = nullptr;

                delete MainBitmapInfo;
                MainBitmapInfo = nullptr;

                delete PopupBitmapInfo;
                PopupBitmapInfo = nullptr;
            }

            // CefClient
            virtual CefRefPtr<CefRenderHandler> GetRenderHandler() OVERRIDE{ return this; };

            // CefRenderHandler
            virtual DECL bool GetViewRect(CefRefPtr<CefBrowser> browser, CefRect& rect) OVERRIDE
            {
                if ((IRenderWebBrowser^)_renderWebBrowser == nullptr)
                {
                    return false;
                }

                rect = CefRect(0, 0, _renderWebBrowser->Width, _renderWebBrowser->Height);
                return true;
            };

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
                if (type == PET_VIEW)
                {
                    SetBuffer(MainBitmapInfo, width, height, buffer);
                }
                else if (type == PET_POPUP)
                {
                    SetBuffer(PopupBitmapInfo, width, height, buffer);
                }
            };

            virtual DECL void OnCursorChange(CefRefPtr<CefBrowser> browser, CefCursorHandle cursor) OVERRIDE
            {
                _renderWebBrowser->SetCursor((IntPtr)cursor);
            };

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
            };

            CefRefPtr<CefFrame> TryGetCefMainFrame()
            {
                auto cefBrowser = this->GetCefBrowser().get();

                if (!cefBrowser)
                {
                    return nullptr;
                }

                return cefBrowser->GetMainFrame();
            };

            void ShowDevTools()
            {
                auto cefHost = TryGetCefHost();

                if (cefHost != nullptr)
                {
                    CefWindowInfo windowInfo;
                    CefBrowserSettings settings;

                    windowInfo.SetAsPopup(cefHost->GetWindowHandle(), "DevTools");
                
                    cefHost->ShowDevTools(windowInfo, this, settings);
                }
            }

            void CloseDevTools()
            {
                auto cefHost = TryGetCefHost();

                if (cefHost != nullptr)
                {
                    cefHost->CloseDevTools();
                }
            }

        private:

            void SetBuffer(BitmapInfo^ bitmapInfo, int newWidth, int newHeight, const void* buffer)
            {
                lock l(bitmapInfo->BitmapLock);

                int currentWidth = bitmapInfo->Width, currentHeight = bitmapInfo->Height;

                auto fileMappingHandle = (HANDLE)bitmapInfo->FileMappingHandle;
                auto backBufferHandle = (HANDLE)bitmapInfo->BackBufferHandle;

                SetBufferHelper(bitmapInfo, currentWidth, currentHeight, newWidth, newHeight, &fileMappingHandle,
                    &backBufferHandle, buffer);

                bitmapInfo->FileMappingHandle = (IntPtr)fileMappingHandle;
                bitmapInfo->BackBufferHandle = (IntPtr)backBufferHandle;

                bitmapInfo->Width = newWidth;
                bitmapInfo->Height = newHeight;

                _renderWebBrowser->InvokeRenderAsync(bitmapInfo);
            };

            void SetBufferHelper(BitmapInfo^ bitmapInfo, int &currentWidth, int& currentHeight, int width, int height,
                HANDLE* fileMappingHandle, HANDLE* backBufferHandle, const void* buffer)
            {
                int pixels = width * height;
                int numberOfBytes = pixels * _renderWebBrowser->BytesPerPixel;

                if (*backBufferHandle == NULL ||
                    currentWidth != width ||
                    currentHeight != height)
                {
                    _renderWebBrowser->ClearBitmap(bitmapInfo);

                    if (*backBufferHandle != NULL)
                    {
                        UnmapViewOfFile(*backBufferHandle);
                        *backBufferHandle = NULL;
                    }

                    if (*fileMappingHandle != NULL)
                    {
                        CloseHandle(*fileMappingHandle);
                        *fileMappingHandle = NULL;
                    }

                    *fileMappingHandle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, numberOfBytes, NULL);
                    if (*fileMappingHandle == NULL)
                    {
                        // TODO: Consider doing something more sensible here, since the browser will be very badly broken if this
                        // TODO: method call fails.
                        return;
                    }

                    *backBufferHandle = MapViewOfFile(*fileMappingHandle, FILE_MAP_ALL_ACCESS, 0, 0, numberOfBytes);
                    if (*backBufferHandle == NULL)
                    {
                        // TODO: Consider doing something more sensible here, since the browser will be very badly broken if this
                        // TODO: method call fails.
                        return;
                    }

                    currentWidth = width;
                    currentHeight = height;
                }

                CopyMemory(*backBufferHandle, (void*)buffer, numberOfBytes);
            };

            IMPLEMENT_REFCOUNTING(RenderClientAdapter)
        };
    }
}
