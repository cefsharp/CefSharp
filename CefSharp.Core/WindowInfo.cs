// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using System;

namespace CefSharp
{
    /// <inheritdoc/>
    public class WindowInfo : IWindowInfo
    {
        private CefSharp.Core.WindowInfo windowInfo = new CefSharp.Core.WindowInfo();

        /// <inheritdoc/>
        public int X
        {
            get { return windowInfo.X; }
            set { windowInfo.X = value; }
        }

        /// <inheritdoc/>
        public int Y
        {
            get { return windowInfo.Y; }
            set { windowInfo.Y = value; }
        }

        /// <inheritdoc/>
        public int Width
        {
            get { return windowInfo.Width; }
            set { windowInfo.Width = value; }
        }

        /// <inheritdoc/>
        public int Height
        {
            get { return windowInfo.Height; }
            set { windowInfo.Height = value; }
        }

        /// <inheritdoc/>
        public uint Style
        {
            get { return windowInfo.Style; }
            set { windowInfo.Style = value; }
        }

        /// <inheritdoc/>
        public uint ExStyle
        {
            get { return windowInfo.ExStyle; }
            set { windowInfo.ExStyle = value; }
        }

        /// <inheritdoc/>
        public IntPtr ParentWindowHandle
        {
            get { return windowInfo.ParentWindowHandle; }
            set { windowInfo.ParentWindowHandle = value; }
        }

        /// <inheritdoc/>
        public bool WindowlessRenderingEnabled
        {
            get { return windowInfo.WindowlessRenderingEnabled; }
            set { windowInfo.WindowlessRenderingEnabled = value; }
        }

        /// <inheritdoc/>
        public bool SharedTextureEnabled
        {
            get { return windowInfo.SharedTextureEnabled; }
            set { windowInfo.SharedTextureEnabled = value; }
        }

        /// <inheritdoc/>
        public bool ExternalBeginFrameEnabled
        {
            get { return windowInfo.ExternalBeginFrameEnabled; }
            set { windowInfo.ExternalBeginFrameEnabled = value; }
        }

        /// <inheritdoc/>
        public IntPtr WindowHandle
        {
            get { return windowInfo.WindowHandle; }
            set { windowInfo.WindowHandle = value; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            windowInfo.Dispose();
        }

        /// <inheritdoc/>
        public void SetAsChild(IntPtr parentHandle)
        {
            windowInfo.SetAsChild(parentHandle);
        }

        /// <inheritdoc/>
        public void SetAsChild(IntPtr parentHandle, int left, int top, int right, int bottom)
        {
            windowInfo.SetAsChild(parentHandle, left, top, right, bottom);
        }

        /// <inheritdoc/>
        public void SetAsPopup(IntPtr parentHandle, string windowName)
        {
            windowInfo.SetAsPopup(parentHandle, windowName);
        }

        /// <inheritdoc/>
        public void SetAsWindowless(IntPtr parentHandle)
        {
            windowInfo.SetAsWindowless(parentHandle);
        }

        /// <summary>
        /// Create a new <see cref="IWindowInfo"/> instance
        /// </summary>
        /// <returns>WindowInfo</returns>
        public static IWindowInfo Create()
        {
            return new CefSharp.Core.WindowInfo();
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public IWindowInfo UnWrap()
        {
            return windowInfo;
        }
    }
}
