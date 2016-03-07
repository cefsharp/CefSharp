// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.Internals;
using CefSharp.WinForms.Internals;

namespace CefSharp.WinForms
{
    public class ChromiumWebBrowser : Control, IWebBrowserInternal, IWinFormsWebBrowser
    {
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;
        private ParentFormMessageInterceptor parentFormMessageInterceptor;
        private IntPtr controlHandle;
        private IBrowser browser;

        /// <summary>
        /// Set to true while handing an activating WM_ACTIVATE message.
        /// MUST ONLY be cleared by DefaultFocusHandler.
        /// </summary>
        public bool IsActivating { get; set; }

        public BrowserSettings BrowserSettings { get; set; }
        public RequestContext RequestContext { get; set; }
        public bool IsLoading { get; private set; }
        public string TooltipText { get; private set; }
        public string Address { get; private set; }

        public IDialogHandler DialogHandler { get; set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public ILoadHandler LoadHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IDisplayHandler DisplayHandler { get; set; }
        public IContextMenuHandler MenuHandler { get; set; }
        public IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }
        public IFindHandler FindHandler { get; set; }

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

        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        public event EventHandler<IsBrowserInitializedChangedEventArgs> IsBrowserInitializedChanged;

        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool IsBrowserInitialized { get; private set; }

        static ChromiumWebBrowser()
        {
            if (CefSharpSettings.ShutdownOnExit)
            {
                Application.ApplicationExit += OnApplicationExit;
            }
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
            this.SetHandlersToNull();

            Cef.RemoveDisposable(this);

            if (disposing)
            {
                browser = null;
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
                LoadingStateChanged = null;
                ConsoleMessage = null;
                StatusMessage = null;
                AddressChanged = null;
                TitleChanged = null;
                IsBrowserInitializedChanged = null;
            }
            base.Dispose(disposing);
        }

        public void Load(String url)
        {
            if (IsBrowserInitialized)
            {
                this.GetMainFrame().LoadUrl(url);
            }
            else
            {
                Address = url;
            }
        }

        public void RegisterJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }

            //Enable WCF if not already enabled
            CefSharpSettings.WcfEnabled = true;

            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind, camelCaseJavascriptNames);
        }

        public void RegisterAsyncJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            managedCefBrowserAdapter.RegisterAsyncJsObject(name, objectToBind, camelCaseJavascriptNames);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (((IWebBrowserInternal)this).HasParent == false)
            {
                managedCefBrowserAdapter.CreateBrowser(BrowserSettings, RequestContext, Handle, Address);
            }

            controlHandle = Handle;

            base.OnHandleCreated(e);
        }

        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            this.browser = browser;
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

        void IWebBrowserInternal.SetAddress(AddressChangedEventArgs args)
        {
            Address = args.Address;

            var handler = AddressChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetLoadingStateChange(LoadingStateChangedEventArgs args)
        {
            CanGoBack = args.CanGoBack;
            CanGoForward = args.CanGoForward;
            IsLoading = args.IsLoading;

            var handler = LoadingStateChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetTitle(TitleChangedEventArgs args)
        {
            var handler = TitleChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        void IWebBrowserInternal.OnFrameLoadStart(FrameLoadStartEventArgs args)
        {
            var handler = FrameLoadStart;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.OnFrameLoadEnd(FrameLoadEndEventArgs args)
        {
            var handler = FrameLoadEnd;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.OnConsoleMessage(ConsoleMessageEventArgs args)
        {
            var handler = ConsoleMessage;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.OnStatusMessage(StatusMessageEventArgs args)
        {
            var handler = StatusMessage;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.OnLoadError(LoadErrorEventArgs args)
        {
            var handler = LoadError;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        IBrowserAdapter IWebBrowserInternal.BrowserAdapter
        {
            get { return managedCefBrowserAdapter; }
        }

        bool IWebBrowserInternal.HasParent { get; set; }
        
        IntPtr IWebBrowserInternal.ControlHandle
        {
            get { return controlHandle; }
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

        protected override void OnGotFocus(EventArgs e)
        {
            if (IsBrowserInitialized)
            {
                browser.GetHost().SetFocus(true);
            }

            base.OnGotFocus(e);
        }

        public IBrowser GetBrowser()
        {
            this.ThrowExceptionIfBrowserNotInitialized();

            return browser;
        }
    }
}
