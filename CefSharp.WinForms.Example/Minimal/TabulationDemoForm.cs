// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Minimal
{
    public partial class TabulationDemoForm : Form
    {
        private readonly ChromiumWebBrowser chromiumWebBrowser;
        private Color focusColor = Color.Yellow;
        private Color nonFocusColor = Color.White;

        public TabulationDemoForm()
        {
            InitializeComponent();
            chromiumWebBrowser = new ChromiumWebBrowser(txtURL.Text) { Dock = DockStyle.Fill };
            var userControl = new UserControl { Dock = DockStyle.Fill };
            userControl.Enter += userControl_Enter;
            userControl.Leave += userControl_Leave;
            userControl.Controls.Add(chromiumWebBrowser);
            txtURL.GotFocus += TxtURLGotFocus;
            txtURL.LostFocus += TxtUrlLostFocus;
            grpBrowser.Controls.Add(userControl);
        }

        private void TxtUrlLostFocus(object sender, EventArgs e)
        {
            // Uncomment this if you want the address bar to go white
            // during deactivation:
            //UpdateUrlColor(nonFocusColor);
            Kernel32.OutputDebugString("URLLostFocus: Deactivating: " + this.chromiumWebBrowser.deactivating.ToString() + " \r\n");
        }

        void TxtURLGotFocus(object sender, EventArgs e)
        {
            // Ensure the control turns yellow on form
            // activation (since Enter events don't fire then)
            UpdateUrlColor(focusColor);
            Kernel32.OutputDebugString("URLGotFocus: Activating: " + this.chromiumWebBrowser.activating.ToString() + " \r\n");
        }

        private void UpdateUrlColor(Color color)
        {
            if (txtURL.BackColor != color)
            {
                txtURL.BackColor = color;
            }
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
            UpdateUrlColor(nonFocusColor);
            Kernel32.OutputDebugString("UrlLeave: Deactivating: " + this.chromiumWebBrowser.deactivating + "\r\n");
        }

        private void TxtUrlEnter(object sender, EventArgs e)
        {
            UpdateUrlColor(focusColor);
            Debug.WriteLine("UrlEnter Activating: " + this.chromiumWebBrowser.activating + "\r\n");
        }

        protected override void OnActivated(EventArgs e)
        {
            Kernel32.OutputDebugString(String.Format("Form OnActivated: ACType: {2}, ActiveControl.Handle: {0} BrowserControlHandle: {1}\r\n",
                ActiveControl != null ? ((int)ActiveControl.Handle).ToString() : "null",
                chromiumWebBrowser.Handle,
                ActiveControl != null ? ActiveControl.GetType().FullName : "null"));
            base.OnActivated(e);
            Kernel32.OutputDebugString("Form OnActivated End\r\n");
        }

        protected override void OnDeactivate(EventArgs e)
        {
            Kernel32.OutputDebugString(String.Format("Form OnDeActivated: ACType: {2}, ActiveControl.Handle: {0} BrowserControlHandle: {1}\r\n",
                ActiveControl != null ? ((int)ActiveControl.Handle).ToString() : "null",
                chromiumWebBrowser.Handle,
                ActiveControl != null ? ActiveControl.GetType().FullName : "null"));
            base.OnDeactivate(e);
            Kernel32.OutputDebugString("Form OnDeActivated End\r\n");
        }
    }
}
