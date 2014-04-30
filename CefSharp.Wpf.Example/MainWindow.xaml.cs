// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Data;
using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Example.Views;
using System.Windows;
using System.Windows.Controls;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window
    {
        public FrameworkElement Tab1Content { get; set; }
        public FrameworkElement Tab2Content { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Tab1Content = new BrowserTabView
            {
                DataContext = new BrowserTabViewModel { ShowSidebar = true }
            };

            Tab2Content = CreateNewTab();
        }

        private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1) return;

            var selectedtab = (TabItem)e.AddedItems[0];
            if ((string)selectedtab.Header != "+")
            {
                return;
            }

            var tabItem = CreateTabItem();
            TabControl.Items.Insert(TabControl.Items.Count - 1, tabItem);
        }

        private static TabItem CreateTabItem()
        {
            var tabItem = new TabItem
            {
                Width = 150,
                Height = 20,
                IsSelected = true,
                Content = CreateNewTab()
            };
            tabItem.SetBinding(HeaderedContentControl.HeaderProperty, new Binding("Content.DataContext.Title")
            {
                RelativeSource = RelativeSource.Self
            });
            return tabItem;
        }

        private static BrowserTabView CreateNewTab()
        {
            return new BrowserTabView
            {
                DataContext = new BrowserTabViewModel("http://www.google.com")
            };
        }
    }
}
