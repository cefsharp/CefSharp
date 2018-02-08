// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CefSharp;
using CefSharp.Internals;
using GalaSoft.MvvmLight.Command;

namespace CefSharp.Wpf.Example.Controls
{
    /// <summary>
    /// Example with Screenshot support - adapted from https://github.com/cefsharp/CefSharp/pull/462/
    /// </summary>
    public class ChromiumWebBrowserWithScreenshotSupport : ChromiumWebBrowser
    {
        private volatile bool isTakingScreenshot = false;
        private Size? screenshotSize;
        private int oldFrameRate;
        private int ignoreFrames = 0;
        private TaskCompletionSource<InteropBitmap> screenshotTaskCompletionSource;

        public ICommand ScreenshotCommand { get; set; }

        public ChromiumWebBrowserWithScreenshotSupport() : base()
        {
            ScreenshotCommand = new RelayCommand(TakeScreenshot);
        }

        public Task<InteropBitmap> TakeScreenshot(Size screenshotSize, int? frameRate = 1, int? ignoreFrames = 0, TimeSpan? timeout = null)
        {
            if (screenshotTaskCompletionSource != null && screenshotTaskCompletionSource.Task.Status == TaskStatus.Running)
            {
                throw new Exception("Screenshot already in progress, you must wait for the previous screenshot to complete");
            }

            if(IsBrowserInitialized == false)
            {
                throw new Exception("Browser has not yet finished initializing or is being disposed");
            }

            if(IsLoading)
            {
                throw new Exception("Unable to take screenshot while browser is loading");
            }

            var browserHost = this.GetBrowser().GetHost();

            if(browserHost == null)
            {
                throw new Exception("IBrowserHost is null");
            }

            screenshotTaskCompletionSource = new TaskCompletionSource<InteropBitmap>();

            if(timeout.HasValue)
            {
                screenshotTaskCompletionSource = screenshotTaskCompletionSource.WithTimeout(timeout.Value);
            }

            if(frameRate.HasValue)
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

        protected override ViewRect GetViewRect()
        {
            if(isTakingScreenshot)
            {
                return new ViewRect((int)Math.Ceiling(screenshotSize.Value.Width), (int)Math.Ceiling(screenshotSize.Value.Height));
            }

            return base.GetViewRect();
        }

        protected override void OnPaint(BitmapInfo bitmapInfo)
        {
            if(isTakingScreenshot)
            {
                //We ignore the first n number of frames
                if (ignoreFrames > 0)
                {
                    ignoreFrames--;
                    return;
                }

                //Wait until we have a frame that matches the updated size we requested
                if (screenshotSize.HasValue && screenshotSize.Value.Width == bitmapInfo.Width && screenshotSize.Value.Height == bitmapInfo.Height)
                { 
                    //Bitmaps need to be created on the UI thread
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        var stride = bitmapInfo.Width * bitmapInfo.BytesPerPixel;

                        lock (bitmapInfo.BitmapLock)
                        {
                            //NOTE: Interopbitmap is not capable of supporting DPI scaling
                            var bitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(bitmapInfo.FileMappingHandle,
                                bitmapInfo.Width, bitmapInfo.Height, PixelFormats.Bgra32, stride, 0);
                            //Using TaskExtensions.TrySetResultAsync extension method so continuation runs on Threadpool
                            screenshotTaskCompletionSource.TrySetResultAsync(bitmap);

                            isTakingScreenshot = false;
                            var browserHost = GetBrowser().GetHost();
                            //Return the framerate to the previous value
                            browserHost.WindowlessFrameRate = oldFrameRate;
                            //Let the browser know the size changes so normal rendering can continue
                            browserHost.WasResized();
                        }
                    }));
                }
            }
            else
            { 
                base.OnPaint(bitmapInfo);
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

                    TakeScreenshot(screenshotSize, ignoreFrames:0).ContinueWith((screenshotTask) =>
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

                                Process.Start(new System.Diagnostics.ProcessStartInfo
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
