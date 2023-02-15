// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Xunit;

namespace CefSharp.Test
{
    public abstract class BrowserTests : IAsyncLifetime
    {
        public ChromiumWebBrowser Browser { get; private set; }
        public bool RequestContextIsolated { get; protected set; }

        protected void AssertInitialLoadComplete()
        {
            var t = Browser.WaitForInitialLoadAsync();

            Assert.True(t.IsCompleted, "WaitForInitialLoadAsync Task.IsComplete");
            Assert.True(t.Result.Success, "WaitForInitialLoadAsync Result.Success");
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            Browser?.Dispose();

            return Task.CompletedTask;
        }

        Task IAsyncLifetime.InitializeAsync()
        {
            IRequestContext requestContext = null;

            if (RequestContextIsolated)
            {
                requestContext = new RequestContext();
                requestContext.RegisterSchemeHandlerFactory("https", CefExample.ExampleDomain, new CefSharpSchemeHandlerFactory());
            }            

            Browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl, requestContext: requestContext, useLegacyRenderHandler: false);

            return Browser.WaitForInitialLoadAsync();
        }
    }
}
