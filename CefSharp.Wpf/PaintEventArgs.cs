// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Structs;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Event arguments for the Paint event handler.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class PaintEventArgs : EventArgs
    {
        /// <summary>
        /// Is the OnPaint call for a Popup or the Main View
        /// </summary>
        public bool IsPopup { get; private set; }

        /// <summary>
        /// contains the set of rectangles in pixel coordinates that need to be repainted
        /// </summary>
        public Rect DirtyRect { get; private set; }

        /// <summary>
        /// Buffer
        /// </summary>
        public IntPtr Buffer { get; private set; }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="bitmapInfo">The bitmap information.</param>
        public PaintEventArgs(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            IsPopup = isPopup;
            DirtyRect = dirtyRect;
            Buffer = buffer;
            Width = width;
            Height = height;
        }
    }
}
