using System;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.WinForms.Example.Controls;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserForm : Form
    {
        private readonly WebView webView;

        public BrowserForm()
        {
            InitializeComponent();

            Load += BrowserFormLoad;

            Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            webView = new WebView(ExamplePresenter.DefaultUrl)
            {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(webView);
            
            webView.MenuHandler = new MenuHandler();
            webView.NavStateChanged += WebViewNavStateChanged;
            webView.ConsoleMessage += WebViewConsoleMessage;
            webView.TitleChanged += WebViewTitleChanged;
            webView.AddressChanged += WebViewAddressChanged;

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            DisplayOutput(version);
        }

        private void BrowserFormLoad(object sender, EventArgs e)
        {
            ToggleBottomToolStrip();
        }

        private void WebViewConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void WebViewNavStateChanged(object sender, NavStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        private void WebViewTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Text = args.Title);
        }

        private void WebViewAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private void SetCanGoBack(bool canGoBack)
        {
            this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward)
        {
            this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        private void SetIsLoading(bool isLoading)
        {
            goButton.Text = isLoading ?
                "Stop" :
                "Go";
            goButton.Image = isLoading ?
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
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
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

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            webView.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            webView.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            webView.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                webView.Load(url);
            }
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void FindCloseButtonClick(object sender, EventArgs e)
        {
            ToggleBottomToolStrip();
        }

        private void FindMenuItemClick(object sender, EventArgs e)
        {
            ToggleBottomToolStrip();
        }

        private void ToggleBottomToolStrip()
        {
            if (toolStripContainer.BottomToolStripPanelVisible)
            {
                webView.StopFinding(true);
                toolStripContainer.BottomToolStripPanelVisible = false;
            }
            else
            {
                toolStripContainer.BottomToolStripPanelVisible = true;
                findTextBox.Focus();
            }
        }

        private void FindNextButtonClick(object sender, EventArgs e)
        {
            Find(true);
        }
        
        private void FindPreviousButtonClick(object sender, EventArgs e)
        {
            Find(false);
        }

        private void Find(bool next)
        {
            if (!string.IsNullOrEmpty(findTextBox.Text))
            {
                webView.Find(0, findTextBox.Text, next, false, false);
            }
        }

        private void FindTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            Find(true);
        }
    }
}
