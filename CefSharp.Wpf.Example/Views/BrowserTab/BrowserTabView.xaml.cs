using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Wpf.Example.Mvvm;

namespace CefSharp.Wpf.Example.Views.BrowserTab
{
    public partial class BrowserTabView : UserControl
    {
        public BrowserTabView()
        {
            InitializeComponent();

            webView.RequestHandler = new DefaultRequestHandler();
        }

        public void FocusAddress()
        {
            BrowserAddress.SelectAll();
            BrowserAddress.Focus();
        }

        public void Reload(bool ignoreCache)
        {
            webView.Reload(ignoreCache);
        }

        private void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void OnTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.SelectAll();
        }
    }
}
