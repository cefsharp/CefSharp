// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// WritableBitmapRenderHandler - creates/updates an WritableBitmap
    /// Uses a MemoryMappedFile for double buffering when the size matches
    /// or creates a new WritableBitmap when required
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.IRenderHandler" />
    public class WritableBitmapRenderHandler : IRenderHandler
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        /// <summary>
        /// The pixel format
        /// </summary>
        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;
        private static int BytesPerPixel = PixelFormat.BitsPerPixel / 8;

        private object lockObject = new object();

        private Size viewSize;
        private Size popupSize;
        private double dpiX;
        private double dpiY;
        private bool invalidateDirtyRect;
        private DispatcherPriority dispatcherPriority;

        private MemoryMappedFile viewMemoryMappedFile;
        private MemoryMappedFile popupMemoryMappedFile;
        private MemoryMappedViewAccessor viewMemoryMappedViewAccessor;
        private MemoryMappedViewAccessor popupMemoryMappedViewAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="WritableBitmapRenderHandler"/> class.
        /// </summary>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        /// <param name="invalidateDirtyRect">if true then only the direct rectangle will be updated, otherwise the whole bitmap will be redrawn</param>
        /// <param name="dispatcherPriority">priority at which the bitmap will be updated on the UI thread</param>
        public WritableBitmapRenderHandler(double dpiX, double dpiY, bool invalidateDirtyRect = true, DispatcherPriority dispatcherPriority = DispatcherPriority.Render)
        {
            this.dpiX = dpiX;
            this.dpiY = dpiY;
            this.invalidateDirtyRect = invalidateDirtyRect;
            this.dispatcherPriority = dispatcherPriority;
        }

        public void Dispose()
        {
            ReleaseMemoryMappedView(ref popupMemoryMappedFile, ref popupMemoryMappedViewAccessor);
            ReleaseMemoryMappedView(ref viewMemoryMappedFile, ref viewMemoryMappedViewAccessor);
        }

        void IRenderHandler.OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image)
        {
            if (isPopup)
            {
                CreateOrUpdateBitmap(isPopup, dirtyRect, buffer, width, height, image, ref popupSize, ref popupMemoryMappedFile, ref popupMemoryMappedViewAccessor);
            }
            else
            {
                CreateOrUpdateBitmap(isPopup, dirtyRect, buffer, width, height, image, ref viewSize, ref viewMemoryMappedFile, ref viewMemoryMappedViewAccessor);
            }
        }

        private void CreateOrUpdateBitmap(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image, ref Size currentSize, ref MemoryMappedFile mappedFile, ref MemoryMappedViewAccessor viewAccessor)
        {
            bool createNewBitmap = false;

            if (image.Dispatcher.HasShutdownStarted)
            {
                return;
            }

            lock (lockObject)
            {

                int pixels = width * height;
                int numberOfBytes = pixels * BytesPerPixel;

                createNewBitmap = mappedFile == null || currentSize.Height != height || currentSize.Width != width;

                if (createNewBitmap)
                {
                    ReleaseMemoryMappedView(ref mappedFile, ref viewAccessor);

                    mappedFile = MemoryMappedFile.CreateNew(null, numberOfBytes, MemoryMappedFileAccess.ReadWrite);

                    viewAccessor = mappedFile.CreateViewAccessor();

                    currentSize.Height = height;
                    currentSize.Width = width;
                }

                //TODO: Performance analysis to determine which is the fastest memory copy function
                //NativeMethodWrapper.CopyMemoryUsingHandle(viewAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle(), buffer, numberOfBytes);
                CopyMemory(viewAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle(), buffer, (uint)numberOfBytes);

                //Take a reference to the sourceBuffer that's used to update our WritableBitmap,
                //once we're on the UI thread we need to check if it's still valid
                var sourceBuffer = viewAccessor.SafeMemoryMappedViewHandle;

                image.Dispatcher.BeginInvoke((Action)(() =>
                {
                    lock (lockObject)
                    {
                        if (sourceBuffer.IsClosed || sourceBuffer.IsInvalid)
                        {
                            return;
                        }

                        if (createNewBitmap)
                        {
                            if (image.Source != null)
                            {
                                image.Source = null;
                                GC.Collect(1);
                            }

                            image.Source = new WriteableBitmap(width, height, dpiX, dpiY, PixelFormat, null);
                        }

                        var stride = width * BytesPerPixel;
                        var noOfBytes = stride * height;

                        var bitmap = (WriteableBitmap)image.Source;

                        //By default we'll only update the dirty rect, for those that run into a MILERR_WIN32ERROR Exception (#2035)
                        //it's desirably to either upgrade to a newer .Net version (only client runtime needs to be installed, not compiled
                        //against a newer version. Or invalidate the whole bitmap
                        if (invalidateDirtyRect)
                        {
                            // Update the dirty region
                            var sourceRect = new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

                            bitmap.Lock();
                            bitmap.WritePixels(sourceRect, sourceBuffer.DangerousGetHandle(), noOfBytes, stride, dirtyRect.X, dirtyRect.Y);
                            bitmap.Unlock();
                        }
                        else
                        {
                            // Update whole bitmap
                            var sourceRect = new Int32Rect(0, 0, width, height);

                            bitmap.Lock();
                            bitmap.WritePixels(sourceRect, sourceBuffer.DangerousGetHandle(), noOfBytes, stride);
                            bitmap.Unlock();
                        }
                    }
                }), dispatcherPriority);
            }
        }

        private void ReleaseMemoryMappedView(ref MemoryMappedFile mappedFile, ref MemoryMappedViewAccessor stream)
        {
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }

            if (mappedFile != null)
            {
                mappedFile.Dispose();
                mappedFile = null;
            }
        }
    }
}
