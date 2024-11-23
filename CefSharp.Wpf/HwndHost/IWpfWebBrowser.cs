// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;

namespace CefSharp.Wpf.HwndHost
{
    /// <summary>
    /// WPF specific implementation, has reference to some of the commands
    /// and properties the <see cref="ChromiumWebBrowser" /> exposes.
    /// </summary>
    /// <seealso cref="CefSharp.IWebBrowser" />
    public interface IWpfWebBrowser : IWpfChromiumWebBrowser, IInputElement
    {
        
    }
}
