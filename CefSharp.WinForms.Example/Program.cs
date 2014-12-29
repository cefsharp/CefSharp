// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.WinForms.Example.Minimal;

namespace CefSharp.WinForms.Example
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            CefExample.Init();

            var browser = new BrowserForm();
            //var browser = new SimpleBrowserForm();
            Application.Run(browser);
        }
    }
}
