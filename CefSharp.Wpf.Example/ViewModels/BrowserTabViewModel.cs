﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Example;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace CefSharp.Wpf.Example.ViewModels
{
    public class BrowserTabViewModel : ViewModelBase
    {
        private string address;
        public string Address
        {
            get { return address; }
            set { Set(ref address, value); }
        }

        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { Set(ref addressEditable, value); }
        }

        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { Set(ref outputMessage, value); }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get { return statusMessage; }
            set { Set(ref statusMessage, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { Set(ref webBrowser, value); }
        }

        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set(ref evaluateJavaScriptResult, value); }
        }

        private bool showSidebar;
        public bool ShowSidebar
        {
            get { return showSidebar; }
            set { Set(ref showSidebar, value); }
        }

        public ICommand GoCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand ExecuteJavaScriptCommand { get; private set; }
        public ICommand EvaluateJavaScriptCommand { get; private set; }
        public ICommand ShowDevToolsCommand { get; private set; }
        public ICommand CloseDevToolsCommand { get; private set; }

        public BrowserTabViewModel(string address)
        {
            Address = address;
            AddressEditable = Address;

            GoCommand = new RelayCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            HomeCommand = new RelayCommand(() => AddressEditable = Address = CefExample.DefaultUrl);
            ExecuteJavaScriptCommand = new RelayCommand<string>(ExecuteJavaScript, s => !String.IsNullOrWhiteSpace(s));
            EvaluateJavaScriptCommand = new RelayCommand<string>(EvaluateJavaScript, s => !String.IsNullOrWhiteSpace(s));
            ShowDevToolsCommand = new RelayCommand(() => webBrowser.ShowDevTools());
            CloseDevToolsCommand = new RelayCommand(() => webBrowser.CloseDevTools());

            PropertyChanged += OnPropertyChanged;

            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            OutputMessage = version;
        }

        private async void EvaluateJavaScript(string s)
        {
            try
            {
                var response = await webBrowser.EvaluateScriptAsync(s);
                if (response.Success && response.Result is IJavascriptCallback)
                {
                    response = await ((IJavascriptCallback)response.Result).ExecuteAsync("This is a callback from EvaluateJavaScript");
                }

                EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
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

        public void LoadCustomRequestExample()
        {
            var frame = WebBrowser.GetMainFrame();

            //Create a new request knowing we'd like to use PostData
            var request = frame.CreateRequest(initializePostData:true);
            request.Method = "POST";
            request.Url = "custom://cefsharp/PostDataTest.html";
            request.PostData.AddData("test=123&data=456");

            frame.LoadRequest(request);
        }
    }
}
