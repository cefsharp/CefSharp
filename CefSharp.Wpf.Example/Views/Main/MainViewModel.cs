using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CefSharp.Wpf.Example.Views.Main
{
    public class MainViewModel : ObjectBase
    {
        public static PropertyChangedEventArgs AddressChangedEventArgs = GetArgs<MainViewModel>( p => p.Address );
        private string address;
        public string Address
        {
            get { return address; }
            set { Set( ref address, value, AddressChangedEventArgs ); }
        }

        public static PropertyChangedEventArgs AddressEditableChangedEventArgs = GetArgs<MainViewModel>( p => p.AddressEditable );
        private string addressEditable;
        public string AddressEditable
        {
            get { return addressEditable; }
            set { Set( ref addressEditable, value, AddressEditableChangedEventArgs ); }
        }

        public static PropertyChangedEventArgs HomeAddressChangedEventArgs = GetArgs<MainViewModel>( p => p.HomeAddress );
        private string homeAddress;
        public string HomeAddress
        {
            get { return homeAddress; }
            set { Set( ref homeAddress, value, HomeAddressChangedEventArgs ); }
        }

        public static PropertyChangedEventArgs OutputMessageChangedEventArgs = GetArgs<MainViewModel>( p => p.OutputMessage );
        private string outputMessage;
        public string OutputMessage
        {
            get { return outputMessage; }
            set { Set( ref outputMessage, value, OutputMessageChangedEventArgs ); }
        }

        public static PropertyChangedEventArgs TitleChangedEventArgs = GetArgs<MainViewModel>( p => p.Title );
        private string title;
        public string Title
        {
            get { return title; }
            set { Set( ref title, value, TitleChangedEventArgs ); }
        }

        public static PropertyChangedEventArgs WebBrowserChangedEventArgs = GetArgs<MainViewModel>( p => p.WebBrowser );
        private IWpfWebBrowser webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { Set( ref webBrowser, value, WebBrowserChangedEventArgs ); }
        }

        public static PropertyChangedEventArgs EvaluateJavaScriptResultChangedEventArgs = GetArgs<MainViewModel>( p => p.EvaluateJavaScriptResult );
        private object evaluateJavaScriptResult;
        public object EvaluateJavaScriptResult
        {
            get { return evaluateJavaScriptResult; }
            set { Set( ref evaluateJavaScriptResult, value, EvaluateJavaScriptResultChangedEventArgs ); }
        }

        public DelegateCommand GoCommand { get; set; }
        public DelegateCommand ViewSourceCommand { get; set; }
        public DelegateCommand<string> ExecuteJavaScriptCommand { get; set; }
        public DelegateCommand<string> EvaluateJavaScriptCommand { get; set; }

        public MainViewModel()
        {
            HomeAddress = Address = AddressEditable = ExamplePresenter.DefaultUrl;

            GoCommand = new DelegateCommand( Go, () => !String.IsNullOrWhiteSpace( Address ) );
            ViewSourceCommand = new DelegateCommand( ViewSource );
            ExecuteJavaScriptCommand = new DelegateCommand<string>( ExecuteJavaScript, s => !String.IsNullOrWhiteSpace( s ) );
            EvaluateJavaScriptCommand = new DelegateCommand<string>( EvaluateJavaScript, s => !String.IsNullOrWhiteSpace( s ) );

            var version = String.Format( "Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion );
            OutputMessage = version;
        }

        private void EvaluateJavaScript( string s )
        {
            try
            {
                EvaluateJavaScriptResult = webBrowser.EvaluateScript( s ).Result ?? "null";
            }
            catch ( Exception e )
            {
                MessageBox.Show( "Error while evaluating Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        private void ExecuteJavaScript( string s )
        {
            try
            {
                webBrowser.EvaluateScript(s);
            }
            catch ( Exception e )
            {
                MessageBox.Show( "Error while executing Javascript: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        protected override void OnPropertyChanged<T>( T oldvalue, T newValue, PropertyChangedEventArgs e )
        {
            base.OnPropertyChanged( oldvalue, newValue, e );

            if ( e.PropertyName == AddressChangedEventArgs.PropertyName )
            {
                AddressEditable = Address;
            }
            else if ( e.PropertyName == TitleChangedEventArgs.PropertyName )
            {
                Application.Current.MainWindow.Title = "CefSharp.Wpf.Example - " + Title;
            }
            else if ( e.PropertyName == WebBrowserChangedEventArgs.PropertyName )
            {
                if ( WebBrowser != null )
                {
                    WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                    WebBrowser.LoadError += OnWebBrowserLoadError;

                    // TODO: This is a bit of a hack. It would be nicer/cleaner to give the webBrowser focus in the Go()
                    // TODO: method, but it seems like "something" gets messed up (= doesn't work correctly) if we give it
                    // TODO: focus "too early" in the loading process...
                    WebBrowser.LoadCompleted +=
                        delegate
                        {
                            Application.Current.Dispatcher.BeginInvoke( (Action) ( () => webBrowser.Focus() ) );
                        };
                }
            }
        }

        private void OnWebBrowserConsoleMessage( object sender, ConsoleMessageEventArgs e )
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserLoadError( string failedUrl, CefErrorCode errorCode, string errorText )
        {
            // Don't display an error for downloaded files where the user aborted the download.
            if ( errorCode == CefErrorCode.Aborted )
                return;

            var errorMessage = "<html><body><h2>Failed to load URL " + failedUrl +
                  " with error " + errorText + " (" + errorCode +
                  ").</h2></body></html>";

            webBrowser.LoadHtml( errorMessage, failedUrl );
        }

        private void Go()
        {
            Address = AddressEditable;

            // Part of the Focus hack further described in the OnPropertyChanged() method...
            Keyboard.ClearFocus();
        }

        private void ViewSource()
        {
            webBrowser.ViewSource();
        }
    }
}
