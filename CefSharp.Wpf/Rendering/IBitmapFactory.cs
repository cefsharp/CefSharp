// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Media;
using CefSharp.Internals;

namespace CefSharp.Wpf.Rendering
{
    public interface IBitmapFactory
    {
        BitmapInfo CreateBitmap(bool isPopup, Matrix matrix);
    }
}
