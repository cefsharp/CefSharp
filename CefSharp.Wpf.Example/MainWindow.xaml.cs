using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CefSharp.Example;
using CefSharp.Wpf.Example.Views.Main;

namespace CefSharp.Wpf.Example
{
	public partial class MainWindow : Window
	{
		private const string DefaultUrl = "https://www.google.com.au";

		public ObservableCollection<BrowserTabViewModel> BrowserTabs { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;

			BrowserTabs = new ObservableCollection<BrowserTabViewModel>();

			CommandBindings.Add(new CommandBinding(ApplicationCommands.New, OpenNewTab));
			CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseTab));
			CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, ReloadTab));
			
			Loaded += MainWindowLoaded;
		}

		private void ReloadTab(object sender, ExecutedRoutedEventArgs e)
		{
			//TODO: Look at using routed commands as this isn't really a concern of the ViewModel
			// It's more a presentation issue and should probably be handled by the View directly
			var currentViewModel = BrowserTabs[TabControl.SelectedIndex];
			currentViewModel.WebBrowser.Reload(true);
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

		private void CreateNewTab(string url = DefaultUrl, bool showSideBar = false)
		{
			BrowserTabs.Add(new BrowserTabViewModel(url) { ShowSidebar = showSideBar });
		}
	}
}
