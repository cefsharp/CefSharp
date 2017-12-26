// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <summary>
    /// Information about the bitmap that should be rendered
    /// </summary>
    public abstract class BitmapInfo
    {
        public object BitmapLock;
        public IntPtr BackBufferHandle;

        public bool IsPopup { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public IntPtr FileMappingHandle { get; set; }

        public int BytesPerPixel { get; protected set; }
        public bool DirtyRectSupport { get; protected set; }
        public int NumberOfBytes { get; set; }

        public abstract bool CreateNewBitmap { get; }
        public abstract void ClearBitmap();

        public Rect DirtyRect { get; set; }

        protected BitmapInfo()
        {
            BitmapLock = new object();
        }
    }
}
