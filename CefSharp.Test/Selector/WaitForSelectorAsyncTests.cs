// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Xunit.Abstractions;
using Xunit;
using System.Threading.Tasks;
using CefSharp.OffScreen;
using CefSharp.Example;
using System;
using CefSharp.Web;

namespace CefSharp.Test.Selector
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class WaitForSelectorAsyncTests
    {

        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public WaitForSelectorAsyncTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task CanWork()
        {
            const string elementId = "newElement";

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var selectorTask = browser.WaitForSelectorAsync($"#{elementId}");
                var evaluateTask = browser.EvaluateScriptAsync("const newDiv = document.createElement('div'); newDiv.id = 'newElement'; const newContent = document.createTextNode('Hi there and greetings!'); newDiv.appendChild(newContent);  document.body.append(newDiv);");

                await Task.WhenAll(selectorTask, evaluateTask);

                var selectorResponse = selectorTask.Result;
                var evalauteResponse = evaluateTask.Result;

                Assert.True(selectorResponse.Success);
                Assert.True(evalauteResponse.Success);

                Assert.Equal(elementId, selectorResponse.ElementId);
                Assert.Equal("DIV", selectorResponse.TagName);
            }
        }

        [Fact]
        public async Task CanWorkForDelayedAction()
        {
            const string elementId = "newElement";

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var selectorTask = browser.WaitForSelectorAsync($"#{elementId}");
                var evaluateTask = Task.Run(async () =>
                {
                    await Task.Delay(500);
                    return await browser.EvaluateScriptAsync("const newDiv = document.createElement('div'); newDiv.id = 'newElement'; const newContent = document.createTextNode('Hi there and greetings!'); newDiv.appendChild(newContent);  document.body.append(newDiv);");
                });

                await Task.WhenAll(selectorTask, evaluateTask);

                var selectorResponse = selectorTask.Result;
                var evalauteResponse = evaluateTask.Result;

                Assert.True(selectorResponse.Success);
                Assert.True(evalauteResponse.Success);

                Assert.True(selectorResponse.ElementAdded);
                Assert.Equal(elementId, selectorResponse.ElementId);
                Assert.Equal("DIV", selectorResponse.TagName);
            }
        }

        [Fact]
        public async Task CanWorkForRemoved()
        {
            const string elementId = "content";

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var selectorTask = browser.WaitForSelectorAsync($"#{elementId}", removed:true);
                var evaluateTask = browser.EvaluateScriptAsync($"document.querySelector('#{elementId}').remove();");

                await Task.WhenAll(selectorTask, evaluateTask);

                var selectorResponse = selectorTask.Result;
                var evalauteResponse = evaluateTask.Result;

                Assert.True(selectorResponse.Success);
                Assert.True(evalauteResponse.Success);

                Assert.False(selectorResponse.ElementAdded);

                var removedCheck = await browser.EvaluateScriptAsync($"document.querySelector('#{elementId}') === null");

                Assert.True(removedCheck.Success);
                Assert.True((bool)removedCheck.Result);
            }
        }

        [Fact]
        public async Task ShouldReturnTrueForRemovedNonExistingElement()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var selectorResponse = await browser.WaitForSelectorAsync("non-existing", removed:true );

                Assert.True(selectorResponse.Success);
                Assert.False(selectorResponse.ElementAdded);
            }
        }

        [Fact]
        public async Task CanTimeout()
        {
            const string expected = "The operation has timed out.";

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var exception = await Assert.ThrowsAnyAsync<TimeoutException>(async () =>
                {
                    await browser.WaitForSelectorAsync("#notExist", timeout: TimeSpan.FromMilliseconds(100));
                });

                Assert.Contains(expected, exception.Message);

                output.WriteLine("Exception {0}", exception.Message);
            }
        }

        [Fact]
        public async Task ShouldCancelIfNavigationOccurs()
        {
            const string expected = "A task was canceled.";
            const string url = CefExample.HelloWorldUrl;

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var exception = await Assert.ThrowsAnyAsync<TaskCanceledException>(async () =>
                {
                    var navigationTask = browser.WaitForSelectorAsync("non-existant");
                    var evaluateTask = browser.EvaluateScriptAsync($"setTimeout(() => window.location.href = '{url}', 100);");

                    await Task.WhenAll(navigationTask, evaluateTask);
                });

                Assert.Contains(expected, exception.Message);

                output.WriteLine("Exception {0}", exception.Message);
            }
        }

        [Fact]
        public async Task ShouldRespondToNodeAttributeMutation()
        {
            var html = new HtmlString("<div class='notZombo'></div>");

            using (var browser = new ChromiumWebBrowser(html, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var divFound = false;
                var waitForSelector = browser.WaitForSelectorAsync(".zombo").ContinueWith(_ => divFound = true);

                Assert.False(divFound);

                browser.ExecuteScriptAsync("document.querySelector('div').className = 'zombo'");

                var actual = await waitForSelector;

                Assert.True(actual);
            }
        }
    }
}
