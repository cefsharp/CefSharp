// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.WinForms.Example.Minimal;

namespace CefSharp.WinForms.Example
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show("When running this Example outside of Visual Studio" +
                                "please make sure you compile in `Release` mode.", "Warning");
            }
#endif

            CefExample.Init();

            var browser = new BrowserForm();
            //var browser = new SimpleBrowserForm();
            //var browser = new TabulationDemoForm();
            Application.Run(browser);
        }
    }
}
