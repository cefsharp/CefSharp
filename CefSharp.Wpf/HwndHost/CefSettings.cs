// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Wpf.HwndHost
{
    /// <summary>
    /// Initialization settings. Many of these and other settings can also configured using command-line switches.
    /// </summary>
    public class CefSettings : CefSettingsBase
    {
        /// <summary>
        /// Intialize with default values
        /// </summary>
        public CefSettings() : base()
        {
            // CEF doesn't call GetAuthCredentials unless the Chrome login prompt is disabled
            // https://github.com/chromiumembedded/cef/issues/3603 
            CefCommandLineArgs.Add("disable-chrome-login-prompt");

            // Disable "Restore pages" popup after incorrect shutdown
            // https://github.com/chromiumembedded/cef/issues/3767
            CefCommandLineArgs.Add("hide-crash-restore-bubble");

            // Disable the back-forward cache
            // https://github.com/cefsharp/CefSharp/issues/4621
            CefCommandLineArgs.Add("disable-back-forward-cache");
        }
    }
}
