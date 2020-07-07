// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Internals
{
    internal static class InternalWebBrowserExtensions
    {
        internal static void SetHandlersToNullExceptLifeSpan(this IWebBrowserInternal browser)
        {
            browser.AudioHandler = null;
            browser.DialogHandler = null;
            browser.FindHandler = null;
            browser.RequestHandler = null;
            browser.DisplayHandler = null;
            browser.LoadHandler = null;
            browser.KeyboardHandler = null;
            browser.JsDialogHandler = null;
            browser.DragHandler = null;
            browser.DownloadHandler = null;
            browser.MenuHandler = null;
            browser.FocusHandler = null;
            browser.ResourceRequestHandlerFactory = null;
            browser.RenderProcessMessageHandler = null;
        }
    }
}
