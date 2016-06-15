// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// InteropBitmapInfo.
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.Rendering.WpfBitmapInfo" />
    public class InteropBitmapInfo : WpfBitmapInfo
    {
        /// <summary>
        /// The pixel format
        /// </summary>
        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;

        /// <summary>
        /// Gets the bitmap.
        /// </summary>
        /// <value>The bitmap.</value>
        public InteropBitmap Bitmap { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteropBitmapInfo"/> class.
        /// </summary>
        public InteropBitmapInfo()
        {
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
            if (Bitmap != null)
            {
                var sourceRect = new Int32Rect(DirtyRect.X, DirtyRect.Y, DirtyRect.Width, DirtyRect.Height);
                Bitmap.Invalidate(sourceRect);
            }
        }

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <returns>BitmapSource.</returns>
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
