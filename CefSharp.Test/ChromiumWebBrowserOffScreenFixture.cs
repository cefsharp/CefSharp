// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Xunit;

namespace CefSharp.Test
{
    public class ChromiumWebBrowserOffScreenFixture : IAsyncLifetime
    {
        public ChromiumWebBrowser Browser { get; private set; }

        Task IAsyncLifetime.DisposeAsync()
        {
            Browser?.Dispose();

            return Task.CompletedTask;
        }

        Task IAsyncLifetime.InitializeAsync()
        {
            Browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl, useLegacyRenderHandler: false);

            return Browser.WaitForInitialLoadAsync();
        }
    }
}
