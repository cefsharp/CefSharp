// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Xunit;

namespace CefSharp.Test.JavascriptBinding
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class JavascriptBindingSimpleTest
    {
        // Keep this class inline as to provide a single file example.
        private class MyBoundObject
        {
            public int EchoMethodCallCount { get; private set; }
            public string Echo(string arg)
            {
                EchoMethodCallCount++;

                return arg;
            }
        }

        [Fact]
        public async Task ShouldWork()
        {
            const string script = @"
                (async function()
                {
                    await CefSharp.BindObjectAsync('bound');
                    return await bound.echo('Welcome to CefSharp!');
                })();";

            using (var browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl))
            {
                var boundObj = new MyBoundObject();

#if NETCOREAPP
                browser.JavascriptObjectRepository.Register("bound", boundObj);
#else
                browser.JavascriptObjectRepository.Register("bound", boundObj, true);
#endif
                await browser.WaitForInitialLoadAsync();

                var result = await browser.EvaluateScriptAsync<string>(script);

                Assert.Equal(1, boundObj.EchoMethodCallCount);
                Assert.Equal("Welcome to CefSharp!", result);
            }
        }
    }
}
