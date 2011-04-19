using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace CefSharp.WpfExample
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            BrowserSettings browserSettings = new BrowserSettings();

            if (!CEF.Initialize(settings, browserSettings))
            {
                return;
            }

            var source = PresentationSource.FromVisual(sender as Visual) as HwndSource;
            this.frame.Content = new CefWpfWebBrowser(source, "https://github.com/ataranto/CefSharp");
        }
    }
}
