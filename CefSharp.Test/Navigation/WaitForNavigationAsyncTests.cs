using Xunit.Abstractions;
using Xunit;
using System.Threading.Tasks;
using CefSharp.OffScreen;
using CefSharp.Example;
using System;
using System.Threading;

namespace CefSharp.Test.Navigation
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class WaitForNavigationAsyncTests
    {

        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public WaitForNavigationAsyncTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task CanWork()
        {
            const string expected = CefExample.HelloWorldUrl;

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var navigationTask = browser.WaitForNavigationAsync();
                var evaluateTask = browser.EvaluateScriptAsync($"window.location.href = '{expected}';");

                await Task.WhenAll(navigationTask, evaluateTask);

                var navigationResponse = navigationTask.Result;
                var mainFrame = browser.GetMainFrame();

                Assert.True(mainFrame.IsValid);
                Assert.Equal(expected, mainFrame.Url);
                Assert.Equal(200, navigationResponse.HttpStatusCode);

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [Fact]
        public async Task CanWaitForInvalidDomain()
        {
            const string expected = "https://notfound.cefsharp.test";
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                Assert.True(response.Success);

                var navigationTask = browser.WaitForNavigationAsync();
                var evaluateTask = browser.EvaluateScriptAsync($"window.location.href = '{expected}';");

                await Task.WhenAll(navigationTask, evaluateTask);

                var navigationResponse = navigationTask.Result;
                var mainFrame = browser.GetMainFrame();

                Assert.True(mainFrame.IsValid);
                Assert.False(navigationResponse.Success);
                Assert.Contains(expected, mainFrame.Url);
                Assert.Equal(CefErrorCode.NameNotResolved, navigationResponse.ErrorCode);

                output.WriteLine("Url {0}", mainFrame.Url);
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
                    await browser.WaitForNavigationAsync(timeout:TimeSpan.FromMilliseconds(100));
                });

                Assert.Contains(expected, exception.Message);

                output.WriteLine("Exception {0}", exception.Message);
            }
        }

        [Fact]
        public async Task CanCancel()
        {
            const string expected = "A task was canceled.";

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));

                Assert.True(response.Success);

                var exception = await Assert.ThrowsAnyAsync<TaskCanceledException>(async () =>
                {
                    await browser.WaitForNavigationAsync(cancellationToken: cancellationTokenSource.Token);
                });

                Assert.Contains(expected, exception.Message);

                output.WriteLine("Exception {0}", exception.Message);
            }
        }
    }
}
