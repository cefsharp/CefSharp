using System;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.OffScreen;
using CefSharp.SchemeHandler;
using Xunit;
using Xunit.Abstractions;

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
        public async Task ShouldWork()
        {
            const string expected = "https://folderschemehandlerfactory.test/";

            using (var requestContext = new RequestContext(Cef.GetGlobalRequestContext()))
            using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, requestContext: requestContext, useLegacyRenderHandler: false))
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
        public async Task ShouldPreventPathTraversalAttack()
        {
            const string hostUrl = "https://folderschemehandlerfactory.test/";

            // 1. Setup temporary directory structure
            var tempParent = Path.Combine(Path.GetTempPath(), "CefSharpShouldPreventPathTraversalAttack-" + Guid.NewGuid());
            var root = Path.Combine(tempParent, "www");
            var sibling = Path.Combine(tempParent, "www2");

            try
            {
                Directory.CreateDirectory(root);
                Directory.CreateDirectory(sibling);

                File.WriteAllText(Path.Combine(root, "index.html"), "root-index");
                File.WriteAllText(Path.Combine(sibling, "secret.txt"), "sibling-secret");

                // 2. Initialize the CefSharp context and browser instances
                using (var requestContext = new RequestContext(Cef.GetGlobalRequestContext()))
                using (var browser = new ChromiumWebBrowser(CefExample.DefaultUrl, requestContext: requestContext, useLegacyRenderHandler: false))
                {
                    _ = await browser.WaitForInitialLoadAsync();

                    // Register factory targeting our custom root directory
                    requestContext.RegisterSchemeHandlerFactory(
                        "https",
                        "folderschemehandlerfactory.test",
                        new FolderSchemeHandlerFactory(root, defaultPage: "index.html"));

                    // 3. Attempt to break out of 'www' using an escaped path traversal sequence
                    var traversalUrl = hostUrl + "..%2fwww2/secret.txt";
                    var response = await browser.LoadUrlAsync(traversalUrl);

                    var mainFrame = browser.GetMainFrame();
                    Assert.True(mainFrame.IsValid);

                    // 4. Security Assertions: The factory should sanitize the path and return a 404.
                    // If the code is secure, HttpStatusCode should be 404 (NotFound).
                    Assert.Equal(404, response.HttpStatusCode);

                    // Fetch DOM contents to double-check that the file contents leaked nowhere
                    var jsResponse = await browser.EvaluateScriptAsync("document.documentElement.innerText");
                    var bodyText = jsResponse.Result?.ToString() ?? string.Empty;

                    Assert.DoesNotContain("sibling-secret", bodyText);
                }
            }
            finally
            {
                // 5. Clean up temporary directories and files safely
                if (Directory.Exists(tempParent))
                {
                    Directory.Delete(tempParent, recursive: true);
                }
            }
        }

        [Fact]
        public async Task ShouldAllowFileDeletionAfterLoading()
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
