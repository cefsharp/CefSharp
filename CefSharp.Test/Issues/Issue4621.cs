using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.OffScreen;
using Xunit;
using Xunit.Abstractions;

namespace CefSharp.Test.Issues
{
    //NOTE: All Test classes must be part of this collection as it manages the Cef Initialize/Shutdown lifecycle
    [Collection(CefSharpFixtureCollection.Key)]
    public class Issue4621
    {
        private readonly ITestOutputHelper output;

        public Issue4621(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact(Skip = "Issue https://github.com/cefsharp/CefSharp/issues/4621")]
        public async Task GoogleSearchToGoogleAccountsBreaksJS()
        {
            using (var browser = new ChromiumWebBrowser("https://www.google.com", useLegacyRenderHandler: false))
            {
                var initialResponse = await browser.WaitForInitialLoadAsync();

                var response = await browser.LoadUrlAsync("https://accounts.google.com/");
                var mainFrame = browser.GetMainFrame();

                Assert.True(response.Success);
                Assert.True(mainFrame.IsValid);
                Assert.Contains("accounts.google", mainFrame.Url);
                Assert.Equal(200, response.HttpStatusCode);

                output.WriteLine("Url {0}", mainFrame.Url);

                var buttonText = await mainFrame.EvaluateScriptAsync<string>("(function() { return document.querySelector(\"button[aria-haspopup='menu']\").innerText; })();");
                Assert.Equal("Create account", buttonText);
            }
        }
    }
}
