using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using CefSharp.Example;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window, IExampleView
    {
        public event Action<object, string> UrlActivated;
        public event EventHandler BackActivated;
        public event EventHandler ForwardActivated;
        public event EventHandler ExitActivated;

        private WebView web_view;

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
            web_view = new WebView(source, "https://github.com/ataranto/CefSharp");

            this.frame.Content = web_view;
            new ExamplePresenter(web_view, this, invoke => { });
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetTooltip(string tooltip)
        {

        }

        public void SetAddress(string address)
        {
            urlTextBox.Text = address;
        }

        public void SetCanGoBack(bool can_go_back)
        {
            backButton.IsEnabled = can_go_back;
        }

        public void SetCanGoForward(bool can_go_forward)
        {
            forwardButton.IsEnabled = can_go_forward;
        }

        public void SetIsLoading(bool is_loading)
        {

        }

        private void urlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var handler = UrlActivated;
            if (handler)
            {
                handler(this, urlTextBox.Text);
            }
        }

        /*
        private WebView browser;

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.Exit += OnApplicationExit;
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            CEF.Shutdown();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Settings settings = new Settings();

            if (!CEF.Initialize(settings))
            {
                return;
            }

            var source = PresentationSource.FromVisual(sender as Visual) as HwndSource;
            browser = new WebView(source, "https://github.com/ataranto/CefSharp");

            browser.PropertyChanged += HandleBrowserPropertyChanged;
            this.frame.Content = browser;
        }

        private void HandleBrowserPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => UpdateBrowserControls(sender, e)));
        }

        private void UpdateBrowserControls(object sender, PropertyChangedEventArgs e)
        {
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

        private void executeJsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            browser.ExecuteScript("document.body.style.background = 'red'");
        }

        private void evaluateJsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);

            try
            {
                String result = browser.EvaluateScript(a + "+" + b);

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

        private void evaluateArbiraryJsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };

            var text = new TextBox
            {
                Width = 320,
            };
            panel.Children.Add(text);

            var button = new Button
            {
                Content = "Run",
            };
            button.Click += delegate
            {
                var result = browser.EvaluateScript(text.Text);
                Console.WriteLine("Result: {0}", result);
            };
            panel.Children.Add(button);

            var dialog = new Window
            {
                Content = panel,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
            };
            dialog.Show();
        }

        private void homeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            browser.Load("https://github.com/chillitom/CefSharp");
        }

        private void fireBugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            browser.Load("http://getfirebug.com/firebuglite");
        }
         */

    }
}
