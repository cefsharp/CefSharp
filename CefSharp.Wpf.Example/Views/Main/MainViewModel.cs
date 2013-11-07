using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CefSharp.Wpf.Example.Views.Main
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string address;
        private IWpfWebBrowser webBrowser;
        private string addressEditable;
        private string title;
        private string outputMessage;

        public string Address
        {
            get { return address; }
            set { PropertyChanged.ChangeAndNotify(ref address, value, () => Address); }
        }

        public string AddressEditable
        {
            get { return addressEditable; }
            set { PropertyChanged.ChangeAndNotify(ref addressEditable, value, () => AddressEditable); }
        }

        public string OutputMessage
        {
            get { return outputMessage; }
            set { PropertyChanged.ChangeAndNotify(ref outputMessage, value, () => OutputMessage); }
        }

        public string Title
        {
            get { return title; }
            set { PropertyChanged.ChangeAndNotify(ref title, value, () => Title); }
        }

        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { PropertyChanged.ChangeAndNotify(ref webBrowser, value, () => WebBrowser); }
        }

        public DelegateCommand GoCommand { get; set; }
        public DelegateCommand ViewSourceCommand { get; set; }
        public DelegateCommand HomeCommand { get; set; }
        public DelegateCommand<string> ExecuteJavaScriptCommand { get; set; }
        public DelegateCommand<string> EvaluateJavaScriptCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Address = ExamplePresenter.DefaultUrl;
            AddressEditable = ExamplePresenter.DefaultUrl;

            GoCommand = new DelegateCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            ViewSourceCommand = new DelegateCommand(ViewSource);
            HomeCommand = new DelegateCommand(() => AddressEditable = Address = ExamplePresenter.DefaultUrl);
            ExecuteJavaScriptCommand = new DelegateCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
            EvaluateJavaScriptCommand = new DelegateCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));

            PropertyChanged += OnPropertyChanged;

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            OutputMessage = version;
        }

        private void EvaluateJavaScript(string s)
        {
            var result = webBrowser.EvaluateScript(s) ?? "null";
            MessageBox.Show("Result: " + result);
        }

        private void ExecuteJavaScript(string s)
        {
            webBrowser.ExecuteScriptAsync(s);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "Title":
                    Application.Current.MainWindow.Title = "CefSharp.Wpf.Example - " + Title;
                    break;

                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        WebBrowser.LoadError += OnWebBrowserLoadError;

                        // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
                        // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
                        // TODO: focus "too early" in the loading process...
                        WebBrowser.LoadCompleted += delegate { Application.Current.Dispatcher.BeginInvoke((Action) (() => webBrowser.Focus())); };
                    }

                    break;
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserLoadError(string failedUrl, CefErrorCode errorCode, string errorText)
        {
            // Don't display an error for downloaded files where the user aborted the download.
            if (errorCode == CefErrorCode.Aborted)
                return;

            var errorMessage = "<html><body><h2>Failed to load URL " + failedUrl +
                  " with error " + errorText + " (" + errorCode +
                  ").</h2></body></html>";

            webBrowser.LoadHtml(errorMessage, failedUrl);
        }

        private void Go()
        {
            Address = AddressEditable;

            // Part of the Focus hack further described in the OnPropertyChanged() method...
            Keyboard.ClearFocus();

            // Commented out, just want to be able to test EvaluateScript.
            try
            {
                var result = webBrowser.EvaluateScript("10 * 20;", null);
                var result2 = result;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while evaluating Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewSource()
        {
            webBrowser.ViewSource();
        }
    }
}
