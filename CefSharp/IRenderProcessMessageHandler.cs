// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Messages sent by the render process can be handled by implementing this
    /// interface.
    /// </summary>
    public interface IRenderProcessMessageHandler
    {
        /// <summary>
        /// Invoked when an element in the UI gains focus (or possibly no
        /// element gains focus; i.e. an element lost focus).
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame object</param>
        /// <param name="node">An object with information about the node (if any) that has focus.</param>
        void OnFocusedNodeChanged(IWebBrowser browserControl, IBrowser browser, IFrame frame, IDomNode node);
    }
}
