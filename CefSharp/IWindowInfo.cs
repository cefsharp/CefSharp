// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        uint ExStyle { get; set; }
        IntPtr ParentWindowHandle { get; set; }
        bool WindowlessRenderingEnabled { get; set; }
        IntPtr WindowHandle { get; set; }

        void SetAsChild(IntPtr parentHandle, int left, int top, int right, int bottom);
        void SetAsPopup(IntPtr parentHandle, string windowName);

        /// <summary>
        /// Create the browser using windowless (off-screen) rendering.
        /// No window will be created for the browser and all rendering will occur via the CefRenderHandler interface. This window will automatically be transparent unless a colored backgrond is set in the browser settings.
        /// </summary>
        /// <param name="parentHandle">Value will be used to identify monitor info and to act as the parent window for dialogs, context menus, etc.
        /// If not provided then the main screen monitor will be used and some functionality that requires a parent window may not function correctly.</param>
        /// In order to create windowless browsers the CefSettings.windowless_rendering_enabled value must be set to true.</param>
        void SetAsWindowless(IntPtr parentHandle);
    }
}
