// Copyright © 2022 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows;

namespace CefSharp.Wpf.Example
{
    /// <summary>
    /// Interaction logic for ChangeParentWindow.xaml
    /// </summary>
    public partial class ChangeParentWindow : Window
    {
        private ChromiumWebBrowser browser;
        private Window newWindow;

        public ChangeParentWindow()
        {
            InitializeComponent();

            StaticBrowser.Child = new ChromiumWebBrowser("http://www.msn.net");
        }

        private void OnAddBrowser(object sender, RoutedEventArgs e)
        {
            if (browser == null)
            {
                browser = new ChromiumWebBrowser("http://www.msn.net");
            }

            if (newWindow == null)
            {
                newWindow = new Window
                {
                    Owner = this,
                    Content = browser,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                newWindow.Show();
            }
        }

        private void OnRemoveBrowser(object sender, RoutedEventArgs e)
        {
            if (newWindow != null)
            {
                newWindow.Content = null;
                newWindow.Close();
                newWindow = null;
            }

            BrowserSite.Child = browser;
        }
    }
}

