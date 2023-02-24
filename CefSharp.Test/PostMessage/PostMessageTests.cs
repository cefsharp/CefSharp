// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.PostMessage
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class PostMessageTests : BrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public PostMessageTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task ShouldWork()
        {
            AssertInitialLoadComplete();

            var evt = await Assert.RaisesAsync<JavascriptMessageReceivedEventArgs>(
                a => Browser.JavascriptMessageReceived += a,
                a => Browser.JavascriptMessageReceived -= a,
                () => Browser.EvaluateScriptAsync("cefSharp.postMessage('test');"));

            Assert.NotNull(evt);
            Assert.Equal("test", evt.Arguments.Message);
        }

        [Fact]
        public async Task ShouldWorkWithJavascriptCallback()
        {
            const string expected = "Echo";

            AssertInitialLoadComplete();

            var evt = await Assert.RaisesAsync<JavascriptMessageReceivedEventArgs>(
                a => Browser.JavascriptMessageReceived += a,
                a => Browser.JavascriptMessageReceived -= a,
                () => Browser.EvaluateScriptAsync("cefSharp.postMessage({ 'Type': 'Update', Data: { 'Property': 123 }, 'Callback': (p1) => { return p1; } });"));

            Assert.NotNull(evt);

            dynamic msg = evt.Arguments.Message;
            var callback = (IJavascriptCallback)msg.Callback;
            var response = await callback.ExecuteAsync(expected);

            Assert.True(response.Success);
            Assert.Equal(expected, response.Result);
        }

        [Theory]
        [InlineData("Event", "Event1")]
        [InlineData("Event", "Event2")]
        [InlineData("CustomEvent", "Event1")]
        [InlineData("CustomEvent", "Event2")]
        public async Task ShouldWorkForCustomEvent(string jsEventObject, string expected)
        {
            const string Script = @"
                    const postMessageHandler = e => { cefSharp.postMessage(e.type); };
                    window.addEventListener(""Event1"", postMessageHandler, false);
                    window.addEventListener(""Event2"", postMessageHandler, false);";

            string rawHtml = $"<html><head><script>window.dispatchEvent(new {jsEventObject}(\"{expected}\"));</script></head><body><h1>testing</h1></body></html>";

            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            //Make sure to load an initial page so we can then add our script using
            //Page.AddScriptToEvaluateOnNewDocument (via DevTools)

            using (var devToolsClient = Browser.GetDevToolsClient())
            {
                var result = await devToolsClient.Page.AddScriptToEvaluateOnNewDocumentAsync(Script);
                var scriptId = int.Parse(result.Identifier);

                //We must use Page.Enable for the script to be added
                await devToolsClient.Page.EnableAsync();

                Browser.LoadHtml(rawHtml);

                Browser.JavascriptMessageReceived += (o, e) =>
                {
                    tcs.SetResult((string)e.Message);
                };

                var actual = await tcs.Task;

                Assert.True(scriptId > 0);
                Assert.Equal(expected, actual);
            }
        }
    }
}
