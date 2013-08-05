using System.Windows.Controls;
using System.Windows.Input;

namespace CefSharp.Wpf.Example.Views.Main
{
    public partial class MainView : UserControl
    {
        public MainView()
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
