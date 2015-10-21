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

            const bool multiThreadedMessageLoop = true;
            CefExample.Init(false, multiThreadedMessageLoop: multiThreadedMessageLoop);

            if(multiThreadedMessageLoop == false)
            {
                //http://magpcss.org/ceforum/apidocs3/projects/%28default%29/%28_globals%29.html#CefDoMessageLoopWork%28%29
                //Perform a single iteration of CEF message loop processing.
                //This function is used to integrate the CEF message loop into an existing application message loop.
                //Care must be taken to balance performance against excessive CPU usage.
                //This function should only be called on the main application thread and only if CefInitialize() is called with a CefSettings.multi_threaded_message_loop value of false.
                //This function will not block. 

                Application.Idle += (s, e) => Cef.DoMessageLoopWork();
            }

            var browser = new BrowserForm();
            //var browser = new SimpleBrowserForm();
            //var browser = new TabulationDemoForm();
            Application.Run(browser);
        }
    }
}
