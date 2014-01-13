using System;
using System.Windows.Forms;
using CefSharp.Example;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserForm : Form
    {
        public event EventHandler ShowDevToolsActivated
        {
            add { showDevToolsMenuItem.Click += value; }
            remove { showDevToolsMenuItem.Click -= value; }
        }

        public event EventHandler CloseDevToolsActivated
        {
            add { closeDevToolsMenuItem.Click += value; }
            remove { showDevToolsMenuItem.Click -= value; }
        }

        public event EventHandler ExitActivated
        {
            add { exitToolStripMenuItem.Click += value; }
            remove { exitToolStripMenuItem.Click -= value; }
        }

        public event EventHandler UndoActivated
        {
            add { undoMenuItem.Click += value; }
            remove { undoMenuItem.Click -= value; }
        }

        public event EventHandler RedoActivated
        {
            add { redoMenuItem.Click += value; }
            remove { redoMenuItem.Click -= value; }
        }

        public event EventHandler CutActivated
        {
            add { cutMenuItem.Click += value; }
            remove { cutMenuItem.Click -= value; }
        }

        public event EventHandler CopyActivated
        {
            add { copyMenuItem.Click += value; }
            remove { copyMenuItem.Click -= value; }
        }

        public event EventHandler PasteActivated
        {
            add { pasteMenuItem.Click += value; }
            remove { pasteMenuItem.Click -= value; }
        }

        public event EventHandler DeleteActivated
        {
            add { deleteMenuItem.Click += value; }
            remove { deleteMenuItem.Click -= value; }
        }

        public event EventHandler SelectAllActivated
        {
            add { selectAllMenuItem.Click += value; }
            remove { selectAllMenuItem.Click -= value; }
        }

        public event Action<object, string> UrlActivated;

        public event EventHandler BackActivated
        {
            add { backButton.Click += value; }
            remove { backButton.Click -= value; }
        }

        public event EventHandler ForwardActivated
        {
            add { forwardButton.Click += value; }
            remove { forwardButton.Click -= value; }
        }

        private readonly WebView webView;

        public BrowserForm()
        {
            InitializeComponent();
            Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            webView = new WebView(ExamplePresenter.DefaultUrl)
            {
                Dock = DockStyle.Fill
            };
            toolStripContainer.ContentPanel.Controls.Add(webView);
            
            webView.MenuHandler = new MenuHandler();
        }

        public void SetTitle(string title)
        {
            Text = title;
        }

        public void SetAddress(string address)
        {
            urlTextBox.Text = address;
        }

        public void SetAddress(Uri uri)
        {
            urlTextBox.Text = uri.ToString();
        }

        public void SetCanGoBack(bool can_go_back)
        {
            backButton.Enabled = can_go_back;
        }

        public void SetCanGoForward(bool can_go_forward)
        {
            forwardButton.Enabled = can_go_forward;
        }

        public void SetIsLoading(bool is_loading)
        {
            goButton.Text = is_loading ?
                "Stop" :
                "Go";
            goButton.Image = is_loading ?
                Properties.Resources.nav_plain_red :
                Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        public void ExecuteScript(string script)
        {
            webView.ExecuteScriptAsync(script);
        }

        public object EvaluateScript(string script)
        {
            return webView.EvaluateScript(script);
        }

        public void DisplayOutput(string output)
        {
            outputLabel.Text = output;
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout()
        {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        private void HandleGoButtonClick(object sender, EventArgs e)
        {
            var handler = this.UrlActivated;
            if (handler != null)
            {
                handler(this, urlTextBox.Text);
            }
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            var handler = UrlActivated;
            if (handler != null)
            {
                handler(this, urlTextBox.Text);
            }
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }
    }
}
