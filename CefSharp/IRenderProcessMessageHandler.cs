﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
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
        /// OnContextCreated is called in the Render process immediately after a CefV8Context is created.
        /// An IPC message is immediately sent to notify the context has been created
        /// (should be safe to execute javascript). If the page has no javascript then on context will be created
        /// and as a result this method will not be called. Currently only called for the Main frame <see cref="IFrame.IsMain"/>
        /// </summary>
        /// <param name="browserControl">The ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame.</param>
        void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame);

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
