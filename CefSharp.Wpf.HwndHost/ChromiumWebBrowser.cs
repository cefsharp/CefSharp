// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using CefSharp.Internals;
using CefSharp.Wpf.HwndHost.Internals;

namespace CefSharp.Wpf.HwndHost
{
    /// <summary>
    /// ChromiumWebBrowser is the WPF web browser control
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    /// <seealso cref="CefSharp.Wpf.HwndHost.IWpfWebBrowser" />
    /// based on https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/walkthrough-hosting-a-win32-control-in-wpf
    /// and https://stackoverflow.com/questions/6500336/custom-dwm-drawn-window-frame-flickers-on-resizing-if-the-window-contains-a-hwnd/17471534#17471534
    public class ChromiumWebBrowser : System.Windows.Interop.HwndHost, IWpfWebBrowser
    {
        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateWindowEx(int dwExStyle,
                                              string lpszClassName,
                                              string lpszWindowName,
                                              int style,
                                              int x, int y,
                                              int width, int height,
                                              IntPtr hwndParent,
                                              IntPtr hMenu,
                                              IntPtr hInst,
                                              [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        private static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int index, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int index, IntPtr dwNewLong);

        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlongptra
        //SetWindowLongPtr for x64, SetWindowLong for x86
        private void RemoveExNoActivateStyle(IntPtr hwnd)
        {
            var exStyle = GetWindowLongPtr(hwnd, GWL_EXSTYLE);

            if (IntPtr.Size == 8)
            {
                if ((exStyle.ToInt64() & WS_EX_NOACTIVATE) == WS_EX_NOACTIVATE)
                {
                    exStyle = new IntPtr(exStyle.ToInt64() & ~WS_EX_NOACTIVATE);
                    //Remove WS_EX_NOACTIVATE
                    SetWindowLongPtr64(new HandleRef(this, hwnd), GWL_EXSTYLE, exStyle);
                }
            }
            else
            {
                if ((exStyle.ToInt32() & WS_EX_NOACTIVATE) == WS_EX_NOACTIVATE)
                {
                    //Remove WS_EX_NOACTIVATE
                    SetWindowLong32(new HandleRef(this, hwnd), GWL_EXSTYLE, (int)(exStyle.ToInt32() & ~WS_EX_NOACTIVATE));
                }
            }
        }

        private const int WS_CHILD = 0x40000000,
            WS_VISIBLE = 0x10000000,
            LBS_NOTIFY = 0x00000001,
            HOST_ID = 0x00000002,
            LISTBOX_ID = 0x00000001,
            WS_VSCROLL = 0x00200000,
            WS_BORDER = 0x00800000,
            WS_CLIPCHILDREN = 0x02000000;

        private const uint WS_EX_NOACTIVATE = 0x08000000;
        private const int GWL_EXSTYLE = -20;

        /// <summary>
        /// Handle we'll use to host the browser
        /// </summary>
        private IntPtr hwndHost;
        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private IBrowserAdapter managedCefBrowserAdapter;
        /// <summary>
        /// The ignore URI change
        /// </summary>
        private bool ignoreUriChange;
        /// <summary>
        /// Initial address
        /// </summary>
        private readonly string initialAddress;
        /// <summary>
        /// Has the underlying Cef Browser been created (slightly different to initliazed in that
        /// the browser is initialized in an async fashion)
        /// </summary>
        private bool browserCreated;
        /// <summary>
        /// The browser initialized - boolean represented as 0 (false) and 1(true) as we use Interlocker to increment/reset
        /// </summary>
        private int browserInitialized;
        /// <summary>
        /// The browser
        /// </summary>
        private IBrowser browser;
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
        /// A flag that indicates whether or not the designer is active
        /// NOTE: Needs to be static for OnApplicationExit
        /// </summary>
        private static bool DesignMode;

        /// <summary>
        /// The value for disposal, if it's 1 (one) then this instance is either disposed
        /// or in the process of getting disposed
        /// </summary>
        private int disposeSignaled;

        /// <summary>
        /// If true the the WS_EX_NOACTIVATE style will be removed so that future mouse clicks
        /// inside the browser correctly activate and focus the window.
        /// </summary>
        private bool removeExNoActivateStyle;

        /// <summary>
        /// Current DPI Scale
        /// </summary>
        private double dpiScale;

        /// <summary>
        /// The HwndSource RootVisual (Window) - We store a reference
        /// to unsubscribe event handlers
        /// </summary>
        private Window sourceWindow;

        /// <summary>
        /// Store the previous window state, used to determine if the
        /// Windows was previous <see cref="WindowState.Minimized"/>
        /// and resume rendering
        /// </summary>
        private WindowState previousWindowState;

        /// <summary>
        /// This flag is set when the browser gets focus before the underlying CEF browser
        /// has been initialized.
        /// </summary>
        private bool initialFocus;

        /// <summary>
        /// Activates browser upon creation, the default value is false. Prior to version 73
        /// the default behaviour was to activate browser on creation (Equivilent of setting this property to true).
        /// To restore this behaviour set this value to true immediately after you create the <see cref="ChromiumWebBrowser"/> instance.
        /// https://bitbucket.org/chromiumembedded/cef/issues/1856/branch-2526-cef-activates-browser-window
        /// </summary>
        public bool ActivateBrowserOnCreation { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><see langword="true" /> if this instance is disposed; otherwise, <see langword="false" />.</value>
        public bool IsDisposed
        {
            get
            {
                return Interlocked.CompareExchange(ref disposeSignaled, 1, 1) == 1;
            }
        }

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        public IBrowserSettings BrowserSettings
        {
            get { return browserSettings; }
            set
            {
                if (browserCreated)
                {
                    throw new Exception("Browser has already been created. BrowserSettings must be " +
                                        "set before the underlying CEF browser is created.");
                }

                //New instance is created in the constructor, if you use
                //xaml to initialize browser settings then it will also create a new
                //instance, so we dispose of the old one
                if (browserSettings != null && browserSettings.AutoDispose)
                {
                    browserSettings.Dispose();
                }

                browserSettings = value;
            }
        }
        /// <summary>
        /// Gets or sets the request context.
        /// </summary>
        /// <value>The request context.</value>
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
                    throw new Exception(string.Format("RequestContxt can only be of type {0} or null", typeof(RequestContext)));
                }
                requestContext = value;
            }
        }
        /// <summary>
        /// Implement <see cref="IDialogHandler" /> and assign to handle dialog events.
        /// </summary>
        /// <value>The dialog handler.</value>
        public IDialogHandler DialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IJsDialogHandler" /> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        /// <value>The js dialog handler.</value>
        public IJsDialogHandler JsDialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IKeyboardHandler" /> and assign to handle events related to key press.
        /// </summary>
        /// <value>The keyboard handler.</value>
        public IKeyboardHandler KeyboardHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IRequestHandler" /> and assign to handle events related to browser requests.
        /// </summary>
        /// <value>The request handler.</value>
        public IRequestHandler RequestHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDownloadHandler" /> and assign to handle events related to downloading files.
        /// </summary>
        /// <value>The download handler.</value>
        public IDownloadHandler DownloadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILoadHandler" /> and assign to handle events related to browser load status.
        /// </summary>
        /// <value>The load handler.</value>
        public ILoadHandler LoadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILifeSpanHandler" /> and assign to handle events related to popups.
        /// </summary>
        /// <value>The life span handler.</value>
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDisplayHandler" /> and assign to handle events related to browser display state.
        /// </summary>
        /// <value>The display handler.</value>
        public IDisplayHandler DisplayHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IContextMenuHandler" /> and assign to handle events related to the browser context menu
        /// </summary>
        /// <value>The menu handler.</value>
        public IContextMenuHandler MenuHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IFocusHandler" /> and assign to handle events related to the browser component's focus
        /// </summary>
        /// <value>The focus handler.</value>
        public IFocusHandler FocusHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDragHandler" /> and assign to handle events related to dragging.
        /// </summary>
        /// <value>The drag handler.</value>
        public IDragHandler DragHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IResourceRequestHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        public IResourceRequestHandlerFactory ResourceRequestHandlerFactory { get; set; }
        /// <summary>
        /// Implement <see cref="IRenderProcessMessageHandler" /> and assign to handle messages from the render process.
        /// </summary>
        /// <value>The render process message handler.</value>
        public IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IFindHandler" /> to handle events related to find results.
        /// </summary>
        /// <value>The find handler.</value>
        public IFindHandler FindHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IAudioHandler" /> to handle audio events.
        /// </summary>
        public IAudioHandler AudioHandler { get; set; }

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
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> LoadError;

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
        /// Event handler that will get called when the message that originates from CefSharp.PostMessage
        /// </summary>
        public event EventHandler<JavascriptMessageReceivedEventArgs> JavascriptMessageReceived;

        /// <summary>
        /// Navigates to the previous page in the browser history. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        /// <value>The back command.</value>
        public ICommand BackCommand { get; private set; }
        /// <summary>
        /// Navigates to the next page in the browser history. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        /// <value>The forward command.</value>
        public ICommand ForwardCommand { get; private set; }
        /// <summary>
        /// Reloads the content of the current page. Will automatically be enabled/disabled depending on the browser state.
        /// </summary>
        /// <value>The reload command.</value>
        public ICommand ReloadCommand { get; private set; }
        /// <summary>
        /// Prints the current browser contents.
        /// </summary>
        /// <value>The print command.</value>
        public ICommand PrintCommand { get; private set; }
        /// <summary>
        /// Increases the zoom level.
        /// </summary>
        /// <value>The zoom in command.</value>
        public ICommand ZoomInCommand { get; private set; }
        /// <summary>
        /// Decreases the zoom level.
        /// </summary>
        /// <value>The zoom out command.</value>
        public ICommand ZoomOutCommand { get; private set; }
        /// <summary>
        /// Resets the zoom level to the default. (100%)
        /// </summary>
        /// <value>The zoom reset command.</value>
        public ICommand ZoomResetCommand { get; private set; }
        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        /// <value>The view source command.</value>
        public ICommand ViewSourceCommand { get; private set; }
        /// <summary>
        /// Command which cleans up the Resources used by the ChromiumWebBrowser
        /// </summary>
        /// <value>The cleanup command.</value>
        public ICommand CleanupCommand { get; private set; }
        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        /// <value>The stop command.</value>
        public ICommand StopCommand { get; private set; }
        /// <summary>
        /// Cut selected text to the clipboard.
        /// </summary>
        /// <value>The cut command.</value>
        public ICommand CutCommand { get; private set; }
        /// <summary>
        /// Copy selected text to the clipboard.
        /// </summary>
        /// <value>The copy command.</value>
        public ICommand CopyCommand { get; private set; }
        /// <summary>
        /// Paste text from the clipboard.
        /// </summary>
        /// <value>The paste command.</value>
        public ICommand PasteCommand { get; private set; }
        /// <summary>
        /// Select all text.
        /// </summary>
        /// <value>The select all command.</value>
        public ICommand SelectAllCommand { get; private set; }
        /// <summary>
        /// Undo last action.
        /// </summary>
        /// <value>The undo command.</value>
        public ICommand UndoCommand { get; private set; }
        /// <summary>
        /// Redo last action.
        /// </summary>
        /// <value>The redo command.</value>
        public ICommand RedoCommand { get; private set; }

        /// <summary>
        /// Used as workaround for issue https://github.com/cefsharp/CefSharp/issues/3021
        /// </summary>
        private long canExecuteJavascriptInMainFrameId;

        /// <summary>
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        public bool CanExecuteJavascriptInMainFrame { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        //TODO: Currently only a single Dispatcher is supported, all controls will
        //be Shutdown on that dispatcher which may not actually be the correct Dispatcher.
        //We could potentially use a ThreadStatic variable to implement this behaviour
        //and support multiple dispatchers.
        static ChromiumWebBrowser()
        {
            if (CefSharpSettings.ShutdownOnExit)
            {
                //Use Dispatcher.FromThread as it returns null if no dispatcher
                //is available for this thread.
                var dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
                if (dispatcher == null)
                {
                    //No dispatcher then we'll rely on Application.Exit
                    var app = Application.Current;

                    if (app != null)
                    {
                        app.Exit += OnApplicationExit;
                    }
                }
                else
                {
                    dispatcher.ShutdownStarted += DispatcherShutdownStarted;
                    dispatcher.ShutdownFinished += DispatcherShutdownFinished;
                }
            }
        }

        /// <summary>
        /// Handles Dispatcher Shutdown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventargs</param>
        private static void DispatcherShutdownStarted(object sender, EventArgs e)
        {
            var dispatcher = (Dispatcher)sender;

            dispatcher.ShutdownStarted -= DispatcherShutdownStarted;

            if (!DesignMode)
            {
                CefPreShutdown();
            }
        }

        private static void DispatcherShutdownFinished(object sender, EventArgs e)
        {
            var dispatcher = (Dispatcher)sender;

            dispatcher.ShutdownFinished -= DispatcherShutdownFinished;

            if (!DesignMode)
            {
                CefShutdown();
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ApplicationExit" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExitEventArgs"/> instance containing the event data.</param>
        private static void OnApplicationExit(object sender, ExitEventArgs e)
        {
            if (!DesignMode)
            {
                CefShutdown();
            }
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsequently throw an exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CefPreShutdown()
        {
            Cef.PreShutdown();
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsiquently throw an exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CefShutdown()
        {
            Cef.Shutdown();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cef::Initialize() failed</exception>
        public ChromiumWebBrowser()
        {
            DesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);

            if (!DesignMode)
            {
                NoInliningConstructor();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> instance.
        /// </summary>
        /// <param name="initialAddress">address to load initially</param>
        public ChromiumWebBrowser(string initialAddress)
        {
            this.initialAddress = initialAddress;

            NoInliningConstructor();
        }

        /// <summary>
        /// Constructor logic has been moved into this method
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsiquently throw an exception.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void NoInliningConstructor()
        {
            //Initialize CEF if it hasn't already been initialized
            if (!Cef.IsInitialized)
            {
                var settings = new CefSettings();

                if (!Cef.Initialize(settings))
                {
                    throw new InvalidOperationException("Cef::Initialize() failed");
                }
            }

            //Add this ChromiumWebBrowser instance to a list of IDisposable objects
            // that if still alive at the time Cef.Shutdown is called will be disposed of
            // It's important all browser instances be freed before Cef.Shutdown is called.
            Cef.AddDisposable(this);
            Focusable = true;
            FocusVisualStyle = null;

            WebBrowser = this;

            SizeChanged += OnSizeChanged;
            IsVisibleChanged += OnIsVisibleChanged;

            BackCommand = new DelegateCommand(this.Back, () => CanGoBack);
            ForwardCommand = new DelegateCommand(this.Forward, () => CanGoForward);
            ReloadCommand = new DelegateCommand(this.Reload, () => !IsLoading);
            PrintCommand = new DelegateCommand(this.Print);
            ZoomInCommand = new DelegateCommand(ZoomIn);
            ZoomOutCommand = new DelegateCommand(ZoomOut);
            ZoomResetCommand = new DelegateCommand(ZoomReset);
            ViewSourceCommand = new DelegateCommand(this.ViewSource);
            CleanupCommand = new DelegateCommand(Dispose);
            StopCommand = new DelegateCommand(this.Stop);
            CutCommand = new DelegateCommand(this.Cut);
            CopyCommand = new DelegateCommand(this.Copy);
            PasteCommand = new DelegateCommand(this.Paste);
            SelectAllCommand = new DelegateCommand(this.SelectAll);
            UndoCommand = new DelegateCommand(this.Undo);
            RedoCommand = new DelegateCommand(this.Redo);

            managedCefBrowserAdapter = ManagedCefBrowserAdapter.Create(this, false);

            browserSettings = new BrowserSettings(autoDispose:true);

            PresentationSource.AddSourceChangedHandler(this, PresentationSourceChangedHandler);

            FocusHandler = new FocusHandler();

            UseLayoutRounding = true;
        }

        private void PresentationSourceChangedHandler(object sender, SourceChangedEventArgs args)
        {
            if (args.NewSource != null)
            {
                var source = (HwndSource)args.NewSource;

                var matrix = source.CompositionTarget.TransformToDevice;

                dpiScale = matrix.M11;

                //TODO:
                var window = source.RootVisual as Window;
                if (window != null)
                {
                    window.StateChanged += OnWindowStateChanged;
                    window.LocationChanged += OnWindowLocationChanged;
                    sourceWindow = window;

                    // If CleanupElement is null, set the CleanupElement to the new window that the browser is moved in.
                    //TODO: Test and uncomment this
                    //if (CleanupElement == null)
                    //{
                    //    CleanupElement = window;
                    //}
                }
            }
            else if (args.OldSource != null)
            {
                //TODO:
                var window = args.OldSource.RootVisual as Window;
                if (window != null)
                {
                    window.StateChanged -= OnWindowStateChanged;
                    window.LocationChanged -= OnWindowLocationChanged;
                    sourceWindow = null;

                    // If CleanupElement is the old Window that the browser is moved out of, set CleanupElement to null.
                    //TODO: Test and uncomment this
                    //if (CleanupElement == window)
                    //{
                    //    CleanupElement = null;
                    //}
                }
            }
        }

        ///<inheritdoc/>
        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            dpiScale = newDpi.DpiScaleX;

            //If the DPI changed then we need to resize.
            ResizeBrowser((int)ActualWidth, (int)ActualHeight);

            base.OnDpiChanged(oldDpi, newDpi);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeBrowser((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        ///<inheritdoc/>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            if (hwndHost == IntPtr.Zero)
            {
                hwndHost = CreateWindowEx(0, "static", "",
                            WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN,
                            0, 0,
                            (int)ActualWidth, (int)ActualHeight,
                            hwndParent.Handle,
                            (IntPtr)HOST_ID,
                            IntPtr.Zero,
                            0);
            }

            CreateBrowser();

            if (browserSettings.AutoDispose)
            {
                browserSettings.Dispose();
                browserSettings = null;
            }

            browserSettings = null;

            return new HandleRef(null, hwndHost);
        }

        ///<inheritdoc/>
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            DestroyWindow(hwnd.Handle);
        }

        ///<inheritdoc/>
        protected override bool TabIntoCore(TraversalRequest request)
        {
            if(InternalIsBrowserInitialized())
            {
                var host = browser.GetHost();
                host.SetFocus(true);

                return true;
            }

            return base.TabIntoCore(request);
        }

        ///<inheritdoc/>
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if(!e.Handled)
            {
                if (InternalIsBrowserInitialized())
                {
                    var host = browser.GetHost();
                    host.SetFocus(true);
                }
                else
                {
                    initialFocus = true;
                }
            }

            base.OnGotKeyboardFocus(e);
        }

        ///<inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (!e.Handled)
            {
                if (InternalIsBrowserInitialized())
                {
                    var host = browser.GetHost();
                    host.SetFocus(false);
                }
                else
                {
                    initialFocus = false;
                }
            }

            base.OnLostKeyboardFocus(e);
        }


        ///<inheritdoc/>
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SETFOCUS = 0x0007;
            const int WM_MOUSEACTIVATE = 0x0021;
            switch (msg)
            {
                case WM_SETFOCUS:
                case WM_MOUSEACTIVATE:
                {
                    if(InternalIsBrowserInitialized())
                    {
                        var host = browser.GetHost();
                        host.SetFocus(true);

                        handled = true;

                        return IntPtr.Zero;
                    }
                    break;
                }
            }
            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
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

            if (!DesignMode)
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
            Interlocked.Exchange(ref browserInitialized, 0);

            if (disposing)
            {
                SizeChanged -= OnSizeChanged;
                IsVisibleChanged -= OnIsVisibleChanged;

                PresentationSource.RemoveSourceChangedHandler(this, PresentationSourceChangedHandler);
                // Release window event listeners if PresentationSourceChangedHandler event wasn't
                // fired before Dispose
                if (sourceWindow != null)
                {
                    sourceWindow.StateChanged -= OnWindowStateChanged;
                    sourceWindow.LocationChanged -= OnWindowLocationChanged;
                    sourceWindow = null;
                }


                UiThreadRunAsync(() =>
                {
                    OnIsBrowserInitializedChanged(true, false);

                    //To Minic the WPF behaviour this happens after OnIsBrowserInitializedChanged
                    IsBrowserInitializedChanged?.Invoke(this, EventArgs.Empty);

                    WebBrowser = null;
                });

                // Don't maintain a reference to event listeners anylonger:
                ConsoleMessage = null;
                FrameLoadEnd = null;
                FrameLoadStart = null;
                IsBrowserInitializedChanged = null;
                LoadError = null;
                LoadingStateChanged = null;
                StatusMessage = null;
                TitleChanged = null;
                JavascriptMessageReceived = null;

                // Release reference to handlers, except LifeSpanHandler which we don't set to null
                //so that ILifeSpanHandler.DoClose will not be invoked. Previously we set LifeSpanHandler = null
                //after managedCefBrowserAdapter.Dispose, there's still cases where the LifeSpanHandler was null
                //when DoClose was called Issue https://github.com/cefsharp/CefSharp.Wpf.HwndHost/issues/10
                FindHandler = null;
                DialogHandler = null;
                RequestHandler = null;
                DisplayHandler = null;
                LoadHandler = null;
                KeyboardHandler = null;
                JsDialogHandler = null;
                DragHandler = null;
                DownloadHandler = null;
                MenuHandler = null;
                FocusHandler = null;
                ResourceRequestHandlerFactory = null;
                RenderProcessMessageHandler = null;

                browser = null;

                if (CleanupElement != null)
                {
                    CleanupElement.Unloaded -= OnCleanupElementUnloaded;
                }

                managedCefBrowserAdapter?.Dispose();
                managedCefBrowserAdapter = null;
            }

            Cef.RemoveDisposable(this);
        }

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="args">The <see cref="AddressChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetAddress(AddressChangedEventArgs args)
        {
            UiThreadRunAsync(() =>
            {
                ignoreUriChange = true;
                SetCurrentValue(AddressProperty, args.Address);
                ignoreUriChange = false;

                // The tooltip should obviously also be reset (and hidden) when the address changes.
                SetCurrentValue(TooltipTextProperty, null);
            });
        }

        /// <summary>
        /// Sets the loading state change.
        /// </summary>
        /// <param name="args">The <see cref="LoadingStateChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetLoadingStateChange(LoadingStateChangedEventArgs args)
        {
            if (removeExNoActivateStyle && InternalIsBrowserInitialized())
            {
                removeExNoActivateStyle = false;

                var host = this.GetBrowserHost();
                var hwnd = host.GetWindowHandle();
                //Remove the WS_EX_NOACTIVATE style so that future mouse clicks inside the
                //browser correctly activate and focus the browser. 
                //https://github.com/chromiumembedded/cef/blob/9df4a54308a88fd80c5774d91c62da35afb5fd1b/tests/cefclient/browser/root_window_win.cc#L1088
                RemoveExNoActivateStyle(hwnd);
            }

            UiThreadRunAsync(() =>
            {
                SetCurrentValue(CanGoBackProperty, args.CanGoBack);
                SetCurrentValue(CanGoForwardProperty, args.CanGoForward);
                SetCurrentValue(IsLoadingProperty, args.IsLoading);

                ((DelegateCommand)BackCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)ForwardCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)ReloadCommand).RaiseCanExecuteChanged();
            });

            LoadingStateChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="args">The <see cref="TitleChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetTitle(TitleChangedEventArgs args)
        {
            UiThreadRunAsync(() => SetCurrentValue(TitleProperty, args.Title));
        }

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="tooltipText">The tooltip text.</param>
        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            UiThreadRunAsync(() => SetCurrentValue(TooltipTextProperty, tooltipText));
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadStart" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadStartEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadStart(FrameLoadStartEventArgs args)
        {
            FrameLoadStart?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadEnd" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadEndEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadEnd(FrameLoadEndEventArgs args)
        {
            FrameLoadEnd?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:ConsoleMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ConsoleMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnConsoleMessage(ConsoleMessageEventArgs args)
        {
            ConsoleMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:StatusMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="StatusMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnStatusMessage(StatusMessageEventArgs args)
        {
            StatusMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:LoadError" /> event.
        /// </summary>
        /// <param name="args">The <see cref="LoadErrorEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnLoadError(LoadErrorEventArgs args)
        {
            LoadError?.Invoke(this, args);
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
        /// Called when [after browser created].
        /// </summary>
        /// <param name="browser">The browser.</param>
        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            if (IsDisposed || browser.IsDisposed)
            {
                return;
            }

            Interlocked.Exchange(ref browserInitialized, 1);
            this.browser = browser;

            UiThreadRunAsync(() =>
            {
                if (!IsDisposed)
                {
                    OnIsBrowserInitializedChanged(false, true);
                    //To Minic the WPF behaviour this happens after OnIsBrowserInitializedChanged
                    IsBrowserInitializedChanged?.Invoke(this, EventArgs.Empty);

                    // Only call Load if initialAddress is null and Address is not empty
                    if (string.IsNullOrEmpty(initialAddress) && !string.IsNullOrEmpty(Address))
                    {
                        Load(Address);
                    }
                }
            });

            ResizeBrowser((int)ActualWidth, (int)ActualHeight);

            if (initialFocus)
            {
                browser.GetHost()?.SetFocus(true);
            }
        }

        /// <summary>
        /// A flag that indicates whether the state of the control current supports the GoBack action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool CanGoBack
        {
            get { return (bool)GetValue(CanGoBackProperty); }
        }

        /// <summary>
        /// The can go back property
        /// </summary>
        public static DependencyProperty CanGoBackProperty = DependencyProperty.Register(nameof(CanGoBack), typeof(bool), typeof(ChromiumWebBrowser));

        /// <summary>
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool CanGoForward
        {
            get { return (bool)GetValue(CanGoForwardProperty); }
        }

        /// <summary>
        /// The can go forward property
        /// </summary>
        public static DependencyProperty CanGoForwardProperty = DependencyProperty.Register(nameof(CanGoForward), typeof(bool), typeof(ChromiumWebBrowser));

        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <value>The address.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        /// <summary>
        /// The address property
        /// </summary>
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register(nameof(Address), typeof(string), typeof(ChromiumWebBrowser),
                                        new UIPropertyMetadata(null, OnAddressChanged));

        /// <summary>
        /// Handles the <see cref="E:AddressChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAddressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var owner = (ChromiumWebBrowser)sender;
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;

            owner.OnAddressChanged(oldValue, newValue);
        }

        /// <summary>
        /// Called when [address changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnAddressChanged(string oldValue, string newValue)
        {
            if (ignoreUriChange || newValue == null || !InternalIsBrowserInitialized())
            {
                return;
            }

            Load(newValue);
        }

        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is loading; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
        }

        /// <summary>
        /// The is loading property
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(ChromiumWebBrowser), new PropertyMetadata(false));

        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool IsBrowserInitialized
        {
            get { return InternalIsBrowserInitialized(); }
        }

        /// <summary>
        /// Event called after the underlying CEF browser instance has been created and
        /// when the <see cref="ChromiumWebBrowser"/> instance has been Disposed.
        /// <see cref="IsBrowserInitialized"/> will be true when the underlying CEF Browser
        /// has been created and false when the browser is being Disposed.
        /// </summary>
        public event EventHandler IsBrowserInitializedChanged;

        /// <summary>
        /// Called when [is browser initialized changed].
        /// </summary>
        /// <param name="oldValue">if set to <c>true</c> [old value].</param>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        protected virtual void OnIsBrowserInitializedChanged(bool oldValue, bool newValue)
        {
            if (newValue && !IsDisposed)
            {
                var task = this.GetZoomLevelAsync();
                task.ContinueWith(previous =>
                {
                    if (previous.Status == TaskStatus.RanToCompletion)
                    {
                        UiThreadRunAsync(() =>
                        {
                            if (!IsDisposed)
                            {
                                SetCurrentValue(ZoomLevelProperty, previous.Result);
                            }
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException("Unexpected failure of calling CEF->GetZoomLevelAsync", previous.Exception);
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        /// <summary>
        /// The title of the web page being currently displayed.
        /// </summary>
        /// <value>The title.</value>
        /// <remarks>This property is implemented as a Dependency Property and fully supports data binding.</remarks>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ChromiumWebBrowser), new PropertyMetadata(null, OnTitleChanged));

        /// <summary>
        /// Event handler that will get called when the browser title changes
        /// </summary>
        public event DependencyPropertyChangedEventHandler TitleChanged;

        /// <summary>
        /// Handles the <see cref="E:TitleChanged" /> event.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (ChromiumWebBrowser)d;

            owner.TitleChanged?.Invoke(owner, e);
        }

        /// <summary>
        /// The zoom level at which the browser control is currently displaying.
        /// Can be set to 0 to clear the zoom level (resets to default zoom level).
        /// </summary>
        /// <value>The zoom level.</value>
        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        /// <summary>
        /// The zoom level property
        /// </summary>
        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register(nameof(ZoomLevel), typeof(double), typeof(ChromiumWebBrowser),
                                        new UIPropertyMetadata(0d, OnZoomLevelChanged));

        /// <summary>
        /// Handles the <see cref="E:ZoomLevelChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnZoomLevelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var owner = (ChromiumWebBrowser)sender;
            var oldValue = (double)args.OldValue;
            var newValue = (double)args.NewValue;

            owner.OnZoomLevelChanged(oldValue, newValue);
        }

        /// <summary>
        /// Called when [zoom level changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnZoomLevelChanged(double oldValue, double newValue)
        {
            this.SetZoomLevel(newValue);
        }

        /// <summary>
        /// Specifies the amount used to increase/decrease to ZoomLevel by
        /// By Default this value is 0.10
        /// </summary>
        /// <value>The zoom level increment.</value>
        public double ZoomLevelIncrement
        {
            get { return (double)GetValue(ZoomLevelIncrementProperty); }
            set { SetValue(ZoomLevelIncrementProperty, value); }
        }

        /// <summary>
        /// The zoom level increment property
        /// </summary>
        public static readonly DependencyProperty ZoomLevelIncrementProperty =
            DependencyProperty.Register(nameof(ZoomLevelIncrement), typeof(double), typeof(ChromiumWebBrowser), new PropertyMetadata(0.10));

        /// <summary>
        /// The CleanupElement controls when the Browser will be Disposed.
        /// The <see cref="ChromiumWebBrowser"/> will be Disposed when <see cref="FrameworkElement.Unloaded"/> is called.
        /// Be aware that this Control is not usable anymore after it has been disposed.
        /// </summary>
        /// <value>The cleanup element.</value>
        public FrameworkElement CleanupElement
        {
            get { return (FrameworkElement)GetValue(CleanupElementProperty); }
            set { SetValue(CleanupElementProperty, value); }
        }

        /// <summary>
        /// The cleanup element property
        /// </summary>
        public static readonly DependencyProperty CleanupElementProperty =
            DependencyProperty.Register(nameof(CleanupElement), typeof(FrameworkElement), typeof(ChromiumWebBrowser), new PropertyMetadata(null, OnCleanupElementChanged));

        /// <summary>
        /// Handles the <see cref="E:CleanupElementChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCleanupElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var owner = (ChromiumWebBrowser)sender;
            var oldValue = (FrameworkElement)args.OldValue;
            var newValue = (FrameworkElement)args.NewValue;

            owner.OnCleanupElementChanged(oldValue, newValue);
        }

        /// <summary>
        /// Called when [cleanup element changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnCleanupElementChanged(FrameworkElement oldValue, FrameworkElement newValue)
        {
            if (oldValue != null)
            {
                oldValue.Unloaded -= OnCleanupElementUnloaded;
            }

            if (newValue != null)
            {
                newValue.Unloaded += OnCleanupElementUnloaded;
            }
        }


        /// <summary>
        /// Handles the <see cref="E:CleanupElementUnloaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCleanupElementUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        /// <value>The tooltip text.</value>
        public string TooltipText
        {
            get { return (string)GetValue(TooltipTextProperty); }
        }

        /// <summary>
        /// The tooltip text property
        /// </summary>
        public static readonly DependencyProperty TooltipTextProperty =
            DependencyProperty.Register(nameof(TooltipText), typeof(string), typeof(ChromiumWebBrowser));

        /// <summary>
        /// Gets or sets the WebBrowser.
        /// </summary>
        /// <value>The WebBrowser.</value>
        public IWebBrowser WebBrowser
        {
            get { return (IWebBrowser)GetValue(WebBrowserProperty); }
            set { SetValue(WebBrowserProperty, value); }
        }

        /// <summary>
        /// The WebBrowser property
        /// </summary>
        public static readonly DependencyProperty WebBrowserProperty =
            DependencyProperty.Register(nameof(WebBrowser), typeof(IWebBrowser), typeof(ChromiumWebBrowser), new UIPropertyMetadata(defaultValue: null));

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
                var windowInfo = CreateBrowserWindowInfo(hwndHost);

                //We actually check if WS_EX_NOACTIVATE was set for instances
                //the user has override CreateBrowserWindowInfo and not called base.CreateBrowserWindowInfo
                removeExNoActivateStyle = (windowInfo.ExStyle & WS_EX_NOACTIVATE) == WS_EX_NOACTIVATE;

                managedCefBrowserAdapter.CreateBrowser(windowInfo, browserSettings as BrowserSettings, requestContext as RequestContext, Address);
            }
        }

        /// <summary>
        /// Runs the specific Action on the Dispatcher in an async fashion
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        internal void UiThreadRunAsync(Action action, DispatcherPriority priority = DispatcherPriority.DataBind)
        {
            if (Dispatcher.CheckAccess())
            {
                action();
            }
            else if (!Dispatcher.HasShutdownStarted)
            {
                Dispatcher.BeginInvoke(action, priority);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:IsVisibleChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var isVisible = (bool)args.NewValue;

            if (InternalIsBrowserInitialized())
            {
                var host = browser.GetHost();

                if (isVisible)
                {
                    ResizeBrowser((int)ActualWidth, (int)ActualHeight);

                    //Fix for #1778 - When browser becomes visible we update the zoom level
                    //browsers of the same origin will share the same zoomlevel and
                    //we need to track the update, so our ZoomLevelProperty works
                    //properly
                    host.GetZoomLevelAsync().ContinueWith(t =>
                    {
                        if (!IsDisposed)
                        {
                            SetCurrentValue(ZoomLevelProperty, t.Result);
                        }
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    //Hide browser
                    ResizeBrowser(0, 0);
                }
            }
        }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        public void Load(string url)
        {
            if (!InternalIsBrowserInitialized())
            {
                throw new Exception("The browser has not been initialized. Load can only be called " +
                                    "after the underlying CEF browser is initialized (CefLifeSpanHandler::OnAfterCreated).");
            }

            using (var frame = browser.MainFrame)
            {
                frame.LoadUrl(url);
            }
        }

        /// <inheritdoc/>
        public Task<LoadUrlAsyncResponse> LoadUrlAsync(string url = null, SynchronizationContext ctx = null)
        {
            //LoadUrlAsync is actually a static method so that CefSharp.Wpf.HwndHost can reuse the code
            //It's not actually an extension method so we can have it included as part of the
            //IWebBrowser interface
            return CefSharp.WebBrowserExtensions.LoadUrlAsync(this, url, ctx);
        }

        /// <summary>
        /// Zooms the browser in.
        /// </summary>
        private void ZoomIn()
        {
            UiThreadRunAsync(() =>
            {
                ZoomLevel = ZoomLevel + ZoomLevelIncrement;
            });
        }

        /// <summary>
        /// Zooms the browser out.
        /// </summary>
        private void ZoomOut()
        {
            UiThreadRunAsync(() =>
            {
                ZoomLevel = ZoomLevel - ZoomLevelIncrement;
            });
        }

        /// <summary>
        /// Reset the browser's zoom level to default.
        /// </summary>
        private void ZoomReset()
        {
            UiThreadRunAsync(() =>
            {
                ZoomLevel = 0;
            });
        }

        /// <summary>
        /// The javascript object repository, one repository per ChromiumWebBrowser instance.
        /// </summary>
        public IJavascriptObjectRepository JavascriptObjectRepository
        {
            get { return managedCefBrowserAdapter?.JavascriptObjectRepository; }
        }

        /// <summary>
        /// Returns the current IBrowser Instance
        /// </summary>
        /// <returns>browser instance or null</returns>
        public IBrowser GetBrowser()
        {
            return browser;
        }

        /// <summary>
        /// Check is browserisinitialized
        /// </summary>
        /// <returns>true if browser is initialized</returns>
        private bool InternalIsBrowserInitialized()
        {
            // Use CompareExchange to read the current value - if disposeCount is 1, we set it to 1, effectively a no-op
            // Volatile.Read would likely use a memory barrier which I believe is unnecessary in this scenario
            return Interlocked.CompareExchange(ref browserInitialized, 0, 0) == 1;
        }

        /// <summary>
        /// Resizes the browser.
        /// </summary>
        private void ResizeBrowser(int width, int height)
        {
            if (InternalIsBrowserInitialized())
            {
                if (dpiScale > 1)
                {
                    width = (int)(width * dpiScale);
                    height = (int)(height * dpiScale);
                }

                managedCefBrowserAdapter.Resize(width, height);
            }
        }

        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            var window = (Window)sender;

            switch (window.WindowState)
            {
                case WindowState.Normal:
                case WindowState.Maximized:
                {
                    if (previousWindowState == WindowState.Minimized && InternalIsBrowserInitialized())
                    {
                        ResizeBrowser((int)ActualWidth, (int)ActualHeight);
                    }
                    break;
                }
                case WindowState.Minimized:
                {
                    if (InternalIsBrowserInitialized())
                    {
                        //Set the browser size to 0,0 to reduce CPU usage
                        ResizeBrowser(0, 0);
                    }
                    break;
                }
            }

            previousWindowState = window.WindowState;
        }

        private void OnWindowLocationChanged(object sender, EventArgs e)
        {
            if (InternalIsBrowserInitialized())
            {
                var host = browser.GetHost();

                host.NotifyMoveOrResizeStarted();
            }
        }
    }
}
