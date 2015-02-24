// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Wpf.Rendering;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Event arguments to the Rendering event handler set up in IWebBrowser.
    /// </summary>
    public class RenderingEventArgs : EventArgs
    {
        public RenderingEventArgs(WpfBitmapInfo bitmapInfo)
        {
            BitmapInfo = bitmapInfo;
        }

        /// <summary>
        /// The bitmap info being rendered.
        /// </summary>
        public WpfBitmapInfo BitmapInfo { get; private set; }
    }
}
