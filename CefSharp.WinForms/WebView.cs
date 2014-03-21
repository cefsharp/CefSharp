using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using CefSharp.Internals;

namespace CefSharp.WinForms
{
    public class WebView : Control, IWebBrowserInternal, IWinFormsWebBrowser
    {
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;

        public BrowserSettings BrowserSettings { get; set; }
        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public string TooltipText { get; set; }
        public string Address { get; set; }
                
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IMenuHandler MenuHandler { get; set; }

        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanReload { get; private set; }
        public bool IsBrowserInitialized { get; private set; }
        public IDictionary<string, object> BoundObjects { get; private set; }

        static WebView()
        {
            Application.ApplicationExit += OnApplicationExit;
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Cef.Shutdown();
        }

        public WebView(string address)
        {
                
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (managedCefBrowserAdapter != null)
                {
                    managedCefBrowserAdapter.Close();
                    managedCefBrowserAdapter.Dispose();
                    managedCefBrowserAdapter = null;
                }
            }
            base.Dispose(disposing);
        }

        public void OnInitialized()
        {
        }

        public void Load(String url)
        {
            managedCefBrowserAdapter.LoadUrl(url);
        }

        public void LoadHtml(string html, string url)
        {
            managedCefBrowserAdapter.LoadHtml(html, url);
        }

        public void RegisterJsObject(string name, object objectToBind)
        {
            throw new NotImplementedException();
        }

        public void ExecuteScriptAsync(string script)
        {
            managedCefBrowserAdapter.ExecuteScriptAsync(script);
        }

        public object EvaluateScript(string script)
        {
            return EvaluateScript(script, timeout: null);
        }

        public object EvaluateScript(string script, TimeSpan? timeout)
        {
            if (timeout == null)
            {
                timeout = TimeSpan.MaxValue;
            }

            return managedCefBrowserAdapter.EvaluateScript(script, timeout.Value);
        }

        public event LoadErrorEventHandler LoadError;
        public event LoadCompletedEventHandler LoadCompleted;
        public event ConsoleMessageEventHandler ConsoleMessage;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this);
            managedCefBrowserAdapter.CreateBrowser(BrowserSettings ?? new BrowserSettings(), Handle, Address);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (managedCefBrowserAdapter != null)
                managedCefBrowserAdapter.OnSizeChanged(Handle);
        }

        public void SetAddress(string address)
        {
            Address = address;
        }

        public void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }

        public void SetNavState(bool canGoBack, bool canGoForward, bool canReload)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = canReload;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        public void OnFrameLoadStart(string url)
        {
        }

        public void OnFrameLoadEnd(string url)
        {
            if (LoadCompleted != null)
            {
                LoadCompleted(this, new LoadCompletedEventArgs(url));
            }
        }

        public void OnTakeFocus(bool next)
        {
            throw new NotImplementedException();
        }

        public void OnConsoleMessage(string message, string source, int line)
        {
            if (ConsoleMessage != null)
            {
                ConsoleMessage(this, new ConsoleMessageEventArgs(message, source, line));
            }
        }

        public void OnLoadError(string url, CefErrorCode errorCode, string errorText)
        {
            if (LoadError != null)
            {
                LoadError(url, errorCode, errorText);
            }
        }

        

        public void ShowDevTools()
        {
            // TODO: Do something about this one.
            var devToolsUrl = managedCefBrowserAdapter.DevToolsUrl;
            throw new NotImplementedException();
        }

        public void CloseDevTools()
        {
            throw new NotImplementedException();
        }
    }
}
