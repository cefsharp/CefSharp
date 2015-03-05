// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    public interface IRenderWebBrowser : IWebBrowserInternal
    {
        ScreenInfo GetScreenInfo();
        ViewInfo GetViewInfo();

        BitmapInfo CreateBitmapInfo(bool isPopup);
        void InvokeRenderAsync(BitmapInfo bitmapInfo);

        void SetCursor(IntPtr cursor);

        void SetPopupIsOpen(bool show);
        void SetPopupSizeAndPosition(int width, int height, int x, int y);

        void GetScreenPoint(int x, int y, ref int outX, ref int outY);
    };
}
