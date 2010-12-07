using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CefSharp;

namespace CefTest
{
    public partial class Browser : Form, IBeforeResourceLoad
    {
        private readonly BrowserControl _browserControl;

        public Browser()
        {
            InitializeComponent();
            Text = "CefSharp";
            _browserControl = new BrowserControl("http://www.google.co.uk/search?q=cefsharp");
            _browserControl.Dock = DockStyle.Fill;
            _browserControl.PropertyChanged += HandleBrowserPropertyChanged;
            _browserControl.BeforeResourceLoadHandler = this;
            toolStripContainer.ContentPanel.Controls.Add(_browserControl);            
        }

        private void HandleBrowserPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Invoke(new MethodInvoker(() => { if (!IsDisposed) UpdateBrowserControls(sender, e); }));
        }

        private void UpdateBrowserControls(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Title":
                    Text = _browserControl.Title;
                    break;
                case "Address":
                    urlTextBox.Text = _browserControl.Address;
                    break;
                case "CanGoBack":
                    backButton.Enabled = _browserControl.CanGoBack;
                    break;
                case "CanGoForward":
                    forwardButton.Enabled = _browserControl.CanGoForward;
                    break;
                case "IsLoading":
                    goButton.Text = _browserControl.IsLoading ? "Stop" : "Go";
                    break;
            }
        }

        private void HandleGoButtonClick(object sender, EventArgs e)
        {
            if(_browserControl.IsLoading)
            {
                _browserControl.Stop();
            }
            else
            {
                _browserControl.Load(urlTextBox.Text);    
            }            
        }

        private void HandleBackButtonClick(object sender, EventArgs e)
        {
            _browserControl.Back();
        }

        private void HandleForwardButtonClick(object sender, EventArgs e)
        {
            _browserControl.Forward();
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            int width = toolStrip1.DisplayRectangle.Width;
            foreach (ToolStripItem tsi in toolStrip1.Items)
            {
                if (tsi != urlTextBox)
                {
                    width -= tsi.Width;
                    width -= tsi.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal);
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _browserControl.Load(urlTextBox.Text);
            }
        }

        public ReturnValue HandleBeforeResourceLoad(BrowserControl browserControl, IRequest request, ref string redirectUrl, ref Stream resourceStream, ref string mimeType, int loadFlags)        
        {
            if(request.Url.StartsWith("http://test/resource/load"))
            {
                resourceStream = new MemoryStream(Encoding.UTF8.GetBytes(
                    "<html><body><h1>Success</h1><p>This document is loaded from a System.IO.Stream</p></body></html>"));
                mimeType = "text/html";
            }
            Console.WriteLine(request.Url);
            return ReturnValue.Continue;
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TestResourceLoadToolStripMenuItemClick(object sender, EventArgs e)
        {
            _browserControl.Load("http://test/resource/load");
        }

        private void TestRunJsSynchronouslyToolStripMenuItemClick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);

            try
            {
                String result = _browserControl.RunScript(a + "+" + b, "RunJsTest", 1, 5000);

                if (result == (a + b).ToString())
                {
                    MessageBox.Show(string.Format("{0} + {1} = {2}", a, b, result), "Success");
                }
                else
                {
                    MessageBox.Show(string.Format("{0} + {1} != {2}", a, b, result), "Failure");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString(), "Failure");
            }
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void TestRunArbitraryJavaScriptToolStripMenuItemClick(object sender, EventArgs e)
        {                       
            InputForm inputForm = new InputForm();
            if(inputForm.ShowDialog() == DialogResult.OK)
            {
                string script = inputForm.GetInput();
                try
                {
                    string result = _browserControl.RunScript(script, "about:blank", 1, 5000);
                    MessageBox.Show(result, "Result");
                } 
                catch(Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error");
                }
            }
        }

        private void TestSchemeHandlerToolStripMenuItemClick(object sender, EventArgs e)
        {
            _browserControl.Load("test://test/SchemeTest.html");
        }
    }
}
