// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Text;
using CefSharp.Example.Properties;
using CefSharp.Example.Proxy;
using CefSharp.SchemeHandler;

namespace CefSharp.Example
{
    public static class CefExample
    {
        //TODO: Revert after https://bitbucket.org/chromiumembedded/cef/issues/2685/networkservice-custom-scheme-unable-to
        //has been fixed.
        public const string BaseUrl = "https://cefsharp.example";
        public const string DefaultUrl = BaseUrl + "/home.html";
        public const string BindingTestUrl = BaseUrl + "/BindingTest.html";
        public const string BindingTestSingleUrl = BaseUrl + "/BindingTestSingle.html";
        public const string BindingTestsAsyncTaskUrl = BaseUrl + "/BindingTestsAsyncTask.html";
        public const string LegacyBindingTestUrl = BaseUrl + "/LegacyBindingTest.html";
        public const string PostMessageTestUrl = BaseUrl + "/PostMessageTest.html";
        public const string PluginsTestUrl = BaseUrl + "/plugins.html";
        public const string PopupTestUrl = BaseUrl + "/PopupTest.html";
        public const string TooltipTestUrl = BaseUrl + "/TooltipTest.html";
        public const string BasicSchemeTestUrl = BaseUrl + "/SchemeTest.html";
        public const string ResponseFilterTestUrl = BaseUrl + "/ResponseFilterTest.html";
        public const string DraggableRegionTestUrl = BaseUrl + "/DraggableRegionTest.html";
        public const string DragDropCursorsTestUrl = BaseUrl + "/DragDropCursorsTest.html";
        public const string CssAnimationTestUrl = BaseUrl + "/CssAnimationTest.html";
        public const string CdmSupportTestUrl = BaseUrl + "/CdmSupportTest.html";
        public const string TestResourceUrl = "http://test/resource/load";
        public const string RenderProcessCrashedUrl = "http://processcrashed";
        public const string TestUnicodeResourceUrl = "http://test/resource/loadUnicode";
        public const string PopupParentUrl = "http://www.w3schools.com/jsref/tryit.asp?filename=tryjsref_win_close";

        // Use when debugging the actual SubProcess, to make breakpoints etc. inside that project work.
        private static readonly bool DebuggingSubProcess = Debugger.IsAttached;
        private static string PluginInformation = "";

        public static void Init(AbstractCefSettings settings, IBrowserProcessHandler browserProcessHandler)
        {
            // Set Google API keys, used for Geolocation requests sans GPS.  See http://www.chromium.org/developers/how-tos/api-keys
            // Environment.SetEnvironmentVariable("GOOGLE_API_KEY", "");
            // Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_ID", "");
            // Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_SECRET", "");

            // Widevine CDM registration - pass in directory where Widevine CDM binaries and manifest.json are located.
            // For more information on support for DRM content with Widevine see: https://github.com/cefsharp/CefSharp/issues/1934
            //Cef.RegisterWidevineCdm(@".\WidevineCdm");

            //Chromium Command Line args
            //http://peter.sh/experiments/chromium-command-line-switches/
            //NOTE: Not all relevant in relation to `CefSharp`, use for reference purposes only.
            //CEF specific command line args
            //https://bitbucket.org/chromiumembedded/cef/src/master/libcef/common/cef_switches.cc?fileviewer=file-view-default
            //IMPORTANT: For enabled/disabled command line arguments like disable-gpu specifying a value of "0" like
            //settings.CefCommandLineArgs.Add("disable-gpu", "0"); will have no effect as the second argument is ignored.

            settings.RemoteDebuggingPort = 8088;
            //The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
            //HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
            settings.CachePath = "cache";
            //settings.UserAgent = "CefSharp Browser" + Cef.CefSharpVersion; // Example User Agent
            //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
            //settings.CefCommandLineArgs.Add("renderer-startup-dialog");
            //settings.CefCommandLineArgs.Add("enable-media-stream"); //Enable WebRTC
            //settings.CefCommandLineArgs.Add("no-proxy-server"); //Don't use a proxy server, always make direct connections. Overrides any other proxy server flags that are passed.
            //settings.CefCommandLineArgs.Add("debug-plugin-loading"); //Dumps extra logging about plugin loading to the log file.
            //settings.CefCommandLineArgs.Add("disable-plugins-discovery"); //Disable discovering third-party plugins. Effectively loading only ones shipped with the browser plus third-party ones as specified by --extra-plugin-dir and --load-plugin switches
            //settings.CefCommandLineArgs.Add("enable-system-flash"); //Automatically discovered and load a system-wide installation of Pepper Flash.
            //settings.CefCommandLineArgs.Add("allow-running-insecure-content"); //By default, an https page cannot run JavaScript, CSS or plugins from http URLs. This provides an override to get the old insecure behavior. Only available in 47 and above.

            //settings.CefCommandLineArgs.Add("enable-logging"); //Enable Logging for the Renderer process (will open with a cmd prompt and output debug messages - use in conjunction with setting LogSeverity = LogSeverity.Verbose;)
            //settings.LogSeverity = LogSeverity.Verbose; // Needed for enable-logging to output messages

            //settings.CefCommandLineArgs.Add("disable-extensions"); //Extension support can be disabled
            //settings.CefCommandLineArgs.Add("disable-pdf-extension"); //The PDF extension specifically can be disabled

            //Load the pepper flash player that comes with Google Chrome - may be possible to load these values from the registry and query the dll for it's version info (Step 2 not strictly required it seems)
            //settings.CefCommandLineArgs.Add("ppapi-flash-path", @"C:\Program Files (x86)\Google\Chrome\Application\47.0.2526.106\PepperFlash\pepflashplayer.dll"); //Load a specific pepper flash version (Step 1 of 2)
            //settings.CefCommandLineArgs.Add("ppapi-flash-version", "20.0.0.228"); //Load a specific pepper flash version (Step 2 of 2)

            //Audo play example
            //settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";

            //NOTE: For OSR best performance you should run with GPU disabled:
            // `--disable-gpu --disable-gpu-compositing --enable-begin-frame-scheduling`
            // (you'll loose WebGL support but gain increased FPS and reduced CPU usage).
            // http://magpcss.org/ceforum/viewtopic.php?f=6&t=13271#p27075
            //https://bitbucket.org/chromiumembedded/cef/commits/e3c1d8632eb43c1c2793d71639f3f5695696a5e8

            //NOTE: The following function will set all three params
            //settings.SetOffScreenRenderingBestPerformanceArgs();
            //settings.CefCommandLineArgs.Add("disable-gpu");
            //settings.CefCommandLineArgs.Add("disable-gpu-compositing");
            //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling");

            //settings.CefCommandLineArgs.Add("disable-gpu-vsync"); //Disable Vsync

            // The following options control accessibility state for all frames.
            // These options only take effect if accessibility state is not set by IBrowserHost.SetAccessibilityState call.
            // --force-renderer-accessibility enables browser accessibility.
            // --disable-renderer-accessibility completely disables browser accessibility.
            //settings.CefCommandLineArgs.Add("force-renderer-accessibility");
            //settings.CefCommandLineArgs.Add("disable-renderer-accessibility");

            //Enables Uncaught exception handler
            settings.UncaughtExceptionStackSize = 10;

            // Off Screen rendering (WPF/Offscreen)
            if (settings.WindowlessRenderingEnabled)
            {
                //Disable Direct Composition to test https://github.com/cefsharp/CefSharp/issues/1634
                //settings.CefCommandLineArgs.Add("disable-direct-composition");

                // DevTools doesn't seem to be working when this is enabled
                // http://magpcss.org/ceforum/viewtopic.php?f=6&t=14095
                //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling");
            }

            var proxy = ProxyConfig.GetProxyInformation();
            switch (proxy.AccessType)
            {
                case InternetOpenType.Direct:
                {
                    //Don't use a proxy server, always make direct connections.
                    settings.CefCommandLineArgs.Add("no-proxy-server");
                    break;
                }
                case InternetOpenType.Proxy:
                {
                    settings.CefCommandLineArgs.Add("proxy-server", proxy.ProxyAddress);
                    break;
                }
                case InternetOpenType.PreConfig:
                {
                    settings.CefCommandLineArgs.Add("proxy-auto-detect");
                    break;
                }
            }

            //settings.LogSeverity = LogSeverity.Verbose;

            if (DebuggingSubProcess)
            {
                var architecture = Environment.Is64BitProcess ? "x64" : "x86";
                settings.BrowserSubprocessPath = "..\\..\\..\\..\\CefSharp.BrowserSubprocess\\bin\\" + architecture + "\\Debug\\CefSharp.BrowserSubprocess.exe";
            }

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory(),
                IsSecure = true, //treated with the same security rules as those applied to "https" URLs
            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "https",
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory(),
                DomainName = "cefsharp.example"
            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeNameTest,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory(),
                IsSecure = true //treated with the same security rules as those applied to "https" URLs
            });

            //You can use the http/https schemes - best to register for a specific domain
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "https",
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory(),
                DomainName = "cefsharp.com",
                IsSecure = true //treated with the same security rules as those applied to "https" URLs
            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(rootFolder: @"..\..\..\..\CefSharp.Example\Resources",
                                                                    schemeName: "localfolder", //Optional param no schemename checking if null
                                                                    hostName: "cefsharp", //Optional param no hostname checking if null
                                                                    defaultPage: "home.html") //Optional param will default to index.html
            });

            settings.RegisterExtension(new V8Extension("cefsharp/example", Resources.extension));

            //This must be set before Cef.Initialized is called
            CefSharpSettings.FocusedNodeChangedEnabled = true;

            //Async Javascript Binding - methods are queued on TaskScheduler.Default.
            //Set this to true to when you have methods that return Task<T>
            //CefSharpSettings.ConcurrentTaskExecution = true;

            //Legacy Binding Behaviour - Same as Javascript Binding in version 57 and below
            //See issue https://github.com/cefsharp/CefSharp/issues/1203 for details
            //CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            //Exit the subprocess if the parent process happens to close
            //This is optional at the moment
            //https://github.com/cefsharp/CefSharp/pull/2375/
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

            //NOTE: Set this before any calls to Cef.Initialize to specify a proxy with username and password
            //One set this cannot be changed at runtime. If you need to change the proxy at runtime (dynamically) then
            //see https://github.com/cefsharp/CefSharp/wiki/General-Usage#proxy-resolution
            //CefSharpSettings.Proxy = new ProxyOptions(ip: "127.0.0.1", port: "8080", username: "cefsharp", password: "123");

            if (!Cef.Initialize(settings, performDependencyCheck: !DebuggingSubProcess, browserProcessHandler: browserProcessHandler))
            {
                throw new Exception("Unable to Initialize Cef");
            }

            Cef.AddCrossOriginWhitelistEntry(BaseUrl, "https", "cefsharp.com", false);
        }

        public static async void RegisterTestResources(IWebBrowser browser)
        {
            var handler = browser.ResourceRequestHandlerFactory as ResourceRequestHandlerFactory;
            if (handler != null)
            {
                const string renderProcessCrashedBody = "<html><body><h1>Render Process Crashed</h1><p>Your seeing this message as the render process has crashed</p></body></html>";
                handler.RegisterHandler(RenderProcessCrashedUrl, ResourceHandler.GetByteArray(renderProcessCrashedBody, Encoding.UTF8));

                const string responseBody = "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>";
                handler.RegisterHandler(TestResourceUrl, ResourceHandler.GetByteArray(responseBody, Encoding.UTF8));

                const string unicodeResponseBody = "<html><body>整体满意度</body></html>";
                handler.RegisterHandler(TestUnicodeResourceUrl, ResourceHandler.GetByteArray(unicodeResponseBody, Encoding.UTF8));

                if (string.IsNullOrEmpty(PluginInformation))
                {
                    var pluginBody = new StringBuilder();
                    pluginBody.Append("<html><body><h1>Plugins</h1><table>");
                    pluginBody.Append("<tr>");
                    pluginBody.Append("<th>Name</th>");
                    pluginBody.Append("<th>Description</th>");
                    pluginBody.Append("<th>Version</th>");
                    pluginBody.Append("<th>Path</th>");
                    pluginBody.Append("</tr>");

                    var plugins = await Cef.GetPlugins();

                    if (plugins.Count == 0)
                    {
                        pluginBody.Append("<tr>");
                        pluginBody.Append("<td colspan='4'>Cef.GetPlugins returned an empty list - likely no plugins were loaded on your system</td>");
                        pluginBody.Append("</tr>");
                        pluginBody.Append("<tr>");
                        pluginBody.Append("<td colspan='4'>You may find that NPAPI/PPAPI need to be enabled</td>");
                        pluginBody.Append("</tr>");
                    }
                    else
                    {
                        foreach (var plugin in plugins)
                        {
                            pluginBody.Append("<tr>");
                            pluginBody.Append("<td>" + plugin.Name + "</td>");
                            pluginBody.Append("<td>" + plugin.Description + "</td>");
                            pluginBody.Append("<td>" + plugin.Version + "</td>");
                            pluginBody.Append("<td>" + plugin.Path + "</td>");
                            pluginBody.Append("</tr>");
                        }
                    }

                    pluginBody.Append("</table></body></html>");

                    PluginInformation = pluginBody.ToString();
                }

                handler.RegisterHandler(PluginsTestUrl, ResourceHandler.GetByteArray(PluginInformation, Encoding.UTF8));
            }
        }
    }
}
