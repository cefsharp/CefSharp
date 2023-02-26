using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit;
using System.IO;
using CefSharp.OffScreen;
using CefSharp.Example;
using System;

namespace CefSharp.Test.PrintToPdf
{
    [Collection(CefSharpFixtureCollection.Key)]
    public class PrintToPdfTests : BrowserTests
    {
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public PrintToPdfTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task ShouldWork()
        {
            AssertInitialLoadComplete();

            var tempFile = Path.Combine(Path.GetTempPath(), "test.pdf");

            if(File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            var success = await Browser.PrintToPdfAsync(tempFile);

            Assert.True(success, $"PDF Generation Failed {tempFile}");
            Assert.True(File.Exists(tempFile), $"PDF File not found {tempFile}");
        }

        [Fact]
        public async Task ShouldFailIfPageNotLoaded()
        {
            var tempFile = Path.Combine(Path.GetTempPath(), "test.pdf");

            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            using (var browser = new ChromiumWebBrowser(CefExample.HelloWorldUrl, automaticallyCreateBrowser: false))
            {
                var exception = await Assert.ThrowsAsync<Exception>(async () => await browser.PrintToPdfAsync(tempFile));

                Assert.Equal(WebBrowserExtensions.BrowserNullExceptionString, exception.Message);
            }
        }
    }
}
