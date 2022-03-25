// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CefSharp.SchemeHandler;

namespace CefSharp.Example.Handlers
{
    public class BrowserProcessHandler : CefSharp.Handler.BrowserProcessHandler
    {
        /// <summary>
        /// The interval between calls to Cef.DoMessageLoopWork
        /// </summary>
        protected const int SixtyTimesPerSecond = 1000 / 60;  // 60fps
        /// <summary>
        /// The maximum number of milliseconds we're willing to wait between calls to OnScheduleMessagePumpWork().
        /// </summary>
        protected const int ThirtyTimesPerSecond = 1000 / 30;  //30fps

        protected override void OnContextInitialized()
        {
            //The Global CookieManager has been initialized, you can now set cookies
            var cookieManager = Cef.GetGlobalCookieManager();
            
            if (cookieManager.SetCookie("custom://cefsharp/home.html", new Cookie
            {
                Name = "CefSharpTestCookie",
                Value = "ILikeCookies",
                Expires = DateTime.Now.AddDays(1)
            }))
            {
                cookieManager.VisitUrlCookiesAsync("custom://cefsharp/home.html", false).ContinueWith(previous =>
                {
                    if (previous.Status == TaskStatus.RanToCompletion)
                    {
                        var cookies = previous.Result;

                        foreach (var cookie in cookies)
                        {
                            Debug.WriteLine("CookieName: " + cookie.Name);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No Cookies found");
                    }
                });

                cookieManager.VisitAllCookiesAsync().ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        var cookies = t.Result;

                        foreach (var cookie in cookies)
                        {
                            Debug.WriteLine("CookieName: " + cookie.Name);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No Cookies found");
                    }
                });
            }

            //The Request Context has been initialized, you can now set preferences, like proxy server settings
            //Dispose of context when finished - preferable not to keep a reference if possible.
            using (var context = Cef.GetGlobalRequestContext())
            {
                string errorMessage;

                //You can set most preferences using a `.` notation rather than having to create a complex set of dictionaries.
                //The default is true, you can change to false to disable resizing of text areas
                //This is just one example, there are many many configuration options avaliable via preferences
                var success = context.SetPreference("webkit.webprefs.text_areas_are_resizable", true, out errorMessage);

                if(!success)
                {
                    //Check the errorMessage for more details
                }

                //string error;
                //var dicts = new List<string> { "en-GB", "en-US" };
                //var success = context.SetPreference("spellcheck.dictionaries", dicts, out error);

                //The no-proxy-server flag is set in CefExample.cs class, you'll have to remove that before you can test
                //this code out
                //var v = new Dictionary<string, string>
                //{
                //    ["mode"] = "fixed_servers",
                //    ["server"] = "scheme://host:port"
                //};
                //success = context.SetPreference("proxy", v, out errorMessage);

                //It's possible to register a scheme handler for the default http and https schemes
                //In this example we register the FolderSchemeHandlerFactory for https://cefsharp.example
                //Best to include the domain name, so only requests for that domain are forwarded to your scheme handler
                //It is possible to intercept all requests for a scheme, including the built in http/https ones, be very careful doing this!
                const string cefSharpExampleResourcesFolder =
#if !NETCOREAPP
                    @"..\..\..\..\CefSharp.Example\Resources";
#else
                    @"..\..\..\..\..\..\CefSharp.Example\Resources";
#endif
                var folderSchemeHandlerExample = new FolderSchemeHandlerFactory(rootFolder: cefSharpExampleResourcesFolder,
                                                                        hostName: "cefsharp.example", //Optional param no hostname checking if null
                                                                        defaultPage: "home.html"); //Optional param will default to index.html

                context.RegisterSchemeHandlerFactory("https", "cefsharp.example", folderSchemeHandlerExample);
            }
        }
    }
}
