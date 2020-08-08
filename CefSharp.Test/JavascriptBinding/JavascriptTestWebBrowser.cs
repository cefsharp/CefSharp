// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.OffScreen;

namespace CefSharp.Test.JavascriptBinding
{
    internal class JavascriptTestWebBrowser : ChromiumWebBrowser
    {
        public JavascriptTestWebBrowser()
            : base()
        {
            RenderProcessMessageHandler = new LoadJavascriptHandler();
            JavascriptMessageReceived += OnJavascriptMessageReceived;
        }

        public event EventHandler<string> PageEvent;

        private void OnJavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e.Message is string)
            {
                PageEvent?.Invoke(this, (string)e.Message);
            }
        }

        private sealed class LoadJavascriptHandler : IRenderProcessMessageHandler
        {
            public void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
            {
                const string Script = @"
                    const postMessageHandler = e => { cefSharp.postMessage(e.type); };
                    window.addEventListener(""Event1"", postMessageHandler, false);
                    window.addEventListener(""Event2"", postMessageHandler, false);";

                frame.ExecuteJavaScriptAsync(Script);
            }

            public void OnContextReleased(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
            {
            }

            public void OnFocusedNodeChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IDomNode node)
            {
            }

            public void OnUncaughtException(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, JavascriptException exception)
            {
            }
        }
    }
}
