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
            LegacyJavascriptBindingEnabled = false;
            WcfTimeout = TimeSpan.FromSeconds(2);
            SubprocessExitIfParentProcessClosed = true;
        }

        /// <summary>
        /// Objects registered using RegisterJsObject and RegisterAsyncJsObject
        /// will be automatically bound when a V8Context is created. (Soon as the Javascript
        /// context is created for a browser). This behaviour is like that seen with Javascript
        /// Binding in version 57 and earlier.
        /// NOTE: Set this before your first call to RegisterJsObject or RegisterAsyncJsObject
        /// </summary>
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
        /// This influences the behavior of RegisterAsyncJsObject and how method calls are made.
        /// By default the <see cref="Internals.MethodRunnerQueue"/> executes Tasks in a sync fashion.
        /// Setting this property to true will allocate new Tasks on TaskScheduler.Default for execution.
        /// </summary>
        public static bool ConcurrentTaskExecution { get; set; }


        /// <summary>
        /// This changes the behavior of CefSharp so that marshaling of data between .NET and Javascript is type-safe. <br/> <br/>
        /// 
        /// Javascript objects passed to .NET will be run through a strict type binder that validates their structure. <br/> <br/>
        /// 
        /// .NET objects returned to Javascript are properly serialized so that .NET only types like <see cref="Guid"/> and <see cref="Tuple"/> become available in Javascript. <br/> <br/>
        ///
        /// This means if you have a class with a <see cref="Guid"/> property, it will automatically be handled when you return that class to Javascript. <br/> <br/>
        /// And when you pass the same object back to .NET as the parameter of a method, it will be marshaled back to the original class with the correct values. <br/> <br/>
        ///
        /// It will also enable asynchronous task to be ran without <see cref="ConcurrentTaskExecution"/> being set to true. <br/> <br/>
        ///
        /// When this setting is enabled any failures to marshal data will raise exceptions so you can catch errors in your code. <br/> <br/>
        ///
        /// The best part is your .NET code can continue to use .NET coding conventions without forcing your Javascript and TypeScript code to drop theirs.  <br/> <br/>
        /// Please note, returning Structs to Javascript when this mode is enabled for security reasons.   <br/>
        ///
        ///  This setting has no backwards compatibility with the default binding behavior, so if you built code around it enabling this will probably break app. <br/> <br/>
        ///  All bound objects and CefSharp post will use a internal TypeSafe binder and interceptor to passively handle objects.
        /// </summary>
        public static bool TypeSafeMarshaling { get; set; }

        /// <summary>
        /// If true a message will be sent from the render subprocess to the
        /// browser when a DOM node (or no node) gets focus. The default is
        /// false.
        /// </summary>
        public static bool FocusedNodeChangedEnabled { get; set; }
    }
}
