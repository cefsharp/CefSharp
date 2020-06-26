// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Experimental
{
    public static class DevToolsExtensions
    {
        private static int LastMessageId = 600000;
        /// <summary>
        /// https://chromedevtools.github.io/devtools-protocol/tot/Page/#method-captureScreenshot
        /// </summary>
        /// <param name="browser">the browser instance (main browser, popup etc)</param>
        /// <returns>image or null</returns>
        public static async Task<byte[]> CaptureScreenShotAsPng(this IBrowser browser)
        {
            //if (!browser.HasDocument)
            //{
            //    throw new System.Exception("Page hasn't loaded");
            //}

            var host = browser.GetHost();

            if (host == null || host.IsDisposed)
            {
                throw new System.Exception("BrowserHost is Null or Disposed");
            }

            //var param = new Dictionary<string, object>
            //{
            //    { "format", "png" },
            //}

            var msgId = Interlocked.Increment(ref LastMessageId);

            var observer = new TaskMethodDevToolsMessageObserver(msgId);

            //Make sure to dispose of our observer registration when done
            //TODO: Create a single observer that maps tasks to Id's
            //Or at least create one for each type, events and method
            using (var observerRegistration = host.AddDevToolsMessageObserver(observer))
            {
                //Page.captureScreenshot defaults to PNG, all params are optional
                //for this DevTools method
                var id = host.ExecuteDevToolsMethod(msgId, "Page.captureScreenshot");

                if (id != msgId)
                {
                    throw new System.Exception("Message Id doesn't match the provided Id");
                }

                var result = await observer.Task;

                var success = result.Item1;
                var response = System.Text.Encoding.UTF8.GetString(result.Item2);

                //Success
                if (success)
                {
                    using (var reader = JsonReaderWriterFactory.CreateJsonReader(result.Item2, 0, result.Item2.Length, Encoding.UTF8, System.Xml.XmlDictionaryReaderQuotas.Max, null))
                    {

                        var xml = reader.ReadInnerXml();

                    }


                }
            }

            return null;
        }
    }
}
