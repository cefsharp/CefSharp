using System.Windows.Controls;
using System.Windows.Input;

namespace CefSharp.Wpf.Example.Views.BrowserTab
{
    public partial class BrowserTabView : UserControl
    {
        public BrowserTabView()
        {
            InitializeComponent();
        }

        private void OnTextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.SelectAll();
        }

        private void OnTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            var textBox = (TextBox) sender;
            textBox.SelectAll();
        }
    }
}
