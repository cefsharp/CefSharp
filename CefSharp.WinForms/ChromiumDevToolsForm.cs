using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CefSharp.WinForms
{
  public partial class ChromiumDevToolsForm : Form
  {
    private ChromiumWebBrowser browser;

    public ChromiumDevToolsForm(string devToolsUrl)
    {
      InitializeComponent();
      browser = new ChromiumWebBrowser(devToolsUrl)
      {
        Dock = DockStyle.Fill,
      };

      Controls.Add(browser);
    }
  }
}
