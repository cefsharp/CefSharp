// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// Implements the basics of a <see cref="IRenderHandler"/>
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.IRenderHandler" />
    public abstract class AbstractRenderHandler : IDisposable, IRenderHandler
    {
        internal static readonly PixelFormat PixelFormat = PixelFormats.Pbgra32;
        internal static int BytesPerPixel = PixelFormat.BitsPerPixel / 8;

        protected object lockObject = new object();

        protected Size viewSize;
        protected Size popupSize;
        protected DispatcherPriority dispatcherPriority;

        protected MemoryMappedFile viewMemoryMappedFile;
        protected MemoryMappedFile popupMemoryMappedFile;
        protected MemoryMappedViewAccessor viewMemoryMappedViewAccessor;
        protected MemoryMappedViewAccessor popupMemoryMappedViewAccessor;

        /// <summary>
        /// The value for disposal, if it's 1 (one) then this instance is either disposed
        /// or in the process of getting disposed
        /// </summary>
        private int disposeSignaled;

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
                ReleaseMemoryMappedView(ref popupMemoryMappedFile, ref popupMemoryMappedViewAccessor);
                ReleaseMemoryMappedView(ref viewMemoryMappedFile, ref viewMemoryMappedViewAccessor);
            }
        }

        protected void ReleaseMemoryMappedView(ref MemoryMappedFile mappedFile, ref MemoryMappedViewAccessor stream)
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

        /// <summary>
        /// Called when an element has been rendered to the shared texture handle.
        /// This method is only called when <see cref="IWindowInfo.SharedTextureEnabled"/> is set to true
        /// </summary>
        /// <param name="isPopup">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="sharedHandle">is the handle for a D3D11 Texture2D that can be accessed via ID3D11Device using the OpenSharedResource method.</param>
        public virtual void OnAcceleratedPaint(bool isPopup, Rect dirtyRect, IntPtr sharedHandle)
        {
            // NOT USED
        }

        /// <summary>
        /// Called when an element should be painted. (Invoked from CefRenderHandler.OnPaint)
        /// This method is only called when <see cref="IWindowInfo.SharedTextureEnabled"/> is set to false.
        /// </summary>
        /// <param name="isPopup">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="buffer">The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="image">image used as parent for rendered bitmap</param>
        public virtual void OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image)
        {
            if (IsDisposed || image.Dispatcher.HasShutdownStarted)
            {
                return;
            }

            if (isPopup)
            {
                CreateOrUpdateBitmap(isPopup, dirtyRect, buffer, width, height, image, ref popupSize, ref popupMemoryMappedFile, ref popupMemoryMappedViewAccessor);
            }
            else
            {
                CreateOrUpdateBitmap(isPopup, dirtyRect, buffer, width, height, image, ref viewSize, ref viewMemoryMappedFile, ref viewMemoryMappedViewAccessor);
            }
        }

        protected abstract void CreateOrUpdateBitmap(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image, ref Size currentSize, ref MemoryMappedFile mappedFile, ref MemoryMappedViewAccessor viewAccessor);
    }
}
