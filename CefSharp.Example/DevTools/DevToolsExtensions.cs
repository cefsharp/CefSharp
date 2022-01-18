using System;
using System.Threading.Tasks;

namespace CefSharp.Example.DevTools
{
    public static class DevToolsExtensions
    {
        /// <summary>
        /// Calls Page.captureScreenshot without any optional params
        /// (Results in PNG image of default viewport)
        /// https://chromedevtools.github.io/devtools-protocol/tot/Page/#method-captureScreenshot
        /// </summary>
        /// <param name="browser">the ChromiumWebBrowser</param>
        /// <returns>png encoded image as byte[]</returns>
        public static async Task<byte[]> CaptureScreenShotAsPng(this IChromiumWebBrowserBase chromiumWebBrowser)
        {
            //Make sure to dispose of our observer registration when done
            //If you need to make multiple calls then reuse the devtools client
            //and Dispose when done.
            using (var devToolsClient = chromiumWebBrowser.GetDevToolsClient())
            {
                var result = await devToolsClient.Page.CaptureScreenshotAsync();

                return result.Data;
            }
        }
    }
}
