// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Nito.AsyncEx;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CefSharp.Test
{
    public class CefSharpFixture : IAsyncLifetime, IDisposable
    {
        private readonly AsyncContextThread contextThread;
        private ProxyServer proxyServer;
        private readonly IMessageSink diagnosticMessageSink;

        public CefSharpFixture(IMessageSink messageSink)
        {
            contextThread = new AsyncContextThread();
            diagnosticMessageSink = messageSink;
        }

        private void CefInitialize()
        {
            if (Cef.IsInitialized == null)
            {
                var isDefault = AppDomain.CurrentDomain.IsDefaultAppDomain();
                if (!isDefault)
                {
                    throw new Exception(@"Add <add key=""xunit.appDomain"" value=""denied""/> to your app.config to disable appdomains");
                }

                var apiHash = Cef.ApiHash(Cef.ApiVersion);

                if (Cef.ApiHashPlatform != apiHash)
                {
                    throw new Exception($"CEF API Has does not match expected. {apiHash} {Cef.ApiHashPlatform}");
                }                    

                Cef.EnableWaitForBrowsersToClose();
                CefSharp.Internals.BrowserRefCounter.Instance.EnableLogging();

                CefSharpSettings.ShutdownOnExit = false;
                var settings = new CefSettings();

                settings.RegisterScheme(new CefCustomScheme
                {
                    SchemeName = "https",
                    SchemeHandlerFactory = new CefSharpSchemeHandlerFactory(),
                    DomainName = CefExample.ExampleDomain
                });

                //The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
                //HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
                settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests\\Cache");
                settings.RootCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Tests");
                //settings.CefCommandLineArgs.Add("renderer-startup-dialog");
                //settings.CefCommandLineArgs.Add("disable-site-isolation-trials");
                settings.SetOffScreenRenderingBestPerformanceArgs();
                settings.CefCommandLineArgs.Add("use-gl", "angle");
                settings.CefCommandLineArgs.Add("use-angle", "swiftshader");

                var success = Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

                diagnosticMessageSink.OnMessage(new DiagnosticMessage("Cef Initialized:" + success));
            }

            if (Cef.IsInitialized == false)
            {
                throw new InvalidOperationException("Cef.Initialize failed.");
            }
        }

        private void CefShutdown()
        {
            if (Cef.IsInitialized == true)
            {
                diagnosticMessageSink.OnMessage(new DiagnosticMessage("Before Cef Shutdown"));

                Cef.WaitForBrowsersToClose();

                try
                {
                    Cef.Shutdown();
                }
                catch(Exception ex)
                {
                    diagnosticMessageSink.OnMessage(new DiagnosticMessage("Cef Shutdown Exception:" + ex.ToString()));
                }

                diagnosticMessageSink.OnMessage(new DiagnosticMessage("After Cef Shutdown"));
            }

            StopProxyServer();
        }

        public Task InitializeAsync()
        {
            return contextThread.Factory.StartNew(CefInitialize);
        }

        public Task DisposeAsync()
        {
            return contextThread.Factory.StartNew(CefShutdown);
        }

        public void Dispose()
        {
            contextThread.Dispose();
        }

        public void StartProxyServerIfRequired()
        {
            if (proxyServer == null)
            {
                proxyServer = new ProxyServer(userTrustRootCertificate: false);

                var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Loopback, 8080, false);

                // An explicit endpoint is where the client knows about the existence of a proxy
                // So client sends request in a proxy friendly manner
                proxyServer.AddEndPoint(explicitEndPoint);
                proxyServer.Start();
            }
        }

        public void StopProxyServer()
        {
            proxyServer?.Stop();
            proxyServer = null;
        }
    }
}
