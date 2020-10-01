// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Class representing window information.
    /// </summary>
    public interface IWindowInfo : IDisposable
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        int X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        int Y { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        int Height { get; set; }
        /// <summary>
        /// Window style
        /// </summary>
        uint Style { get; set; }
        /// <summary>
        /// Ex window style
        /// </summary>
        uint ExStyle { get; set; }
        /// <summary>
        /// Parent window handle
        /// </summary>
        IntPtr ParentWindowHandle { get; set; }
        /// <summary>
        /// Set to true to create the browser using windowless (off-screen) rendering.
        /// No window will be created for the browser and all rendering will occur via the
        /// IRenderHandler interface. The <see cref="ParentWindowHandle"/> value will be used to identify monitor info
        /// and to act as the parent window for dialogs, context menus, etc. If |<see cref="ParentWindowHandle"/> is not provided then the main screen monitor will be used and some functionality that requires a parent window may not function correctly.
        /// In order to create windowless browsers the CefSettings.WindowlessRenderingEnabled value must be set to true.
        /// Transparent painting is enabled by default but can be disabled by setting <see cref="IBrowserSettings.BackgroundColor"/> to an opaque value.
        /// </summary>
        bool WindowlessRenderingEnabled { get; set; }
        /// <summary>
        /// Set to true to enable shared textures for windowless rendering. Only
        /// valid if <see cref="WindowlessRenderingEnabled"/> is also set to true. Currently
        /// only supported on Windows (D3D11). This feature is experimental and has many bugs
        /// at the moment.
        /// </summary>
        bool SharedTextureEnabled { get; set; }
        /// <summary>
        /// Set to true to enable the ability to issue BeginFrame requests from the
        /// client application by calling <see cref="IBrowserHost.SendExternalBeginFrame"/>.
        /// </summary>
        bool ExternalBeginFrameEnabled { get; set; }
        /// <summary>
        /// Handle for the new browser window. Only used with windowed rendering.
        /// </summary>
        IntPtr WindowHandle { get; set; }

        /// <summary>
        /// Create the browser as a child window.
        /// Calls GetClientRect(Hwnd) to obtain the window bounds
        /// </summary>
        /// <param name="parentHandle">parent handle</param>
        void SetAsChild(IntPtr parentHandle);

        /// <summary>
        /// Create the browser as a child window.
        /// </summary>
        /// <param name="parentHandle">parent handle</param>
        /// <param name="left">left</param>
        /// <param name="top">top</param>
        /// <param name="right">right</param>
        /// <param name="bottom">bottom</param>
        void SetAsChild(IntPtr parentHandle, int left, int top, int right, int bottom);
        /// <summary>
        /// Create the browser as a popup window.
        /// </summary>
        /// <param name="parentHandle">parent handle</param>
        /// <param name="windowName">window name</param>
        void SetAsPopup(IntPtr parentHandle, string windowName);

        /// <summary>
        /// Create the browser using windowless (off-screen) rendering.
        /// No window will be created for the browser and all rendering will occur via the CefRenderHandler interface. This window will automatically be transparent unless a colored backgrond is set in the browser settings.
        /// </summary>
        /// <param name="parentHandle">Value will be used to identify monitor info and to act as the parent window for dialogs, context menus, etc.
        /// If not provided then the main screen monitor will be used and some functionality that requires a parent window may not function correctly.
        /// In order to create windowless browsers the CefSettings.windowless_rendering_enabled value must be set to true.</param>
        void SetAsWindowless(IntPtr parentHandle);
    }
}
