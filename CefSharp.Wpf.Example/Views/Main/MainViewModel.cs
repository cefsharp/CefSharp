using System;
using System.ComponentModel;
using System.Windows;
using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;

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

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Address = ExamplePresenter.DefaultUrl;
            AddressEditable = ExamplePresenter.DefaultUrl;

            GoCommand = new DelegateCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            ViewSourceCommand = new DelegateCommand(ViewSource);

            PropertyChanged += OnPropertyChanged;

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            OutputMessage = version;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "Title":
                    Application.Current.MainWindow.Title = "CefSharp.WPf.Example - " + Title;
                    break;

                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                    }

                    break;
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void Go()
        {
            Address = AddressEditable;
        }

        private void ViewSource()
        {
            webBrowser.ViewSource();
        }
    }
}
