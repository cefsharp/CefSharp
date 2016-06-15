// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Media.Imaging;
using CefSharp.Internals;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// WpfBitmapInfo.
    /// </summary>
    /// <seealso cref="CefSharp.Internals.BitmapInfo" />
    public abstract class WpfBitmapInfo : BitmapInfo
    {
        /// <summary>
        /// Invalidates this instance.
        /// </summary>
        public abstract void Invalidate();
        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <returns>BitmapSource.</returns>
        public abstract BitmapSource CreateBitmap();
    }
}
