// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.WinForms
{
    /// <summary>
    /// WinForms specific implementation, has events the
    /// <see cref="ChromiumWebBrowser" /> implementation exposes.
    /// </summary>
    /// </summary>
    /// <seealso cref="CefSharp.IWebBrowser" />
    public interface IWinFormsWebBrowser : IWebBrowser
    {
        /// <summary>
        /// Occurs when the browser title changed.
        /// </summary>
        event EventHandler<TitleChangedEventArgs> TitleChanged;
        /// <summary>
        /// Occurs when the browser address changed.
        /// </summary>
        event EventHandler<AddressChangedEventArgs> AddressChanged;
    }
}
