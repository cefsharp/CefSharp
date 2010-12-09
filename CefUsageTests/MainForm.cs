using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CefSharp;

namespace CefUsageTests
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private BrowserControl browser;
        private void btnTest1_Click(object sender, EventArgs e)
        {
            browser = new BrowserControl("http://google.com");
            browser.Parent = panel;
            browser.Dock = DockStyle.Fill;
        }
    }
}
