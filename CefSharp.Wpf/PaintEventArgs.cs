// Copyright © 2018 The CefSharp Authors. All rights reserved.
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
        /// Pointer to the unmanaged buffer that holds the bitmap.
        /// The buffer shouldn't be accessed outside the scope of <see cref="ChromiumWebBrowser.Paint"/> event.
        /// A copy should be taken as the buffer is reused internally and may potentialy be freed. 
        /// </summary>
        /// <remarks>The bitmap will be width * height * 4 bytes in size and represents a BGRA image with an upper-left origin</remarks>
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
        /// <param name="isPopup">is popup</param>
        /// <param name="dirtyRect">direct rectangle</param>
        /// <param name="buffer">buffer</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
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
