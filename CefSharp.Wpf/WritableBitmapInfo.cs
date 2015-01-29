// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CefSharp.Wpf
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
            //int stride = browserWidth * 4;
            var stride = Width * BytesPerPixel;
            var sourceBufferSize = stride * Height;

            if (Width == 0 || Height == 0)
            {
                return;
            }

            foreach (var dirtyRect in DirtyRects)
            {
                if (dirtyRect.Width == 0 || dirtyRect.Height == 0)
                {
                    continue;
                }

                // Update the dirty region
                var sourceRect = new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
                Bitmap.WritePixels(sourceRect, BackBufferHandle, sourceBufferSize, stride, dirtyRect.X, dirtyRect.Y);
            }
        }

        public override BitmapSource CreateBitmap()
        {
            Bitmap = new WriteableBitmap(Width, Height, 96, 96, PixelFormat, null);

            return Bitmap;
        }
    }
}
