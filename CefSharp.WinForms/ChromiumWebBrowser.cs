// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using CefSharp.WinForms.Internals;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;

namespace CefSharp.WinForms
{
    /// <summary>
    /// ChromiumWebBrowser is the WinForms web browser control
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="CefSharp.Internals.IWebBrowserInternal" />
    /// <seealso cref="CefSharp.WinForms.IWinFormsWebBrowser" />
    [Docking(DockingBehavior.AutoDock),DefaultEvent("LoadingStateChanged"), ToolboxBitmap(typeof(ChromiumWebBrowser)),
    Description("CefSharp ChromiumWebBrowser - Chromium Embedded Framework .Net wrapper. https://github.com/cefsharp/CefSharp"),
    Designer(typeof(ChromiumWebBrowserDesigner))]
    public class ChromiumWebBrowser : Control, IWebBrowserInternal, IWinFormsWebBrowser
    {
        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;
        /// <summary>
        /// The parent form message interceptor
        /// </summary>
        private ParentFormMessageInterceptor parentFormMessageInterceptor;
        /// <summary>
        /// The browser
        /// </summary>
        private IBrowser browser;
        /// <summary>
        /// A flag that indicates whether or not the designer is active
        /// NOTE: DesignMode becomes false by the time we get to the destructor/dispose so it gets stored here
        /// </summary>
        private bool designMode;
        /// <summary>
        /// A flag that indicates whether or not <see cref="InitializeFieldsAndCefIfRequired"/> has been called.
        /// </summary>
        private bool initialized;
        /// <summary>
        /// Has the underlying Cef Browser been created (slightly different to initliazed in that
        /// the browser is initialized in an async fashion)
        /// </summary>
        private bool browserCreated;
        /// <summary>
        /// The request context (we deliberately use a private variable so we can throw an exception if
        /// user attempts to set after browser created)
        /// </summary>
        private IRequestContext requestContext;

        /// <summary>
        /// Set to true while handing an activating WM_ACTIVATE message.
        /// MUST ONLY be cleared by DefaultFocusHandler.
        /// </summary>
        /// <value><c>true</c> if this instance is activating; otherwise, <c>false</c>.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public bool IsActivating { get; set; }

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public BrowserSettings BrowserSettings { get; set; }
        /// <summary>
        /// Gets or sets the request context.
        /// </summary>
        /// <value>The request context.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IRequestContext RequestContext
        {
            get { return requestContext; }
            set
            {
                if (browserCreated)
                {
                    throw new Exception("Browser has already been created. RequestContext must be" +
                                        "set before the underlying CEF browser is created.");
                }
                if(value != null && value.GetType() != typeof(RequestContext))
                {
                    throw new Exception(string.Format("RequestContxt can only be of type {0} or null", typeof(RequestContext)));
                }
                requestContext = value;
            }
        }
        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is loading; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public bool IsLoading { get; private set; }
        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        /// <value>The tooltip text.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public string TooltipText { get; private set; }
        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <value>The address.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public string Address { get; private set; }

        /// <summary>
        /// Implement <see cref="IDialogHandler" /> and assign to handle dialog events.
        /// </summary>
        /// <value>The dialog handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IDialogHandler DialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IJsDialogHandler" /> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        /// <value>The js dialog handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IJsDialogHandler JsDialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IKeyboardHandler" /> and assign to handle events related to key press.
        /// </summary>
        /// <value>The keyboard handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IKeyboardHandler KeyboardHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IRequestHandler" /> and assign to handle events related to browser requests.
        /// </summary>
        /// <value>The request handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IRequestHandler RequestHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDownloadHandler" /> and assign to handle events related to downloading files.
        /// </summary>
        /// <value>The download handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IDownloadHandler DownloadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILoadHandler" /> and assign to handle events related to browser load status.
        /// </summary>
        /// <value>The load handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public ILoadHandler LoadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILifeSpanHandler" /> and assign to handle events related to popups.
        /// </summary>
        /// <value>The life span handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDisplayHandler" /> and assign to handle events related to browser display state.
        /// </summary>
        /// <value>The display handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IDisplayHandler DisplayHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IContextMenuHandler" /> and assign to handle events related to the browser context menu
        /// </summary>
        /// <value>The menu handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IContextMenuHandler MenuHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IRenderProcessMessageHandler" /> and assign to handle messages from the render process.
        /// </summary>
        /// <value>The render process message handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IFindHandler" /> to handle events related to find results.
        /// </summary>
        /// <value>The find handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IFindHandler FindHandler { get; set; }

        /// <summary>
        /// The <see cref="IFocusHandler" /> for this ChromiumWebBrowser.
        /// </summary>
        /// <value>The focus handler.</value>
        /// <remarks>If you need customized focus handling behavior for WinForms, the suggested
        /// best practice would be to inherit from DefaultFocusHandler and try to avoid
        /// needing to override the logic in OnGotFocus. The implementation in
        /// DefaultFocusHandler relies on very detailed behavior of how WinForms and
        /// Windows interact during window activation.</remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IFocusHandler FocusHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDragHandler" /> and assign to handle events related to dragging.
        /// </summary>
        /// <value>The drag handler.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IDragHandler DragHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IResourceHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(null)]
        public IResourceHandlerFactory ResourceHandlerFactory { get; set; }
        
        /// <summary>
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> LoadError;
        /// <summary>
        /// Event handler that will get called when the browser begins loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method may not be called for a
        /// particular frame if the load request for that frame fails. For notification of overall browser load status use
        /// OnLoadingStateChange instead.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        /// <remarks>Whilst this may seem like a logical place to execute js, it's called before the DOM has been loaded, implement
        /// <see cref="IRenderProcessMessageHandler.OnContextCreated" /> as it's called when the underlying V8Context is created
        /// (Only called for the main frame at this stage)</remarks>
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        /// <summary>
        /// Event handler that will get called when the browser is done loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method will always be called
        /// for all frames irrespective of whether the request completes successfully.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        /// <summary>
        /// Event handler that will get called when the Loading state has changed.
        /// This event will be fired twice. Once when loading is initiated either programmatically or
        /// by user action, and once when loading is terminated due to completion, cancellation of failure.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        /// <summary>
        /// Event handler for changes to the status message.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang.
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        /// <summary>
        /// Occurs when the browser address changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        /// <summary>
        /// Occurs when the browser title changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<TitleChangedEventArgs> TitleChanged;
        /// <summary>
        /// Occurs when [is browser initialized changed].
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<IsBrowserInitializedChangedEventArgs> IsBrowserInitializedChanged;

        /// <summary>
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public bool CanGoForward { get; private set; }
        /// <summary>
        /// A flag that indicates whether the state of the control current supports the GoBack action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public bool CanGoBack { get; private set; }
        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public bool IsBrowserInitialized { get; private set; }

        /// <summary>
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public bool CanExecuteJavascriptInMainFrame { get; private set; }

        /// <summary>
        /// ParentFormMessageInterceptor hooks the Form handle and forwards
        /// the move/active messages to the browser, the default is true
        /// and should only be required when using <see cref="CefSettings.MultiThreadedMessageLoop"/>
        /// set to true.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public bool UseParentFormMessageInterceptor { get; set; } = true;

        /// <summary>
        /// Initializes static members of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        static ChromiumWebBrowser()
        {
            if (CefSharpSettings.ShutdownOnExit)
            {
                Application.ApplicationExit += OnApplicationExit;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ApplicationExit" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Cef.Shutdown();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// NOTE: Should only be used by the designer
        /// </summary>
        [Obsolete("Should only be used by the designer")]
        public ChromiumWebBrowser()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="requestContext">Request context that will be used for this browser instance,
        /// if null the Global Request Context will be used</param>
        public ChromiumWebBrowser(string address, IRequestContext requestContext = null)
        {
            Dock = DockStyle.Fill;
            Address = address;
            RequestContext = requestContext;

            InitializeFieldsAndCefIfRequired();
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsiquently throw an exception.
        /// TODO: Still not happy with this method name, need something better
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void InitializeFieldsAndCefIfRequired()
        {
            if (!initialized)
            {
                if (!Cef.IsInitialized && !Cef.Initialize(new CefSettings()))
                {
                    throw new InvalidOperationException("Cef::Initialize() failed");
                }

                Cef.AddDisposable(this);

                if (FocusHandler == null)
                {
                    FocusHandler = new DefaultFocusHandler();
                }

                if (ResourceHandlerFactory == null)
                {
                    ResourceHandlerFactory = new DefaultResourceHandlerFactory();
                }

                if (BrowserSettings == null)
                {
                    BrowserSettings = new BrowserSettings();
                }

                managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, false);

                initialized = true;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            IsBrowserInitialized = false;

            if(!designMode)
            {
                RemoveFromListOfCefBrowsers();
            }

            //The unmanaged resources should never be created in design mode, so only dispose when
            //at runtime
            if (disposing && !designMode)
            {
                FreeUnmanagedResources();
            }

            // Don't maintain a reference to event listeners anylonger:
            AddressChanged = null;
            ConsoleMessage = null;
            FrameLoadEnd = null;
            FrameLoadStart = null;
            IsBrowserInitializedChanged = null;
            LoadError = null;
            LoadingStateChanged = null;
            StatusMessage = null;
            TitleChanged = null;            

            // Release reference to handlers, make sure this is done after we dispose managedCefBrowserAdapter
            // otherwise the ILifeSpanHandler.DoClose will not be invoked.
            this.SetHandlersToNull();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsiquently throw an exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void RemoveFromListOfCefBrowsers()
        {
            Cef.RemoveDisposable(this);
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsiquently throw an exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void FreeUnmanagedResources()
        {
            browser = null;

            if (parentFormMessageInterceptor != null)
            {
                parentFormMessageInterceptor.Dispose();
                parentFormMessageInterceptor = null;
            }

            if (BrowserSettings != null)
            {
                BrowserSettings.Dispose();
                BrowserSettings = null;
            }

            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.Dispose();
                managedCefBrowserAdapter = null;
            }
        }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
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

        /// <summary>
        /// Registers a Javascript object in this specific browser instance.
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        /// <param name="options">binding options - camelCaseJavascriptNames default to true </param>
        /// <exception cref="System.Exception">Browser is already initialized. RegisterJsObject must be +
        ///                                     called before the underlying CEF browser is created.</exception>
        public void RegisterJsObject(string name, object objectToBind, BindingOptions options = null)
        {
            if (!CefSharpSettings.LegacyJavascriptBindingEnabled)
            {
                throw new Exception(@"CefSharpSettings.LegacyJavascriptBindingEnabled is currently false,
                                    for legacy binding you must set CefSharpSettings.LegacyJavascriptBindingEnabled = true
                                    before registering your first object see https://github.com/cefsharp/CefSharp/issues/2246
                                    for details on the new binding options. If you perform cross-site navigations bound objects will
                                    no longer be registered and you will have to migrate to the new method.");
            }

            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            
            InitializeFieldsAndCefIfRequired();

            //Enable WCF if not already enabled
            CefSharpSettings.WcfEnabled = true;

            var objectRepository = managedCefBrowserAdapter.JavascriptObjectRepository;

            if (objectRepository == null)
            {
                throw new Exception("Object Repository Null, Browser has likely been Disposed.");
            }

            objectRepository.Register(name, objectToBind, false, options);
        }

        /// <summary>
        /// <para>Asynchronously registers a Javascript object in this specific browser instance.</para>
        /// <para>Only methods of the object will be availabe.</para>
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        /// <param name="options">binding options - camelCaseJavascriptNames default to true </param>
        /// <exception cref="System.Exception">Browser is already initialized. RegisterJsObject must be +
        ///                                     called before the underlying CEF browser is created.</exception>
        /// <remarks>The registered methods can only be called in an async way, they will all return immeditaly and the resulting
        /// object will be a standard javascript Promise object which is usable to wait for completion or failure.</remarks>
        public void RegisterAsyncJsObject(string name, object objectToBind, BindingOptions options = null)
        {
            if (!CefSharpSettings.LegacyJavascriptBindingEnabled)
            {
                throw new Exception(@"CefSharpSettings.LegacyJavascriptBindingEnabled is currently false,
                                    for legacy binding you must set CefSharpSettings.LegacyJavascriptBindingEnabled = true
                                    before registering your first object see https://github.com/cefsharp/CefSharp/issues/2246
                                    for details on the new binding options. If you perform cross-site navigations bound objects will
                                    no longer be registered and you will have to migrate to the new method.");
            }

            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }

            InitializeFieldsAndCefIfRequired();

            var objectRepository = managedCefBrowserAdapter.JavascriptObjectRepository;

            if (objectRepository == null)
            {
                throw new Exception("Object Repository Null, Browser has likely been Disposed.");
            }

            objectRepository.Register(name, objectToBind, true, options);
        }

        public IJavascriptObjectRepository JavascriptObjectRepository
        {
            get { return managedCefBrowserAdapter == null ? null : managedCefBrowserAdapter.JavascriptObjectRepository; }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            designMode = DesignMode;

            if (!designMode)
            {
                InitializeFieldsAndCefIfRequired();

                // NOTE: Had to move the code out of this function otherwise the designer would crash
                CreateBrowser();

                ResizeBrowser();
            }

            base.OnHandleCreated(e);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void CreateBrowser()
        {
            browserCreated = true;

            if (((IWebBrowserInternal)this).HasParent == false)
            {
                if(IsBrowserInitialized == false || browser == null)
                { 
                    //TODO: Revert temp workaround for default url not loading
                    managedCefBrowserAdapter.CreateBrowser(BrowserSettings, (RequestContext)RequestContext, Handle, null);
                }
                else
                {
                    //If the browser already exists we'll reparent it to the new Handle
                    var browserHandle = browser.GetHost().GetWindowHandle();
                    NativeMethodWrapper.SetWindowParent(browserHandle, Handle);
                }
            }
        }

        /// <summary>
        /// Called after browser created.
        /// </summary>
        /// <param name="browser">The browser.</param>
        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            this.browser = browser;
            IsBrowserInitialized = true;

            //TODO: Revert temp workaround for default url not loading
            if(!string.IsNullOrEmpty(Address))
            { 
                browser.MainFrame.LoadUrl(Address);
            }

            // By the time this callback gets called, this control
            // is most likely hooked into a browser Form of some sort. 
            // (Which is what ParentFormMessageInterceptor relies on.)
            // Ensure the ParentFormMessageInterceptor construction occurs on the WinForms UI thread:
            if (UseParentFormMessageInterceptor)
            {
                this.InvokeOnUiThreadIfRequired(() =>
                {
                    parentFormMessageInterceptor = new ParentFormMessageInterceptor(this);
                });
            }

            ResizeBrowser();

            var handler = IsBrowserInitializedChanged;

            if (handler != null)
            {
                handler(this, new IsBrowserInitializedChangedEventArgs(IsBrowserInitialized));
            }
        }

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="args">The <see cref="AddressChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetAddress(AddressChangedEventArgs args)
        {
            Address = args.Address;

            var handler = AddressChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Sets the loading state change.
        /// </summary>
        /// <param name="args">The <see cref="LoadingStateChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="args">The <see cref="TitleChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetTitle(TitleChangedEventArgs args)
        {
            var handler = TitleChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="tooltipText">The tooltip text.</param>
        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadStart" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadStartEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadStart(FrameLoadStartEventArgs args)
        {
            var handler = FrameLoadStart;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadEnd" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadEndEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadEnd(FrameLoadEndEventArgs args)
        {
            var handler = FrameLoadEnd;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ConsoleMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ConsoleMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnConsoleMessage(ConsoleMessageEventArgs args)
        {
            var handler = ConsoleMessage;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:StatusMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="StatusMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnStatusMessage(StatusMessageEventArgs args)
        {
            var handler = StatusMessage;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:LoadError" /> event.
        /// </summary>
        /// <param name="args">The <see cref="LoadErrorEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnLoadError(LoadErrorEventArgs args)
        {
            var handler = LoadError;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetCanExecuteJavascriptOnMainFrame(bool canExecute)
        {
            CanExecuteJavascriptInMainFrame = canExecute;
        }

        /// <summary>
        /// Gets the browser adapter.
        /// </summary>
        /// <value>The browser adapter.</value>
        IBrowserAdapter IWebBrowserInternal.BrowserAdapter
        {
            get { return managedCefBrowserAdapter; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has parent.
        /// </summary>
        /// <value><c>true</c> if this instance has parent; otherwise, <c>false</c>.</value>
        bool IWebBrowserInternal.HasParent { get; set; }

        /// <summary>
        /// Manually implement Focused because cef does not implement it.
        /// </summary>
        /// <value><c>true</c> if focused; otherwise, <c>false</c>.</value>
        /// <remarks>This is also how the Microsoft's WebBrowserControl implements the Focused property.</remarks>
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

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (!designMode && initialized)
            {
                ResizeBrowser();
            }
        }

        /// <summary>
        /// Resizes the browser.
        /// </summary>
        private void ResizeBrowser()
        {
            if (IsBrowserInitialized)
            {
                managedCefBrowserAdapter.Resize(Width, Height);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (IsBrowserInitialized)
            {
                browser.GetHost().SetFocus(true);
            }

            base.OnGotFocus(e);
        }

        /// <summary>
        /// Returns the current IBrowser Instance
        /// </summary>
        /// <returns>browser instance or null</returns>
        public IBrowser GetBrowser()
        {
            this.ThrowExceptionIfBrowserNotInitialized();

            return browser;
        }

        /// <summary>
        /// Makes certain keys as Input keys when CefSettings.MultiThreadedMessageLoop = false
        /// </summary>
        /// <param name="keyData">key data</param>
        /// <returns>true for a select list of keys otherwise defers to base.IsInputKey</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            //This code block is only called/required when CEF is running in the
            //same message loop as the WinForms UI (CefSettings.MultiThreadedMessageLoop = false)
            //Without this code, arrows and tab won't be processed
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Tab:
                {
                    return true;
                }
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                {
                    return true;
                }
            }

            return base.IsInputKey(keyData);
        }
    }
}
