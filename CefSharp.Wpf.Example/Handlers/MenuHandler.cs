// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Wpf.Example.Handlers
{
    public class MenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(IWebBrowser browser, IContextMenuParams parameters)
        {
            Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            return true;
        }
    }
}
