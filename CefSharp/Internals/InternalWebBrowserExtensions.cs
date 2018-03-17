// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    internal static class InternalWebBrowserExtensions
    {
        internal static void SetHandlersToNull(this IWebBrowserInternal browser)
        {
            browser.DialogHandler = null;
            browser.RequestHandler = null;
            browser.DisplayHandler = null;
            browser.LoadHandler = null;
            browser.LifeSpanHandler = null;
            browser.KeyboardHandler = null;
            browser.JsDialogHandler = null;
            browser.DragHandler = null;
            browser.DownloadHandler = null;
            browser.MenuHandler = null;
            browser.FocusHandler = null;
            browser.ResourceHandlerFactory = null;
            browser.RenderProcessMessageHandler = null;
        }
    }
}
