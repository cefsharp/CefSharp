// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.Wpf.Rendering
{
    public class BitmapFactory : IBitmapFactory
    {
        public const int DefaultDpi = 96;

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
