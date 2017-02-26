// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Factory class used to generate a BitmapInfo object for OSR rendering (WPF and OffScreen projects)
    /// Implement this interface if you wish to render the underlying Bitmap to a custom type
    /// e.g. a GDI Bitmap in the WPF Control
    /// </summary>
    public interface IBitmapFactory
    {
        /// <summary>
        /// Create an instance of BitmapInfo based on the params
        /// </summary>
        /// <param name="isPopup">create bitmap info for a popup (typically just a bool flag used internally)</param>
        /// <param name="dpiScale">DPI scale</param>
        /// <returns>newly created BitmapInfo</returns>
        BitmapInfo CreateBitmap(bool isPopup, double dpiScale);
    }
}
