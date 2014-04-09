using CefSharp.Example;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CefSharp.Wpf.Example.Mvvm;

namespace CefSharp.Wpf.Example.Views.Main
{
    public class BrowserTabViewModel : ViewModelBase
    {
        private string address;
        public string Address
        {
            get { return address; }
            set { ChangeAndNotify(ref address, value, () => Address); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { ChangeAndNotify(ref addressEditable, value, () => AddressEditable); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { ChangeAndNotify(ref outputMessage, value, () => OutputMessage); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { ChangeAndNotify(ref title, value, () => Title); }
        }

        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { ChangeAndNotify(ref webBrowser, value, () => WebBrowser); }
        }

        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { ChangeAndNotify(ref evaluateJavaScriptResult, value, () => EvaluateJavaScriptResult); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { ChangeAndNotify(ref showSidebar, value, () => ShowSidebar); }
        }

        public ICommand GoCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand ExecuteJavaScriptCommand { get; set; }
        public ICommand EvaluateJavaScriptCommand { get; set; }

        public BrowserTabViewModel(string address)
        {
            Address = address;
            AddressEditable = Address;

            GoCommand = new DelegateCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            HomeCommand = new DelegateCommand(() => AddressEditable = Address = ExamplePresenter.DefaultUrl);
            ExecuteJavaScriptCommand = new DelegateCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
            EvaluateJavaScriptCommand = new DelegateCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));

            PropertyChanged += OnPropertyChanged;

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            OutputMessage = version;
        }

        private void EvaluateJavaScript(string s)
        {
            try
            {
                EvaluateJavaScriptResult = webBrowser.EvaluateScript(s) ?? "null";
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while evaluating Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteJavaScript(string s)
        {
            try
            {
                webBrowser.ExecuteScriptAsync(s);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "WebBrowser":
                    if (webBrowser != null)
                    {
                        webBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        webBrowser.LoadError += OnWebBrowserLoadError;

                        // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
                        // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
                        // TODO: focus "too early" in the loading process...
                        webBrowser.LoadCompleted += delegate { Application.Current.Dispatcher.BeginInvoke((Action)(() => webBrowser.Focus())); };
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
        }
    }
}
