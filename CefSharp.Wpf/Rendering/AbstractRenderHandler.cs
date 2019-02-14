// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// AbstractRenderHandler - abstracts away the basics of implementing a <see cref="IRenderHandler"/>
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.IRenderHandler" />
    public abstract class AbstractRenderHandler : IDisposable, IRenderHandler
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        protected static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        protected static readonly PixelFormat PixelFormat = PixelFormats.Pbgra32;
        protected static int BytesPerPixel = PixelFormat.BitsPerPixel / 8;

        protected object lockObject = new object();

        protected Size viewSize;
        protected Size popupSize;
        protected DispatcherPriority dispatcherPriority;

        protected MemoryMappedFile viewMemoryMappedFile;
        protected MemoryMappedFile popupMemoryMappedFile;
        protected MemoryMappedViewAccessor viewMemoryMappedViewAccessor;
        protected MemoryMappedViewAccessor popupMemoryMappedViewAccessor;

        #region IDisposable Support
        /// <summary>
        /// The value for disposal, if it's 1 (one) then this instance is either disposed
        /// or in the process of getting disposed
        /// </summary>
        private int disposeSignaled;

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed
        {
            get
            {
                return Interlocked.CompareExchange(ref disposeSignaled, 1, 1) == 1;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

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

            ReleaseMemoryMappedView(ref popupMemoryMappedFile, ref popupMemoryMappedViewAccessor);
            ReleaseMemoryMappedView(ref viewMemoryMappedFile, ref viewMemoryMappedViewAccessor);
        }
        #endregion

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

        public virtual void OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image)
        {
            if (image.Dispatcher.HasShutdownStarted)
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
