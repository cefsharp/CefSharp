// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Minimal
{
    public partial class TabulationDemoForm : Form
    {
        private readonly ChromiumWebBrowser chromiumWebBrowser;
        private readonly Color focusColor = Color.Yellow;
        private readonly Color nonFocusColor = Color.White;

        public TabulationDemoForm()
        {
            InitializeComponent();
            chromiumWebBrowser = new ChromiumWebBrowser(txtURL.Text) { Dock = DockStyle.Fill };
            var userControl = new UserControl { Dock = DockStyle.Fill };
            userControl.Enter += UserControlEnter;
            userControl.Leave += UserControlLeave;
            userControl.Controls.Add(chromiumWebBrowser);
            txtURL.GotFocus += TxtUrlGotFocus;
            txtURL.LostFocus += TxtUrlLostFocus;
            grpBrowser.Controls.Add(userControl);
        }

        public IContainer Components
        {
            get
            {
                if (components == null)
                {
                    components = new Container();
                }

                return components;
            }
        }

        private void TxtUrlLostFocus(object sender, EventArgs e)
        {
            // Uncomment this if you want the address bar to go white
            // during deactivation:
            //UpdateUrlColor(nonFocusColor);
        }

        private void TxtUrlGotFocus(object sender, EventArgs e)
        {
            // Ensure the control turns yellow on form
            // activation (since Enter events don't fire then)
            UpdateUrlColor(focusColor);
        }

        private void UpdateUrlColor(Color color)
        {
            if (txtURL.BackColor != color)
            {
                txtURL.BackColor = color;
            }
        }

        private void UserControlLeave(object sender, EventArgs e)
        {
            txtDummy.Text = "UserControl OnLeave";
        }

        private void UserControlEnter(object sender, EventArgs e)
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
        }

        private void TxtUrlEnter(object sender, EventArgs e)
        {
            UpdateUrlColor(focusColor);
        }
    }
}
