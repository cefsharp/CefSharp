// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// BitmapFactory.
    /// </summary>
    /// <seealso cref="CefSharp.IBitmapFactory" />
    public class BitmapFactory : IBitmapFactory
    {
        /// <summary>
        /// The bitmap lock
        /// </summary>
        private readonly object bitmapLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFactory"/> class.
        /// </summary>
        /// <param name="lockObject">The lock object.</param>
        public BitmapFactory(object lockObject)
        {
            bitmapLock = lockObject;
        }

        /// <summary>
        /// Create an instance of BitmapInfo based on the params
        /// </summary>
        /// <param name="isPopup">create bitmap info for a popup (typically just a bool flag used internally)</param>
        /// <param name="dpiScale">DPI scale</param>
        /// <returns>newly created BitmapInfo</returns>
        BitmapInfo IBitmapFactory.CreateBitmap(bool isPopup, double dpiScale)
        {
            //The bitmap buffer is 32 BPP
            return new GdiBitmapInfo { IsPopup = isPopup, BitmapLock = bitmapLock };
        }
    }
}
