// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.IO;

namespace CefSharp
{
    public interface IMouseEvent
    {
        int X { get; set; }
        int Y { get; set; }
        CefEventFlags Modifiers { get; set; }
    }
}