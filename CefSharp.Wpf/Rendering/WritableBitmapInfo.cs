// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// Uses WriteableBitmap to create a bitmap from the backbuffer
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.Rendering.WpfBitmapInfo" />
    public class WritableBitmapInfo : WpfBitmapInfo
    {
        /// <summary>
        /// The pixel format
        /// </summary>
        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;

        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <value>The bitmap.</value>
        public WriteableBitmap Bitmap { get; private set; }
        /// <summary>
        /// Gets the dpi x.
        /// </summary>
        /// <value>The dpi x.</value>
        public double DpiX { get; private set; }
        /// <summary>
        /// Gets the dpi y.
        /// </summary>
        /// <value>The dpi y.</value>
        public double DpiY { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritableBitmapInfo"/> class.
        /// </summary>
        public WritableBitmapInfo()
            : this(BitmapFactory.DefaultDpi, BitmapFactory.DefaultDpi)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WritableBitmapInfo"/> class.
        /// </summary>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        public WritableBitmapInfo(double dpiX, double dpiY)
        {
            DpiX = dpiX;
            DpiY = dpiY;

            BytesPerPixel = PixelFormat.BitsPerPixel / 8;
            DirtyRectSupport = true;
        }

        /// <summary>
        /// Gets a value indicating whether [create new bitmap].
        /// </summary>
        /// <value><c>true</c> if [create new bitmap]; otherwise, <c>false</c>.</value>
        public override bool CreateNewBitmap
        {
            get { return Bitmap == null; }
        }

        /// <summary>
        /// Clears the bitmap.
        /// </summary>
        public override void ClearBitmap()
        {
            Bitmap = null;
        }

        /// <summary>
        /// Invalidates this instance.
        /// </summary>
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

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <returns>BitmapSource.</returns>
        public override BitmapSource CreateBitmap()
        {
            Bitmap = new WriteableBitmap(Width, Height, DpiX, DpiY, PixelFormat, null);

            return Bitmap;
        }
    }
}
