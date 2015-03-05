// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Example;

namespace CefSharp.OffScreen.Example
{
    public class Program
    {
        private static ChromiumWebBrowser browser;
        private static bool captureFirstRenderedImage = false;

        public static void Main(string[] args)
        {
            const string testUrl = "https://www.google.com/";

            Console.WriteLine("This example application will load {0}, take a screenshot, and save it to your desktop.", testUrl);
            Console.WriteLine("You may see a lot of Chromium debugging output, please wait...");
            Console.WriteLine();

            // You need to replace this with your own call to Cef.Initialize();
            CefExample.Init();

            // Create the offscreen Chromium browser.
            using (browser = new ChromiumWebBrowser(testUrl))
            {

                // An event that is fired when the first page is finished loading.
                // This returns to us from another thread.
                if (captureFirstRenderedImage)
                {
                    browser.ResourceHandlerFactory.RegisterHandler(testUrl, ResourceHandler.FromString("<html><body><h1>CefSharp OffScreen</h1></body></html>"));
                    browser.ScreenshotAsync().ContinueWith(DisplayBitmap);
                }
                else
                {
                    browser.FrameLoadEnd += BrowserFrameLoadEnd;
                }

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
                // Remove the load event handler, because we only want one snapshot of the initial page.
                browser.FrameLoadEnd -= BrowserFrameLoadEnd;

                // Wait for the screenshot to be taken.
                browser.ScreenshotAsync().ContinueWith(DisplayBitmap);
            }
        }

        private static void DisplayBitmap(Task<Bitmap> task)
        {
            // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
            var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

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
