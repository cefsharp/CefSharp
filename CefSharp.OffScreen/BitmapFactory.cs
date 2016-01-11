// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    public class BitmapFactory : IBitmapFactory
    {
        private readonly object bitmapLock;

        public BitmapFactory(object lockObject)
        {
            bitmapLock = lockObject;
        }

        BitmapInfo IBitmapFactory.CreateBitmap(bool isPopup, double dpiScale)
        {
            //The bitmap buffer is 32 BPP
            return new GdiBitmapInfo { IsPopup = isPopup, BitmapLock = bitmapLock };
        }
    }
}
