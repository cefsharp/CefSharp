// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.Example;

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

        private void AddTab(string url, int? insertIndex = null)
        {
            var browser = new BrowserTabUserControl(url)
            {
                Dock = DockStyle.Fill,
            };

            var tabPage = new TabPage(url)
            {
                Dock = DockStyle.Fill
            };
            tabPage.SuspendLayout();
            tabPage.Controls.Add(browser);

            int activeTabIndex;

            if (insertIndex == null)
            {
                browserTabControl.TabPages.Add(tabPage);
                activeTabIndex = browserTabControl.Controls.Count - 1;
            }
            else
            {
                browserTabControl.TabPages.Insert(insertIndex.Value, tabPage);
                activeTabIndex = insertIndex.Value;
            }

            //Make newly created tab active
            browserTabControl.SelectedIndex = activeTabIndex;
            
            tabPage.ResumeLayout(true);
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

            if (browserTabControl.Controls.Count == 0)
            {
                Close();
            }
        }

        private void UndoMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.Undo();
            }
        }

        private void RedoMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.Redo();
            }
        }

        private void CutMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.Cut();
            }
        }

        private void CopyMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.Copy();
            }
        }

        private void PasteMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.Paste();
            }
        }

        private void DeleteMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.Delete();
            }
        }

        private void SelectAllMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.SelectAll();
            }
        }

        private void PrintToolStripMenuItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.Browser.Print();
            }
        }

        private void ShowDevToolsItemClick(object sender, EventArgs e)
        {
            var control = GetCurrentTabControl();
            if (control != null)
            {
                control.ShowDevTools();
            }
        }

        private void CloseDevToolsMenuItemClick(object sender, EventArgs e)
        {
			var control = GetCurrentTabControl();
			if (control != null)
			{
				control.CloseDevTools();
			}
        }

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void CloseDevToolsMenuItemClick(object sender, EventArgs e)
        {
            browser.CloseDevTools();
        }
    }
}
