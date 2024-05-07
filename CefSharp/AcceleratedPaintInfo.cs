// Copyright © 2024 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp.Enums;

namespace CefSharp
{
    /// <summary>
    /// AcceleratedPaintInfo
    /// </summary>
    public sealed class AcceleratedPaintInfo
    {
        public IntPtr SharedTextureHandle;
        public ColorType Format;
    }
}
