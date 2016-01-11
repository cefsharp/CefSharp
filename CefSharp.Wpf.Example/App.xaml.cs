// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;
using CefSharp.Example;

namespace CefSharp.Wpf.Example
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                MessageBox.Show("When running this Example outside of Visual Studio " +
                                "please make sure you compile in `Release` mode.", "Warning");
            }
#endif

            CefExample.Init(true, multiThreadedMessageLoop: true);

            base.OnStartup(e);
        }
    }
}