// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Wpf
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

            if (Cef.CefVersion.StartsWith("r3.3497"))
            {
                throw new System.Exception("Issue #2408 should have been resolved, remove the below code");
            }

            //https://github.com/cefsharp/CefSharp/issues/2408
            CefCommandLineArgs.Add("disable-features", "AsyncWheelEvents,SurfaceSynchronization,TouchpadAndWheelScrollLatching");
            CefCommandLineArgs.Add("disable-blink-features", "RootLayerScrolling");
        }
    }
}
