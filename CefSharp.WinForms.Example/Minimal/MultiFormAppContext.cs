// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Minimal
{
    public class MultiFormAppContext : ApplicationContext
    {
        private SimpleBrowserForm form1;
        private SimpleBrowserForm form2;

        public MultiFormAppContext()
        {
            form1 = new SimpleBrowserForm();
            form1.WindowState = FormWindowState.Normal;
            form2 = new SimpleBrowserForm();
            form2.WindowState = FormWindowState.Normal;

            form1.FormClosed += OnFormClosed;
            form2.FormClosed += OnFormClosed;

            MainForm = form1;

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
