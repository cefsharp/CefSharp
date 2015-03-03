// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System.Windows;

namespace CefSharp.Wpf.Rendering
{
    public class BitmapFactory : IBitmapFactory
    {
        public BitmapInfo CreateBitmap(bool isPopup)
        {
            var presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);
            var matrix = presentationSource.CompositionTarget.TransformToDevice;

            if (matrix.M11 > 1.0 || matrix.M22 > 1.0)
            {
                return new WritableBitmapInfo(96*matrix.M11, 96*matrix.M22)
                {
                    IsPopup = isPopup
                };
            }

            return new InteropBitmapInfo
            {
                IsPopup = isPopup
            };
        }
    }
}
