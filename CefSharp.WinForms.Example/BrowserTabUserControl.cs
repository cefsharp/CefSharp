// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Example;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp.WinForms.Example.Handlers;
using CefSharp.WinForms.Internals;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserTabUserControl : UserControl
    {
        public IWinFormsWebBrowser Browser { get; private set; }
        private IntPtr browserHandle;
        private ChromeWidgetMessageInterceptor messageInterceptor;
        private bool multiThreadedMessageLoopEnabled;

        public BrowserTabUserControl(Action<string, int?> openNewTab, string url, bool multiThreadedMessageLoopEnabled)
        {
            InitializeComponent();

            var browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill
            };

            browserPanel.Controls.Add(browser);

            Browser = browser;

            browser.MenuHandler = new MenuHandler();
            browser.RequestHandler = new WinFormsRequestHandler(openNewTab);
            browser.JsDialogHandler = new JsDialogHandler();
            browser.GeolocationHandler = new GeolocationHandler();
            browser.DownloadHandler = new DownloadHandler();
            if (multiThreadedMessageLoopEnabled)
            {
                browser.KeyboardHandler = new KeyboardHandler();
            }
            browser.LifeSpanHandler = new LifeSpanHandler();
            browser.LoadingStateChanged += OnBrowserLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadError += OnLoadError;
            browser.RegisterJsObject("bound", new BoundObject());
            browser.RegisterAsyncJsObject("boundAsync", new AsyncBoundObject());
            browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();
            browser.DisplayHandler = new DisplayHandler();
            //browser.MouseDown += OnBrowserMouseClick;
            browser.HandleCreated += OnBrowserHandleCreated;
            //browser.ResourceHandlerFactory = new FlashResourceHandlerFactory();
            this.multiThreadedMessageLoopEnabled = multiThreadedMessageLoopEnabled;

            var eventObject = new ScriptedMethodsBoundObject();
            eventObject.EventArrived += OnJavascriptEventArrived;
            // Use the default of camelCaseJavascriptNames
            // .Net methods starting with a capitol will be translated to starting with a lower case letter when called from js
            browser.RegisterJsObject("boundEvent", eventObject, BindingOptions.DefaultBinder);

            CefExample.RegisterTestResources(browser);

            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            DisplayOutput(version);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }

                if (messageInterceptor != null)
                {
                    messageInterceptor.ReleaseHandle();
                    messageInterceptor = null;
                }
            }
            base.Dispose(disposing);
        }

        private void OnBrowserHandleCreated(object sender, EventArgs e)
        {
            browserHandle = ((ChromiumWebBrowser)Browser).Handle;
        }

        private void OnBrowserMouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Mouse Clicked" + e.X + ";" + e.Y + ";" + e.Button);
        }

        private void OnLoadError(object sender, LoadErrorEventArgs args)
        {
            DisplayOutput("Load Error:" + args.ErrorCode + ";" + args.ErrorText);
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnBrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Parent.Text = args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private static void OnJavascriptEventArrived(string eventName, object eventData)
        {
            switch (eventName)
            {
                case "click":
                {
                    var message = eventData.ToString();
                    var dataDictionary = eventData as Dictionary<string, object>;
                    if (dataDictionary != null)
                    {
                        var result = string.Join(", ", dataDictionary.Select(pair => pair.Key + "=" + pair.Value));
                        message = "event data: " + result;
                    }
                    MessageBox.Show(message, "Javascript event arrived", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }
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

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void OnIsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs args)
        {
            if (args.IsBrowserInitialized)
            {
                //Get the underlying browser host wrapper
                var browserHost = Browser.GetBrowser().GetHost();
                var requestContext = browserHost.RequestContext;
                string errorMessage;
                // Browser must be initialized before getting/setting preferences
                var success = requestContext.SetPreference("enable_do_not_track", true, out errorMessage);
                if(!success)
                {
                    this.InvokeOnUiThreadIfRequired(() => MessageBox.Show("Unable to set preference enable_do_not_track errorMessage: " + errorMessage));
                }

                //Example of disable spellchecking
                //success = requestContext.SetPreference("browser.enable_spellchecking", false, out errorMessage);

                var preferences = requestContext.GetAllPreferences(true);
                var doNotTrack = (bool)preferences["enable_do_not_track"];

                //Use this to check that settings preferences are working in your code
                //success = requestContext.SetPreference("webkit.webprefs.minimum_font_size", 24, out errorMessage);

                //If we're using CefSetting.MultiThreadedMessageLoop (the default) then to hook the message pump,
                // which running in a different thread we have to use a NativeWindow
                if (multiThreadedMessageLoopEnabled)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            while (true)
                            {
                                IntPtr chromeWidgetHostHandle;
                                if (ChromeWidgetHandleFinder.TryFindHandle(browserHandle, out chromeWidgetHostHandle))
                                {
                                    messageInterceptor = new ChromeWidgetMessageInterceptor((Control)Browser, chromeWidgetHostHandle, message =>
                                    {
                                        const int WM_MOUSEACTIVATE = 0x0021;
                                        const int WM_NCLBUTTONDOWN = 0x00A1;
                                        const int WM_LBUTTONDOWN = 0x0201;

                                        if (message.Msg == WM_MOUSEACTIVATE)
                                        {
                                            // The default processing of WM_MOUSEACTIVATE results in MA_NOACTIVATE,
                                            // and the subsequent mouse click is eaten by Chrome.
                                            // This means any .NET ToolStrip or ContextMenuStrip does not get closed.
                                            // By posting a WM_NCLBUTTONDOWN message to a harmless co-ordinate of the
                                            // top-level window, we rely on the ToolStripManager's message handling
                                            // to close any open dropdowns:
                                            // http://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/ToolStripManager.cs,1249
                                            var topLevelWindowHandle = message.WParam;
                                            PostMessage(topLevelWindowHandle, WM_NCLBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
                                        }
                                        //Forward mouse button down message to browser control
                                        //else if(message.Msg == WM_LBUTTONDOWN)
                                        //{
                                        //    PostMessage(browserHandle, WM_LBUTTONDOWN, message.WParam, message.LParam);
                                        //}

                                        // The ChromiumWebBrowserControl does not fire MouseEnter/Move/Leave events, because Chromium handles these.
                                        // However we can hook into Chromium's messaging window to receive the events.
                                        //
                                        //const int WM_MOUSEMOVE = 0x0200;
                                        //const int WM_MOUSELEAVE = 0x02A3;
                                        //
                                        //switch (message.Msg) {
                                        //    case WM_MOUSEMOVE:
                                        //        Console.WriteLine("WM_MOUSEMOVE");
                                        //        break;
                                        //    case WM_MOUSELEAVE:
                                        //        Console.WriteLine("WM_MOUSELEAVE");
                                        //        break;
                                        //}
                                    });

                                    break;
                                }
                                else
                                {
                                    // Chrome hasn't yet set up its message-loop window.
                                    Thread.Sleep(10);
                                }
                            }
                        }
                        catch
                        {
                            // Errors are likely to occur if browser is disposed, and no good way to check from another thread
                        }
                    });
                }
            }
        }

        private void DisplayOutput(string output)
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

        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            Browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            Browser.Forward();
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
                Browser.Load(url);
            }
        }

        public async void CopySourceToClipBoardAsync()
        {
            var htmlSource = await Browser.GetSourceAsync();

            Clipboard.SetText(htmlSource);
            DisplayOutput("HTML Source copied to clipboard");
        }

        private void ToggleBottomToolStrip()
        {
            if (toolStrip2.Visible)
            {
                Browser.StopFinding(true);
                toolStrip2.Visible = false;
            }
            else
            {
                toolStrip2.Visible = true;
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
                Browser.Find(0, findTextBox.Text, next, false, false);
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

        public void ShowFind()
        {
            ToggleBottomToolStrip();
        }

        private void FindCloseButtonClick(object sender, EventArgs e)
        {
            ToggleBottomToolStrip();
        }
    }
}
