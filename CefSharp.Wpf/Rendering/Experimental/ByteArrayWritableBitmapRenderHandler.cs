// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering.Experimental
{
    /// <summary>
    /// ByteArrayWritableBitmapRenderHandler - creates/updates an WritableBitmap
    /// For each OnPaint call a new byte[] is created and then updated. No locking is
    /// performed and memory is allocated for every OnPaint call, so will be very expensive memory
    /// wise.
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.IRenderHandler" />
    public class ByteArrayWritableBitmapRenderHandler : IRenderHandler
    {
        /// <summary>
        /// The pixel format
        /// </summary>
        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;
        private static int BytesPerPixel = PixelFormat.BitsPerPixel / 8;

        private double dpiX;
        private double dpiY;
        private bool invalidateDirtyRect;
        private DispatcherPriority dispatcherPriority;

        /// <summary>
        /// Initializes a new instance of the <see cref="WritableBitmapRenderHandler"/> class.
        /// </summary>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        /// <param name="invalidateDirtyRect">if true then only the direct rectangle will be updated, otherwise the whole bitmap will be redrawn</param>
        /// <param name="dispatcherPriority">priority at which the bitmap will be updated on the UI thread</param>
        public ByteArrayWritableBitmapRenderHandler(double dpiX, double dpiY, bool invalidateDirtyRect = true, DispatcherPriority dispatcherPriority = DispatcherPriority.Render)
        {
            this.dpiX = dpiX;
            this.dpiY = dpiY;
            this.invalidateDirtyRect = invalidateDirtyRect;
            this.dispatcherPriority = dispatcherPriority;
        }

        void IRenderHandler.OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image)
        {
            if (image.Dispatcher.HasShutdownStarted)
            {
                return;
            }

            int pixels = width * height;
            int numberOfBytes = pixels * BytesPerPixel;
            var stride = width * BytesPerPixel;
            var tempBuffer = new byte[numberOfBytes];

            //Copy unmanaged memory to our buffer
            Marshal.Copy(buffer, tempBuffer, 0, numberOfBytes);

            image.Dispatcher.BeginInvoke((Action)(() =>
            {
                var bitmap = image.Source as WriteableBitmap;

                if (bitmap == null || bitmap.PixelHeight != height || bitmap.PixelWidth != width)
                {
                    if (image.Source != null)
                    {
                        image.Source = null;
                        GC.Collect(1);
                    }

                    image.Source = bitmap = new WriteableBitmap(width, height, dpiX, dpiY, PixelFormat, null);
                }
                
                //Get a ptr to our temp buffer
                var tempBufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(tempBuffer, 0);

                //By default we'll only update the dirty rect, for those that run into a MILERR_WIN32ERROR Exception (#2035)
                //it's desirably to either upgrade to a newer .Net version (only client runtime needs to be installed, not compiled
                //against a newer version. Or invalidate the whole bitmap
                if (invalidateDirtyRect)
                {
                    // Update the dirty region
                    var sourceRect = new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

                    bitmap.Lock();
                    bitmap.WritePixels(sourceRect, tempBufferPtr, numberOfBytes, stride, dirtyRect.X, dirtyRect.Y);
                    bitmap.Unlock();
                }
                else
                {
                    // Update whole bitmap
                    var sourceRect = new Int32Rect(0, 0, width, height);

                    bitmap.Lock();
                    bitmap.WritePixels(sourceRect, tempBufferPtr, numberOfBytes, stride);
                    bitmap.Unlock();
                }
            }), dispatcherPriority);
        }

        void IDisposable.Dispose()
        {
            
        }
    }
}
