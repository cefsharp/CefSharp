// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.WinForms.Example
{
    internal class MenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(IWebBrowser browser)
        {
            // Return false if you want to disable the context menu.
            return true;
        }
    }
}