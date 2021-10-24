// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// AllocHGlobalWritableBitmapRenderHandler - creates/updates an WritableBitmap
    /// Uses <see cref="Marshal.AllocHGlobal"/> to allocate memory for
    /// double buffering when the size matches or creates a new WritableBitmap
    /// when required.
    /// </summary>
    /// <seealso cref="IRenderHandler" />
    public class AllocHGlobalWritableBitmapRenderHandler : IRenderHandler
    {
        private readonly double dpiX;
        private readonly double dpiY;
        private readonly PaintElement view;
        private readonly PaintElement popup;
        private readonly DispatcherPriority dispatcherPriority;
        private readonly object lockObject = new object();

        /// <summary>
        /// The value for disposal, if it's 1 (one) then this instance is either disposed
        /// or in the process of getting disposed
        /// </summary>
        private int disposeSignaled;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocHGlobalWritableBitmapRenderHandler"/> class.
        /// </summary>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        /// <param name="invalidateDirtyRect">if true then only the direct rectangle will be updated, otherwise the whole bitmap will be redrawn</param>
        /// <param name="dispatcherPriority">priority at which the bitmap will be updated on the UI thread</param>
        public AllocHGlobalWritableBitmapRenderHandler(double dpiX, double dpiY, bool invalidateDirtyRect = true, DispatcherPriority dispatcherPriority = DispatcherPriority.Render)
        {
            this.dpiX = dpiX;
            this.dpiY = dpiY;
            this.dispatcherPriority = dispatcherPriority;

            view = new PaintElement(dpiX, dpiY, invalidateDirtyRect);
            popup = new PaintElement(dpiX, dpiY, invalidateDirtyRect);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><see langword="true"/> if this instance is disposed; otherwise, <see langword="true"/>.</value>
        public bool IsDisposed
        {
            get
            {
                return Interlocked.CompareExchange(ref disposeSignaled, 1, 1) == 1;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AbstractRenderHandler"/> object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources for the <see cref="AbstractRenderHandler"/>
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref disposeSignaled, 1, 0) != 0)
            {
                return;
            }

            if (!disposing)
            {
                return;
            }

            lock (lockObject)
            {
                view?.Dispose();
                popup?.Dispose();
            }
        }

        void IRenderHandler.OnAcceleratedPaint(bool isPopup, Rect dirtyRect, IntPtr sharedHandle)
        {
            //NOT USED
        }

        void IRenderHandler.OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image)
        {
            lock (lockObject)
            {
                if (IsDisposed || image.Dispatcher.HasShutdownStarted)
                {
                    return;
                }

                var paintElement = isPopup ? popup : view;
                paintElement?.UpdateBuffer(dirtyRect, buffer, width, height, image);
                paintElement?.UpdateImage(lockObject);
            }
        }

        //TODO: No nested classes and better name for this class
        /// <summary>
        /// Details of the bitmap to be rendered
        /// </summary>
        private class PaintElement
        {
            private readonly double dpiX;
            private readonly double dpiY;
            private Image image;
            private int width;
            private int height;
            private Rect dirtyRect;
            private IntPtr buffer;
            private int bufferSize;
            private int imageSize;
            private readonly bool invalidateDirtyRect;
            internal bool IsDirty { get; set; }

            internal PaintElement(double dpiX, double dpiY, bool invalidateDirtyRect)
            {
                this.dpiX = dpiX;
                this.dpiY = dpiY;
                this.invalidateDirtyRect = invalidateDirtyRect;
            }

            internal void Dispose()
            {
                image = null;

                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                    buffer = IntPtr.Zero;
                }
            }

            internal void UpdateBuffer(Rect dirtyRect, IntPtr sourceBuffer, int width, int height, Image image)
            {
                imageSize = (width * height) * AbstractRenderHandler.BytesPerPixel;

                if (bufferSize < imageSize)
                {
                    Marshal.FreeHGlobal(buffer);
                    buffer = Marshal.AllocHGlobal(imageSize);
                    bufferSize = imageSize;
                }

                this.width = width;
                this.height = height;
                this.dirtyRect = dirtyRect;

                NativeMethodWrapper.MemoryCopy(buffer, sourceBuffer, imageSize);

                this.image = image;
                IsDirty = true;
            }

            internal void UpdateImage(object lockObject)
            {
                image.Dispatcher.BeginInvoke((Action)(() =>
                {
                    lock (lockObject)
                    {
                        //If OnPaint was called a couple of times before our BeginInvoke call
                        //we can end up here with nothing to do.
                        if (IsDirty && image != null)
                        {
                            var bitmap = image.Source as WriteableBitmap;
                            var createNewBitmap = bitmap == null || bitmap.PixelWidth != width || bitmap.PixelHeight != height;
                            if (createNewBitmap)
                            {
                                if (image.Source != null)
                                {
                                    image.Source = null;
                                    GC.Collect(1);
                                }

                                image.Source = bitmap = new WriteableBitmap(width, height, dpiX, dpiY, AbstractRenderHandler.PixelFormat, null);
                            }

                            if (bitmap != null)
                            {
                                //By default we'll only update the dirty rect, for those that run into a MILERR_WIN32ERROR Exception (#2035)
                                //it's desirably to either upgrade to a newer .Net version (only client runtime needs to be installed, not compiled
                                //against a newer version. Or invalidate the whole bitmap
                                if (invalidateDirtyRect)
                                {
                                    // Update the dirty region
                                    var sourceRect = new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

                                    bitmap.Lock();
                                    bitmap.WritePixels(sourceRect, buffer, imageSize, width * AbstractRenderHandler.BytesPerPixel, sourceRect.X, sourceRect.Y);
                                    bitmap.Unlock();
                                }
                                else
                                {
                                    // Update whole bitmap
                                    var sourceRect = new Int32Rect(0, 0, width, height);

                                    bitmap.Lock();
                                    bitmap.WritePixels(sourceRect, buffer, imageSize, width * AbstractRenderHandler.BytesPerPixel, sourceRect.X, sourceRect.Y);
                                    bitmap.Unlock();
                                }
                            }

                            IsDirty = false;
                        }
                    }
                }));
            }
        };
    }
}
