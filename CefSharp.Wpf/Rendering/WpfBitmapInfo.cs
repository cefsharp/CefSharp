// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Media.Imaging;
using CefSharp.Internals;

namespace CefSharp.Wpf.Rendering
{
    public abstract class WpfBitmapInfo : BitmapInfo
    {
        public abstract void Invalidate();
        public abstract BitmapSource CreateBitmap();
    }
}
