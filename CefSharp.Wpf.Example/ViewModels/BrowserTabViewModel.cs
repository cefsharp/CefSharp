// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Threading.Tasks;
using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CefSharp.Wpf.Example.ViewModels
{
    public class BrowserTabViewModel : INotifyPropertyChanged
    {
        private string address;
        public string Address
        {
            get { return address; }
            set { PropertyChanged.ChangeAndNotify(ref address, value, () => Address); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { PropertyChanged.ChangeAndNotify(ref addressEditable, value, () => AddressEditable); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { PropertyChanged.ChangeAndNotify(ref outputMessage, value, () => OutputMessage); }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get { return statusMessage; }
            set { PropertyChanged.ChangeAndNotify(ref statusMessage, value, () => StatusMessage); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { PropertyChanged.ChangeAndNotify(ref title, value, () => Title); }
        }

        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { PropertyChanged.ChangeAndNotify(ref webBrowser, value, () => WebBrowser); }
        }

        private object evaluateJavaScriptResult;

        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { PropertyChanged.ChangeAndNotify(ref evaluateJavaScriptResult, value, () => EvaluateJavaScriptResult); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { PropertyChanged.ChangeAndNotify(ref showSidebar, value, () => ShowSidebar); }
        }

        public ICommand GoCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand ExecuteJavaScriptCommand { get; set; }
        public ICommand EvaluateJavaScriptCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BrowserTabViewModel(string address)
        {
            Address = address;
            AddressEditable = Address;

            GoCommand = new DelegateCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            HomeCommand = new DelegateCommand(() => AddressEditable = Address = CefExample.DefaultUrl);
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
                var task = webBrowser.EvaluateScriptAsync(s);

                task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                    }
                },
                TaskScheduler.FromCurrentSynchronizationContext());
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
                    if (WebBrowser != null)
                    {
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        WebBrowser.StatusMessage += OnWebBrowserStatusMessage;
                        WebBrowser.LoadError += OnWebBrowserLoadError;

                        // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
                        // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
                        // TODO: focus "too early" in the loading process...
                        WebBrowser.FrameLoadEnd += delegate { Application.Current.Dispatcher.BeginInvoke((Action)(() => webBrowser.Focus())); };
                    }

                    break;
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            StatusMessage = e.Value;
        }

        private void OnWebBrowserLoadError(object sender, LoadErrorEventArgs args)
        {
            // Don't display an error for downloaded files where the user aborted the download.
            if (args.ErrorCode == CefErrorCode.Aborted)
                return;

            var errorMessage = "<html><body><h2>Failed to load URL " + args.FailedUrl +
                  " with error " + args.ErrorText + " (" + args.ErrorCode +
                  ").</h2></body></html>";

            webBrowser.LoadHtml(errorMessage, args.FailedUrl);
        }

        private void Go()
        {
            Address = AddressEditable;

            // Part of the Focus hack further described in the OnPropertyChanged() method...
            Keyboard.ClearFocus();
        }
    }
}
