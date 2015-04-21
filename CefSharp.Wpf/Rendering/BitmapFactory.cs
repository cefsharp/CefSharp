// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Media;
using CefSharp.Internals;

namespace CefSharp.Wpf.Rendering
{
    public class BitmapFactory : IBitmapFactory
    {
        public const int DefaultDpi = 96;

        public BitmapInfo CreateBitmap(bool isPopup, Matrix matrix)
        {
            if (matrix.M11 > 1.0 || matrix.M22 > 1.0)
            {
                return new WritableBitmapInfo(DefaultDpi * matrix.M11, DefaultDpi * matrix.M22)
                {
                    IsPopup = isPopup
                };
            }

            return new InteropBitmapInfo { IsPopup = isPopup };
        }
    }
}
