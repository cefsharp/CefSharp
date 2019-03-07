// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CefSharp.Structs;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// BitmapBuffer contains a byte[] used to store the Bitmap generated from <see cref="IRenderHandler.OnPaint"/>
    /// and associated methods for updating that buffer and creating a <see cref="Bitmap"/> from the actaual Buffer
    /// </summary>
    public class BitmapBuffer
    {
        private const int BytesPerPixel = 4;
        private const PixelFormat Format = PixelFormat.Format32bppPArgb;

        private byte[] buffer;

        /// <summary>
        /// Number of bytes
        /// </summary>
        public int NumberOfBytes { get; private set; }
        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// Dirty Rect - unified region containing th
        /// </summary>
        public Rect DirtyRect { get; private set; }

        /// <summary>
        /// Locking object used to syncronise access to the underlying buffer
        /// </summary>
        public object BitmapLock { get; private set; }

        /// <summary>
        /// Create a new instance of BitmapBuffer
        /// </summary>
        /// <param name="bitmapLock">Reference to the bitmapLock, a shared lock object is expected</param>
        public BitmapBuffer(object bitmapLock)
        {
            BitmapLock = bitmapLock;
        }

        /// <summary>
        /// Get the byte[] array that represents the Bitmap
        /// </summary>
        public byte[] Buffer
        {
            get { return buffer; }
        }

        //TODO: May need to Pin the buffer in memory using GCHandle.Alloc(this.buffer, GCHandleType.Pinned);
        private void ResizeBuffer(int width, int height)
        {
            if (buffer == null || width != Width || height != Height)
            {
                //No of Pixels (width * height) * BytesPerPixel
                NumberOfBytes = width * height * BytesPerPixel;

                buffer = new byte[NumberOfBytes];

                Width = width;
                Height = height;
            }
        }

        /// <summary>
        /// Copy data from the unmanaged buffer (IntPtr) into our managed buffer.
        /// Locks BitmapLock before performing any update
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="buffer">pointer to unmanaged buffer (void*)</param>
        /// <param name="dirtyRect">rectangle to be updated</param>
        public void UpdateBuffer(int width, int height, IntPtr buffer, Rect dirtyRect)
        {
            lock (BitmapLock)
            {
                DirtyRect = dirtyRect;
                ResizeBuffer(width, height);
                Marshal.Copy(buffer, this.buffer, 0, NumberOfBytes);
            }
        }

        /// <summary>
        /// Creates a new Bitmap given with the current Width/Height and <see cref="Format"/>
        /// then copies the buffer that represents the bitmap.
        /// Locks <see cref="BitmapLock"/> before creating the <see cref="Bitmap"/>
        /// </summary>
        /// <returns>A new bitmap</returns>
        public Bitmap CreateBitmap()
        {
            lock (BitmapLock)
            {
                if (Width == 0 || Height == 0 || buffer.Length == 0)
                {
                    return null;
                }

                var bitmap = new Bitmap(Width, Height, Format);

                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, Format);

                Marshal.Copy(Buffer, 0, bitmapData.Scan0, NumberOfBytes);

                bitmap.UnlockBits(bitmapData);

                return bitmap;
            }
        }
    }
}
