// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Minimal
{
    public partial class TabulationDemoForm : Form
    {
        private readonly ChromiumWebBrowser chromiumWebBrowser; 

        public TabulationDemoForm()
        {
            InitializeComponent();
            chromiumWebBrowser = new ChromiumWebBrowser(txtURL.Text) { Dock = DockStyle.Fill };
            var userControl = new UserControl { Dock = DockStyle.Fill };
            userControl.Enter += userControl_Enter;
            userControl.Leave += userControl_Leave;
            userControl.Controls.Add(chromiumWebBrowser);
            grpBrowser.Controls.Add(userControl);
        }

        void userControl_Leave(object sender, EventArgs e)
        {
            txtDummy.Text = "UserControl OnLeave";
        }

        void userControl_Enter(object sender, EventArgs e)
        {
            txtDummy.Text = "UserControl OnEnter";
        }

        private void BtnGoClick(object sender, EventArgs e)
        {
            chromiumWebBrowser.Load(txtURL.Text);
        }

        private void TxtUrlLeave(object sender, EventArgs e)
        {
            txtURL.BackColor = Color.White;
        }

        private void TxtUrlEnter(object sender, EventArgs e)
        {
            txtURL.BackColor = Color.Yellow;
        }

        protected override void OnActivated(EventArgs e)
        {
            Kernel32.OutputDebugString(String.Format("Form OnActivated: ACType: {2}, ActiveControl.Handle: {0} BrowserControlHandle: {1}\r\n",
                ActiveControl != null ? ((int)ActiveControl.Handle).ToString() : "null",
                chromiumWebBrowser.Handle,
                ActiveControl != null ? ActiveControl.GetType().FullName : "null"));
            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            Kernel32.OutputDebugString(String.Format("Form OnDeActivated: ACType: {2}, ActiveControl.Handle: {0} BrowserControlHandle: {1}\r\n",
                ActiveControl != null ? ((int)ActiveControl.Handle).ToString() : "null",
                chromiumWebBrowser.Handle,
                ActiveControl != null ? ActiveControl.GetType().FullName : "null"));
            base.OnDeactivate(e);
        }
    }
}
