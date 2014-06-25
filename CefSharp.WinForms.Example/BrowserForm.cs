// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.using CefSharp.Example;
using System;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserForm : Form
    {
        public BrowserForm()
        {
            InitializeComponent();

            Load += BrowserFormLoad;

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
            tabPage.Controls.Add(browser);
            browserTabControl.Controls.Add(tabPage);
        }

        private void BrowserFormLoad(object sender, EventArgs e)
        {
            
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
            
        }


        private void CopySourceToClipBoardAsyncClick(object sender, EventArgs e)
        {
            //var task = browser.GetSourceAsync();

            //task.ContinueWith(t =>
            //{
            //	if (!t.IsFaulted)
            //	{
            //		Clipboard.SetText(t.Result);
            //		DisplayOutput("HTML Source copied to clipboard");
            //	}
            //},
            //TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
