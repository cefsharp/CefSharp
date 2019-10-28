// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using CefSharp.WinForms.Example.Handlers;
using CefSharp.WinForms.Example.Minimal;

namespace CefSharp.WinForms.Example
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            const bool simpleSubProcess = false;

            Cef.EnableHighDPISupport();

            //NOTE: Using a simple sub processes uses your existing application executable to spawn instances of the sub process.
            //Features like JSB, EvaluateScriptAsync, custom schemes require the CefSharp.BrowserSubprocess to function
            if (simpleSubProcess)
            {
                var exitCode = Cef.ExecuteProcess();

                if (exitCode >= 0)
                {
                    return exitCode;
                }

#if DEBUG
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    MessageBox.Show("When running this Example outside of Visual Studio " +
                                    "please make sure you compile in `Release` mode.", "Warning");
                }
#endif

                var settings = new CefSettings();
                settings.BrowserSubprocessPath = "CefSharp.WinForms.Example.exe";

                Cef.Initialize(settings);

                var browser = new SimpleBrowserForm(true);
                Application.Run(browser);
            }
            else
            {
#if DEBUG
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    MessageBox.Show("When running this Example outside of Visual Studio " +
                                    "please make sure you compile in `Release` mode.", "Warning");
                }
#endif

                //When multiThreadedMessageLoop = true then externalMessagePump must be set to false
                // To enable externalMessagePump set  multiThreadedMessageLoop = false and externalMessagePump = true
                const bool multiThreadedMessageLoop = true;
                const bool externalMessagePump = false;

                var browser = new BrowserForm(multiThreadedMessageLoop);
                //var browser = new SimpleBrowserForm(multiThreadedMessageLoop);
                //var browser = new TabulationDemoForm();

                IBrowserProcessHandler browserProcessHandler;

                if (multiThreadedMessageLoop)
                {
                    browserProcessHandler = new BrowserProcessHandler();
                }
                else
                {
                    if (externalMessagePump)
                    {
                        //Get the current taskScheduler (must be called after the form is created)
                        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                        browserProcessHandler = new ScheduleMessagePumpBrowserProcessHandler(scheduler);
                    }
                    else
                    {
                        //We'll add out WinForms timer to the components container so it's Diposed
                        browserProcessHandler = new WinFormsBrowserProcessHandler(browser.Components);
                    }

                }

                var settings = new CefSettings();
                settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;
                settings.ExternalMessagePump = externalMessagePump;

                CefExample.Init(settings, browserProcessHandler: browserProcessHandler);

                //Application.Run(new MultiFormAppContext(multiThreadedMessageLoop));
                Application.Run(browser);
            }

            return 0;
        }
    }
}
