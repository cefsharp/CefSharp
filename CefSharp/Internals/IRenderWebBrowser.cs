// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public interface IRenderWebBrowser : IWebBrowserInternal
    {
        int BytesPerPixel { get; }

        int Width { get; }
        int Height { get; }

        void InvokeRenderAsync(BitmapInfo bitmapInfo);

        void SetCursor(IntPtr cursor);

        void ClearBitmap(BitmapInfo bitmapInfo);
        void SetBitmap(BitmapInfo bitmapInfo);

        void SetPopupIsOpen(bool show);
        void SetPopupSizeAndPosition(int width, int height, int x, int y);
    };
}
