using CefSharp.Example;
using CefSharp.OffScreen;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.OffScreen
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class WaitForRenderIdleTests
    {

        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public WaitForRenderIdleTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task ShouldWork()
        {
            const int expected = 500;

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler:false))
            {
                var start = DateTime.Now;
                await browser.WaitForRenderIdleAsync();

                var end = DateTime.Now;

                var time = (end - start).TotalMilliseconds;

                Assert.True(end > start);
                Assert.True(time > expected, $"Executed in {time}ms");

                output.WriteLine("Time {0}ms", time);

                Assert.Equal(0, browser.PaintEventHandlerCount());
            }
        }

        [Fact]
        public async Task ShouldWorkForManualInvalidateCalls()
        {
            const int expected = 600;

            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var start = DateTime.Now;

                var invalidateTask = Task.Run(async () =>
                {
                    await Task.Delay(400);

                    browser.GetBrowserHost().Invalidate(PaintElementType.View);

                    await Task.Delay(100);

                    browser.GetBrowserHost().Invalidate(PaintElementType.View);

                    await Task.Delay(100);

                    browser.GetBrowserHost().Invalidate(PaintElementType.View);
                });

                await Task.WhenAll(browser.WaitForRenderIdleAsync(), invalidateTask);

                var end = DateTime.Now;

                var time = (end - start).TotalMilliseconds;

                Assert.True(end > start);
                Assert.True(time > expected, $"Executed in {time}ms");

                Assert.Equal(0, browser.PaintEventHandlerCount());

                output.WriteLine("Time {0}ms", time);
            }
        }

        [Fact]
        public async Task ShouldRespectTimeout()
        {
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                var exception = await Assert.ThrowsAsync<TimeoutException>(async () =>
                {
                    await browser.WaitForRenderIdleAsync(timeout: TimeSpan.FromMilliseconds(100));
                });

                Assert.Equal("The operation has timed out.", exception.Message);

                Assert.Equal(0, browser.PaintEventHandlerCount());
            }
        }

        [Fact]
        public async Task ShouldRespectCancellation()
        {

            using (var cancellationSource = new CancellationTokenSource())
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, useLegacyRenderHandler: false))
            {
                cancellationSource.CancelAfter(400);

                var exception = await Assert.ThrowsAsync<TaskCanceledException>(async () =>
                {
                    await browser.WaitForRenderIdleAsync(cancellationToken:cancellationSource.Token);
                });

                Assert.Equal("A task was canceled.", exception.Message);

                Assert.Equal(0, browser.PaintEventHandlerCount());
            }
        }
    }
}
