// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Wpf.Rendering;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Event arguments to the Rendering event handler set up in IWebBrowser.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RenderingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingEventArgs"/> class.
        /// </summary>
        /// <param name="bitmapInfo">The bitmap information.</param>
        public RenderingEventArgs(WpfBitmapInfo bitmapInfo)
        {
            BitmapInfo = bitmapInfo;
        }

        /// <summary>
        /// The bitmap info being rendered.
        /// </summary>
        /// <value>The bitmap information.</value>
        public WpfBitmapInfo BitmapInfo { get; private set; }
    }
}
