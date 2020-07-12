// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using CefSharp.Internals;
using CefSharp.Web;
using CefSharp.WinForms.Internals;

namespace CefSharp.WinForms
{
    /// <summary>
    /// ChromiumWebBrowser is the WinForms web browser control
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="CefSharp.WinForms.IWinFormsWebBrowser" />
    [Docking(DockingBehavior.AutoDock), DefaultEvent("LoadingStateChanged"), ToolboxBitmap(typeof(ChromiumWebBrowser)),
    Description("CefSharp ChromiumWebBrowser - Chromium Embedded Framework .Net wrapper. https://github.com/cefsharp/CefSharp"),
    Designer(typeof(ChromiumWebBrowserDesigner))]
    public class ChromiumWebBrowser : Control, IWebBrowserInternal, IWinFormsWebBrowser
    {
        //TODO: If we start adding more consts then extract them into a common class
        //Possibly in the CefSharp assembly and move the WPF ones into there as well.
        private const uint WS_EX_NOACTIVATE = 0x08000000;

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
        /// A flag indicating if the <see cref="Address"/> was used when calling CreateBrowser
        /// If false and <see cref="Address"/> contains a non empty string Load will be called
        /// on the main frame
        /// </summary>
        private bool initialAddressLoaded;
        /// <summary>
        /// If true the the WS_EX_NOACTIVATE style will be removed so that future mouse clicks
        /// inside the browser correctly activate and focus the window.
        /// </summary>
        private bool removeExNoActivateStyle;
        /// <summary>
        /// Browser initialization settings
        /// </summary>
        private IBrowserSettings browserSettings;
        /// <summary>
        /// The request context (we deliberately use a private variable so we can throw an exception if
        /// user attempts to set after browser created)
        /// </summary>
        private IRequestContext requestContext;

        /// <summary>
        /// The value for disposal, if it's 1 (one) then this instance is either disposed
        /// or in the process of getting disposed
        /// </summary>
        private int disposeSignaled;

        /// <summary>
        /// Parking control used to temporarily host the CefBrowser instance
        /// when <see cref="RecreatingHandle"/> is <c>true</c>.
        /// </summary>
        private Control parkingControl;

        /// <summary>
        /// Used as workaround for issue https://github.com/cefsharp/CefSharp/issues/3021
        /// </summary>
        private long canExecuteJavascriptInMainFrameId;

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><see langword="true" /> if this instance is disposed; otherwise, <see langword="false" />.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public new bool IsDisposed
        {
            get
            {
                return Interlocked.CompareExchange(ref disposeSignaled, 1, 1) == 1;
            }
        }

        /// <summary>
        /// Set to true while handing an activating WM_ACTIVATE message.
        /// MUST ONLY be cleared by DefaultFocusHandler.
        /// </summary>
        /// <value><c>true</c> if this instance is activating; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsActivating { get; set; }

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IBrowserSettings BrowserSettings
        {
            get
            {
                //We keep a reference to the browserSettings for the case where
                //the Control Handle is destroyed then Created see https://github.com/cefsharp/CefSharp/issues/2840
                //As it's not possible to change settings after the browser has been
                //created, and changing browserSettings then creating a new handle will
                //give a subtle different user experience if you aren't expecting it we
                //return null here even though we still have a reference.
                if (browserCreated)
                {
                    return null;
                }
                return browserSettings;
            }
            set
            {
                if (browserCreated)
                {
                    throw new Exception("Browser has already been created. BrowserSettings must be " +
                                        "set before the underlying CEF browser is created.");
                }
                if (value != null && value.GetType() != typeof(BrowserSettings))
                {
                    throw new Exception(string.Format("BrowserSettings can only be of type {0} or null", typeof(BrowserSettings)));
                }
                browserSettings = value;
            }
        }
        /// <summary>
        /// Activates browser upon creation, the default value is false. Prior to version 73
        /// the default behaviour was to activate browser on creation (Equivilent of setting this property to true).
        /// To restore this behaviour set this value to true immediately after you create the <see cref="ChromiumWebBrowser"/> instance.
        /// https://bitbucket.org/chromiumembedded/cef/issues/1856/branch-2526-cef-activates-browser-window
        /// </summary>
        public bool ActivateBrowserOnCreation { get; set; }
        /// <summary>
        /// Gets or sets the request context.
        /// </summary>
        /// <value>The request context.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IRequestContext RequestContext
        {
            get { return requestContext; }
            set
            {
                if (browserCreated)
                {
                    throw new Exception("Browser has already been created. RequestContext must be " +
                                        "set before the underlying CEF browser is created.");
                }
                if (value != null && value.GetType() != typeof(RequestContext))
                {
                    throw new Exception(string.Format("RequestContext can only be of type {0} or null", typeof(RequestContext)));
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsLoading { get; private set; }
        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        /// <value>The tooltip text.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public string TooltipText { get; private set; }
        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <value>The address.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public string Address { get; private set; }

        /// <summary>
        /// Implement <see cref="IDialogHandler" /> and assign to handle dialog events.
        /// </summary>
        /// <value>The dialog handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDialogHandler DialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IJsDialogHandler" /> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        /// <value>The js dialog handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IJsDialogHandler JsDialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IKeyboardHandler" /> and assign to handle events related to key press.
        /// </summary>
        /// <value>The keyboard handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IKeyboardHandler KeyboardHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IRequestHandler" /> and assign to handle events related to browser requests.
        /// </summary>
        /// <value>The request handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IRequestHandler RequestHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDownloadHandler" /> and assign to handle events related to downloading files.
        /// </summary>
        /// <value>The download handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDownloadHandler DownloadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILoadHandler" /> and assign to handle events related to browser load status.
        /// </summary>
        /// <value>The load handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public ILoadHandler LoadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILifeSpanHandler" /> and assign to handle events related to popups.
        /// </summary>
        /// <value>The life span handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDisplayHandler" /> and assign to handle events related to browser display state.
        /// </summary>
        /// <value>The display handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDisplayHandler DisplayHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IContextMenuHandler" /> and assign to handle events related to the browser context menu
        /// </summary>
        /// <value>The menu handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IContextMenuHandler MenuHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IRenderProcessMessageHandler" /> and assign to handle messages from the render process.
        /// </summary>
        /// <value>The render process message handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IFindHandler" /> to handle events related to find results.
        /// </summary>
        /// <value>The find handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IFindHandler FindHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IAudioHandler" /> to handle audio events.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IAudioHandler AudioHandler { get; set; }
        /// <summary>
        /// The <see cref="IFocusHandler" /> for this ChromiumWebBrowser.
        /// </summary>
        /// <value>The focus handler.</value>
        /// <remarks>If you need customized focus handling behavior for WinForms, the suggested
        /// best practice would be to inherit from DefaultFocusHandler and try to avoid
        /// needing to override the logic in OnGotFocus. The implementation in
        /// DefaultFocusHandler relies on very detailed behavior of how WinForms and
        /// Windows interact during window activation.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IFocusHandler FocusHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDragHandler" /> and assign to handle events related to dragging.
        /// </summary>
        /// <value>The drag handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDragHandler DragHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IResourceRequestHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IResourceRequestHandlerFactory ResourceRequestHandlerFactory { get; set; }

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
        /// </remarks>
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
        /// Event called after the underlying CEF browser instance has been created. 
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler IsBrowserInitializedChanged;

        /// <summary>
        /// Event handler that will get called when the message that originates from CefSharp.PostMessage
        /// </summary>
        public event EventHandler<JavascriptMessageReceivedEventArgs> JavascriptMessageReceived;

        /// <summary>
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool CanGoForward { get; private set; }
        /// <summary>
        /// A flag that indicates whether the state of the control current supports the GoBack action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool CanGoBack { get; private set; }
        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsBrowserInitialized { get; private set; }

        /// <summary>
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool CanExecuteJavascriptInMainFrame { get; private set; }

        /// <summary>
        /// ParentFormMessageInterceptor hooks the Form handle and forwards
        /// the move/active messages to the browser, the default is true
        /// and should only be required when using <see cref="CefSettingsBase.MultiThreadedMessageLoop"/>
        /// set to true.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(true)]
        public bool UseParentFormMessageInterceptor { get; set; } = true;

        /// <summary>
        /// By default when <see cref="Control.OnHandleDestroyed(EventArgs)"/> is called
        /// the underlying Browser Hwnd is only parked (moved to a temp parent) 
        /// when <see cref="Control.RecreatingHandle"/> is <c>true</c>, there are a few other
        /// cases where parking of the control is desired, you can force parking by setting
        /// this property to <c>true</c>.
        /// </summary>
        /// <remarks>
        /// You may wish to set this property to <c>true</c> when using the browser in conjunction
        /// with https://github.com/dockpanelsuite/dockpanelsuite
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(true)]
        public bool ParkControlOnHandleDestroyed { get; set; } = false;

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
        /// This constructor exists as the WinForms designer requires a parameterless constructor, if you are instantiating
        /// an instance of this class in code then use the <see cref="ChromiumWebBrowser(string, IRequestContext)"/>
        /// constructor overload instead. Using this constructor in code is unsupported and you may experience <see cref="NullReferenceException"/>'s
        /// when attempting to access some of the properties immediately after instantiation. 
        /// </summary>
        [Obsolete("Should only be used by the WinForms Designer. Use the ChromiumWebBrowser(string, IRequestContext) constructor overload instead.")]
        public ChromiumWebBrowser()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// **Important** - When using this constructor the <see cref="Control.Dock"/> property
        /// will default to <see cref="DockStyle.Fill"/>.
        /// </summary>
        /// <param name="html">html string to be initially loaded in the browser.</param>
        /// <param name="requestContext">(Optional) Request context that will be used for this browser instance, if null the Global
        /// Request Context will be used.</param>
        public ChromiumWebBrowser(HtmlString html, IRequestContext requestContext = null) : this(html.ToDataUriString(), requestContext)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// **Important** - When using this constructor the <see cref="Control.Dock"/> property
        /// will default to <see cref="DockStyle.Fill"/>.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="requestContext">(Optional) Request context that will be used for this browser instance, if null the Global
        /// Request Context will be used.</param>
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
                    //If the WinForms UI thread and the CEF UI thread are one in the same
                    //then we don't need the FocusHandler, it's only required when using
                    //MultiThreadedMessageLoop (the default)
                    if (!Cef.CurrentlyOnThread(CefThreadIds.TID_UI))
                    {
                        FocusHandler = new DefaultFocusHandler();
                    }
                }

                if (browserSettings == null)
                {
                    browserSettings = new BrowserSettings(frameworkCreated: true);
                }

                managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, false);

                initialized = true;
            }
        }

        /// <summary>
        /// If not in design mode; Releases unmanaged and - optionally - managed resources for the <see cref="ChromiumWebBrowser"/>
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Attempt to move the disposeSignaled state from 0 to 1. If successful, we can be assured that
            // this thread is the first thread to do so, and can safely dispose of the object.
            if (Interlocked.CompareExchange(ref disposeSignaled, 1, 0) != 0)
            {
                return;
            }

            if (!designMode)
            {
                InternalDispose(disposing);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources for the <see cref="ChromiumWebBrowser"/>
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        /// <remarks>
        /// This method cannot be inlined as the designer will attempt to load libcef.dll and will subsiquently throw an exception.
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void InternalDispose(bool disposing)
        {
            if (disposing)
            {
                CanExecuteJavascriptInMainFrame = false;
                IsBrowserInitialized = false;

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
                JavascriptMessageReceived = null;

                // Release reference to handlers, except LifeSpanHandler which is done after Disposing
                // ManagedCefBrowserAdapter otherwise the ILifeSpanHandler.DoClose will not be invoked.
                this.SetHandlersToNullExceptLifeSpan();

                browser = null;

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

                //Dispose of BrowserSettings if we created it, if user created then they're responsible
                if (browserSettings != null && browserSettings.FrameworkCreated)
                {
                    browserSettings.Dispose();
                }

                browserSettings = null;

                parkingControl?.Dispose();
                parkingControl = null;

                // LifeSpanHandler is set to null after managedCefBrowserAdapter.Dispose so ILifeSpanHandler.DoClose
                // is called.
                LifeSpanHandler = null;
            }

            Cef.RemoveDisposable(this);
        }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        public void Load(string url)
        {
            if (IsBrowserInitialized)
            {
                using (var frame = this.GetMainFrame())
                {
                    frame.LoadUrl(url);
                }
            }
            else
            {
                Address = url;
            }
        }

        /// <summary>
        /// The javascript object repository, one repository per ChromiumWebBrowser instance.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IJavascriptObjectRepository JavascriptObjectRepository
        {
            get
            {
                InitializeFieldsAndCefIfRequired();
                return managedCefBrowserAdapter == null ? null : managedCefBrowserAdapter.JavascriptObjectRepository;
            }
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

                ResizeBrowser(Width, Height);
            }

            base.OnHandleCreated(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            //When the Control is being Recreated then we'll park
            //the browser (set to a temp parent) and assign to
            //our new handle when it's ready.
            if (RecreatingHandle || ParkControlOnHandleDestroyed)
            {
                parkingControl = new Control();
                parkingControl.CreateControl();

                var host = this.GetBrowserHost();
                var hwnd = host.GetWindowHandle();

                NativeMethodWrapper.SetWindowParent(hwnd, parkingControl.Handle);
            }

            base.OnHandleDestroyed(e);
        }

        /// <summary>
        /// Override this method to handle creation of WindowInfo. This method can be used to customise aspects of
        /// browser creation including configuration of settings such as <see cref="IWindowInfo.ExStyle"/>.
        /// Window Activation is disabled by default, you can re-enable it by overriding and removing the
        /// WS_EX_NOACTIVATE style from <see cref="IWindowInfo.ExStyle"/>.
        /// </summary>
        /// <param name="handle">Window handle for the Control</param>
        /// <returns>Window Info</returns>
        /// <example>
        /// To re-enable Window Activation then remove WS_EX_NOACTIVATE from ExStyle
        /// <code>
        /// const uint WS_EX_NOACTIVATE = 0x08000000;
        /// windowInfo.ExStyle &amp;= ~WS_EX_NOACTIVATE;
        ///</code>
        /// </example>
        protected virtual IWindowInfo CreateBrowserWindowInfo(IntPtr handle)
        {
            var windowInfo = new WindowInfo();
            windowInfo.SetAsChild(handle);

            if (!ActivateBrowserOnCreation)
            {
                //Disable Window activation by default
                //https://bitbucket.org/chromiumembedded/cef/issues/1856/branch-2526-cef-activates-browser-window
                windowInfo.ExStyle |= WS_EX_NOACTIVATE;
            }

            return windowInfo;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void CreateBrowser()
        {
            browserCreated = true;

            if (((IWebBrowserInternal)this).HasParent == false)
            {
                //If we are Recreating our handle we will have re-parented our
                //browser to parkingControl. We'll assign the browser to our newly
                //created handle now.
                if ((RecreatingHandle || ParkControlOnHandleDestroyed) && IsBrowserInitialized && browser != null)
                {
                    var host = this.GetBrowserHost();
                    var hwnd = host.GetWindowHandle();

                    NativeMethodWrapper.SetWindowParent(hwnd, Handle);

                    parkingControl.Dispose();
                    parkingControl = null;
                }
                else
                {
                    var windowInfo = CreateBrowserWindowInfo(Handle);

                    //We actually check if WS_EX_NOACTIVATE was set for instances
                    //the user has override CreateBrowserWindowInfo and not called base.CreateBrowserWindowInfo
                    removeExNoActivateStyle = (windowInfo.ExStyle & WS_EX_NOACTIVATE) == WS_EX_NOACTIVATE;

                    initialAddressLoaded = !string.IsNullOrEmpty(Address);

                    managedCefBrowserAdapter.CreateBrowser(windowInfo, browserSettings as BrowserSettings, requestContext as RequestContext, Address);
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

            ResizeBrowser(Width, Height);

            //If Load was called after the call to CreateBrowser we'll call Load
            //on the MainFrame
            if (!initialAddressLoaded && !string.IsNullOrEmpty(Address))
            {
                browser.MainFrame.LoadUrl(Address);
            }

            IsBrowserInitializedChanged?.Invoke(this, EventArgs.Empty);
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

            if (removeExNoActivateStyle && browser != null)
            {
                removeExNoActivateStyle = false;

                var host = this.GetBrowserHost();
                var hwnd = host.GetWindowHandle();
                //Remove the WS_EX_NOACTIVATE style so that future mouse clicks inside the
                //browser correctly activate and focus the browser. 
                //https://github.com/chromiumembedded/cef/blob/9df4a54308a88fd80c5774d91c62da35afb5fd1b/tests/cefclient/browser/root_window_win.cc#L1088
                NativeMethodWrapper.RemoveExNoActivateStyle(hwnd);
            }

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

        void IWebBrowserInternal.SetCanExecuteJavascriptOnMainFrame(long frameId, bool canExecute)
        {
            //When loading pages of a different origin the frameId changes
            //For the first loading of a new origin the messages from the render process
            //Arrive in a different order than expected, the OnContextCreated message
            //arrives before the OnContextReleased, then the message for OnContextReleased
            //incorrectly overrides the value
            //https://github.com/cefsharp/CefSharp/issues/3021

            if (frameId > canExecuteJavascriptInMainFrameId && !canExecute)
            {
                return;
            }

            canExecuteJavascriptInMainFrameId = frameId;
            CanExecuteJavascriptInMainFrame = canExecute;
        }

        void IWebBrowserInternal.SetJavascriptMessageReceived(JavascriptMessageReceivedEventArgs args)
        {
            JavascriptMessageReceived?.Invoke(this, args);
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
                ResizeBrowser(Width, Height);
            }
        }

        /// <summary>
        /// Resizes the browser.
        /// </summary>
        private void ResizeBrowser(int width, int height)
        {
            if (IsBrowserInitialized)
            {
                managedCefBrowserAdapter.Resize(width, height);
            }
        }

        /// <summary>
        /// When minimized set the browser window size to 0x0 to reduce resource usage.
        /// https://github.com/chromiumembedded/cef/blob/c7701b8a6168f105f2c2d6b239ce3958da3e3f13/tests/cefclient/browser/browser_window_std_win.cc#L87
        /// </summary>
        internal void HideInternal()
        {
            ResizeBrowser(0, 0);
        }

        /// <summary>
        /// Show the browser (called after previous minimised)
        /// </summary>
        internal void ShowInternal()
        {
            ResizeBrowser(Width, Height);
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
            this.ThrowExceptionIfDisposed();
            this.ThrowExceptionIfBrowserNotInitialized();

            return browser;
        }

        /// <summary>
        /// Gets the default size of the control.
        /// </summary>
        /// <value>
        /// The default <see cref="T:System.Drawing.Size" /> of the control.
        /// </value>
        protected override Size DefaultSize
        {
            get { return new Size(200, 100); }
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
                case Keys.Shift | Keys.Tab:
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
