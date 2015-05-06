// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CefSharp.Wpf.Rendering
{
    public class InteropBitmapInfo : WpfBitmapInfo
    {
        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;

        public InteropBitmap Bitmap { get; private set; }

        public InteropBitmapInfo()
        {
            BytesPerPixel = PixelFormat.BitsPerPixel / 8;
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
            if (Bitmap != null)
            {
                Bitmap.Invalidate();
            }
        }

        public override BitmapSource CreateBitmap()
        {
            // Unable to create bitmap without valid File Handle (Most likely control is being disposed)
            if (FileMappingHandle == IntPtr.Zero)
            {
                return null;
            }

            var stride = Width * BytesPerPixel;

            Bitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(FileMappingHandle, Width, Height, PixelFormat, stride, 0);

            return Bitmap;
        }
    }
}
