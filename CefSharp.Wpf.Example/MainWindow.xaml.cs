using System.Windows;
using CefSharp.Wpf.Example.Views.Main;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window //, IExampleView
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainView = new MainView
            {
                DataContext = new MainViewModel()
            };

            Content.Content = mainView;
        }
    }
}

//        public void SetTitle(string title)
        //        {
        //            Title = title;
        //        }

        //        public void SetAddress(string address)
        //        {
        //            urlTextBox.Text = address;
        //        }

        //        public void SetAddress(Uri uri)
        //        {
        //            urlTextBox.Text = uri.ToString();
        //        }

        //        public void SetCanGoBack(bool can_go_back)
        //        {
        //            backButton.IsEnabled = can_go_back;
        //        }

        //        public void SetCanGoForward(bool can_go_forward)
        //        {
        //            forwardButton.IsEnabled = can_go_forward;
        //        }

        //        public void SetIsLoading(bool is_loading)
        //        {

        //        }

        //        public void ExecuteScript(string script)
        //        {
        //            webView.ExecuteScript(script);
        //        }

        //        public object EvaluateScript(string script)
        //        {
        //            return webView.EvaluateScript(script);
        //        }

        //        public void DisplayOutput(string output)
        //        {
        //            outputLabel.Content = output;
        //        }
        //    }