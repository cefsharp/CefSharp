// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Example.Handlers;
using CefSharp.Example.Properties;
using CefSharp.Example.Proxy;
using CefSharp.Internals;
using CefSharp.SchemeHandler;

namespace CefSharp.Example
{
    public static class CefExample
    {
        public const string DefaultUrl = "custom://cefsharp/home.html";
        public const string BindingTestUrl = "custom://cefsharp/BindingTest.html";
        public const string PluginsTestUrl = "custom://cefsharp/plugins.html";
        public const string PopupTestUrl = "custom://cefsharp/PopupTest.html";
        public const string TooltipTestUrl = "custom://cefsharp/TooltipTest.html";
        public const string BasicSchemeTestUrl = "custom://cefsharp/SchemeTest.html";
        public const string ResponseFilterTestUrl = "custom://cefsharp/ResponseFilterTest.html";
        public const string DraggableRegionTestUrl = "custom://cefsharp/DraggableRegionTest.html";
        public const string TestResourceUrl = "http://test/resource/load";
        public const string RenderProcessCrashedUrl = "http://processcrashed";
        public const string TestUnicodeResourceUrl = "http://test/resource/loadUnicode";
        public const string PopupParentUrl = "http://www.w3schools.com/jsref/tryit.asp?filename=tryjsref_win_close";
        
        // Use when debugging the actual SubProcess, to make breakpoints etc. inside that project work.
        private static readonly bool DebuggingSubProcess = Debugger.IsAttached;
        private static string PluginInformation = "";

        public static void Init(bool osr, bool multiThreadedMessageLoop, IBrowserProcessHandler browserProcessHandler)
        {
            // Set Google API keys, used for Geolocation requests sans GPS.  See http://www.chromium.org/developers/how-tos/api-keys
            // Environment.SetEnvironmentVariable("GOOGLE_API_KEY", "");
            // Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_ID", "");
            // Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_SECRET", "");

            //Chromium Command Line args
            //http://peter.sh/experiments/chromium-command-line-switches/
            //NOTE: Not all relevant in relation to `CefSharp`, use for reference purposes only.

            var settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            //The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
            //HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
            settings.CachePath = "cache";
            //settings.UserAgent = "CefSharp Browser" + Cef.CefSharpVersion; // Example User Agent
            //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
            //settings.CefCommandLineArgs.Add("renderer-startup-dialog", "1");
            //settings.CefCommandLineArgs.Add("enable-media-stream", "1"); //Enable WebRTC
            //settings.CefCommandLineArgs.Add("no-proxy-server", "1"); //Don't use a proxy server, always make direct connections. Overrides any other proxy server flags that are passed.
            //settings.CefCommandLineArgs.Add("debug-plugin-loading", "1"); //Dumps extra logging about plugin loading to the log file.
            //settings.CefCommandLineArgs.Add("disable-plugins-discovery", "1"); //Disable discovering third-party plugins. Effectively loading only ones shipped with the browser plus third-party ones as specified by --extra-plugin-dir and --load-plugin switches
            //settings.CefCommandLineArgs.Add("enable-system-flash", "1"); //Automatically discovered and load a system-wide installation of Pepper Flash.
            //settings.CefCommandLineArgs.Add("allow-running-insecure-content", "1"); //By default, an https page cannot run JavaScript, CSS or plugins from http URLs. This provides an override to get the old insecure behavior. Only available in 47 and above.

            //settings.CefCommandLineArgs.Add("enable-logging", "1"); //Enable Logging for the Renderer process (will open with a cmd prompt and output debug messages - use in conjunction with setting LogSeverity = LogSeverity.Verbose;)
            //settings.LogSeverity = LogSeverity.Verbose; // Needed for enable-logging to output messages

            //settings.CefCommandLineArgs.Add("disable-extensions", "1"); //Extension support can be disabled
            //settings.CefCommandLineArgs.Add("disable-pdf-extension", "1"); //The PDF extension specifically can be disabled

            //Load the pepper flash player that comes with Google Chrome - may be possible to load these values from the registry and query the dll for it's version info (Step 2 not strictly required it seems)
            //settings.CefCommandLineArgs.Add("ppapi-flash-path", @"C:\Program Files (x86)\Google\Chrome\Application\47.0.2526.106\PepperFlash\pepflashplayer.dll"); //Load a specific pepper flash version (Step 1 of 2)
            //settings.CefCommandLineArgs.Add("ppapi-flash-version", "20.0.0.228"); //Load a specific pepper flash version (Step 2 of 2)

            //NOTE: For OSR best performance you should run with GPU disabled:
            // `--disable-gpu --disable-gpu-compositing --enable-begin-frame-scheduling`
            // (you'll loose WebGL support but gain increased FPS and reduced CPU usage).
            // http://magpcss.org/ceforum/viewtopic.php?f=6&t=13271#p27075
            //https://bitbucket.org/chromiumembedded/cef/commits/e3c1d8632eb43c1c2793d71639f3f5695696a5e8

            //NOTE: The following function will set all three params
            //settings.SetOffScreenRenderingBestPerformanceArgs();
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");

            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1"); //Disable Vsync

            //Disables the DirectWrite font rendering system on windows.
            //Possibly useful when experiencing blury fonts.
            //settings.CefCommandLineArgs.Add("disable-direct-write", "1");

            settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;
            settings.ExternalMessagePump = !multiThreadedMessageLoop;

            // Off Screen rendering (WPF/Offscreen)
            if(osr)
            {
                settings.WindowlessRenderingEnabled = true;

                //Disable Direct Composition to test https://github.com/cefsharp/CefSharp/issues/1634
                //settings.CefCommandLineArgs.Add("disable-direct-composition", "1");
                
                // DevTools doesn't seem to be working when this is enabled
                // http://magpcss.org/ceforum/viewtopic.php?f=6&t=14095
                //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");
            }

            var proxy = ProxyConfig.GetProxyInformation();
            switch (proxy.AccessType)
            {
                case InternetOpenType.Direct:
                {
                    //Don't use a proxy server, always make direct connections.
                    settings.CefCommandLineArgs.Add("no-proxy-server", "1");
                    break;
                }
                case InternetOpenType.Proxy:
                {
                    settings.CefCommandLineArgs.Add("proxy-server", proxy.ProxyAddress);
                    break;
                }
                case InternetOpenType.PreConfig:
                {
                    settings.CefCommandLineArgs.Add("proxy-auto-detect", "1");
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
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
                //SchemeHandlerFactory = new InMemorySchemeAndResourceHandlerFactory()
            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeNameTest,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(rootFolder: @"..\..\..\..\CefSharp.Example\Resources",
                                                                    schemeName: "localfolder", //Optional param no schemename checking if null
                                                                    hostName: "cefsharp", //Optional param no hostname checking if null
                                                                    defaultPage: "home.html") //Optional param will default to index.html
            });			

            settings.RegisterExtension(new CefExtension("cefsharp/example", Resources.extension));

            settings.FocusedNodeChangedEnabled = true;

            if (!Cef.Initialize(settings, performDependencyCheck: !DebuggingSubProcess, browserProcessHandler: browserProcessHandler))
            {
                throw new Exception("Unable to Initialize Cef");
            }
        }

        public static async void RegisterTestResources(IWebBrowser browser)
        {
            var handler = browser.ResourceHandlerFactory as DefaultResourceHandlerFactory;
            if (handler != null)
            {
                const string renderProcessCrashedBody = "<html><body><h1>Render Process Crashed</h1><p>Your seeing this message as the render process has crashed</p></body></html>";
                handler.RegisterHandler(RenderProcessCrashedUrl, ResourceHandler.FromString(renderProcessCrashedBody));

                const string responseBody = "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>";
                var response = ResourceHandler.FromString(responseBody);
                response.Headers.Add("HeaderTest1", "HeaderTest1Value");
                handler.RegisterHandler(TestResourceUrl, response);

                const string unicodeResponseBody = "<html><body>整体满意度</body></html>";
                handler.RegisterHandler(TestUnicodeResourceUrl, ResourceHandler.FromString(unicodeResponseBody));

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

                    if(plugins.Count == 0)
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

                handler.RegisterHandler(PluginsTestUrl, ResourceHandler.FromString(PluginInformation));
            }
        }
    }
}
