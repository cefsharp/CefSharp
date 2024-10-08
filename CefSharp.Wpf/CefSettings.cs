// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Wpf
{
    /// <summary>
    /// Initialization settings. Many of these and other settings can also configured
    /// using command-line switches.
    /// </summary>
    public class CefSettings : CefSettingsBase
    {
        /// <summary>
        /// Intialize with default values
        /// </summary>
        public CefSettings() : base()
        {
            WindowlessRenderingEnabled = true;

            //Disable multithreaded, compositor scrolling of web content
            //With OSR rendering it's fairly common for this to improve scrolling performace
            //https://peter.sh/experiments/chromium-command-line-switches/#disable-threaded-scrolling
            //CefCommandLineArgs.Add("disable-threaded-scrolling");

            // CEF doesn't call GetAuthCredentials unless
            // the Chrome login prompt is disabled
            // https://github.com/chromiumembedded/cef/issues/3603 
            CefCommandLineArgs.Add("disable-chrome-login-prompt");

            // Disable "Restore pages" popup after incorrect shutdown
            // https://github.com/chromiumembedded/cef/issues/3767
            CefCommandLineArgs.Add("hide-crash-restore-bubble");
        }
    }
}
