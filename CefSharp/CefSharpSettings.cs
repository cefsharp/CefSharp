// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Use this static class to configure some CefSharp specific settings like WcfTimeout
    /// </summary>
    public static class CefSharpSettings
    {
        /// <summary>
        /// Set default values for CefSharpSettings
        /// </summary>
        static CefSharpSettings()
        {
            ShutdownOnExit = true;
            WcfTimeout = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// WCF is used by JavascriptBinding
        /// Disabling effectively disables both of these features.
        /// Defaults to true
        /// </summary>
        public static bool WcfEnabled { get; set; }

        /// <summary>
        /// Change the Close timeout for the WCF channel used by the sync JSB binding.
        /// The default value is currently 10 seconds. Chaning this to <see cref="TimeSpan.Zero"/>
        /// will result on Abort() being called on the WCF Channel Host
        /// </summary>
        public static TimeSpan WcfTimeout { get; set; }

        /// <summary>
        /// For the WinForms and WPF instances of ChromiumWebBrowser the relevant Application Exit event
        /// is hooked and Cef.Shutdown() called by default. Set this to false to disable this behaviour.
        /// This value needs to be set before the first instance of ChromiumWebBrowser is created as
        /// the event handlers are hooked in the static constructor for the ChromiumWebBrowser class
        /// </summary>
        public static bool ShutdownOnExit { get; set; }
    }
}
