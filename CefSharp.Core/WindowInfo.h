// Copyright � 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\internal\cef_win.h"

namespace CefSharp
{
    public ref class WindowInfo : public IWindowInfo
    {
    private:
        CefWindowInfo* _windowInfo;

    public:
        WindowInfo() : _windowInfo(new CefWindowInfo())
        {
        }

        WindowInfo(CefWindowInfo* windowInfo) : _windowInfo(windowInfo)
        {
            
        }

        !WindowInfo()
        {
            _windowInfo = NULL;
        }

        ~WindowInfo()
        {
            this->!WindowInfo();
        }

        virtual property int X
        {
            int get()
            {
                return _windowInfo->x;
            }
            void set(int x)
            {
                _windowInfo->x = x;
            }
        }

        virtual property int Y
        {
            int get()
            {
                return _windowInfo->y;
            }
            void set(int y)
            {
                _windowInfo->y = y;
            }
        }

        virtual property int Width
        {
            int get()
            {
                return _windowInfo->width;
            }
            void set(int width)
            {
                _windowInfo->width = width;
            }
        }

        virtual property int Height
        {
            int get()
            {
                return _windowInfo->height;
            }
            void set(int height)
            {
                _windowInfo->height = height;
            }
        }

        virtual property UINT32 Style
        {
            UINT32 get()
            {
                return _windowInfo->style;
            }
            void set(UINT32 style)
            {
                _windowInfo->style = style;
            }
        }

        virtual property IntPtr ParentWindowHandle
        {
            IntPtr get()
            {
                return IntPtr(_windowInfo->parent_window);
            }
            void set(IntPtr parentWindowHandle)
            {
                _windowInfo->parent_window = (HWND)parentWindowHandle.ToPointer();
            }
        }

        virtual property IntPtr WindowHandle
        {
            IntPtr get()
            {
                return IntPtr(_windowInfo->window);
            }
            void set(IntPtr windowHandle)
            {
                _windowInfo->window = (HWND)windowHandle.ToPointer();
            }
        }

        virtual property bool TransparentPaintingEnabled
        {
            bool get()
            {
                return _windowInfo->transparent_painting_enabled == 1;
            }
            void set(bool transparentPaintingEnabled)
            {
                _windowInfo->transparent_painting_enabled = transparentPaintingEnabled;
            }
        }

        virtual property bool WindowlessRenderingEnabled
        {
            bool get()
            {
                return _windowInfo->windowless_rendering_enabled == 1;
            }
            void set(bool windowlessRenderingEnabled)
            {
                _windowInfo->windowless_rendering_enabled = windowlessRenderingEnabled;
            }
        }

        virtual void SetAsChild(IntPtr parentHandle, int left, int top, int right, int bottom)
        {
            RECT rect;
            rect.left = left;
            rect.top = top;
            rect.right = right;
            rect.bottom = bottom;
            _windowInfo->SetAsChild((HWND)parentHandle.ToPointer(), rect);
        }

        virtual void SetAsPopup(IntPtr parentHandle, String^ windowName)
        {
            _windowInfo->SetAsPopup((HWND)parentHandle.ToPointer(), StringUtils::ToNative(windowName));
        }

        virtual void SetAsWindowless(IntPtr parentHandle, bool transparent)
        {
            _windowInfo->SetAsWindowless((HWND)parentHandle.ToPointer(), transparent);
        }

        CefWindowInfo* GetWindowInfo()
        {
            return _windowInfo;
        }
    };
}