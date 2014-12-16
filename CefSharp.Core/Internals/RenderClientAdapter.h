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

                DisposeBitmapInfo(MainBitmapInfo);

                delete MainBitmapInfo;
                MainBitmapInfo = nullptr;

                DisposeBitmapInfo(PopupBitmapInfo);

                delete PopupBitmapInfo;
                PopupBitmapInfo = nullptr;
            }

            void DisposeBitmapInfo(BitmapInfo^ bitmapInfo)
            {
                auto backBufferHandle = (HANDLE)bitmapInfo->BackBufferHandle;
                auto fileMappingHandle = (HANDLE)bitmapInfo->FileMappingHandle;

                ReleaseBitmapHandlers(&backBufferHandle, &fileMappingHandle);
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
                SetBuffer(type == PET_VIEW ? MainBitmapInfo : PopupBitmapInfo, width, height, buffer);
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
                
                    cefHost->ShowDevTools(windowInfo, this, settings, CefPoint());
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

                auto fileMappingHandle = (HANDLE)bitmapInfo->FileMappingHandle;
                auto backBufferHandle = (HANDLE)bitmapInfo->BackBufferHandle;

                SetBufferHelper(bitmapInfo, newWidth, newHeight, &fileMappingHandle,
                    &backBufferHandle, buffer);

                bitmapInfo->FileMappingHandle = (IntPtr)fileMappingHandle;
                bitmapInfo->BackBufferHandle = (IntPtr)backBufferHandle;

                _renderWebBrowser->InvokeRenderAsync(bitmapInfo);
            };

            void SetBufferHelper(BitmapInfo^ bitmapInfo, int newWidth, int newHeight,
                HANDLE* fileMappingHandle, HANDLE* backBufferHandle, const void* buffer)
            {
                int pixels = newWidth * newHeight;
                int numberOfBytes = pixels * _renderWebBrowser->BytesPerPixel;

                if (*backBufferHandle == NULL ||
                    bitmapInfo->Width != newWidth ||
                    bitmapInfo->Height != newHeight)
                {
                    //Clear the reference to InteropBitmap so a new one is created by InvokeRenderAsync
                    bitmapInfo->InteropBitmap = nullptr;

                    //Release the current handles (if not null)
                    ReleaseBitmapHandlers(backBufferHandle, fileMappingHandle);

                    // Create new fileMappingHandle
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
                }

                bitmapInfo->Width = newWidth;
                bitmapInfo->Height = newHeight;

                CopyMemory(*backBufferHandle, (void*)buffer, numberOfBytes);
            };

            void ReleaseBitmapHandlers(HANDLE* backBufferHandle, HANDLE* fileMappingHandle)
            {
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
            }

            IMPLEMENT_REFCOUNTING(RenderClientAdapter)
        };
    }
}
