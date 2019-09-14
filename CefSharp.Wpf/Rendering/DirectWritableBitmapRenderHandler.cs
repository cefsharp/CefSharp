// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// DirectWritableBitmapRenderHandler - directly copyies the buffer
    /// into writeableBitmap.BackBuffer. No additional copies or locking are used.
    /// Can only be used when CEF UI thread and WPF UI thread are the same (MultiThreadedMessageLoop = false)
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.IRenderHandler" />
    public class DirectWritableBitmapRenderHandler : IRenderHandler
    {
        private readonly double dpiX;
        private readonly double dpiY;
        private readonly bool invalidateDirtyRect;

        /// <summary>
        /// Initializes a new instance of the <see cref="WritableBitmapRenderHandler"/> class.
        /// </summary>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        /// <param name="invalidateDirtyRect">if true then only the direct rectangle will be updated, otherwise the whole bitmap will be redrawn</param>
        /// <param name="dispatcherPriority">priority at which the bitmap will be updated on the UI thread</param>
        public DirectWritableBitmapRenderHandler(double dpiX, double dpiY, bool invalidateDirtyRect = true, DispatcherPriority dispatcherPriority = DispatcherPriority.Render)
        {
            if (!Cef.CurrentlyOnThread(CefThreadIds.TID_UI))
            {
                throw new NotSupportedException("Can only be used when CEF is integrated into your WPF Message Loop (MultiThreadedMessageLoop = false).");
            }

            this.dpiX = dpiX;
            this.dpiY = dpiY;
            this.invalidateDirtyRect = invalidateDirtyRect;
        }

        void IDisposable.Dispose()
        {

        }

        void IRenderHandler.OnAcceleratedPaint(bool isPopup, Rect dirtyRect, IntPtr sharedHandle)
        {
            throw new NotImplementedException();
        }

        void IRenderHandler.OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image)
        {
            var writeableBitmap = image.Source as WriteableBitmap;
            if (writeableBitmap == null || writeableBitmap.PixelWidth != width || writeableBitmap.PixelHeight != height)
            {
                image.Source = writeableBitmap = new WriteableBitmap(width, height, dpiX, dpiY, AbstractRenderHandler.PixelFormat, null);
            }

            if (writeableBitmap != null)
            {
                writeableBitmap.Lock();

                NativeMethodWrapper.MemoryCopy(writeableBitmap.BackBuffer, buffer, writeableBitmap.BackBufferStride * writeableBitmap.PixelHeight);

                if (invalidateDirtyRect)
                {
                    writeableBitmap.AddDirtyRect(new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height));
                }
                else
                {
                    writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
                }
                writeableBitmap.Unlock();
            }
        }
    }
}

