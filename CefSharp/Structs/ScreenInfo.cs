// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Structs
{
    public struct ScreenInfo
    {
        public float ScaleFactor { get; private set; }
        
        public ScreenInfo(float scaleFactor) : this()
        {
            ScaleFactor = scaleFactor;
        }
    }
}
