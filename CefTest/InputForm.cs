using System;
using System.Windows.Forms;

namespace CefSharp.Example
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
        }

        public String GetInput()
        {
            return input.Text;
        }
    }
}
