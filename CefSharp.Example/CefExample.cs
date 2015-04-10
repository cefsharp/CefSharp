﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CefSharp.Example
{
    public static class CefExample
    {
        public const string DefaultUrl = "custom://cefsharp/BindingTest.html";
        public const string TestResourceUrl = "http://test/resource/load";
        public const string TestUnicodeResourceUrl = "http://test/resource/loadUnicode";

        // Use when debugging the actual SubProcess, to make breakpoints etc. inside that project work.
        private static readonly bool DebuggingSubProcess = Debugger.IsAttached;

        public static void Init()
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
            //settings.UserAgent = "CefSharp Browser" + Cef.CefSharpVersion; // Example User Agent
            //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
            //settings.CefCommandLineArgs.Add("renderer-startup-dialog", "renderer-startup-dialog");
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            //settings.CefCommandLineArgs.Add("enable-media-stream", "1"); //Enable WebRTC
            //settings.CefCommandLineArgs.Add("no-proxy-server", "1"); //Don't use a proxy server, always make direct connections. Overrides any other proxy server flags that are passed.
            
            //Disables the DirectWrite font rendering system on windows.
            //Possibly useful when experiencing blury fonts.
            //settings.CefCommandLineArgs.Add("disable-direct-write", "1");
            
            settings.LogSeverity = LogSeverity.Verbose;

            if (DebuggingSubProcess)
            {
                var architecture = Environment.Is64BitProcess ? "x64" : "x86";
                settings.BrowserSubprocessPath = "..\\..\\..\\..\\CefSharp.BrowserSubprocess\\bin\\" + architecture + "\\Debug\\CefSharp.BrowserSubprocess.exe";
            }

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });

            Cef.OnContextInitialized = delegate
            {
                Cef.SetCookiePath("cookies", true);
            };

            //Cef will check if all dependencies are present
            //For special case when Checking Windows Xp Dependencies
            //DependencyChecker.IsWindowsXp = true;

            if (!Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true))
            {
                throw new Exception("Unable to Initialize Cef");
            }
        }

        public static async void RegisterTestResources(IWebBrowser browser)
        {
            var handler = browser.ResourceHandlerFactory;
            if (handler != null)
            {
                const string responseBody = "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>";
                var response = ResourceHandler.FromString(responseBody);
                response.Headers.Add("HeaderTest1", "HeaderTest1Value");
                handler.RegisterHandler(TestResourceUrl, response);

                const string unicodeResponseBody = "<html><body>整体满意度</body></html>";
                handler.RegisterHandler(TestUnicodeResourceUrl, ResourceHandler.FromString(unicodeResponseBody));

                var plugins = await Cef.GetPlugins();

                var pluginBody = new StringBuilder();
                pluginBody.Append("<html><body><h1>Plugins</h1><table>");
                pluginBody.Append("<tr>");
                pluginBody.Append("<th>Name</th>");
                pluginBody.Append("<th>Description</th>");
                pluginBody.Append("<th>Version</th>");
                pluginBody.Append("<th>Path</th>");
                pluginBody.Append("</tr>");

                foreach(var plugin in plugins)
                {
                    pluginBody.Append("<tr>");
                    pluginBody.Append("<td>" + plugin.Name + "</td>");
                    pluginBody.Append("<td>" + plugin.Description + "</td>");
                    pluginBody.Append("<td>" + plugin.Version + "</td>");
                    pluginBody.Append("<td>" + plugin.Path + "</td>");
                    pluginBody.Append("</tr>");
                }
                pluginBody.Append("</table></body></html>");


                handler.RegisterHandler("custom://cefsharp/plugins", ResourceHandler.FromString(pluginBody.ToString()));
            }
        }
    }
}
