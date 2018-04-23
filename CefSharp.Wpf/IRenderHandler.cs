// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Controls;
using CefSharp.Structs;

namespace CefSharp.Wpf
{
    /// <summary>
    /// Implement this interface to handle Offscreen Rendering (OSR).
    /// NOTE: Currently only OnPaint is implemented, at some point expand the API to include all
    /// of CefRenderHandler methods http://magpcss.org/ceforum/apidocs3/projects/(default)/CefRenderHandler.html
    /// </summary>
    public interface IRenderHandler : IDisposable
    {
        void OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height, Image image);
    }
}