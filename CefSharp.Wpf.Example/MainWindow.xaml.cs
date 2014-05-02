// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.Controls;
using CefSharp.Wpf.Example.Mvvm;
using CefSharp.Wpf.Example.ViewModels;
using CefSharp.Wpf.Example.Views;

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
			CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, ReloadTab));
			CommandBindings.Add(new CommandBinding(CefBrowserRoutedCommands.FocusAddress, FocusAddress));

			Loaded += MainWindowLoaded;
		}

		//TODO: This is a bit of a hack - ideally would like to find a WPF/RoutedEvent method of passing the focus command
		// Previously had the Command binding captured in the BrowserTabView which only works if it has focus - this will work in all cases
		private void FocusAddress(object sender, ExecutedRoutedEventArgs e)
		{
			var view = GetCurrentView();
			view.FocusAddress();
		}

		//TODO: Look at using routed commands, this should probably be handled in the View Directly
		private void ReloadTab(object sender, ExecutedRoutedEventArgs e)
		{
			var view = GetCurrentView();

			var forceReload = e.Parameter != null && string.Compare(e.Parameter.ToString(), "Force", StringComparison.OrdinalIgnoreCase) == 0;

			view.Reload(forceReload);
		}

		private void CloseTab(object sender, ExecutedRoutedEventArgs e)
		{
			if (BrowserTabs.Count > 0)
			{
				//Obtain the original source element for this event
				var originalSource = (FrameworkElement)e.OriginalSource;

				if (originalSource is MainWindow)
				{
					BrowserTabs.RemoveAt(TabControl.SelectedIndex);
				}
				else
				{
					//Remove the matching DataContext from the BrowserTabs collection
					BrowserTabs.Remove((BrowserTabViewModel)originalSource.DataContext);
				}
			}
		}

		private void OpenNewTab(object sender, ExecutedRoutedEventArgs e)
		{
			CreateNewTab();

			TabControl.SelectedIndex = TabControl.Items.Count - 1;
		}

		private void MainWindowLoaded(object sender, RoutedEventArgs e)
		{
			CreateNewTab(ExamplePresenter.DefaultUrl, true);
		}

		private void CreateNewTab(string url = DefaultUrlForAddedTabs, bool showSideBar = false)
		{
			BrowserTabs.Add(new BrowserTabViewModel(url) { ShowSidebar = showSideBar });
		}

		private BrowserTabView GetCurrentView()
		{
			var children = TabControl.FindChildren<BrowserTabView>();

			return children.Count == 0 ? null : children[TabControl.SelectedIndex];
		}
	}
}
