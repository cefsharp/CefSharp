using System.Windows;

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

            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Browser?.GetBrowserHost()?.SetFocus(true);
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
