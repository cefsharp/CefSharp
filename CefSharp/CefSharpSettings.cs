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
            LegacyJavascriptBindingEnabled = false;
            WcfTimeout = TimeSpan.FromSeconds(2);
        }

        /// <summary>
        /// Objects registered using RegisterJsObject and RegisterAsyncJsObject
        /// will be automatically bound in the first render process that's created
        /// for a ChromiumWebBrowser instance. If you perform a cross-site
        /// navigation a process switch will occur and bound objects will no longer
        /// be automatically avaliable. For those upgrading from version 57 or below
        /// that do no perform cross-site navigation (e.g. Single Page applications or
        /// applications that only refer to a single domain) can set this property to 
        /// true and use the old behaviour.Defaults to false
        /// NOTE: Set this before your first call to RegisterJsObject or RegisterAsyncJsObject
        /// </summary>
        /// <remarks>
        /// Javascript binding in CefSharp version 57 and below used the
        /// --process-per-tab Process Model to limit the number of render
        /// processes to 1 per ChromiumWebBrowser instance, this allowed
        /// us to communicate bound javascript objects when the process was
        /// initially created (OnRenderViewReady is only called for the first
        /// process creation or after a crash), subsiquently all bound objects
        /// were registered in ever V8Context in OnContextCreated (executed in the render process).
        /// Chromium has made changes and --process-per-tab is not currently working.
        /// Performing a cross-site navigation (from one domain to a different domain)
        /// will cause a new render process to be created, subsiquent render processes 
        /// won't have access to the bound object information by default.
        /// </remarks>
        public static bool LegacyJavascriptBindingEnabled { get; set; }

        /// <summary>
        /// WCF is used by RegisterJsObject feature for Javascript Binding
        /// It's reccomended that anyone developing a new application use 
        /// the RegisterAsyncJsObject version which communicates using native
        /// Chromium IPC.
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
        /// CefSharp.BrowserSubprocess will monitor the parent process and exit if the parent process closes
        /// before the subprocess. This currently defaults to false. 
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
        /// This influences the behavior of RegisterAsyncJsObject and how method calls are made.
        /// By default the <see cref="MethodRunnerQueue"/> executes Tasks in a sync fashion.
        /// Setting this property to true will allocate new Tasks on TaskScheduler.Default for execution.
        /// </summary>
        public static bool ConcurrentTaskExecution { get; set; }
    }
}
