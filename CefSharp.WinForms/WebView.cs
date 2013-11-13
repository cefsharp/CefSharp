using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace CefSharp.WinForms
{
    // TODO: Should not implement IRenderWebBrowser straight away, since it means we have to add a bunch of boilerplate properties
    // TODO: which aren't even used. The proper way would be to add another interface, which is only used for WinForms (which the
    // TODO: IRenderWebBrowser should then inherit from, and extend as appropriate).
    public class WebView : Control, IRenderWebBrowser
    {
        private readonly BrowserCore browserCore;
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;
        public BrowserSettings BrowserSettings { get; set; }

        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public string TooltipText { get; set; }

        public string Address
        {
            get
            {
                return browserCore.Address;
            }
            set
            {
                browserCore.Address = value;
            }
        }
        bool IWebBrowser.CanGoForward
        {
            get { return browserCore.CanGoForward; }
        }
        bool IWebBrowser.CanGoBack
        {
            get { return browserCore.CanGoBack; }
        }

        public WebView(string address)
        {
            browserCore = new BrowserCore(address);
            Application.ApplicationExit += OnApplicationExit;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (managedCefBrowserAdapter == null)
            {
                return;
            }

            managedCefBrowserAdapter.Close();
            managedCefBrowserAdapter.Dispose();
            managedCefBrowserAdapter = null;
        
            Cef.Shutdown();
        }

        public void LoadHtml(string html, string url)
        {
        }

        public void Load(String url)
        {
            managedCefBrowserAdapter.LoadUrl(url);
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
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnInitialized()
        {
            browserCore.OnInitialized();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this);
            managedCefBrowserAdapter.CreateBrowser(BrowserSettings ?? new BrowserSettings(), Handle, browserCore.Address);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (managedCefBrowserAdapter != null)
                managedCefBrowserAdapter.OnSizeChanged(Handle);
        }

        public void SetAddress(string address)
        {
            browserCore.Address = address;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Address"));
            }
        }

        public void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }

        public void SetNavState(bool canGoBack, bool canGoForward, bool canReload)
        {
            browserCore.SetNavState(canGoBack, canGoForward, canReload);
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void ClearHistory()
        {
            throw new NotImplementedException();
        }

        public void ShowDevTools()
        {
            throw new NotImplementedException();
        }

        public void CloseDevTools()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void Cut()
        {
            throw new NotImplementedException();
        }

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public void Paste()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void SelectAll()
        {
            throw new NotImplementedException();
        }

        public void Print()
        {
            throw new NotImplementedException();
        }

        public void OnFrameLoadStart(string url)
        {
            browserCore.OnFrameLoadStart();
        }

        public void OnFrameLoadEnd(string url)
        {
            browserCore.OnFrameLoadEnd();

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

        public IDictionary<string, object> GetBoundObjects()
        {
            throw new NotImplementedException();
        }

        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public bool IsBrowserInitialized { get; private set; }
        public void InvokeRenderAsync(Action callback)
        {
            throw new NotImplementedException();
        }

        public void SetCursor(IntPtr cursor)
        {
            throw new NotImplementedException();
        }

        public void ClearBitmap()
        {
            throw new NotImplementedException();
        }

        public void SetBitmap()
        {
            throw new NotImplementedException();
        }

        public int BytesPerPixel { get; private set; }
        public IntPtr FileMappingHandle { get; set; }
    }
}
