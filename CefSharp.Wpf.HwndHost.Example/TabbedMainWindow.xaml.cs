using System.Windows;
using System.Windows.Controls;

namespace CefSharp.Wpf.HwndHost.Example
{
    /// <summary>
    /// Interaction logic for TabbedMainWindow.xaml
    /// </summary>
    public partial class TabbedMainWindow : Window
    {
        public TabbedMainWindow()
        {
            InitializeComponent();

            NewTab();
            NewTab();
            NewTab();
        }

        private void NewTab()
        {
            var browser = new ChromiumWebBrowser("https://example.com/");
            browser.Address = "https://example.com/";
            Tabs.Items.Add(new TabItem { Header = "Tab", Content = browser });
        }

        private void OnNewTabButtonClick(object sender, RoutedEventArgs e)
        {
            NewTab();
        }

        private void OnCloseTabButtonClick(object sender, RoutedEventArgs e)
        {
            var Tab = (TabItem)Tabs.Items[Tabs.SelectedIndex];
            var browser = (ChromiumWebBrowser)Tab.Content;
            Tab.Content = null;
            browser.Dispose();
            Tabs.Items.Remove(Tab);
        }
    }
}
