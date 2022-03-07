// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.Example;
using CefSharp.Example.Handlers;
using CefSharp.Example.JavascriptBinding;
using CefSharp.WinForms.Example.Handlers;
using CefSharp.WinForms.Experimental;
using CefSharp.WinForms.Handler;
using CefSharp.WinForms.Host;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserTabUserControl : UserControl
    {
        public IWinFormsChromiumWebBrowser Browser { get; private set; }
        private ChromiumWidgetNativeWindow messageInterceptor;
        private bool multiThreadedMessageLoopEnabled;

        public BrowserTabUserControl(ChromiumHostControl chromiumHostControl)
        {
            InitializeComponent();

            Browser = chromiumHostControl;

            browserPanel.Controls.Add(chromiumHostControl);

            chromiumHostControl.LoadingStateChanged += OnBrowserLoadingStateChanged;
            chromiumHostControl.ConsoleMessage += OnBrowserConsoleMessage;
            chromiumHostControl.TitleChanged += OnBrowserTitleChanged;
            chromiumHostControl.AddressChanged += OnBrowserAddressChanged;
            chromiumHostControl.StatusMessage += OnBrowserStatusMessage;
            chromiumHostControl.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            chromiumHostControl.LoadError += OnLoadError;
        }

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
            browser.DownloadHandler = new DownloadHandler();
            browser.AudioHandler = new CefSharp.Handler.AudioHandler();
            browser.FrameHandler = new CefSharp.Handler.FrameHandler();

            if (multiThreadedMessageLoopEnabled)
            {
                browser.KeyboardHandler = new KeyboardHandler();
            }

            //The CefSharp.WinForms.Handler.LifeSpanHandler implementation
            //allows for Popups to be hosted in Controls/Tabs
            //This example also demonstrates docking DevTools in a SplitPanel
            browser.LifeSpanHandler = CefSharp.WinForms.Handler.LifeSpanHandler
                .Create()
                .OnBeforePopupCreated((chromiumWebBrowser, b, frame, targetUrl, targetFrameName, targetDisposition, userGesture, browserSettings) =>
                {
                    //Can cancel opening popup based on Url if required.
                    if(targetUrl?.StartsWith(CefExample.BaseUrl + "/cancelme.html") == true)
                    {
                        return PopupCreation.Cancel;
                    }

                    return PopupCreation.Continue;
                })
                .OnPopupCreated((ctrl, targetUrl) =>
                {
                    //Don't try using ctrl.FindForm() here as
                    //the control hasn't been attached to a parent yet.
                    if (FindForm() is BrowserForm owner)
                    {
                        owner.AddTab(ctrl, targetUrl);
                    }
                })
                .OnPopupDestroyed((ctrl, popupBrowser) =>
                {
                    //If we docked  DevTools (hosted it ourselves rather than the default popup)
                    //Used when the BrowserTabUserControl.ShowDevToolsDocked method is called
                    if (popupBrowser.MainFrame.Url.Equals("devtools://devtools/devtools_app.html"))
                    {
                        //Dispose of the parent control we used to host DevTools, this will release the DevTools window handle
                        //and the ILifeSpanHandler.OnBeforeClose() will be call after.
                        ctrl.Dispose();
                    }
                    else
                    {
                        //If browser is disposed or the handle has been released then we don't
                        //need to remove the tab in this example. The user likely used the
                        // File -> Close Tab menu option which also calls BrowserForm.RemoveTab
                        if (!ctrl.IsDisposed && ctrl.IsHandleCreated)
                        {
                            if (ctrl.FindForm() is BrowserForm owner)
                            {
                                owner.RemoveTab(ctrl);
                            }

                            ctrl.Dispose();
                        }
                    }
                })
                .OnPopupBrowserCreated((ctrl, popupBrowser) =>
                {
                    //The host control maybe null if the popup was hosted in a native Window e.g. Devtools by default
                    if(ctrl == null)
                    {
                        return;
                    }

                    //You can access all the core browser functionality via IBrowser
                    //frames, browwser host, etc.
                    var isPopup = popupBrowser.IsPopup;
                })
                .Build();            

            browser.LoadingStateChanged += OnBrowserLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadError += OnLoadError;

#if NETCOREAPP
            browser.JavascriptObjectRepository.Register("boundAsync", new AsyncBoundObject(), options: BindingOptions.DefaultBinder);
#else
            browser.JavascriptObjectRepository.Register("bound", new BoundObject(), isAsync: false, options: BindingOptions.DefaultBinder);
            browser.JavascriptObjectRepository.Register("boundAsync", new AsyncBoundObject(), isAsync: true, options: BindingOptions.DefaultBinder);
#endif

            //If you call CefSharp.BindObjectAsync in javascript and pass in the name of an object which is not yet
            //bound, then ResolveObject will be called, you can then register it
            browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "boundAsync2")
                {
#if NETCOREAPP
                    repo.Register("boundAsync2", new AsyncBoundObject(), options: BindingOptions.DefaultBinder);
#else
                    repo.Register("boundAsync2", new AsyncBoundObject(), isAsync: true, options: BindingOptions.DefaultBinder);
#endif
                }
            };

            browser.RenderProcessMessageHandler = new RenderProcessMessageHandler();
            browser.DisplayHandler = new WinFormsDisplayHandler();
            //browser.MouseDown += OnBrowserMouseClick;
            this.multiThreadedMessageLoopEnabled = multiThreadedMessageLoopEnabled;

            var eventObject = new ScriptedMethodsBoundObject();
            eventObject.EventArrived += OnJavascriptEventArrived;
            // Use the default of camelCaseJavascriptNames
            // .Net methods starting with a capitol will be translated to starting with a lower case letter when called from js
#if !NETCOREAPP
            browser.JavascriptObjectRepository.Register("boundEvent", eventObject, isAsync: false, options: BindingOptions.DefaultBinder);
#endif

            CefExample.RegisterTestResources(browser);

            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
            //Set label directly, don't use DisplayOutput as call would be a NOOP (no valid handle yet).
            outputLabel.Text = version;
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

        private void OnBrowserMouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Mouse Clicked" + e.X + ";" + e.Y + ";" + e.Button);
        }

        private void OnLoadError(object sender, LoadErrorEventArgs args)
        {
            //Aborted is generally safe to ignore
            //Actions like starting a download will trigger an Aborted error
            //which doesn't require any user action.
            if(args.ErrorCode == CefErrorCode.Aborted)
            {
                return;
            }

            //Don't display an error for external protocols such as mailto which
            //we might want to open in the default viewer
            if (args.ErrorCode == CefErrorCode.UnknownUrlScheme && args.Frame.Url.StartsWith("mailto"))
            {
                return;
            }

            var errorHtml = string.Format("<html><body><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              args.FailedUrl, args.ErrorText, args.ErrorCode);

            _ = args.Browser.SetMainFrameDocumentContentAsync(errorHtml);

            //AddressChanged isn't called for failed Urls so we need to manually update the Url TextBox
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.FailedUrl);
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

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(args.IsLoading));
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
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            //Get the underlying browser host wrapper
            var browserHost = Browser.BrowserCore.GetHost();
            var requestContext = browserHost.RequestContext;
            string errorMessage;
            // Browser must be initialized before getting/setting preferences
            var success = requestContext.SetPreference("enable_do_not_track", true, out errorMessage);
            if (!success)
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
                SetupMessageInterceptor();
            }
        }

        /// <summary>
        /// The ChromiumWebBrowserControl does not fire MouseEnter/Move/Leave events, because Chromium handles these.
        /// This method provides a demo of hooking the Chrome_RenderWidgetHostHWND handle to receive low level messages.
        /// You can likely hook other window messages using this technique, drag/drog etc
        /// </summary>
        private void SetupMessageInterceptor()
        {
            if (messageInterceptor != null)
            {
                messageInterceptor.ReleaseHandle();
                messageInterceptor = null;
            }

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        IntPtr chromeWidgetHostHandle;
                        if (ChromiumRenderWidgetHandleFinder.TryFindHandle(Browser.BrowserCore, out chromeWidgetHostHandle))
                        {
                            messageInterceptor = new ChromiumWidgetNativeWindow((Control)Browser, chromeWidgetHostHandle);

                            messageInterceptor.OnWndProc(message =>
                            {
                                const int WM_MOUSEACTIVATE = 0x0021;
                                const int WM_NCLBUTTONDOWN = 0x00A1;
                                const int WM_DESTROY = 0x0002;

                                // Render process switch happened, need to find the new handle
                                if (message.Msg == WM_DESTROY)
                                {
                                    SetupMessageInterceptor();
                                    return false;
                                }

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

                                return false;
                            });

                            break;
                        }
                        else
                        {
                            // Chrome hasn't yet set up its message-loop window.
                            await Task.Delay(10);
                        }
                    }
                }
                catch
                {
                    // Errors are likely to occur if browser is disposed, and no good way to check from another thread
                }
            });
        }

        private void DisplayOutput(string output)
        {
            outputLabel.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
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
                Browser.LoadUrl(url);
            }
            else
            {
                var searchUrl = "https://www.google.com/search?q=" + Uri.EscapeDataString(url);

                Browser.LoadUrl(searchUrl);
            }

        }

        public async Task HideScrollbarsAsync()
        {
            var devTools = Browser.GetDevToolsClient();

            await devTools.Emulation.SetScrollbarsHiddenAsync(true);
        }

        public async Task CopySourceToClipBoardAsync()
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
                Browser.Find(findTextBox.Text, next, false, false);
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

        //Example of DevTools docked within the existing UserControl,
        //in this example it's hosted in a Panel with a SplitContainer
        public void ShowDevToolsDocked()
        {
            if (browserSplitContainer.Panel2Collapsed)
            {
                browserSplitContainer.Panel2Collapsed = false;
            }

            //Find devToolsControl in Controls collection
            Control devToolsControl = null;
            devToolsControl = browserSplitContainer.Panel2.Controls.Find(nameof(devToolsControl), false).FirstOrDefault();

            if (devToolsControl == null || devToolsControl.IsDisposed)
            {
                devToolsControl = Browser.ShowDevToolsDocked(
                    parentControl: browserSplitContainer.Panel2,
                    controlName: nameof(devToolsControl));

                EventHandler devToolsPanelDisposedHandler = null;
                devToolsPanelDisposedHandler = (s, e) =>
                {
                    browserSplitContainer.Panel2.Controls.Remove(devToolsControl);
                    browserSplitContainer.Panel2Collapsed = true;
                    devToolsControl.Disposed -= devToolsPanelDisposedHandler;
                };

                //Subscribe for devToolsPanel dispose event
                devToolsControl.Disposed += devToolsPanelDisposedHandler;
            }
        }

        public Task<bool> CheckIfDevToolsIsOpenAsync()
        {
            return Cef.UIThreadTaskFactory.StartNew(() =>
            {
                return Browser.GetBrowserHost().HasDevTools;
            });
        }
    }
}
