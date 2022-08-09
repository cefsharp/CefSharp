using Xunit.Abstractions;
using Xunit;
using System.Threading.Tasks;
using CefSharp.OffScreen;
using CefSharp.Example;
using CefSharp.SchemeHandler;
using System.IO;

namespace CefSharp.Test.SchemeHandler
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class FolderSchemeHandlerFactoryTests
    {

#if NETCOREAPP
        private static string ExampleResourcesFolderPath = Path.GetFullPath(@"..\..\..\..\..\..\CefSharp.Example\Resources");
#else
        private static string ExampleResourcesFolderPath = Path.GetFullPath(@"..\..\..\..\..\CefSharp.Example\Resources");
#endif
        private readonly ITestOutputHelper output;
        private readonly CefSharpFixture fixture;

        public FolderSchemeHandlerFactoryTests(ITestOutputHelper output, CefSharpFixture fixture)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task CanWork()
        {
            const string expected = "https://folderschemehandlerfactory.test/";

            using (var requestContext = new RequestContext(Cef.GetGlobalRequestContext()))
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, requestContext: requestContext))
            {
                _ = await browser.WaitForInitialLoadAsync();

                requestContext.RegisterSchemeHandlerFactory("https",
                    "folderSchemeHandlerFactory.test",
                    new FolderSchemeHandlerFactory(ExampleResourcesFolderPath, defaultPage: "HelloWorld.html"));

                var response = await browser.LoadUrlAsync(expected);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Equal(expected, mainFrame.Url);
                Assert.Equal(200, response.HttpStatusCode);

                var jsResponse = await browser.EvaluateScriptAsync("document.documentElement.outerHTML");

                Assert.Contains("Hello World", jsResponse.Result.ToString());

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }

        [Fact]
        public async Task CanDeleteFileAfterLoading()
        {
            const string expected = "https://folderschemehandlerfactory.test/";
            const string html = "I'm going to be deleted after use!";

            using (var requestContext = new RequestContext(Cef.GetGlobalRequestContext()))
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, requestContext: requestContext))
            {
                _ = await browser.WaitForInitialLoadAsync();

                var tempFile = Path.Combine(ExampleResourcesFolderPath, "DeleteAfterUse.html");

                if(File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }

                Assert.False(File.Exists(tempFile));

                File.WriteAllText(tempFile, html);

                Assert.True(File.Exists(tempFile));

                requestContext.RegisterSchemeHandlerFactory("https",
                    "folderSchemeHandlerFactory.test",
                    new FolderSchemeHandlerFactory(ExampleResourcesFolderPath, defaultPage: "DeleteAfterUse.html"));

                var response = await browser.LoadUrlAsync(expected);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                Assert.Equal(expected, mainFrame.Url);
                Assert.Equal(200, response.HttpStatusCode);

                var jsResponse = await browser.EvaluateScriptAsync("document.documentElement.outerHTML");

                Assert.Contains(html, jsResponse.Result.ToString());

                //Delete the file
                File.Delete(tempFile);

                Assert.False(File.Exists(tempFile));

                output.WriteLine("Url {0}", mainFrame.Url);
            }
        }
    }
}
