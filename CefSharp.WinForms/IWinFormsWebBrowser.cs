// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.WinForms
{
    /// <summary>
    /// WinForms specific implementation, has events the
    /// <see cref="ChromiumWebBrowser" /> implementation exposes.
    /// </summary>
    /// <seealso cref="IWebBrowser" /> and <seealso cref="IChromiumWebBrowserBase"/>
    public interface IWinFormsWebBrowser : IWebBrowser, IWinFormsChromiumWebBrowser
    {

    }
}
