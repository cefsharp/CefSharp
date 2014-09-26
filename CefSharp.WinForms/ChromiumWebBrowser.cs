// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp.Internals;

namespace CefSharp.WinForms
{
    public class ChromiumWebBrowser : Control, IWebBrowserInternal, IWinFormsWebBrowser
    {
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;

        public BrowserSettings BrowserSettings { get; set; }
        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public string TooltipText { get; set; }
        public string Address { get; set; }

        public IDialogHandler DialogHandler { get; set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IMenuHandler MenuHandler { get; set; }

        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanReload { get; private set; }
        public bool IsBrowserInitialized { get; private set; }
        public IDictionary<string, object> BoundObjects { get; private set; }
        public double ZoomLevel
        {
            get { return managedCefBrowserAdapter.GetZoomLevel(); }
            set { managedCefBrowserAdapter.SetZoomLevel(value); }
         }

        static ChromiumWebBrowser()
        {
            Application.ApplicationExit += OnApplicationExit;
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Cef.Shutdown();
        }

        public ChromiumWebBrowser(string address)
        {
            Cef.AddDisposable(this);
            Address = address;
        }

        protected override void Dispose(bool disposing)
        {
            Cef.RemoveDisposable(this);

            if (disposing)
            {
                if (managedCefBrowserAdapter != null)
                {
                    managedCefBrowserAdapter.Dispose();
                    managedCefBrowserAdapter = null;
                }
            }
            base.Dispose(disposing);
        }

        void IWebBrowserInternal.OnInitialized()
        {
            IsBrowserInitialized = true;
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
        public event FrameLoadStartEventHandler FrameLoadStart;
        public event FrameLoadEndEventHandler FrameLoadEnd;
        public event NavStateChangedEventHandler NavStateChanged;
        public event ConsoleMessageEventHandler ConsoleMessage;
        public event StatusMessageEventHandler StatusMessage;
        public event AddressChangedEventHandler AddressChanged;
        public event TitleChangedEventHandler TitleChanged;

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

        void IWebBrowserInternal.SetAddress(string address)
        {
            Address = address;

            var handler = AddressChanged;
            if (handler != null)
            {
                handler(this, new AddressChangedEventArgs(address));
            }
        }

        void IWebBrowserInternal.SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
        }

        void IWebBrowserInternal.SetNavState(bool canGoBack, bool canGoForward, bool canReload)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = canReload;

            var handler = NavStateChanged;
            if (handler != null)
            {
                handler(this, new NavStateChangedEventArgs(canGoBack, canGoForward, canReload));
            }
        }

        void IWebBrowserInternal.SetTitle(string title)
        {
            Title = title;

            var handler = TitleChanged;
            if (handler != null)
            {
                handler(this, new TitleChangedEventArgs(title));
            }
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        void IWebBrowserInternal.OnFrameLoadStart(string url, bool isMainFrame)
        {
            var handler = FrameLoadStart;
            if (handler != null)
            {
                handler(this, new FrameLoadStartEventArgs(url, isMainFrame));
            }
        }

        void IWebBrowserInternal.OnFrameLoadEnd(string url, bool isMainFrame, int httpStatusCode)
        {
            var handler = FrameLoadEnd;
            if (handler != null)
            {
                handler(this, new FrameLoadEndEventArgs(url, isMainFrame, httpStatusCode));
            }
        }

        void IWebBrowserInternal.OnTakeFocus(bool next)
        {
            SelectNextControl(this, next, true, true, true);
        }

        void IWebBrowserInternal.OnConsoleMessage(string message, string source, int line)
        {
            var handler = ConsoleMessage;
            if (handler != null)
            {
                handler(this, new ConsoleMessageEventArgs(message, source, line));
            }
        }

        void IWebBrowserInternal.OnStatusMessage(string value)
        {
            var handler = StatusMessage;
            if (handler != null)
            {
                handler(this, new StatusMessageEventArgs(value));
            }
        }

        void IWebBrowserInternal.OnLoadError(string url, CefErrorCode errorCode, string errorText)
        {
            var handler = LoadError;
            if (handler != null)
            {
                handler(this, new LoadErrorEventArgs(url, errorCode, errorText));
            }
        }

        public void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            managedCefBrowserAdapter.Find(identifier, searchText, forward, matchCase, findNext);
        }

        public void StopFinding(bool clearSelection)
        {
            managedCefBrowserAdapter.StopFinding(clearSelection);
        }

        void IWebBrowserInternal.ShowDevTools()
        {
            // TODO: Do something about this one.
            throw new NotImplementedException("Implement when Cef upgraded to 1750.");
        }

        void IWebBrowserInternal.CloseDevTools()
        {
            throw new NotImplementedException("Implement when Cef upgraded to 1750.");
        }

        public void Stop()
        {
            managedCefBrowserAdapter.Stop();
        }

        public void Back()
        {
            managedCefBrowserAdapter.GoBack();
        }

        public void Forward()
        {
            managedCefBrowserAdapter.GoForward();
        }

        public void Reload()
        {
            Reload(false);
        }

        public void Reload(bool ignoreCache)
        {
            managedCefBrowserAdapter.Reload(ignoreCache);
        }

        public void Undo()
        {
            managedCefBrowserAdapter.Undo();
        }

        public void Redo()
        {
            managedCefBrowserAdapter.Redo();
        }

        public void Cut()
        {
            managedCefBrowserAdapter.Cut();
        }

        public void Copy()
        {
            managedCefBrowserAdapter.Copy();
        }

        public void Paste()
        {
            managedCefBrowserAdapter.Paste();
        }

        public void Delete()
        {
            managedCefBrowserAdapter.Delete();
        }

        public void SelectAll()
        {
            managedCefBrowserAdapter.SelectAll();
        }

        public void Print()
        {
            managedCefBrowserAdapter.Print();
        }

        public void ViewSource()
        {
            managedCefBrowserAdapter.ViewSource();
        }

        public Task<string> GetSourceAsync()
        {
            var taskStringVisitor = new TaskStringVisitor();
            managedCefBrowserAdapter.GetSource(taskStringVisitor);
            return taskStringVisitor.Task;
        }

        public Task<string> GetTextAsync()
        {
            var taskStringVisitor = new TaskStringVisitor();
            managedCefBrowserAdapter.GetText(taskStringVisitor);
            return taskStringVisitor.Task;
        }
    }
}
