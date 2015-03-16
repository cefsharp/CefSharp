// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public class MouseEvent
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CefEventFlags Modifiers { get; set; }
    }
}
