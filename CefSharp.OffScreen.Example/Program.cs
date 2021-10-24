// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CefSharp.OffScreen.Example
{
    public class Program
    {
        private const string TestUrlOne = "https://www.google.com/";
        private const string TestUrlTwo = "https://github.com/";

        public static int Main(string[] args)
        {
            Console.WriteLine("This example application will load {0}, take a screenshot, and save it to your desktop.", TestUrlOne);
            Console.WriteLine("You may see a lot of Chromium debugging output, please wait...");
            Console.WriteLine();

            //Console app doesn't have a message loop which we need as Cef.Initialize/Cef.Shutdown must be called on the same
            //thread. We use a super simple implementation from
            //https://devblogs.microsoft.com/pfxteam/await-synchronizationcontext-and-console-apps/
            //Continuations will happen on the main thread
            //The Nito.AsyncEx.Context Nuget package has a more advanced implementation
            //https://github.com/StephenCleary/AsyncEx/blob/8a73d0467d40ca41f9f9cf827c7a35702243abb8/doc/AsyncContext.md#console-example-using-asynccontext

            AsyncContext.Run(async delegate
            {
                Cef.EnableWaitForBrowsersToClose();

                var success = await Cef.InitializeAsync(new CefSettings());

                if (!success)
                {
                    return;
                }

                var t1 =  MainAsync(TestUrlOne, "cache\\path1", 1.0);
                //Demo showing Zoom Level of 2.0
                //Using seperate request contexts allows the urls from the same domain to have independent zoom levels
                //otherwise they would be the same - default behaviour of Chromium
                var t2 = MainAsync(TestUrlTwo, "cache\\path2", 2.0);

                await Task.WhenAll(t1, t2);

                // Wait for user input
                Console.ReadKey();

                //Wait until the browser has finished closing (which by default happens on a different thread).
                //Cef.EnableWaitForBrowsersToClose(); must be called before Cef.Initialize to enable this feature
                //See https://github.com/cefsharp/CefSharp/issues/3047 for details
                Cef.WaitForBrowsersToClose();

                // Clean up Chromium objects.  You need to call this in your application otherwise
                // you will get a crash when closing.
                Cef.Shutdown();
            });

            //Success
            return 0;
        }

        private static async Task MainAsync(string url, string cachePath, double zoomLevel)
        {
            var browserSettings = new BrowserSettings
            {
                //Reduce rendering speed to one frame per second so it's easier to take screen shots
                WindowlessFrameRate = 1
            };

            var requestContextSettings = new RequestContextSettings
            {
                CachePath = Path.GetFullPath(cachePath)
            };

            // RequestContext can be shared between browser instances and allows for custom settings
            // e.g. CachePath
            using (var requestContext = new RequestContext(requestContextSettings))
            using (var browser = new ChromiumWebBrowser(url, browserSettings, requestContext))
            {
                if (zoomLevel > 1)
                {
                    browser.FrameLoadStart += (s, argsi) =>
                    {
                        var b = (ChromiumWebBrowser)s;
                        if (argsi.Frame.IsMain)
                        {
                            b.SetZoomLevel(zoomLevel);
                        }
                    };
                }
                await browser.LoadUrlAsync();

                //Check preferences on the CEF UI Thread
                await Cef.UIThreadTaskFactory.StartNew(delegate
                {
                    var preferences = requestContext.GetAllPreferences(true);

                    //Check do not track status
                    var doNotTrack = (bool)preferences["enable_do_not_track"];

                    Debug.WriteLine("DoNotTrack: " + doNotTrack);
                });

                var onUi = Cef.CurrentlyOnThread(CefThreadIds.TID_UI);

                // For Google.com pre-pupulate the search text box
                if (url.Contains("google.com"))
                {
                    await browser.EvaluateScriptAsync("document.querySelector('[name=q]').value = 'CefSharp Was Here!'");
                }

                //Example using SendKeyEvent for input instead of javascript
                //var browserHost = browser.GetBrowserHost();
                //var inputString = "CefSharp Was Here!";
                //foreach(var c in inputString)
                //{
                //	browserHost.SendKeyEvent(new KeyEvent { WindowsKeyCode = c, Type = KeyEventType.Char });
                //}

                ////Give the browser a little time to finish drawing our SendKeyEvent input
                //await Task.Delay(100);

                // Wait for the screenshot to be taken,
                // if one exists ignore it, wait for a new one to make sure we have the most up to date
                var bitmap = await browser.ScreenshotAsync(true);

                DisplayBitmap(bitmap);

                //await browser.LoadUrlAsync("http://github.com");

                // Gets a warpper around the CefBrowserHost instance
                // You can perform a lot of low level browser operations using this interface
                //var cefbrowserHost = browser.GetBrowserHost();                

                //You can call Invalidate to redraw/refresh the image
                //cefbrowserHost.Invalidate(PaintElementType.View);

                // Wait for the screenshot to be taken.
                //bitmap = await browser.ScreenshotAsync(true);

                //DisplayBitmap(bitmap);
            }
        }

        private static void DisplayBitmap(Bitmap bitmap)
        {
            // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
            var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot" + DateTime.Now.Ticks + ".png");

            Console.WriteLine();
            Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

            // Save the Bitmap to the path.
            // The image type is auto-detected via the ".png" extension.
            bitmap.Save(screenshotPath);

            // We no longer need the Bitmap.
            // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
            bitmap.Dispose();

            Console.WriteLine("Screenshot saved. Launching your default image viewer...");

            // Tell Windows to launch the saved image.
            Process.Start(new ProcessStartInfo(screenshotPath)
            {
                // UseShellExecute is false by default on .NET Core.
                UseShellExecute = true
            });

            Console.WriteLine("Image viewer launched.  Press any key to exit.");
        }
    }
}
