// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.using CefSharp.Example;
using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserForm : Form
    {
        private const string DefaultUrlForAddedTabs = "https://www.google.com";

        public BrowserForm()
        {
            InitializeComponent();

            Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            AddTab(CefExample.DefaultUrl);
        }

        private void AddTab(string url)
        {
            var browser = new BrowserTabUserControl
            {
                Dock = DockStyle.Fill,
            };

            var tabPage = new TabPage(url)
            {
                Dock = DockStyle.Fill
            };
            tabPage.SuspendLayout();
            tabPage.Controls.Add(browser);
            browserTabControl.Controls.Add(tabPage);

            //Make newly created tab active
            browserTabControl.SelectedIndex = browserTabControl.Controls.Count - 1;
            tabPage.ResumeLayout(false);
            tabPage.PerformLayout();
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            Cef.Shutdown();
            Close();
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void FindMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.ShowFind();
            }
        }

        private void CopySourceToClipBoardAsyncClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.CopySourceToClipBoardAsync();
            }
        }

        private BrowserTabUserControl GetCurrentTabControl()
        {
            if (browserTabControl.SelectedIndex == -1)
            {
                return null;
            }

            var tabPage = browserTabControl.Controls[browserTabControl.SelectedIndex];
            var control = (BrowserTabUserControl)tabPage.Controls[0];

            return control;
        }

        private void NewTabToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddTab(DefaultUrlForAddedTabs);
        }

        private void CloseTabToolStripMenuItemClick(object sender, EventArgs e)
        {
            if(browserTabControl.Controls.Count == 0)
            {
                return;
            }

            var tabPage = browserTabControl.Controls[browserTabControl.SelectedIndex];

            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Dispose();
            }

            browserTabControl.Controls.Remove(tabPage);
        }
    }
}
