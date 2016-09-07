// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using System.Windows;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using CefSharp.Wpf.Example.Handlers;

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

            const bool multiThreadedMessageLoop = true;

            IBrowserProcessHandler browserProcessHandler;

            if (multiThreadedMessageLoop)
            {
                browserProcessHandler = new BrowserProcessHandler();
            }
            else
            {
                browserProcessHandler = new WpfBrowserProcessHandler(Dispatcher);
            }

            CefExample.Init(osr: true, multiThreadedMessageLoop: multiThreadedMessageLoop, browserProcessHandler: browserProcessHandler);

            base.OnStartup(e);
        }
    }
}