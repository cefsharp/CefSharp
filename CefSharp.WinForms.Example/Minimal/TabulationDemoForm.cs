// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

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
            userControl.Controls.Add(chromiumWebBrowser);
            grpBrowser.Controls.Add(userControl);
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
    }
}
