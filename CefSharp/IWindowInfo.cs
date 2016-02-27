// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public interface IWindowInfo : IDisposable
    {
        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        uint Style { get; set; }
        IntPtr ParentWindowHandle { get; set; }
        bool TransparentPaintingEnabled { get; set; }
        bool WindowlessRenderingEnabled { get; set; }
        IntPtr WindowHandle { get; set; }

        void SetAsChild(IntPtr parentHandle, int left, int top, int right, int bottom);
        void SetAsPopup(IntPtr parentHandle, string windowName);
        void SetAsWindowless(IntPtr parentHandle, bool transparent);
    }
}
