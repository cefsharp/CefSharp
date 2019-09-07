// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Wpf.Rendering.Experimental
{
    /// <summary>
    /// IncreaseBufferInteropRenderHandler - creates/updates an InteropBitmap
    /// Uses a MemoryMappedFile for double buffering, only ever creates a new buffer
    /// when the size increases
    /// </summary>
    /// <seealso cref="CefSharp.Wpf.IRenderHandler" />
    [Obsolete("Use InteropBitmapRenderHandler instead as it's now functionally the same")]
    public class IncreaseBufferInteropRenderHandler : InteropBitmapRenderHandler
    {

    }
}
