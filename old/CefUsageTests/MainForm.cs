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

        private void PrepareTest()
        {
            if (browser != null)
            {
                browser.Dispose();
                browser = null;
            }
        }

        private CefFormsWebBrowser browser;
        private void Test1Button_Click(object sender, EventArgs e)
        {
            // This is "default" usage of control.
            // We don't call CEF.Initialization - it's called implicitly by CefWebBrowser constructor.
            // Note, that CEF.Shutdown must called explicitly.
            // Also, CEF.Shutdown now is safe to call, even if CEF.Initialization not called before.

            PrepareTest();
            browser = new CefFormsWebBrowser("http://google.com", new BrowserSettings());
            browser.Parent = panel;
            browser.Dock = DockStyle.Fill;
        }

        private void Test2Button_Click(object sender, EventArgs e)
        {
            // This is demonstration of async browser construction.
            // .Load method raises exception with message 'CefBrowser is not ready.', cause underlying CefBrowser doesn't created.
            
            // I propose to that .Load method locks internally and wait for CefBrowser (AfterHandleCreated message).
            // But note, that lock also lock main message loop (if called from UI thread), or we can got inifinite lock, if no message arrived - so some timeout required.
            // If we have plans to support singlethreaded cef message loop - we must process cef message loop.
            // We can give name for event AfterHandleCreated as BrowserReady or Ready.
            // Note, that this is event only signals, that browser constructed, - this is NOT DocumentReady or Navigated.
            // Also .BrowserReady event is good, but method to perform sync operation also can be useful.

            PrepareTest();
            browser = new CefFormsWebBrowser();
            browser.Parent = panel;
            browser.Dock = DockStyle.Fill;
            try
            {
                browser.Load("http://google.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
