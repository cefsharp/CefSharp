// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Structs;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// Event arguments to the OnPaint event handler.
    /// Pixel values are scaled relative to view coordinates based on the value of ScreenInfo.DeviceScaleFactor
    /// returned from <see cref="IRenderHandler.GetScreenInfo"/>. 
    /// </summary>
    public class OnPaintEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event is handled.
        /// </summary>
        public bool Handled { get; set; }
        /// <summary>
        /// Indicates whether the element is the view or the popup widge
        /// </summary>
        public bool IsPopup { get; private set; }
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// contains the pixel data for the whole image. Will be width * height * 4 bytes in size and
        /// represents a BGRA image with an upper-left origin
        /// </summary>
        public IntPtr BufferHandle { get; private set; }
        /// <summary>
        /// Contains a rectangle in pixel coordinates that needs to be repainted.
        /// </summary>
        public Rect DirtyRect { get; private set; }

        /// <summary>
        /// Creates a new OnPaint event arg
        /// </summary>
        /// <param name="isPopup">is popup</param>
        /// <param name="dirtyRect">dirty rect if enabled</param>
        /// <param name="bufferHandle">buffer handle (back buffer)</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public OnPaintEventArgs(bool isPopup, Rect dirtyRect, IntPtr bufferHandle, int width, int height)
        {
            IsPopup = isPopup;
            DirtyRect = dirtyRect;
            BufferHandle = bufferHandle;
            Width = width;
            Height = height;
        }
    }
}

