// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Nito.AsyncEx;
using Xunit;

namespace CefSharp.Test
{
    public class CefSharpFixture : IAsyncLifetime, IDisposable
    {
        private readonly AsyncContextThread contextThread;

        public CefSharpFixture()
        {
            contextThread = new AsyncContextThread();
        }

        private void CefInitialize()
        {
            if (!Cef.IsInitialized)
            {
                var isDefault = AppDomain.CurrentDomain.IsDefaultAppDomain();
                if (!isDefault)
                {
                    throw new Exception(@"Add <add key=""xunit.appDomain"" value=""denied""/> to your app.config to disable appdomains");
                }

                Cef.EnableWaitForBrowsersToClose();

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

                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
        }

        private void CefShutdown()
        {
            if (Cef.IsInitialized)
            {
                Cef.Shutdown();
            }
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
    }
}
