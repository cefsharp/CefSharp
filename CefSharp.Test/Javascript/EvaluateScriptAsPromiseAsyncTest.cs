// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Javascript
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class EvaluateScriptAsPromiseAsyncTest : BrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture collectionFixture;

        public EvaluateScriptAsPromiseAsyncTest(ITestOutputHelper output, CefSharpFixture collectionFixture)
        {
            this.output = output;
            this.collectionFixture = collectionFixture;
        }

        [Theory]
        [InlineData("return 42;", "42")]
        [InlineData("return new Promise(function(resolve, reject) { resolve(42); });", "42")]
        [InlineData("return await 42;", "42")]
        [InlineData("var result = await fetch('./home.html'); return result.status;", "200")]
        public async Task ShouldWork(string script, string expected)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsPromiseAsync(script);

            Assert.True(javascriptResponse.Success);
            Assert.Equal(expected, javascriptResponse.Result.ToString());
        }

        [Theory]
        [InlineData("return new Promise(function(resolve, reject) { reject('reject test'); });", "reject test")]
        [InlineData("return await (function() { throw('reject test'); })();", "reject test")]
        public async Task ShouldFail(string script, string expected)
        {
            AssertInitialLoadComplete();

            var javascriptResponse = await Browser.EvaluateScriptAsPromiseAsync(script);

            Assert.False(javascriptResponse.Success);
            Assert.Equal(expected, javascriptResponse.Message);
        }

        [Theory]
        [InlineData("return { a: 'CefSharp', b: 42, };", "CefSharp", "42")]
        [InlineData("return new Promise(function(resolve, reject) { resolve({ a: 'CefSharp', b: 42, }); });", "CefSharp", "42")]
        [InlineData("return new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); });", "CefSharp", "42")]
        [InlineData("return await { a: 'CefSharp', b: 42, };", "CefSharp", "42")]
        [InlineData("return await new Promise(function(resolve, reject) { setTimeout(resolve.bind(null, { a: 'CefSharp', b: 42, }), 1000); }); ", "CefSharp", "42")]
        [InlineData("function sleep(ms) { return new Promise(resolve => setTimeout(resolve, ms)); }; async function getValAfterSleep() { await sleep(1000); return { a: 'CefSharp', b: 42 }; }; await sleep(2000); const result = await getValAfterSleep(); await sleep(2000); return result;", "CefSharp", "42")]
        public async Task ShouldWorkWithObjects(string script, string expectedA, string expectedB)
        {
            var javascriptResponse = await Browser.EvaluateScriptAsPromiseAsync(script);

            Assert.True(javascriptResponse.Success);

            dynamic result = javascriptResponse.Result;
            Assert.Equal(expectedA, result.a.ToString());
            Assert.Equal(expectedB, result.b.ToString());
        }
    }
}
