using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        public static async Task<byte[]> CaptureScreenShotAsPng(this IWebBrowser chromiumWebBrowser)
        {
            //if (!browser.HasDocument)
            //{
            //    throw new System.Exception("Page hasn't loaded");
            //}

            var browser = chromiumWebBrowser.GetBrowser();

            if (browser == null || browser.IsDisposed)
            {
                throw new Exception("browser is Null or Disposed");
            }

            //var param = new Dictionary<string, object>
            //{
            //    { "format", "png" },
            //}

            //Make sure to dispose of our observer registration when done
            using (var devToolsClient = browser.GetDevToolsClient())
            {
                const string methodName = "Page.captureScreenshot";

                var result = await devToolsClient.ExecuteDevToolsMethodAsync(methodName);

                dynamic response = JsonConvert.DeserializeObject<dynamic>(result.ResponseAsJsonString);

                //Success
                if (result.Success)
                {
                    return Convert.FromBase64String((string)response.data);
                }

                var code = (string)response.code;
                var message = (string)response.message;

                throw new Exception(code + ":" + message);
            }
        }
    }
}
