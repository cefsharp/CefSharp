// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.ViewModels;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window
    {
        private const string DefaultUrlForAddedTabs = "https://www.google.com";

        public ObservableCollection<BrowserTabViewModel> BrowserTabs { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            BrowserTabs = new ObservableCollection<BrowserTabViewModel>();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, OpenNewTab));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseTab));

            Loaded += MainWindowLoaded;

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            Title += " - " + bitness;
        }

        private void CloseTab(object sender, ExecutedRoutedEventArgs e)
        {
            if (BrowserTabs.Count > 0)
            {
                //Obtain the original source element for this event
                var originalSource = (FrameworkElement)e.OriginalSource;

                BrowserTabViewModel browserViewModel = null;

                if (originalSource is MainWindow)
                {
                    browserViewModel = BrowserTabs[TabControl.SelectedIndex];
                    browserViewModel.RequestPopup -= vm_RequestPopup;
                    BrowserTabs.RemoveAt(TabControl.SelectedIndex);
                }
                else
                {
                    //Remove the matching DataContext from the BrowserTabs collection
                    browserViewModel = (BrowserTabViewModel)originalSource.DataContext;
                    browserViewModel.RequestPopup -= vm_RequestPopup;
                    BrowserTabs.Remove(browserViewModel);
                }

                browserViewModel.WebBrowser.Dispose();
            }
        }

        private void OpenNewTab(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewTab();

            TabControl.SelectedIndex = TabControl.Items.Count - 1;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            CreateNewTab(CefExample.DefaultUrl, true);
        }

        private void CreateNewTab(string url = DefaultUrlForAddedTabs, bool showSideBar = false)
        {
            BrowserTabViewModel vm = new BrowserTabViewModel(url)
            {
                ShowSidebar = showSideBar
            };
            vm.RequestPopup += vm_RequestPopup;
            BrowserTabs.Add(vm);
        }

        void vm_RequestPopup(object sender, CefSharp.Wpf.Example.ViewModels.BrowserTabViewModel.RequestPopupEventArgs e)
        {

            BrowserTabViewModel vm = new BrowserTabViewModel(e.Url);
            vm.RequestPopup += vm_RequestPopup;
            BrowserTabs.Add(vm);
            e.NewVm = vm;
            TabControl.SelectedIndex = TabControl.Items.Count - 1;
        }
    }
}
