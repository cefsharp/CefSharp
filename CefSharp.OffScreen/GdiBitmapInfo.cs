// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// Uses GdiBitmap to render the backbuffer.
    /// </summary>
    /// <seealso cref="CefSharp.Internals.BitmapInfo" />
    public class GdiBitmapInfo : BitmapInfo
    {
        /// <summary>
        /// The bitmap
        /// </summary>
        private Bitmap bitmap;

        /// <summary>
        /// The create new bitmap
        /// </summary>
        private bool createNewBitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="GdiBitmapInfo"/> class.
        /// </summary>
        public GdiBitmapInfo()
        {
            BytesPerPixel = 4;
        }

        /// <summary>
        /// Gets a value indicating whether [create new bitmap].
        /// </summary>
        /// <value><c>true</c> if [create new bitmap]; otherwise, <c>false</c>.</value>
        public override bool CreateNewBitmap
        {
            get { return createNewBitmap; }
        }

        /// <summary>
        /// Clears the bitmap.
        /// </summary>
        public override void ClearBitmap()
        {
            createNewBitmap = true;
        }

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <returns>Bitmap.</returns>
        public virtual Bitmap CreateBitmap()
        {
            if(BackBufferHandle == IntPtr.Zero)
            {
                return null;
            }

            var stride = Width * BytesPerPixel;

            bitmap = new Bitmap(Width, Height, stride, PixelFormat.Format32bppPArgb, BackBufferHandle);

            createNewBitmap = false;

            return bitmap;
        }
    }
}
