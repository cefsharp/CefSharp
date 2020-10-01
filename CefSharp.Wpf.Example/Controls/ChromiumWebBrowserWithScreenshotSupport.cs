// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using Size = System.Windows.Size;

namespace CefSharp.Wpf.Example.Controls
{
    /// <summary>
    /// Example with Screenshot support - adapted from https://github.com/cefsharp/CefSharp/pull/462/
    /// </summary>
    public class ChromiumWebBrowserWithScreenshotSupport : ChromiumWebBrowser
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private static readonly PixelFormat PixelFormat = PixelFormats.Pbgra32;
        private static readonly int BytesPerPixel = PixelFormat.BitsPerPixel / 8;

        private volatile bool isTakingScreenshot = false;
        private Size? screenshotSize;
        private int oldFrameRate;
        private int ignoreFrames = 0;
        private TaskCompletionSource<InteropBitmap> screenshotTaskCompletionSource;
        private CancellationTokenRegistration? cancellationTokenRegistration;

        public ICommand ScreenshotCommand { get; set; }

        public ChromiumWebBrowserWithScreenshotSupport() : base()
        {
            ScreenshotCommand = new RelayCommand(TakeScreenshot);
        }

        public Task<InteropBitmap> TakeScreenshot(Size screenshotSize, int? frameRate = 1, int? ignoreFrames = 0, CancellationToken? cancellationToken = null)
        {
            if (screenshotTaskCompletionSource != null && screenshotTaskCompletionSource.Task.Status == TaskStatus.Running)
            {
                throw new Exception("Screenshot already in progress, you must wait for the previous screenshot to complete");
            }

            if (IsBrowserInitialized == false)
            {
                throw new Exception("Browser has not yet finished initializing or is being disposed");
            }

            if (IsLoading)
            {
                throw new Exception("Unable to take screenshot while browser is loading");
            }

            var browserHost = this.GetBrowser().GetHost();

            if (browserHost == null)
            {
                throw new Exception("IBrowserHost is null");
            }

            screenshotTaskCompletionSource = new TaskCompletionSource<InteropBitmap>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (cancellationToken.HasValue)
            {
                var token = cancellationToken.Value;
                cancellationTokenRegistration = token.Register(() =>
                {
                    screenshotTaskCompletionSource.TrySetCanceled();

                    cancellationTokenRegistration?.Dispose();

                }, useSynchronizationContext: false);
            }

            if (frameRate.HasValue)
            {
                oldFrameRate = browserHost.WindowlessFrameRate;
                browserHost.WindowlessFrameRate = frameRate.Value;
            }

            this.screenshotSize = screenshotSize;
            this.isTakingScreenshot = true;
            this.ignoreFrames = ignoreFrames.GetValueOrDefault() < 0 ? 0 : ignoreFrames.GetValueOrDefault();
            //Resize the browser using the desired screenshot dimensions
            //The resulting bitmap will never be rendered to the screen
            browserHost.WasResized();

            return screenshotTaskCompletionSource.Task;
        }

        protected override CefSharp.Structs.Rect GetViewRect()
        {
            if (isTakingScreenshot)
            {
                return new CefSharp.Structs.Rect(0, 0, (int)Math.Ceiling(screenshotSize.Value.Width), (int)Math.Ceiling(screenshotSize.Value.Height));
            }

            return base.GetViewRect();
        }

        protected override void OnPaint(bool isPopup, Structs.Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            if (isTakingScreenshot)
            {
                //We ignore the first n number of frames
                if (ignoreFrames > 0)
                {
                    ignoreFrames--;
                    return;
                }

                //Wait until we have a frame that matches the updated size we requested
                if (screenshotSize.HasValue && screenshotSize.Value.Width == width && screenshotSize.Value.Height == height)
                {
                    var stride = width * BytesPerPixel;
                    var numberOfBytes = stride * height;

                    //Create out own memory mapped view for the screenshot and copy the buffer into it.
                    //If we were going to create a lot of screenshots then it would be better to allocate a large buffer
                    //and reuse it.
                    var mappedFile = MemoryMappedFile.CreateNew(null, numberOfBytes, MemoryMappedFileAccess.ReadWrite);
                    var viewAccessor = mappedFile.CreateViewAccessor();

                    CopyMemory(viewAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle(), buffer, (uint)numberOfBytes);

                    //Bitmaps need to be created on the UI thread
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        var backBuffer = mappedFile.SafeMemoryMappedFileHandle.DangerousGetHandle();
                        //NOTE: Interopbitmap is not capable of supporting DPI scaling
                        var bitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(backBuffer,
                            width, height, PixelFormat, stride, 0);
                        screenshotTaskCompletionSource.TrySetResult(bitmap);

                        isTakingScreenshot = false;
                        var browserHost = GetBrowser().GetHost();
                        //Return the framerate to the previous value
                        browserHost.WindowlessFrameRate = oldFrameRate;
                        //Let the browser know the size changes so normal rendering can continue
                        browserHost.WasResized();

                        viewAccessor?.Dispose();
                        mappedFile?.Dispose();

                        cancellationTokenRegistration?.Dispose();
                    }));
                }
            }
            else
            {
                base.OnPaint(isPopup, dirtyRect, buffer, width, height);
            }
        }

        private void TakeScreenshot()
        {
            var uiThreadTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            const string script = "[document.body.scrollWidth, document.body.scrollHeight]";
            this.EvaluateScriptAsync(script).ContinueWith((scriptTask) =>
            {
                var javascriptResponse = scriptTask.Result;

                if (javascriptResponse.Success)
                {
                    var widthAndHeight = (List<object>)javascriptResponse.Result;

                    var screenshotSize = new Size((int)widthAndHeight[0], (int)widthAndHeight[1]);

                    TakeScreenshot(screenshotSize, ignoreFrames: 0).ContinueWith((screenshotTask) =>
                     {
                         if (screenshotTask.Status == TaskStatus.RanToCompletion)
                         {
                             try
                             {
                                 var bitmap = screenshotTask.Result;
                                 var tempFile = Path.GetTempFileName().Replace(".tmp", ".png");
                                 using (var stream = new FileStream(tempFile, FileMode.Create))
                                 {
                                     var encoder = new PngBitmapEncoder();
                                     encoder.Frames.Add(BitmapFrame.Create(bitmap));
                                     encoder.Save(stream);
                                 }

                                 Process.Start(new ProcessStartInfo
                                 {
                                     UseShellExecute = true,
                                     FileName = tempFile
                                 });
                             }
                             catch (Exception ex)
                             {
                                 var msg = ex.ToString();
                             }
                         }
                         else
                         {
                             MessageBox.Show("Unable to capture screenshot");
                         }
                     }, uiThreadTaskScheduler); //Make sure continuation runs on UI thread

                }
                else
                {
                    MessageBox.Show("Unable to obtain size of screenshot");
                }
            }, uiThreadTaskScheduler);
        }
    }
}
