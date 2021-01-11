// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//NOTE:Classes in the CefSharp.Core namespace have been hidden from intellisnse so users don't use them directly

using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Create <see cref="IBrowserAdapter"/> instance via <see cref="Create(IWebBrowserInternal, bool)"/>
    /// This is the primary object for bridging the ChromiumWebBrowser implementation and VC++
    /// </summary>
    public static class ManagedCefBrowserAdapter
    {
        /// <summary>
        /// Create a new <see cref="IBrowserAdapter"/> instance which is the main method of interaction between the unmanged
        /// CEF implementation and our ChromiumWebBrowser instances.
        /// </summary>
        /// <param name="webBrowserInternal">reference to the ChromiumWebBrowser instance</param>
        /// <param name="offScreenRendering">true for WPF/OffScreen, false for WinForms and other Hwnd based implementations</param>
        /// <returns>instance of <see cref="IBrowserAdapter"/></returns>
        public static IBrowserAdapter Create(IWebBrowserInternal webBrowserInternal, bool offScreenRendering)
        {
            return new CefSharp.Core.ManagedCefBrowserAdapter(webBrowserInternal, offScreenRendering);
        }
    }
}
