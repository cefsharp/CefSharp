// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO.MemoryMappedFiles;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// BitmapFactory.
    /// </summary>
    /// <seealso cref="CefSharp.IBitmapFactory" />
    public class WritableBitmapFactory : IBitmapFactory
    {
        /// <summary>
        /// The pixel format
        /// </summary>
        private static readonly PixelFormat PixelFormat = PixelFormats.Bgra32;
        private static int BytesPerPixel = PixelFormat.BitsPerPixel / 8;

        private object lockObject = new object();

        private Size viewSize;
        private Size popupSize;
        
        private MemoryMappedFile viewMemoryMappedFile;
        private MemoryMappedFile popupMemoryMappedFile;
        private MemoryMappedViewAccessor viewMemoryMappedViewAccessor;
        private MemoryMappedViewAccessor popupMemoryMappedViewAccessor;

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
        /// Initializes a new instance of the <see cref="WritableBitmapFactory"/> class.
        /// </summary>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        public WritableBitmapFactory(double dpiX, double dpiY)
        {
            DpiX = dpiX;
            DpiY = dpiY;
        }

        public void Dispose()
        {
            ReleaseMemoryMappedView(ref popupMemoryMappedFile, ref popupMemoryMappedViewAccessor);
            ReleaseMemoryMappedView(ref viewMemoryMappedFile, ref viewMemoryMappedViewAccessor);
        }

        void IBitmapFactory.CreateOrUpdateBitmap(bool isPopup, IntPtr buffer, Rect dirtyRect, int width, int height, Image image)
        {
            if (isPopup)
            {
                CreateOrUpdateBitmap(isPopup, buffer, dirtyRect, width, height, image, ref popupSize, ref popupMemoryMappedFile, ref popupMemoryMappedViewAccessor);
            }
            else
            {
                CreateOrUpdateBitmap(isPopup, buffer, dirtyRect, width, height, image, ref viewSize, ref viewMemoryMappedFile, ref viewMemoryMappedViewAccessor);
            }
        }

        private void CreateOrUpdateBitmap(bool isPopup, IntPtr buffer, Rect dirtyRect, int width, int height, Image image, ref Size currentSize, ref MemoryMappedFile mappedFile, ref MemoryMappedViewAccessor viewAccessor)
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

                NativeMethodWrapper.CopyMemoryUsingHandle(viewAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle(), buffer, numberOfBytes);
            }

            image.Dispatcher.BeginInvoke((Action)(() =>
            {
                lock (lockObject)
                {
                    if (createNewBitmap)
                    {
                        if (image.Source != null)
                        {
                            image.Source = null;
                            GC.Collect(1);
                        }

                        image.Source = new WriteableBitmap(width, height, DpiX, DpiY, PixelFormat, null);
                    }

                    var backBufferHandle = isPopup ? popupMemoryMappedFile.SafeMemoryMappedFileHandle : viewMemoryMappedFile.SafeMemoryMappedFileHandle;

                    var stride = width * BytesPerPixel;
                    var noOfBytes = stride * height;

                    // Update the dirty region
                    var sourceRect = new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
                    var bitmap = (WriteableBitmap)image.Source;
                    bitmap.WritePixels(sourceRect, backBufferHandle.DangerousGetHandle(), noOfBytes, stride, dirtyRect.X, dirtyRect.Y);
                }
            }), DispatcherPriority.Render);
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
