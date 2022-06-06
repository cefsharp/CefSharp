// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\internal\cef_win.h"

namespace CefSharp
{
    namespace Core
    {
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class WindowInfo : public IWindowInfo
        {
        private:
            CefWindowInfo* _windowInfo;
            bool _ownsPointer = false;

        internal:
            WindowInfo(CefWindowInfo* windowInfo) : _windowInfo(windowInfo)
            {

            }

            CefWindowInfo* GetWindowInfo()
            {
                return _windowInfo;
            }

        public:
            WindowInfo() : _windowInfo(new CefWindowInfo())
            {
                _ownsPointer = true;
            }

            !WindowInfo()
            {
                if (_ownsPointer)
                {
                    delete _windowInfo;
                }

                _windowInfo = nullptr;
            }

            ~WindowInfo()
            {
                this->!WindowInfo();
            }

            virtual property int X
            {
                int get()
                {
                    return _windowInfo->bounds.x;
                }
                void set(int x)
                {
                    _windowInfo->bounds.x = x;
                }
            }

            virtual property int Y
            {
                int get()
                {
                    return _windowInfo->bounds.y;
                }
                void set(int y)
                {
                    _windowInfo->bounds.y = y;
                }
            }

            virtual property int Width
            {
                int get()
                {
                    return _windowInfo->bounds.width;
                }
                void set(int width)
                {
                    _windowInfo->bounds.width = width;
                }
            }

            virtual property int Height
            {
                int get()
                {
                    return _windowInfo->bounds.height;
                }
                void set(int height)
                {
                    _windowInfo->bounds.height = height;
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

            virtual property UINT32 ExStyle
            {
                UINT32 get()
                {
                    return _windowInfo->ex_style;
                }
                void set(UINT32 ex_style)
                {
                    _windowInfo->ex_style = ex_style;
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

            virtual property String^ WindowName
            {
                String^ get()
                {
                    return StringUtils::ToClr(_windowInfo->window_name);
                }
                void set(String^ value)
                {
                    StringUtils::AssignNativeFromClr(_windowInfo->window_name, value);
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

            virtual property bool SharedTextureEnabled
            {
                bool get()
                {
                    return _windowInfo->shared_texture_enabled == 1;
                }
                void set(bool sharedTextureEnabled)
                {
                    _windowInfo->shared_texture_enabled = sharedTextureEnabled;
                }
            }

            virtual property bool ExternalBeginFrameEnabled
            {
                bool get()
                {
                    return _windowInfo->external_begin_frame_enabled == 1;
                }
                void set(bool externalBeginFrameEnabled)
                {
                    _windowInfo->external_begin_frame_enabled = externalBeginFrameEnabled;
                }
            }

            virtual void SetAsChild(IntPtr parentHandle)
            {
                CefWindowInfo window;
                CefRect windowBounds;

                HWND hwnd = static_cast<HWND>(parentHandle.ToPointer());

                RECT rect;
                GetClientRect(hwnd, &rect);

                windowBounds.x = rect.left;
                windowBounds.y = rect.top;
                windowBounds.width = rect.right - rect.left;
                windowBounds.height = rect.bottom - rect.top;

                _windowInfo->SetAsChild(hwnd, windowBounds);
            }

            virtual void SetAsChild(IntPtr parentHandle, CefSharp::Structs::Rect windowBounds)
            {
                auto rect = CefRect(windowBounds.X, windowBounds.Y, windowBounds.Width, windowBounds.Height);
                
                _windowInfo->SetAsChild((HWND)parentHandle.ToPointer(), rect);
            }

            virtual void SetAsChild(IntPtr parentHandle, int left, int top, int right, int bottom)
            {
                CefRect windowBounds;
                windowBounds.x = left;
                windowBounds.y = top;
                windowBounds.width = right - left;
                windowBounds.height = bottom - top;

                _windowInfo->SetAsChild((HWND)parentHandle.ToPointer(), windowBounds);
            }

            virtual void SetAsPopup(IntPtr parentHandle, String^ windowName)
            {
                _windowInfo->SetAsPopup((HWND)parentHandle.ToPointer(), StringUtils::ToNative(windowName));
            }

            virtual void SetAsWindowless(IntPtr parentHandle)
            {
                _windowInfo->SetAsWindowless((HWND)parentHandle.ToPointer());
            }

            virtual IWindowInfo^ UnWrap()
            {
                return this;
            }
        };
    }
}
