// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;

using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Rendering.Experimental
{
    /// <summary>
    /// InteropBitmapRenderHandlerEx - creates/updates an InteropBitmap, copies only dirty rect.
    /// Uses a MemoryMappedFile for double buffering when the size matches
    /// or creates a new InteropBitmap when required
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.IRenderHandler" />
    public class InteropBitmapRenderHandlerEx : AbstractRenderHandler
    {
        private RenderLayer imageLayer;
        private RenderLayer popupLayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteropBitmapRenderHandlerEx"/> class.
        /// </summary>
        /// <param name="dispatcherPriority">priority at which the bitmap will be updated on the UI thread</param>
        public InteropBitmapRenderHandlerEx(DispatcherPriority dispatcherPriority = DispatcherPriority.Render)
        {
            this.dispatcherPriority = dispatcherPriority;

            imageLayer = new RenderLayer(dispatcherPriority);
            popupLayer = new RenderLayer(dispatcherPriority);
        }

        /// <summary>
        /// Find top left inclusive ptr.
        /// </summary>
        /// <param name="dirtyRect"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int DirtyRectStartInclusiveOffset(ref Rect dirtyRect, int width)
        {
            var skippedPixels = 0;

            //all without last line
            skippedPixels += dirtyRect.Y * width;

            //last line
            skippedPixels += dirtyRect.X;

            //current byte is our
            return skippedPixels * BytesPerPixel;
        }

        /// <summary>
        /// Find bottom right exclusive ptr.
        /// </summary>
        /// <param name="dirtyRect"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int DirtyRectEndExclusiveOffset(ref Rect dirtyRect, int width)
        {
            var skippedPixels = 0;

            //all without last line
            skippedPixels += (dirtyRect.Y + dirtyRect.Height - 1) * width;

            //last line
            skippedPixels += dirtyRect.X + dirtyRect.Width;

            //current byte is not our
            return skippedPixels * BytesPerPixel;
        }

        protected override void Dispose(bool disposing)
        {
            imageLayer?.Dispose();
            popupLayer?.Dispose();

            imageLayer = null;
            popupLayer = null;

            base.Dispose(disposing);
        }

        protected override void CreateOrUpdateBitmap(bool                           isPopup,
                                                     Rect                           dirtyRect,
                                                     IntPtr                         buffer,
                                                     int                            width,
                                                     int                            height,
                                                     Image                          image,
                                                     ref Size                       currentSize,
                                                     ref MemoryMappedFile           mappedFile,
                                                     ref MemoryMappedViewAccessor   viewAccessor)
        {
            var layer = isPopup ? popupLayer
                                : imageLayer;

            layer.CreateOrUpdateBitmap(ref dirtyRect,
                                       buffer,
                                       width,
                                       height,
                                       image,
                                       ref currentSize,
                                       ref mappedFile,
                                       ref viewAccessor);
        }

        private class RenderLayer : IDisposable
        {
            private readonly DispatcherPriority dispatcherPriority;
            private DispatcherOperation dispatcherOperation;
            private Rect                dirtyRect;
            private int                 width;
            private int                 height;
            private bool                createNewBitmap;
            private IntPtr              mmfHandle;

            private readonly object lockObject = new object();

            public RenderLayer(DispatcherPriority dispatcherPriority)
            {
                this.dispatcherPriority = dispatcherPriority;
            }

            public void Dispose()
            {
                lock (lockObject)
                {
                    if (dispatcherOperation != null)
                    {
                        //Removes from list in dispatcher, no problem
                        //if Dispose() called from ui thread outside this operation.
                        dispatcherOperation.Abort();
                        dispatcherOperation = null;
                    }

                    mmfHandle = IntPtr.Zero;
                }
            }

            public void CreateOrUpdateBitmap(ref Rect                       dirtyRect,
                                             IntPtr                         buffer,
                                             int                            width,
                                             int                            height,
                                             Image                          image,
                                             ref Size                       currentSize,
                                             ref MemoryMappedFile           mappedFile,
                                             ref MemoryMappedViewAccessor   viewAccessor)
            {
                var createNewBitmap = mmfHandle             == IntPtr.Zero
                                    || currentSize.Height   != height
                                    || currentSize.Width    != width;

                int ptrOffsetStart;
                int numberOfBytes;

                if (createNewBitmap)
                {
                    int pixels          = width * height;
                    numberOfBytes       = pixels * BytesPerPixel;
                    ptrOffsetStart      = 0;
                }
                else
                {
                    ptrOffsetStart      = DirtyRectStartInclusiveOffset(ref dirtyRect, width);
                    int ptrOffsetEnd    = DirtyRectEndExclusiveOffset(ref dirtyRect, width);
                    numberOfBytes       = ptrOffsetEnd - ptrOffsetStart;
                }

                lock (lockObject)
                {
                    if (createNewBitmap)
                    {
                        //NOTE: always create new MMF.

                        //Because DUCE MILCMD_BITMAP_INVALIDATE (used here) has no "lock" primitive (CopyCompletedEvent event)
                        //https://referencesource.microsoft.com/#PresentationCore/Graphics/include/Generated/wgx_commands.cs,67
                        //we dont know when we can write to mmf (and mmf not being used by DUCE),
                        //unlike in MILCMD_DOUBLEBUFFEREDBITMAP_COPYFORWARD that is used in WritableBitmapRenderHandler.
                        //https://referencesource.microsoft.com/#PresentationCore/Graphics/include/Generated/wgx_commands.cs,427

                        //without creating new mmf, will be resize glitches as in 
                        //https://github.com/cefsharp/CefSharp/issues/3114
                        //Even now we will have per pixel color glitches, that can be fixed by creating new mmf always.

                        var newCapacity = numberOfBytes;
                        ReleaseMemoryMappedView(ref mappedFile, ref viewAccessor);
                        CreateMemoryView(newCapacity, ref mappedFile, ref viewAccessor);

                        currentSize.Height  = height;
                        currentSize.Width   = width;
                    }

                    var memCopyDest    = viewAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle() + ptrOffsetStart;
                    var memCopySrc     = buffer + ptrOffsetStart;

                    NativeMethodWrapper.MemoryCopy(memCopyDest, memCopySrc, numberOfBytes);

                    this.width              = width;
                    this.height             = height;

                    if (HasRenderTask())
                    {
                        if (!this.createNewBitmap && !createNewBitmap)
                        {
                            //Special case. Update come after update

                            /**
                             * Here, we have 3 possible solutions:
                             * 1) Union rects [choosed].
                             * 2) Invalidate all.
                             * 3) Create list of rects.
                             *
                             * In the normal case, very rarely we can be here,
                             * so there is no point in using lists, enumerators.
                             */
                            UnionRect(ref this.dirtyRect, ref dirtyRect);
                        }
                        else
                        {
                            //Special case. Update can come after create
                            this.createNewBitmap = true;
                        }
                    }
                    else
                    {
                        this.dirtyRect       = dirtyRect;
                        this.createNewBitmap = createNewBitmap;
                    }

                    CreateRenderTask(image);
                }
            }

            private bool HasRenderTask() => dispatcherOperation != null;

            private void CreateMemoryView(long                          capacity,
                                          ref MemoryMappedFile          mappedFile,
                                          ref MemoryMappedViewAccessor  viewAccessor)
            {
                mappedFile      = MemoryMappedFile.CreateNew(null, capacity, MemoryMappedFileAccess.ReadWrite);
                viewAccessor    = mappedFile.CreateViewAccessor();
                mmfHandle       = mappedFile.SafeMemoryMappedFileHandle.DangerousGetHandle();
            }

            private void Render(Image image)
            {
                lock (lockObject)
                {
                    if (createNewBitmap)
                    {
                        var bitmap   = CreateInteropBitmap();
                        image.Source = bitmap;

                        //reset createNewBitmap, used in special case.
                        createNewBitmap = false;
                    }
                    else if (image.Source != null)
                    {
                        var sourceRect = new Int32Rect(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height);
                        var bitmap     = image.Source as InteropBitmap;
                        bitmap?.Invalidate(sourceRect);//if someone changes source without us
                    }

                    //possible set before, if exception occured
                    dispatcherOperation = null;
                }
            }

            private InteropBitmap CreateInteropBitmap()
            {
                if (mmfHandle == IntPtr.Zero)
                {
                    return null;
                }

                var stride = width * BytesPerPixel;
                return (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(mmfHandle, width, height, PixelFormat, stride, 0);
            }

            private void CreateRenderTask(Image image)
            {
                if (dispatcherOperation == null)
                {
                    Action<Image> action = Render;
                    dispatcherOperation = image.Dispatcher.BeginInvoke(action, dispatcherPriority, image);
                }
            }

            private static void UnionRect(ref Rect rect1, ref Rect rect2)
            {
                //include
                var x1 = Math.Min(rect1.X, rect2.X);
                var y1 = Math.Min(rect1.Y, rect2.Y);

                //exclude
                var x2 = Math.Max(rect1.X + rect1.Width, rect2.X + rect2.Width);
                var y2 = Math.Max(rect1.Y + rect1.Height, rect2.Y + rect2.Height);

                rect1 = new Rect(x1, y1, x2 - x1, y2 - y1);
            }

            private static void ReleaseMemoryMappedView(ref MemoryMappedFile            mappedFile,
                                                        ref MemoryMappedViewAccessor    stream)
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
}
