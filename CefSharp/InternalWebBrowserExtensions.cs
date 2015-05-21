// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp
{
    internal static class InternalWebBrowserExtensions
    {
        internal static void SetHandlersToNull(this IWebBrowserInternal browser)
        {
            browser.ResourceHandlerFactory = null;
            browser.JsDialogHandler = null;
            browser.DialogHandler = null;
            browser.DownloadHandler = null;
            browser.KeyboardHandler = null;
            browser.LifeSpanHandler = null;
            browser.MenuHandler = null;
            browser.FocusHandler = null;
            browser.RequestHandler = null;
            browser.DragHandler = null;
            browser.GeolocationHandler = null;
        }
    }
}
