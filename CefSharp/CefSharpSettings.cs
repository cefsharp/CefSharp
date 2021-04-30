// Copyright © 2015 The CefSharp Authors. All rights reserved.
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
#if !NETCOREAPP
            WcfTimeout = TimeSpan.FromSeconds(2);
#endif
            SubprocessExitIfParentProcessClosed = true;
        }

#if !NETCOREAPP
        /// <summary>
        /// WCF is used by JavascriptObjectRepository.Register(isAsync: false) feature for
        /// Javascript Binding. It's recomended that anyone developing a new application use
        /// the JavascriptObjectRepository.Register(isAsync: true) version which communicates
        /// using native Chromium IPC.
        /// </summary>
        public static bool WcfEnabled { get; set; }

        /// <summary>
        /// Change the Close timeout for the WCF channel used by the sync JSB binding.
        /// The default value is currently 2 seconds. Changing this to <see cref="TimeSpan.Zero"/>
        /// will result on Abort() being called on the WCF Channel Host
        /// </summary>
        public static TimeSpan WcfTimeout { get; set; }
#endif

        /// <summary>
        /// For the WinForms and WPF instances of ChromiumWebBrowser the relevant Application Exit event
        /// is hooked and Cef.Shutdown() called by default. Set this to false to disable this behaviour.
        /// This value needs to be set before the first instance of ChromiumWebBrowser is created as
        /// the event handlers are hooked in the static constructor for the ChromiumWebBrowser class
        /// </summary>
        public static bool ShutdownOnExit { get; set; }

        /// <summary>
        /// CefSharp.BrowserSubprocess will monitor the parent process and exit if the parent process closes
        /// before the subprocess. This currently defaults to true. 
        /// See https://github.com/cefsharp/CefSharp/issues/2359 for more information.
        /// </summary>
        public static bool SubprocessExitIfParentProcessClosed { get; set; }

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
        /// This influences the behavior of how methods are executed for objects registered using
        /// <see cref="IJavascriptObjectRepository.Register"/>.
        /// By default the <see cref="Internals.MethodRunnerQueue"/> queues Tasks for execution in a sequential order.
        /// A single method is exeucted at a time. Setting this property to true allows for concurrent task execution.
        /// Method calls are executed on <see cref="System.Threading.Tasks.TaskScheduler.Default"/> (ThreadPool).
        /// </summary>
        public static bool ConcurrentTaskExecution { get; set; }

        /// <summary>
        /// If true a message will be sent from the render subprocess to the
        /// browser when a DOM node (or no node) gets focus. The default is
        /// false.
        /// </summary>
        public static bool FocusedNodeChangedEnabled { get; set; }
    }
}
