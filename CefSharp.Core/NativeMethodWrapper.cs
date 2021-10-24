// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System;

namespace CefSharp
{
    /// <summary>
    /// Native static methods for low level operations, memory copy
    /// Avoids having to P/Invoke as we can call the C++ API directly.
    /// </summary>
    public static class NativeMethodWrapper
    {
        public static void MemoryCopy(IntPtr dest, IntPtr src, int numberOfBytes)
        {
            CefSharp.Core.NativeMethodWrapper.MemoryCopy(dest, src, numberOfBytes);
        }

        public static bool IsFocused(IntPtr handle)
        {
            return CefSharp.Core.NativeMethodWrapper.IsFocused(handle);
        }

        public static void SetWindowPosition(IntPtr handle, int x, int y, int width, int height)
        {
            CefSharp.Core.NativeMethodWrapper.SetWindowPosition(handle, x, y, width, height);
        }

        public static void SetWindowParent(IntPtr child, IntPtr newParent)
        {
            CefSharp.Core.NativeMethodWrapper.SetWindowParent(child, newParent);
        }

        public static void RemoveExNoActivateStyle(IntPtr browserHwnd)
        {
            CefSharp.Core.NativeMethodWrapper.RemoveExNoActivateStyle(browserHwnd);
        }
    }
}
