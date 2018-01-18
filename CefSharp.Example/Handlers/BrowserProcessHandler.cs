// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CefSharp.Example.Handlers
{
    public class BrowserProcessHandler : IBrowserProcessHandler
    {
        /// <summary>
        /// The maximum number of milliseconds we're willing to wait between calls to OnScheduleMessagePumpWork().
        /// </summary>
        protected const int MaxTimerDelay = 1000 / 30;  // 30fps

        void IBrowserProcessHandler.OnContextInitialized()
        {
            //The Request Context has been initialized, you can now set preferences, like proxy server settings
            var cookieManager = Cef.GetGlobalCookieManager();
            cookieManager.SetStoragePath("cookies", true);
            cookieManager.SetSupportedSchemes(new string[] {"custom"});
            if(cookieManager.SetCookie("custom://cefsharp/home.html", new Cookie
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
                            Debug.WriteLine("CookieName:" + cookie.Name);
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
                            Debug.WriteLine("CookieName:" + cookie.Name);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No Cookies found");
                    }
                });
            }
            
            //Dispose of context when finished - preferable not to keep a reference if possible.
            using (var context = Cef.GetGlobalRequestContext())
            {
                string errorMessage;
                //You can set most preferences using a `.` notation rather than having to create a complex set of dictionaries.
                //The default is true, you can change to false to disable
                context.SetPreference("webkit.webprefs.plugins_enabled", true, out errorMessage);
            }
        }

        void IBrowserProcessHandler.OnScheduleMessagePumpWork(long delay)
        {
            //If the delay is greater than the Maximum then use MaxTimerDelay
            //instead - we do this to achieve a minimum number of FPS
            if(delay > MaxTimerDelay)
            {
                delay = MaxTimerDelay;
            }
            OnScheduleMessagePumpWork((int)delay);
        }

        protected virtual void OnScheduleMessagePumpWork(int delay)
        {
            //TODO: Schedule work on the UI thread - call Cef.DoMessageLoopWork
        }

        public virtual void Dispose()
        {
            
        }
    }
}
