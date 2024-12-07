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

        [SkipIfRunOnAppVeyorFact]
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

        [SkipIfRunOnAppVeyorFact]
        public async Task GoogleSearchToGmailBreaksJS()
        {
            using (var browser = new ChromiumWebBrowser("www.google.com", useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                var navResponse = await browser.LoadUrlAsync("https://mail.google.com/mail/&ogbl");
                Assert.True(navResponse.Success);
                Assert.Equal(200, navResponse.HttpStatusCode);

                var mainFrame = browser.GetMainFrame();
                Assert.True(mainFrame.IsValid);
                var text = await mainFrame.EvaluateScriptAsync<string>("(function() { return document.querySelector(\"span\").innerText; })();");
                Assert.Equal("For work", text);
            }
        }

        [SkipIfRunOnAppVeyorFact]
        public async Task NavigateBetweenTwoUrlsBreaksJs()
        {
            const string theUrl = "https://brave.com/";
            const string theUrl2 = "https://brave.com/search/";

            using (var browser = new ChromiumWebBrowser(theUrl, useLegacyRenderHandler: false))
            {
                var response = await browser.WaitForInitialLoadAsync();

                await Task.Delay(1000);

                for (var x = 0; x < 20; x++)
                {
                    await browser.LoadUrlAsync(x % 2 == 0 ? theUrl : theUrl2);

                    var javascriptResponse = await browser.EvaluateScriptAsync($"{x}+1");

                    Assert.True(javascriptResponse.Success, javascriptResponse.Message);

                    Assert.Equal(x + 1, (int)javascriptResponse.Result);
                }
            }
        }
    }
}
