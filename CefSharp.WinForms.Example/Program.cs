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
            var proxy = string.Empty;
            var apiUrl = string.Empty;
            var phoneNumber = string.Empty;
            var name = string.Empty;
            var currentCmd = string.Empty;

            if (args != null)
            {
                phoneNumber = args[0];
                apiUrl = args[1];
                name = args[2];
                currentCmd = args[3];

                if (args.Length > 4)
                {
                    proxy = args[4];
                }
            }

            // DEMO: Change to true to self host the BrowserSubprocess.
            // instead of using CefSharp.BrowserSubprocess.exe, your applications exe will be used.
            // In this case CefSharp.WinForms.Example.exe
            const bool selfHostSubProcess = false;

            if (selfHostSubProcess)
            {
                var exitCode = CefSharp.BrowserSubprocess.SelfHost.Main(args);

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
                settings.BrowserSubprocessPath = System.IO.Path.GetFullPath("CefSharp.WinForms.Example.exe");
                settings.ChromeRuntime = true;

                Cef.Initialize(settings);

                Application.EnableVisualStyles();
                var browser = new SimpleBrowserForm();
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

                // DEMO: To integrate CEF into your applications existing message loop 
                // set multiThreadedMessageLoop = false;
                const bool multiThreadedMessageLoop = true;
                // When multiThreadedMessageLoop = true then externalMessagePump must be set to false
                // To enable externalMessagePump set  multiThreadedMessageLoop = false and externalMessagePump = true
                const bool externalMessagePump = false;

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
                        //browserProcessHandler = new WinFormsBrowserProcessHandler(browser.Components);
                    }

                }

                var settings = new CefSettings();
                settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;
                settings.ExternalMessagePump = externalMessagePump;
                settings.ChromeRuntime = true;

                CefExample.Init(phoneNumber, proxy, settings, browserProcessHandler: browserProcessHandler);

                Application.EnableVisualStyles();

                //TEST: There are a number of different Forms for testing purposes.
                var browser = new BrowserForm(multiThreadedMessageLoop);
                browser.ApiUrl = apiUrl;
                browser.PhoneNumber = phoneNumber;
                browser.AccountName = name;
                browser.CurrentCmd = currentCmd;
                browser.Proxy = proxy;
                //var browser = new SimpleBrowserForm();
                //var browser = new TabulationDemoForm();

                //Application.Run(new MultiFormAppContext());
                Application.Run(browser);
            }

            return 0;
        }
    }
}
