// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.JavascriptBinding
{
    /// <summary>
    /// This is more of a set of integration tests than it is unit tests, for now we need to
    /// run our QUnit tests in an automated fashion and some other testing.
    /// Long term we should split this out into full integration tests.
    /// </summary>
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class JavascriptBindingFacts
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public JavascriptBindingFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task RunJavascriptBindingQunitTests()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.BindingTestUrl))
            {
                var success = await browser.WaitForQUnitTestExeuctionToComplete();

                Assert.True(success);

                output.WriteLine("QUnit Tests result: {0}", success);
            }
        }

        [Fact]
        public async Task CanModifyJavascriptBindingWindowObjectName()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, automaticallyCreateBrowser: false))
            {
                var settings = browser.JavascriptObjectRepository.Settings;
                settings.JsBindingGlobalObjectName = "customApi";

                //To modify the settings we need to defer browser creation slightly
                browser.CreateBrowser();

                await browser.LoadPageAsync();

                var result = await browser.EvaluateScriptAsync("customApi.IsObjectCached('doesntexist') === false");

                Assert.True(result.Success);
            }
        }
    }
}
