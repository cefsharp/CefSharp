// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.OffScreen
{
    /// <summary>
    /// Initialization settings. Many of these and other settings can also configured
    /// using command-line switches.
    /// </summary>
    public class CefSettings : AbstractCefSettings
    {
        /// <summary>
        /// Intialize with default values
        /// </summary>
        public CefSettings() : base()
        {
            WindowlessRenderingEnabled = true;

            //For OffScreen it doesn't make much sense to enable audio by default, so we disable it.
            //this can be removed in user code if required
            CefCommandLineArgs.Add("mute-audio", "1");

            if (Cef.CefVersion.StartsWith("r3.3497"))
            {
                throw new System.Exception("Issue #2408 should have been resolved, remove the below code");
            }

            //https://github.com/cefsharp/CefSharp/issues/2408
            CefCommandLineArgs.Add("disable-features", "AsyncWheelEvents,TouchpadAndWheelScrollLatching");
        }
    }
}
