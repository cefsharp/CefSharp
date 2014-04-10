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

            CommandBindings.Add(new CommandBinding(CefBrowserRoutedCommands.FocusAddress, FocusAddress));
        }

        private void FocusAddress(object sender, ExecutedRoutedEventArgs e)
        {
            BrowserAddress.SelectAll();
            BrowserAddress.Focus();
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
