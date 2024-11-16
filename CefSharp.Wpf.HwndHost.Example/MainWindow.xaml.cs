using System.Windows;
using CefSharp.Wpf.HwndHost.Example.Handlers;

namespace CefSharp.Wpf.HwndHost.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Browser.DisplayHandler = new DisplayHandler();
        }

        private void ShowDevToolsClick(object sender, RoutedEventArgs e)
        {
            if (Browser.IsBrowserInitialized)
            {
                Browser.ShowDevTools();
            }
        }
    }
}
