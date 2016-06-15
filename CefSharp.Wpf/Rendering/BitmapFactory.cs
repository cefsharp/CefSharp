// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.Wpf.Rendering
{
    /// <summary>
    /// BitmapFactory.
    /// </summary>
    /// <seealso cref="CefSharp.IBitmapFactory" />
    public class BitmapFactory : IBitmapFactory
    {
        /// <summary>
        /// The default dpi
        /// </summary>
        public const int DefaultDpi = 96;

        /// <summary>
        /// Create an instance of BitmapInfo based on the params
        /// </summary>
        /// <param name="isPopup">create bitmap info for a popup (typically just a bool flag used internally)</param>
        /// <param name="dpiScale">DPI scale</param>
        /// <returns>newly created BitmapInfo</returns>
        public BitmapInfo CreateBitmap(bool isPopup, double dpiScale)
        {
            if (dpiScale > 1.0)
            {
                return new WritableBitmapInfo(DefaultDpi * dpiScale, DefaultDpi * dpiScale)
                {
                    IsPopup = isPopup
                };
            }

            return new InteropBitmapInfo { IsPopup = isPopup };
        }
    }
}
