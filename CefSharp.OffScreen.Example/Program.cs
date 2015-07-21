// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Example;

namespace CefSharp.OffScreen.Example
{
    public class Program
    {
        private static ChromiumWebBrowser browser1;
        private static ChromiumWebBrowser browser2;

        public static void Main(string[] args)
        {
            const string testUrl = "https://www.google.com/";

            Console.WriteLine("This example application will load {0}, take a screenshot, and save it to your desktop.", testUrl);
            Console.WriteLine("You may see a lot of Chromium debugging output, please wait...");
            Console.WriteLine();

            // You need to replace this with your own call to Cef.Initialize();
            CefExample.Init();

            var settings1 = new BrowserSettings
            {
                RequestContext = new RequestContext("cookies1", false)
            };

            var settings2 = new BrowserSettings
            {
                RequestContext = new RequestContext("cookies2", false)
            };

            // Create the offscreen Chromium browser.
            using (browser1 = new ChromiumWebBrowser(testUrl, settings1))
            using (browser2 = new ChromiumWebBrowser(testUrl, settings2))
            {
                // An event that is fired when the first page is finished loading.
                // This returns to us from another thread.
                browser1.FrameLoadEnd += BrowserFrameLoadEnd;
                browser1.FrameLoadStart += (s, argsi) =>
                {
                    if (argsi.IsMainFrame)
                    {
                        browser1.ZoomLevel = 0;
                    }
                };

                browser2.FrameLoadEnd += BrowserFrameLoadEnd;
                browser2.FrameLoadStart += (s, argsi) =>
                {
                    if (argsi.IsMainFrame)
                    {
                        browser2.ZoomLevel = 3;
                    }
                };

                // We have to wait for something, otherwise the process will exit too soon.
                Console.ReadKey();
            }

            // Clean up Chromium objects.  You need to call this in your application otherwise
            // you will get a crash when closing.
            Cef.Shutdown();
        }

        private static void BrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            // Check to ensure it is the main frame which has finished loading
            // (rather than an iframe within the main frame).
            if (e.IsMainFrame)
            {
                var b = (ChromiumWebBrowser)sender;

                // Remove the load event handler, because we only want one snapshot of the initial page.
                // b.FrameLoadEnd -= BrowserFrameLoadEnd;

                // Wait for the screenshot to be taken.
                if (b == browser1)
                {
                    b.ScreenshotAsync().ContinueWith(t => DisplayBitmap(t, "CefSharp screenshot Zoomlevel 0.png"));
                }
                else
                {
                    b.ScreenshotAsync().ContinueWith(t => DisplayBitmap(t, "CefSharp screenshot Zoomlevel 3.png"));
                }
            }
        }

        private static void DisplayBitmap(Task<Bitmap> task, string name)
        {
            // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
            var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), name);

            Console.WriteLine();
            Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

            var bitmap = task.Result;

            // Save the Bitmap to the path.
            // The image type is auto-detected via the ".png" extension.
            bitmap.Save(screenshotPath);

            // We no longer need the Bitmap.
            // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
            bitmap.Dispose();

            Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

            // Tell Windows to launch the saved image.
            Process.Start(screenshotPath);

            Console.WriteLine("Image viewer launched.  Press any key to exit.");
        }
    }
}
