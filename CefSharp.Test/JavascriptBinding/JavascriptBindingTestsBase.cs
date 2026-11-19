// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.JavascriptBinding;
using CefSharp.OffScreen;
using Xunit;

namespace CefSharp.Test.JavascriptBinding
{
    /// <summary>
    /// Base class for JavaScript binding tests that need to configure settings before browser creation.
    /// This is necessary because JavascriptBindingSettings are frozen after the browser is created.
    /// </summary>
    public abstract class JavascriptBindingTestsBase : IAsyncLifetime
    {
        public ChromiumWebBrowser Browser { get; private set; }
        public bool RequestContextIsolated { get; protected set; }

        protected void AssertInitialLoadComplete()
        {
            var t = Browser.WaitForInitialLoadAsync();

            Assert.True(t.IsCompleted, "WaitForInitialLoadAsync Task.IsComplete");
            Assert.True(t.Result.Success, "WaitForInitialLoadAsync Result.Success");
        }

        /// <summary>
        /// Override this method to configure browser settings before the browser is created.
        /// This is called before InitializeAsync.
        /// </summary>
        protected virtual void ConfigureBrowserSettings(JavascriptBindingSettings settings)
        {
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

            var browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl, requestContext: requestContext, useLegacyRenderHandler: false);

            // Configure settings before the browser is fully initialized
            ConfigureBrowserSettings(browser.JavascriptObjectRepository.Settings);

            Browser = browser;

            return Browser.WaitForInitialLoadAsync();
        }
    }
}
