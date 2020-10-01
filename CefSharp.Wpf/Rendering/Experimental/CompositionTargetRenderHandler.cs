// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering.Experimental
{
    /// <summary>
    /// RenderHandler implemenetation that updates the image/bitmap in the
    /// <see cref="CompositionTarget.Rendering"/> event.
    /// Initially based on https://github.com/cefsharp/CefSharp/issues/2888#issuecomment-528864931
    /// </summary>
    public class CompositionTargetRenderHandler : IRenderHandler
    {
        private readonly PaintElement view;
        private readonly PaintElement popup;
        private readonly object lockObj = new object();
        private ChromiumWebBrowser browser;

        public CompositionTargetRenderHandler(ChromiumWebBrowser browser, double dpiX, double dpiY)
        {
            this.browser = browser;
            this.browser.IsVisibleChanged += BrowserIsVisibleChanged;

            this.view = new PaintElement(dpiX, dpiY);
            this.popup = new PaintElement(dpiX, dpiY);

            if (browser.IsVisible)
            {
                CompositionTarget.Rendering += OnRendering;
            }
        }

        private void BrowserIsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                CompositionTarget.Rendering += OnRendering;
            }
            else
            {
                CompositionTarget.Rendering -= OnRendering;
            }
        }

        void IDisposable.Dispose()
        {
            CompositionTarget.Rendering -= OnRendering;

            browser.IsVisibleChanged -= BrowserIsVisibleChanged;
            browser = null;

            lock (lockObj)
            {
                view.Dispose();
                popup.Dispose();
            }
        }

        void IRenderHandler.OnAcceleratedPaint(bool isPopup, Rect dirtyRect, IntPtr sharedHandle)
        {
            throw new NotImplementedException();
        }

        void IRenderHandler.OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image)
        {
            if (image.Dispatcher.HasShutdownStarted)
            {
                return;
            }

            lock (lockObj)
            {
                var layer = isPopup ? popup : view;
                layer.OnPaint(dirtyRect, buffer, width, height, image);
            }
        }

        private void OnRendering(object sender, EventArgs args)
        {
            lock (lockObj)
            {
                UpdateImage(view);
                UpdateImage(popup);
            }
        }

        private void UpdateImage(PaintElement element)
        {
            if (element.Image != null)
            {
                var bitmap = element.Image.Source as WriteableBitmap;
                if (bitmap == null || bitmap.PixelWidth != element.Width || bitmap.PixelHeight != element.Height)
                {
                    element.Image.Source = bitmap = new WriteableBitmap(element.Width, element.Height, element.DpiX, element.DpiY, AbstractRenderHandler.PixelFormat, null);
                }

                if (bitmap != null)
                {
                    // Update the dirty region
                    var dirtyRect = element.DirtyRect;
                    var sourceRect = new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);

                    bitmap.Lock();
                    bitmap.WritePixels(sourceRect, element.Buffer, element.ImageSize, element.Width * AbstractRenderHandler.BytesPerPixel, sourceRect.X, sourceRect.Y);
                    bitmap.Unlock();
                }

                element.IsDirty = false; //We've processed this image, clear the reference
            }
        }

        //TODO: No nested classes
        /// <summary>
        /// Details of the bitmap to be rendered
        /// </summary>
        private class PaintElement
        {
            internal double DpiX { get; private set; }
            internal double DpiY { get; private set; }
            internal Image Image { get; set; }
            internal int Width { get; private set; }
            internal int Height { get; private set; }
            internal Rect DirtyRect { get; private set; }
            internal IntPtr Buffer { get; private set; }
            internal int BufferSize { get; private set; }
            internal int ImageSize { get; private set; }
            internal bool IsDirty { get; set; }

            internal PaintElement(double dpiX, double dpiY)
            {
                DpiX = dpiX;
                DpiY = dpiY;
            }

            internal void Dispose()
            {
                if (Buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(Buffer);
                    Buffer = IntPtr.Zero;
                }
            }

            internal void OnPaint(Rect dirtyRect, IntPtr sourceBuffer, int width, int height, Image image)
            {
                ImageSize = (width * height) * AbstractRenderHandler.BytesPerPixel;

                if (BufferSize < ImageSize)
                {
                    Marshal.FreeHGlobal(Buffer);
                    Buffer = Marshal.AllocHGlobal(ImageSize);
                    BufferSize = ImageSize;
                }

                Width = width;
                Height = height;
                DirtyRect = dirtyRect;

                NativeMethodWrapper.MemoryCopy(Buffer, sourceBuffer, ImageSize);

                Image = image;
                IsDirty = true;
            }
        };

    }
}
