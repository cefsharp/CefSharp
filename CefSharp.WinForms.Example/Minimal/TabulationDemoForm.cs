using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example.Minimal
{
    public partial class TabulationDemoForm : Form
    {
        UserControl _userControl;
        ChromiumWebBrowser _chromiumWebBrowser; 
        public TabulationDemoForm()
        {
            InitializeComponent();
            _chromiumWebBrowser = new ChromiumWebBrowser(txtURL.Text) { Dock = DockStyle.Fill };
            _userControl = new UserControl() { Dock = DockStyle.Fill };
            _userControl.Controls.Add(_chromiumWebBrowser);
            grpBrowser.Controls.Add(_userControl);
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            _chromiumWebBrowser.Load(txtURL.Text);
        }

        private void txtURL_Leave(object sender, EventArgs e)
        {
            txtURL.BackColor = Color.White;
        }

        private void txtURL_Enter(object sender, EventArgs e)
        {
            txtURL.BackColor = Color.Yellow;
        }
    }
}
