// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.Controls;
using CefSharp.Wpf.Example.ViewModels;
using Microsoft.Win32;

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

            CommandBindings.Add(new CommandBinding(CefSharpCommands.Exit, Exit));
            CommandBindings.Add(new CommandBinding(CefSharpCommands.OpenTabCommand, OpenTabCommandBinding));
            CommandBindings.Add(new CommandBinding(CefSharpCommands.PrintTabToPdfCommand, PrintToPdfCommandBinding));

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

                BrowserTabViewModel browserViewModel;

                if (originalSource is MainWindow)
                {
                    browserViewModel = BrowserTabs[TabControl.SelectedIndex];
                    BrowserTabs.RemoveAt(TabControl.SelectedIndex);
                }
                else
                {
                    //Remove the matching DataContext from the BrowserTabs collection
                    browserViewModel = (BrowserTabViewModel)originalSource.DataContext;
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
            BrowserTabs.Add(new BrowserTabViewModel(url) { ShowSidebar = showSideBar });
        }

        private void PrintToPdfCommandBinding(object sender, ExecutedRoutedEventArgs e)
        {
            if (BrowserTabs.Count > 0)
            {
                var originalSource = (FrameworkElement)e.OriginalSource;

                BrowserTabViewModel browserViewModel;

                if (originalSource is MainWindow)
                {
                    browserViewModel = BrowserTabs[TabControl.SelectedIndex];
                }
                else
                {
                    browserViewModel = (BrowserTabViewModel)originalSource.DataContext;
                }

                var dialog = new SaveFileDialog
                {
                    DefaultExt = ".pdf",
                    Filter = "Pdf documents (.pdf)|*.pdf"
                };

                if (dialog.ShowDialog() == true)
                {
                    var printToPdf = browserViewModel.WebBrowser.PrintToPdfAsync(dialog.FileName, new CefSharpPdfPrintSettings()
                    {
                        HeaderFooterEnabled = true,
                        MarginType = CefPdfPrintMarginType.Custom,
                        MarginBottom = 10,
                        MarginTop = 0,
                        MarginLeft = 20,
                        MarginRight = 10,
                    });
                    printToPdf.ContinueWith(_ => MessageBox.Show("PDF was saved to " + dialog.FileName), TaskContinuationOptions.OnlyOnRanToCompletion);
                    printToPdf.ContinueWith(t => MessageBox.Show(t.Exception.Message), TaskContinuationOptions.OnlyOnFaulted);
                }
            }
        }

        private void OpenTabCommandBinding(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            string url = "";

            switch (param)
            {
                case "BindingTest":
                {
                    url = CefExample.BindingTestUrl;
                    break;
                }
                case "ListPlugins":
                {
                    url = CefExample.PluginsTestUrl;
                    break;
                }
                case "PopupTest":
                {
                    url = CefExample.PopupParentUrl;
                    break;
                }
                case "FishGl":
                {
                    url = "http://www.fishgl.com/";
                    break;
                }
                case "MsTestDrive":
                {
                    url = "http://dev.modern.ie/testdrive/";
                    break;
                }
                case "DragDemo":
                {
                    url = "http://html5demos.com/drag";
                    break;
                }
                case "PopupTestCustomScheme":
                {
                    url = CefExample.PopupTestUrl;
                    break;
                }
                case "BasicSchemeTest":
                {
                    url = CefExample.BasicSchemeTestUrl;
                    break;
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("Please provide a valid command parameter for binding");
            }

            CreateNewTab(url, true);

            TabControl.SelectedIndex = TabControl.Items.Count - 1;
        }

        private void Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
