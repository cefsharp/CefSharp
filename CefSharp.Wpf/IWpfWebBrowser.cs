// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CefSharp.Enums;
using CefSharp.Internals;

namespace CefSharp.Wpf
{
    /// <summary>
    /// WPF specific implementation, has reference to some of the commands
    /// and properties the <see cref="ChromiumWebBrowser" /> exposes.
    /// </summary>
    /// <seealso cref="CefSharp.IWebBrowser" />
    public interface IWpfWebBrowser : IWpfChromiumWebBrowser, IInputElement
    {
        /// <summary>
        /// Raised every time <see cref="IRenderWebBrowser.OnPaint"/> is called. You can access the underlying buffer, though it's
        /// preferable to either override <see cref="ChromiumWebBrowser.OnPaint"/> or implement your own <see cref="IRenderHandler"/> as there is no outwardly
        /// accessible locking (locking is done within the default <see cref="IRenderHandler"/> implementations).
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI thread
        /// </summary>
        event EventHandler<PaintEventArgs> Paint;

        /// <summary>
        /// Raised every time <see cref="IRenderWebBrowser.OnVirtualKeyboardRequested(IBrowser, TextInputMode)"/> is called. 
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI thread
        /// </summary>
        event EventHandler<VirtualKeyboardRequestedEventArgs> VirtualKeyboardRequested;
    }
}
