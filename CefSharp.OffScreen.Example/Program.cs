// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
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
        private static readonly string TempDirectory = Path.GetTempPath() + "CefSharp\\";
        private static readonly List<Browser> Browsers = new List<Browser>();
        private static string browserName;
        private static Boolean _imageSaved;

        public static void Main(string[] args)
        {
            if (!Directory.Exists(TempDirectory))
            {
                Directory.CreateDirectory(TempDirectory);
            }
            const string testUrl = "http://multibrowser.com/";
            //const string testUrl = "http://cnn.com/";
            var browser1 = new Browser
            {
                BrowserName = "800x600",
                BrowserWidth = 800,
                BrowserHeight = 600
            };
            Browsers.Add(browser1);
            var browser2 = new Browser
            {
                BrowserName = "1024x600",
                BrowserWidth = 1024,
                BrowserHeight = 600
            };
            Browsers.Add(browser2);
            var browser3 = new Browser
            {
                BrowserName = "1024x800",
                BrowserWidth = 1024,
                BrowserHeight = 800
            };
            Browsers.Add(browser3);

            Console.WriteLine("This example application will load {0}, take a screenshot, and save it to your desktop.", testUrl);
            Console.WriteLine();

            // You need to replace this with your own call to Cef.Initialize();
            CefExample.Init();

            // Create the offscreen Chromium browser.
            using (browser = new ChromiumWebBrowser(testUrl))
            {
                browser.FrameLoadEnd += BrowserFrameLoadEnd;
                //browser.LoadingStateChanged += Browser_LoadingStateChanged;
                foreach (var browserToTest in Browsers)
                {
                    _imageSaved = false;
                    browserName = browserToTest.BrowserName;
                    browser.Size = new Size(browserToTest.BrowserWidth, browserToTest.BrowserHeight);
                    browser.Reload(true);
                    while (!_imageSaved)
                    {
                    }
                }
            }
            Process.Start("explorer.exe", TempDirectory);
            Console.ReadKey();

            //using (browser = new ChromiumWebBrowser(testUrl))
            //{

            //    // An event that is fired when the first page is finished loading.
            //    // This returns to us from another thread.
            //    browser.FrameLoadEnd += BrowserFrameLoadEnd;
            //    browser.LoadingStateChanged += Browser_LoadingStateChanged;

            //    foreach (var browserToTest in Browsers)
            //    {
            //        _pageLoaded = false;
            //        browserName = browserToTest.BrowserName;
            //        browser.Size = new Size(browserToTest.BrowserWidth, browserToTest.BrowserHeight);
            //        browser.Reload(true);
            //        while (_pageLoaded)
            //        {

            //        }
            //        Console.WriteLine(browserName + " finished loading");
            //    }
            //    Process.Start("explorer.exe", TempDirectory);

            //    // We have to wait for something, otherwise the process will exit too soon.
            //    Console.ReadKey();
            //}

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
                //Task.Delay(2000).Wait();
                // Wait for the screenshot to be taken.
                browser.ScreenshotAsync().ContinueWith(DisplayBitmap);
            }
        }
        //private static void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        //{
        //    Console.WriteLine("Loading State:" + e.IsLoading);
        //}

        private static void DisplayBitmap(Task<Bitmap> task)
        {
            // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
            var screenshotPath = Path.Combine(TempDirectory, browserName + ".png");


            var bitmap = task.Result;

            // Save the Bitmap to the path.
            // The image type is auto-detected via the ".png" extension.
            bitmap.Save(screenshotPath);

            // We no longer need the Bitmap.
            // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
            bitmap.Dispose();
            _imageSaved = true;
            Console.WriteLine(browserName + " Saved");
        }
    }

    public class Browser
    {
        public string BrowserName { get; set; }
        public int BrowserWidth { get; set; }
        public int BrowserHeight { get; set; }
    }
}
