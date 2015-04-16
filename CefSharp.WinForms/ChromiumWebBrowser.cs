// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.Internals;
using CefSharp.WinForms.Internals;

namespace CefSharp.WinForms
{
    public class ChromiumWebBrowser : Control, IWebBrowserInternal, IWinFormsWebBrowser
    {
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;
        private ParentFormMessageInterceptor parentFormMessageInterceptor;

        /// <summary>
        /// Set to true while handing an activating WM_ACTIVATE message.
        /// MUST ONLY be cleared by DefaultFocusHandler.
        /// </summary>
        public bool IsActivating { get; set; }

        public BrowserSettings BrowserSettings { get; set; }
        public string Title { get; set; }
        public bool IsLoading { get; private set; }
        public string TooltipText { get; private set; }
        public string Address { get; private set; }

        public IDialogHandler DialogHandler { get; set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IMenuHandler MenuHandler { get; set; }

        /// <summary>
        /// The <see cref="IFocusHandler"/> for this ChromiumWebBrowser.
        /// </summary>
        /// <remarks>
        /// If you need customized focus handling behavior for WinForms, the suggested 
        /// best practice would be to inherit from DefaultFocusHandler and try to avoid 
        /// needing to override the logic in OnGotFocus. The implementation in 
        /// DefaultFocusHandler relies on very detailed behavior of how WinForms and 
        /// Windows interact during window activation.
        /// </remarks>
        public IFocusHandler FocusHandler { get; set; }
        public IDragHandler DragHandler { get; set; }
        public IResourceHandlerFactory ResourceHandlerFactory { get; set; }
        public IGeolocationHandler GeolocationHandler { get; set; }

        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanReload { get; private set; }
        public bool IsBrowserInitialized { get; private set; }

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
            if (!Cef.IsInitialized && !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            Cef.AddDisposable(this);
            Address = address;

            Dock = DockStyle.Fill;

            FocusHandler = new DefaultFocusHandler(this);
            ResourceHandlerFactory = new DefaultResourceHandlerFactory();
            BrowserSettings = new BrowserSettings();

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, false);
        }

        protected override void Dispose(bool disposing)
        {
            // Don't utilize any of the handlers anymore:
            DialogHandler = null;
            JsDialogHandler = null;
            KeyboardHandler = null;
            RequestHandler = null;
            DownloadHandler = null;
            LifeSpanHandler = null;
            MenuHandler = null;
            DragHandler = null;
            GeolocationHandler = null;
            FocusHandler = null;
            ResourceHandlerFactory = null;

            Cef.RemoveDisposable(this);

            if (disposing)
            {
                IsBrowserInitialized = false;

                if (BrowserSettings != null)
                {
                    BrowserSettings.Dispose();
                    BrowserSettings = null;
                }

                if (parentFormMessageInterceptor != null)
                {
                    parentFormMessageInterceptor.Dispose();
                    parentFormMessageInterceptor = null;
                }

                if (managedCefBrowserAdapter != null)
                {
                    managedCefBrowserAdapter.Dispose();
                    managedCefBrowserAdapter = null;
                }

                // Don't maintain a reference to event listeners anylonger:
                LoadError = null;
                FrameLoadStart = null;
                FrameLoadEnd = null;
                NavStateChanged = null;
                ConsoleMessage = null;
                StatusMessage = null;
                AddressChanged = null;
                TitleChanged = null;
                IsBrowserInitializedChanged = null;
                IsLoadingChanged = null;
            }
            base.Dispose(disposing);
        }

        void IWebBrowserInternal.OnInitialized()
        {
            IsBrowserInitialized = true;

            // By the time this callback gets called, this control
            // is most likely hooked into a browser Form of some sort. 
            // (Which is what ParentFormMessageInterceptor relies on.)
            // Ensure the ParentFormMessageInterceptor construction occurs on the WinForms UI thread:
            this.InvokeOnUiThreadIfRequired(() =>
            {
                parentFormMessageInterceptor = new ParentFormMessageInterceptor(this);
            });

            ResizeBrowser();

            var handler = IsBrowserInitializedChanged;

            if (handler != null)
            {
                handler(this, new IsBrowserInitializedChangedEventArgs(IsBrowserInitialized));
            }
        }

        public void Load(String url)
        {
            if (IsBrowserInitialized)
            {
                managedCefBrowserAdapter.LoadUrl(url);
            }
            else
            {
                Address = url;
            }
        }

        public void LoadHtml(string html, string url)
        {
            LoadHtml(html, url, Encoding.UTF8);
        }

        public void LoadHtml(string html, string url, Encoding encoding)
        {
            var factory = ResourceHandlerFactory;
            if (factory == null)
            {
                throw new Exception("Implement IResourceHandlerFactory and assign to the ResourceHandlerFactory property to use this feature");
            }

            factory.RegisterHandler(url, ResourceHandler.FromString(html, encoding, true));

            Load(url);
        }

        public void RegisterJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind, camelCaseJavascriptNames);
        }

        public void ExecuteScriptAsync(string script)
        {
            managedCefBrowserAdapter.ExecuteScriptAsync(script);
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(string script)
        {
            return EvaluateScriptAsync(script, timeout: null);
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(string script, TimeSpan? timeout)
        {
            return managedCefBrowserAdapter.EvaluateScriptAsync(script, timeout);
        }

        public void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY)
        {
            managedCefBrowserAdapter.OnMouseWheel(x, y, deltaX, deltaY);
        }

        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<NavStateChangedEventArgs> NavStateChanged;
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        public event EventHandler<IsBrowserInitializedChangedEventArgs> IsBrowserInitializedChanged;
        [Obsolete("IsLoadingChanged is unreliable and will be removed. Use NavStateChanged instead.")]
        public event EventHandler<IsLoadingChangedEventArgs> IsLoadingChanged;

        protected override void OnHandleCreated(EventArgs e)
        {
            managedCefBrowserAdapter.CreateBrowser(BrowserSettings, Handle, Address);

            base.OnHandleCreated(e);
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

            var handler = IsLoadingChanged;
            if (handler != null)
            {
                handler(this, new IsLoadingChangedEventArgs(isLoading));
            }
        }

        void IWebBrowserInternal.SetLoadingStateChange(bool canGoBack, bool canGoForward, bool isLoading)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = !isLoading;

            var handler = NavStateChanged;
            if (handler != null)
            {
                handler(this, new NavStateChangedEventArgs(canGoBack, canGoForward, isLoading));
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

        public void ShowDevTools()
        {
            managedCefBrowserAdapter.ShowDevTools();
        }

        public void CloseDevTools()
        {
            managedCefBrowserAdapter.CloseDevTools();
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

        /// <summary>
        /// Manually implement Focused because cef does not implement it.
        /// </summary>
        /// <remarks>
        /// This is also how the Microsoft's WebBrowserControl implements the Focused property.
        /// </remarks>
        public override bool Focused
        {
            get
            {
                if (base.Focused)
                {
                    return true;
                }

                if (!IsHandleCreated)
                {
                    return false;
                }

                return NativeMethodWrapper.IsFocused(Handle);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            
            ResizeBrowser();
        }

        private void ResizeBrowser()
        {
            if (IsBrowserInitialized)
            {
                managedCefBrowserAdapter.Resize(Width, Height);
            }
        }

        public void NotifyMoveOrResizeStarted()
        {
            if (IsBrowserInitialized)
            {
                managedCefBrowserAdapter.NotifyMoveOrResizeStarted();
            }
        }

        public void ReplaceMisspelling(string word)
        {
            managedCefBrowserAdapter.ReplaceMisspelling(word);
        }

        public void AddWordToDictionary(string word)
        {
            managedCefBrowserAdapter.AddWordToDictionary(word);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            SetFocus(true);
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Tell the browser to acquire/release focus.
        /// </summary>
        public void SetFocus(bool isFocused)
        {
            if (IsBrowserInitialized)
            {
                managedCefBrowserAdapter.SetFocus(isFocused);
            }
        }
    }
}
