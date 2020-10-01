// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Example.Handlers
{
    public class RenderProcessMessageHandler : IRenderProcessMessageHandler
    {
        void IRenderProcessMessageHandler.OnFocusedNodeChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IDomNode node)
        {
            var message = node == null ? "lost focus" : node.ToString();

            Console.WriteLine("OnFocusedNodeChanged() - " + message);
        }

        void IRenderProcessMessageHandler.OnContextCreated(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            // called for every created V8Context, check IFrame.IsMain to determine that V8Context is from Main frame
            if (frame.IsMain)
            {
                 //const string script = "document.addEventListener('DOMContentLoaded', function(){ alert('DomLoaded'); });";

                 //frame.ExecuteJavaScriptAsync(script);
            }
        }

        void IRenderProcessMessageHandler.OnContextReleased(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            //The V8Context is about to be released, use this notification to cancel any long running tasks your might have
        }

        void IRenderProcessMessageHandler.OnUncaughtException(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, JavascriptException exception)
        {
            Console.WriteLine("OnUncaughtException() - " + exception.Message);
        }
    }
}
