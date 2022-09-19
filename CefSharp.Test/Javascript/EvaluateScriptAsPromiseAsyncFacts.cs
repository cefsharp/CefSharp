// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Javascript
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class EvaluateScriptAsPromiseAsyncFacts : IClassFixture<ChromiumWebBrowserOffScreenFixture>
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;
        private readonly ChromiumWebBrowserOffScreenFixture classFixture;

        public EvaluateScriptAsPromiseAsyncFacts(ITestOutputHelper output, CefSharpFixture collectionFixture, ChromiumWebBrowserOffScreenFixture classFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
            this.classFixture = classFixture;
        }

        [Theory]
        [InlineData("return 42;", true, "42")]
        [InlineData("return new Promise(function(resolve, reject) { resolve(42); });", true, "42")]
        [InlineData("return new Promise(function(resolve, reject) { reject('reject test'); });", false, "reject test")]
        [InlineData("return await 42;", true, "42")]
        [InlineData("return await (function() { throw('reject test'); })();", false, "reject test")]
        [InlineData("var result = await fetch('./robots.txt'); return result.status;", true, "200")]
        public async Task CanEvaluateScriptAsPromiseAsync(string script, bool success, string expected)
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var javascriptResponse = await browser.EvaluateScriptAsPromiseAsync(script);

                Assert.Equal(success, javascriptResponse.Success);

                if (success)
                {
                    Assert.Equal(expected, javascriptResponse.Result.ToString());
                }
                else
                {
                    Assert.Equal(expected, javascriptResponse.Message);
                }
            }
        }

        [Theory]
        [InlineData("return { a: 'CefSharp', b: 42, };", true, "CefSharp", "42")]
        [InlineData("return new Promise(function(resolve, reject) { resolve({ a: 'CefSharp', b: 42, }); });", true, "CefSharp", "42")]
        [InlineData("return new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); });", true, "CefSharp", "42")]
        [InlineData("return await { a: 'CefSharp', b: 42, };", true, "CefSharp", "42")]
        [InlineData("return await new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); }); ", true, "CefSharp", "42")]
        [InlineData("function sleep(ms) { return new Promise(resolve => setTimeout(resolve, ms)); }; async function getValAfterSleep() { await sleep(1000); return { a: 'CefSharp', b: 42 }; }; await sleep(2000); const result = await getValAfterSleep(); await sleep(2000); return result;", true, "CefSharp", "42")]
        public async Task CanEvaluateScriptAsPromiseAsyncReturnObject(string script, bool success, string expectedA, string expectedB)
        {
            using (var browser = new ChromiumWebBrowser("http://www.google.com"))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);

                var javascriptResponse = await browser.EvaluateScriptAsPromiseAsync(script);

                Assert.Equal(success, javascriptResponse.Success);

                if (success)
                {
                    dynamic result = javascriptResponse.Result;
                    Assert.Equal(expectedA, result.a.ToString());
                    Assert.Equal(expectedB, result.b.ToString());
                }
                else
                {
                    throw new System.Exception("Failed");
                }
            }
        }
    }
}
