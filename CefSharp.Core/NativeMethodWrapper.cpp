// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "NativeMethodWrapper.h"

namespace CefSharp
{
    void NativeMethodWrapper::MemoryCopy(IntPtr dest, IntPtr src, int numberOfBytes)
    {
        RtlCopyMemory(dest.ToPointer(), src.ToPointer(), numberOfBytes);
    }

    bool NativeMethodWrapper::IsFocused(IntPtr handle)
    {
        // Ask Windows which control has the focus and then check if it's one of our children
        auto focusControl = GetFocus();
        return focusControl != 0 && (IsChild((HWND)handle.ToPointer(), focusControl) == 1);
    }

    void NativeMethodWrapper::SetWindowPosition(IntPtr handle, int x, int y, int width, int height)
    {
        HWND browserHwnd = static_cast<HWND>(handle.ToPointer());
        if (browserHwnd)
        {
            if (width == 0 && height == 0)
            {
                // For windowed browsers when the frame window is minimized set the
                // browser window size to 0x0 to reduce resource usage.
                SetWindowPos(browserHwnd, NULL, 0, 0, 0, 0, SWP_NOZORDER | SWP_NOMOVE | SWP_NOACTIVATE);
            }
            else
            {
                SetWindowPos(browserHwnd, NULL, x, y, width, height, SWP_NOZORDER);
            }
        }
    }

    void NativeMethodWrapper::SetWindowParent(IntPtr child, IntPtr newParent)
    {
        HWND childHwnd = static_cast<HWND>(child.ToPointer());
        HWND newParentHwnd = static_cast<HWND>(newParent.ToPointer());

        SetParent(childHwnd, newParentHwnd);
    }
}
