// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public abstract class BitmapInfo
    {
        public object BitmapLock;
        public IntPtr BackBufferHandle;

        public bool IsPopup { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public IntPtr FileMappingHandle { get; set; }

        public int BytesPerPixel { get; set; }

        public abstract void ClearBitmap();

        protected BitmapInfo()
        {
            BitmapLock = new object();
        }
    }
}
