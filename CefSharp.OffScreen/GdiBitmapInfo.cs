// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    public class GdiBitmapInfo : BitmapInfo
    {
        public bool IsCleared { get; set; }

        public override void ClearBitmap()
        {
            IsCleared = true;
        }
    }
}
