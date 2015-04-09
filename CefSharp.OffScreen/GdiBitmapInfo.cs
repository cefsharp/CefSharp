// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    public class GdiBitmapInfo : BitmapInfo
    {
        public bool IsCleared { get; private set; }

        public GdiBitmapInfo()
        {
            BytesPerPixel = 4;
        }

        public override bool CreateNewBitmap
        {
            get { return IsCleared; }
        }

        public override void ClearBitmap()
        {
            IsCleared = true;
        }
    }
}
