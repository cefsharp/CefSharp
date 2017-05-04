// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
            WcfTimeout = TimeSpan.FromSeconds(2);
        }

        /// <summary>
        /// WCF is used by JavascriptBinding
        /// Disabling effectively disables both of these features.
        /// Defaults to true
        /// </summary>
        public static bool WcfEnabled { get; set; }

        /// <summary>
        /// Change the Close timeout for the WCF channel used by the sync JSB binding.
        /// The default value is currently 2 seconds. Changing this to <see cref="TimeSpan.Zero"/>
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

        /// <summary>
        /// The proxy options that will be used for all connections
        /// 
        /// If set before the call to Cef.Initialize, command line arguments will be set for you
        /// If a username and password is provided and the IPs match authentication is done automatically
        /// 
        /// NOTE: GetAuthCredentials won't be called for a proxy server that matches the IP
        /// NOTE: It isn't possble to change the proxy after the call to Cef.Initialize
        /// </summary>
        public static ProxyOptions Proxy { get; set; }

        /// <summary>
        /// This influences the behavior of RegisterAsyncJsObject and how method calls are made.
        /// By default the <see cref="MethodRunnerQueue"/> executes Tasks in a sync fashion.
        /// Setting this property to true will allocate new Tasks on TaskScheduler.Default for execution.
        /// </summary>
        public static bool ConcurrentTaskExecution { get; set; }
    }
}
