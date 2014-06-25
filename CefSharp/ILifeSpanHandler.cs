// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface ILifeSpanHandler
    {
        bool OnBeforePopup(IWebBrowser browser, string url, ref int x, ref int y, ref int width, ref int height);
        void OnBeforeClose(IWebBrowser browser);
    }
}
