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
