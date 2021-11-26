// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Handlers
{
    public class WinFormsDisplayHandler : CefSharp.WinForms.Handler.DisplayHandler
    {
        private Control parent;
        private Form fullScreenForm;

        protected override void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
        {
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            webBrowser.InvokeOnUiThreadIfRequired(() =>
            {
                if (fullscreen)
                {
                    parent = webBrowser.Parent;

                    parent.Controls.Remove(webBrowser);

                    fullScreenForm = new Form();
                    fullScreenForm.FormBorderStyle = FormBorderStyle.None;
                    fullScreenForm.WindowState = FormWindowState.Maximized;

                    fullScreenForm.Controls.Add(webBrowser);

                    fullScreenForm.ShowDialog(parent.FindForm());
                }
                else
                {
                    fullScreenForm.Controls.Remove(webBrowser);

                    parent.Controls.Add(webBrowser);

                    fullScreenForm.Close();
                    fullScreenForm.Dispose();
                    fullScreenForm = null;
                }
            });

            base.OnFullscreenModeChange(chromiumWebBrowser, browser, fullscreen);
        }
    }
}
