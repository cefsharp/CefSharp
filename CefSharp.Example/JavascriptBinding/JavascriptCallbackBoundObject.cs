// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System.Text;

namespace CefSharp.Example.JavascriptBinding
{
    public class JavascriptCallbackBoundObject
    {
        private IJavascriptCallback callback;
        private IWebBrowser webBrowser;

        public JavascriptCallbackBoundObject(IWebBrowser webBrowser)
        {
            this.webBrowser = webBrowser;
        }

        [JavascriptIgnore]
        public void RunCallback()
        {
            if (callback != null && callback.CanExecute)
            {
                callback.ExecuteAsync("Hello from c#").ContinueWith(t =>
                {
                    var javascriptResponse = t.Result;
                    var builder = new StringBuilder();

                    if (javascriptResponse.Success)
                    {
                        builder.AppendLine("Response From Callback: " + javascriptResponse.Result.ToString());
                    }
                    else
                    {
                        var mainFrame = webBrowser.GetMainFrame();

                        builder.AppendLine("Javascript callback failed with " + javascriptResponse.Message);
                        builder.AppendLine("<br/>");
                        builder.AppendLine("Current MainFrame Id:" + mainFrame.Identifier);
                    }

                    webBrowser.LoadHtml(builder.ToString());
                });
            }
            else
            {
                webBrowser.LoadHtml("Callback CanExecute is now false");
            }
        }

        public void SetCallBack(IJavascriptCallback callback)
        {
            this.callback = callback;
        }
    }
}
