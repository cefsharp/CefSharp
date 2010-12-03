using System;
using System.Windows.Forms;

namespace CefTest
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
