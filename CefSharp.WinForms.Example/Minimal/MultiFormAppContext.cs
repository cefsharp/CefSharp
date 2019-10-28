// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Forms;
using CefSharp.WinForms.Example.Handlers;

namespace CefSharp.WinForms.Example.Minimal
{
    public class MultiFormAppContext : ApplicationContext
    {
        private SimpleBrowserForm form1;
        private SimpleBrowserForm form2;

        public MultiFormAppContext(bool multiThreadedMessageLoop)
        {
            form1 = new SimpleBrowserForm(multiThreadedMessageLoop, new MultiFormFocusHandler());
            form1.WindowState = FormWindowState.Normal;
            form2 = new SimpleBrowserForm(multiThreadedMessageLoop, new MultiFormFocusHandler());
            form2.WindowState = FormWindowState.Normal;

            form1.FormClosed += OnFormClosed;
            form2.FormClosed += OnFormClosed;

            form1.Show();
            form2.Show();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms.Count == 0)
            {
                ExitThread();
            }
        }
    }
}
