// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.Wpf.Rendering
{
    public class BitmapFactory : IBitmapFactory
    {
        public BitmapInfo CreateBitmap(bool isPopup)
        {
            //return new WritableBitmapInfo { IsPopup = isPopup };
            return new InteropBitmapInfo { IsPopup = isPopup };
        }
    }
}
