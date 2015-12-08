// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Example
{
    public class RenderProcessMessageHandler : IRenderProcessMessageHandler
    {
        void IRenderProcessMessageHandler.OnFocusedNodeChanged (IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node)
        {
            if (node != null)
            {
                Console.WriteLine ("OnFocusedNodeChanged() - " + node.ToString ());
            }
            else
            {
                Console.WriteLine ("OnFocusedNodeChanged() - lost focus");
            }
        }
    }
}
