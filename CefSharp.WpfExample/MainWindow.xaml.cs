using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace CefSharp.WpfExample
{
    public partial class MainWindow : Window
    {
        private CefWpfWebBrowser browser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Settings settings = new Settings();

            if (!CEF.Initialize(settings))
            {
                return;
            }

            var source = PresentationSource.FromVisual(sender as Visual) as HwndSource;
            browser = new CefWpfWebBrowser(source, "https://github.com/ataranto/CefSharp");

            browser.PropertyChanged += HandleBrowserPropertyChanged;
            this.frame.Content = browser;
        }

        private void HandleBrowserPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => UpdateBrowserControls(sender, e)));
        }

        private void UpdateBrowserControls(object sender, PropertyChangedEventArgs e)
        {
            var browser = sender as CefWpfWebBrowser;
            if (browser == null)
            {
                return;
            }

            switch (e.PropertyName)
            {
                case "Title":
                    Title = browser.Title;
                    break;
                case "Address":
                    urlTextBox.Text = browser.Address;
                    break;
                case "CanGoBack":
                    backButton.IsEnabled = browser.CanGoBack;
                    break;
                case "CanGoForward":
                    forwardButton.IsEnabled = browser.CanGoForward;
                    break;
                case "IsLoading":
                    goStopButton.Content = browser.IsLoading ? "Stop" : "Go";
                    break;
            }
        }

        private void goStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (browser.IsLoading)
            {
                browser.Stop();
            }
            else
            {
                browser.Load(urlTextBox.Text);
            }    
        }

        private void urlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                browser.Load(urlTextBox.Text);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            browser.Back();
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            browser.Forward();
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void runJsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);

            try
            {
                String result = browser.RunScript(a + "+" + b, "RunJsTest", 1, 5000);

                if (result == (a + b).ToString())
                {
                    MessageBox.Show(string.Format("{0} + {1} = {2}", a, b, result), "Success");
                }
                else
                {
                    MessageBox.Show(string.Format("{0} + {1} != {2}", a, b, result), "Failure");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Failure");
            }
        }

        private void homeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            browser.Load("https://github.com/chillitom/CefSharp");
        }

        private void fireBugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            browser.Load("http://getfirebug.com/firebuglite");
        }
    }
}
