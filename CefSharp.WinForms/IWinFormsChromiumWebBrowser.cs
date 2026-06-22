// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.WinForms.Host;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace CefSharp.WinForms
{
    /// <summary>
    /// Winforms Specific Chromium browser implementation, differs from <see cref="IWinFormsWebBrowser"/> in that
    /// this interface is implemented by both <see cref="ChromiumWebBrowser"/> and <see cref="ChromiumHostControl"/>
    /// where <see cref="IWinFormsWebBrowser"/> is only implemented by <see cref="ChromiumWebBrowser"/>
    /// </summary>
    public interface IWinFormsChromiumWebBrowser : IChromiumWebBrowserBase, IWin32Window, IComponent, ISynchronizeInvoke
    {
        /// <summary>
        /// Occurs when the browser title changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread. 
        /// </summary>
        event EventHandler<TitleChangedEventArgs> TitleChanged;
        /// <summary>
        /// Occurs when the browser address changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread. 
        /// </summary>
        event EventHandler<AddressChangedEventArgs> AddressChanged;

        /// <summary>
        /// Event called after the underlying CEF browser instance has been created. 
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        event EventHandler IsBrowserInitializedChanged;
    }
}
