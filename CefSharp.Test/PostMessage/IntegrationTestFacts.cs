// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.OffScreen;
using CefSharp.Web;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.PostMessage
{
    /// <summary>
    /// This is more a set of integration tests than it is unit tests, for now we need to
    /// run our QUnit tests in an automated fashion and some other testing.
    /// </summary>
    //TODO: Improve Test Naming, we need a naming scheme that fits these cases that's consistent
    //(Ideally we implement consistent naming accross all test classes, though I'm open to a different
    //naming convention as these are more integration tests than unit tests).
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class IntegrationTestFacts
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public IntegrationTestFacts(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Theory]
        [InlineData("Event", "Event1")]
        [InlineData("Event", "Event2")]
        [InlineData("CustomEvent", "Event1")]
        [InlineData("CustomEvent", "Event2")]
        public async Task JavascriptCustomEvent(string jsEventObject, string eventToRaise)
        {
            const string Script = @"
                    const postMessageHandler = e => { cefSharp.postMessage(e.type); };
                    window.addEventListener(""Event1"", postMessageHandler, false);
                    window.addEventListener(""Event2"", postMessageHandler, false);";

            string rawHtml = $"<html><head><script>window.dispatchEvent(new {jsEventObject}(\"{eventToRaise}\"));</script></head><body><h1>testing</h1></body></html>";
            int scriptId = 0;

            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            //Load a dummy page initially so we can then add our script using
            //Page.AddScriptToEvaluateOnNewDocument (via DevTools)
            using (var browser = new ChromiumWebBrowser(new HtmlString("Initial Load")))
            {
                await browser.LoadPageAsync();

                using (var devToolsClient = browser.GetDevToolsClient())
                {
                    var result = await devToolsClient.Page.AddScriptToEvaluateOnNewDocumentAsync(Script);
                    scriptId = int.Parse(result.Identifier);

                    //We must use Page.Enable for the script to be added
                    await devToolsClient.Page.EnableAsync();
                }

                browser.LoadHtml(rawHtml);

                browser.JavascriptMessageReceived += (o, e) =>
                {
                    tcs.SetResult((string)e.Message);
                };

                var responseFromJavascript = await tcs.Task;

                Assert.True(scriptId > 0);
                Assert.Equal(eventToRaise, responseFromJavascript);
            }
        }
    }
}
