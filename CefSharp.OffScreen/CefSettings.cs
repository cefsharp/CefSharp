// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.OffScreen
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

            //For OffScreen it doesn't make much sense to enable audio by default, so we disable it.
            //this can be removed in user code if required
            CefCommandLineArgs.Add("mute-audio");

            // CEF doesn't call GetAuthCredentials unless
            // the Chrome login prompt is disabled
            // https://github.com/chromiumembedded/cef/issues/3603 
            CefCommandLineArgs.Add("disable-chrome-login-prompt");

            // Disable "Restore pages" popup after incorrect shutdown
            // https://github.com/chromiumembedded/cef/issues/3767
            CefCommandLineArgs.Add("hide-crash-restore-bubble");

            // Disable the back-forward cache
            // https://github.com/cefsharp/CefSharp/issues/4621
            CefCommandLineArgs.Add("disable-back-forward-cache");
        }

        /// <summary>
        /// Enables Audio - by default audio is muted in the OffScreen implementatio.
        /// This removes the mute-audio command line flag
        /// </summary>
        public void EnableAudio()
        {
            if (CefCommandLineArgs.ContainsKey("mute-audio"))
            {
                CefCommandLineArgs.Remove("mute-audio");
            }
        }
    }
}
