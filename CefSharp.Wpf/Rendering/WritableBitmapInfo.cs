// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CefSharp.Wpf.Rendering
{
    public class WritableBitmapInfo : WpfBitmapInfo
    {
        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;

        public WriteableBitmap Bitmap { get; private set; }

        public WritableBitmapInfo()
        {
            BytesPerPixel = PixelFormat.BitsPerPixel / 8;
            DirtyRectSupport = true;
        }

        public override bool CreateNewBitmap
        {
            get { return Bitmap == null; }
        }

        public override void ClearBitmap()
        {
            Bitmap = null;
        }

        public override void Invalidate()
        {
            if (BackBufferHandle == IntPtr.Zero || Width == 0 || Height == 0 || DirtyRect.Width == 0 || DirtyRect.Height == 0)
            {
                return;
            }

            var stride = Width * BytesPerPixel;
            var sourceBufferSize = stride * Height;

            // Update the dirty region
            var sourceRect = new Int32Rect(DirtyRect.X, DirtyRect.Y, DirtyRect.Width, DirtyRect.Height);
            Bitmap.WritePixels(sourceRect, BackBufferHandle, sourceBufferSize, stride, DirtyRect.X, DirtyRect.Y);
        }

        public override BitmapSource CreateBitmap()
        {
            Bitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormat, null);

            return Bitmap;
        }
    }
}
