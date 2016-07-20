// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.Handlers
{
    public class BrowserProcessHandler : IBrowserProcessHandler
    {
        void IBrowserProcessHandler.OnContextInitialized()
        {
            //The Request Context has been initialized, you can now set preferences, like proxy server settings
            var cookieManager = Cef.GetGlobalCookieManager();
            cookieManager.SetStoragePath("cookies", true);
            cookieManager.SetSupportedSchemes("custom");

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
            //TODO: Schedule work on the UI thread - call Cef.DoMessageLoopWork
        }
    }
}
