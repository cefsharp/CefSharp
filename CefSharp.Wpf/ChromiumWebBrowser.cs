// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32.SafeHandles;
using CefSharp.Internals;
using CefSharp.Wpf.Internals;
using CefSharp.Wpf.Rendering;
using CefSharp.Enums;
using CefSharp.Structs;

using Point = System.Windows.Point;
using Size = System.Windows.Size;
using CursorType = CefSharp.Enums.CursorType;
using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf 
{
    /// <summary>
    /// ChromiumWebBrowser is the WPF web browser control
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    /// <seealso cref="CefSharp.Internals.IRenderWebBrowser" />
    /// <seealso cref="CefSharp.Wpf.IWpfWebBrowser" />
    public class ChromiumWebBrowser : ContentControl, IRenderWebBrowser, IWpfWebBrowser
    {
        /// <summary>
        /// The source
        /// </summary>
        private HwndSource source;
        /// <summary>
        /// The HwndSource RootVisual (Window) - We store a reference
        /// to unsubscribe event handlers
        /// </summary>
        private Window sourceWindow;
        /// <summary>
        /// The tooltip timer
        /// </summary>
        private DispatcherTimer tooltipTimer;
        /// <summary>
        /// The tool tip
        /// </summary>
        private ToolTip toolTip;
        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;
        /// <summary>
        /// Track the state of the Mouse.Capture set in OnMouseLeave, when the user releases
        /// the left button it's important we release the capture. It's important we track the mouse
        /// capture state ourselves and not just to a Mouse.Captured == this check, as we use
        /// Mouse.Capture for the popup that hosts dropdown menus
        /// </summary>
        private bool mouseCapturedInOnMouseLeave;
        /// <summary>
        /// The ignore URI change
        /// </summary>
        private bool ignoreUriChange;
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
        /// The image that represents this browser instances
        /// </summary>
        private Image image;
        /// <summary>
        /// The popup image
        /// </summary>
        private Image popupImage;
        /// <summary>
        /// The popup
        /// </summary>
        private Popup popup;
        /// <summary>
        /// The browser
        /// </summary>
        private IBrowser browser;
        /// <summary>
        /// The dispose count
        /// </summary>
        private int disposeCount;
        /// <summary>
        /// Location of the control on the screen, relative to Top/Left
        /// Used to calculate GetScreenPoint
        /// We're unable to call PointToScreen directly due to treading restrictions
        /// and calling in a sync fashion on the UI thread was problematic.
        /// </summary>
        private Point browserScreenLocation;
        /// <summary>
        /// The request context (we deliberately use a private variable so we can throw an exception if
        /// user attempts to set after browser created)
        /// </summary>
        private IRequestContext requestContext;
        /// <summary>
        /// Keep a short term copy of IDragData, so when calling DoDragDrop, DragEnter is called, 
        /// we can reuse the drag data provided from CEF
        /// </summary>
        private IDragData currentDragData;

        /// <summary>
        /// A flag that indicates whether or not the designer is active
        /// NOTE: Needs to be static for OnApplicationExit
        /// </summary>
        private static bool designMode;

        /// <summary>
        /// WPF Keyboard Handled forwards key events to the underlying browser
        /// </summary>
        public IWpfKeyboardHandler WpfKeyboardHandler { get; set; }

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        public BrowserSettings BrowserSettings { get; set; }
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
                    throw new Exception("Browser has already been created. RequestContext must be" +
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
        /// Implement <see cref="IResourceHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        public IResourceHandlerFactory ResourceHandlerFactory { get; set; }
        /// <summary>
        /// Implement <see cref="IRenderHandler"/> and control how the control is rendered
        /// </summary>
        /// <value>The render Handler.</value>
        public IRenderHandler RenderHandler { get; set; }
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
        /// Raised every time <see cref="IRenderWebBrowser.OnPaint"/> is called. You can access the underlying buffer, though it's
        /// preferable to either override <see cref="OnPaint"/> or implement your own <see cref="IRenderHandler"/> as there is no outwardly
        /// accessible locking (locking is done within the default <see cref="IRenderHandler"/> implementations).
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI thread
        /// </summary>
        public event EventHandler<PaintEventArgs> Paint;

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
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        public bool CanExecuteJavascriptInMainFrame { get; private set; }

        /// <summary>
        /// The dpi scale factor, if the browser has already been initialized
        /// you must manually call IBrowserHost.NotifyScreenInfoChanged for the
        /// browser to be notified of the change.
        /// </summary>
        public double DpiScaleFactor { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        static ChromiumWebBrowser()
        {
            if (CefSharpSettings.ShutdownOnExit)
            {
                var app = Application.Current;

                if (app != null)
                {
                    app.Exit += OnApplicationExit;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cef::Initialize() failed</exception>
        public ChromiumWebBrowser()
        {
            designMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);

            if (!designMode)
            {
                NoInliningConstructor();
            }
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
                //Disable TouchpadAndWheelScrollLatching
                //Workaround for #2408
                var settings = new CefSettings();
                settings.DisableTouchpadAndWheelScrollLatching();
                settings.WindowlessRenderingEnabled = true;

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
            IsTabStop = true;

            Dispatcher.BeginInvoke((Action)(() => WebBrowser = this));

            Loaded += OnLoaded;
            SizeChanged += OnActualSizeChanged;

            GotKeyboardFocus += OnGotKeyboardFocus;
            LostKeyboardFocus += OnLostKeyboardFocus;

            // Drag Drop events
            DragEnter += OnDragEnter;
            DragOver += OnDragOver;
            DragLeave += OnDragLeave;
            Drop += OnDrop;

            IsVisibleChanged += OnIsVisibleChanged;

            ToolTip = toolTip = new ToolTip();
            toolTip.StaysOpen = true;
            toolTip.Visibility = Visibility.Collapsed;
            toolTip.Closed += OnTooltipClosed;

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

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, true);

            ResourceHandlerFactory = new DefaultResourceHandlerFactory();
            BrowserSettings = new BrowserSettings();
            RenderHandler = new InteropBitmapRenderHandler();

            WpfKeyboardHandler = new WpfKeyboardHandler(this);
            
            PresentationSource.AddSourceChangedHandler(this, PresentationSourceChangedHandler);

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            UseLayoutRounding = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        ~ChromiumWebBrowser()
        {
            if (!designMode)
            {
                Dispose(false);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!designMode)
            {
                Dispose(true);
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        // This method cannot be inlined as the designer will attempt to load libcef.dll and will subsiquently throw an exception.
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected virtual void Dispose(bool isDisposing)
        {
            //If disposeCount is 0 then we'll update it to 1 and begin disposing
            if (Interlocked.CompareExchange(ref disposeCount, 1, 0) == 0)
            {
                // No longer reference event listeners:
                ConsoleMessage = null;
                FrameLoadEnd = null;
                FrameLoadStart = null;
                IsBrowserInitializedChanged = null;
                LoadError = null;
                LoadingStateChanged = null;
                Paint = null;
                StatusMessage = null;
                TitleChanged = null;

                if (isDisposing)
                {
                    browser = null;
                    if (BrowserSettings != null)
                    {
                        BrowserSettings.Dispose();
                        BrowserSettings = null;
                    }

                    //Incase we accidentally have a reference to the CEF drag data
                    if(currentDragData != null)
                    {
                        currentDragData.Dispose();
                        currentDragData = null;
                    }

                    PresentationSource.RemoveSourceChangedHandler(this, PresentationSourceChangedHandler);
                    // Release window event listeners if PresentationSourceChangedHandler event wasn't
                    // fired before Dispose
                    if (sourceWindow != null)
                    {
                        sourceWindow.StateChanged -= OnWindowStateChanged;
                        sourceWindow.LocationChanged -= OnWindowLocationChanged;
                        sourceWindow = null;
                    }

                    // Release internal event listeners:
                    Loaded -= OnLoaded;
                    SizeChanged -= OnActualSizeChanged;
                    GotKeyboardFocus -= OnGotKeyboardFocus;
                    LostKeyboardFocus -= OnLostKeyboardFocus;

                    // Release internal event listeners for Drag Drop events:
                    DragEnter -= OnDragEnter;
                    DragOver -= OnDragOver;
                    DragLeave -= OnDragLeave;
                    Drop -= OnDrop;

                    IsVisibleChanged -= OnIsVisibleChanged;

                    if(popup != null)
                    {
                        popup.Opened -= PopupOpened;
                        popup.Closed -= PopupClosed;
                        popup = null;
                    }

                    if (tooltipTimer != null)
                    {
                        tooltipTimer.Tick -= OnTooltipTimerTick;
                        tooltipTimer.Stop();
                        tooltipTimer = null;
                    }

                    if (CleanupElement != null)
                    {
                        CleanupElement.Unloaded -= OnCleanupElementUnloaded;
                    }

                    if(managedCefBrowserAdapter != null)
                    {
                        managedCefBrowserAdapter.Dispose();
                        managedCefBrowserAdapter = null;
                    }

                    Interlocked.Exchange(ref browserInitialized, 0);
                    UiThreadRunAsync(() =>
                    {
                        SetCurrentValue(IsBrowserInitializedProperty, false);
                        WebBrowser = null;
                    });
                }

                // Release reference to handlers, make sure this is done after we dispose managedCefBrowserAdapter
                // otherwise the ILifeSpanHandler.DoClose will not be invoked. (More important in the WinForms version,
                // we do it here for consistency)
                this.SetHandlersToNull();

                Cef.RemoveDisposable(this);

                WpfKeyboardHandler.Dispose();
                source = null;
            }
        }

        /// <summary>
        /// Gets the ScreenInfo - currently used to get the DPI scale factor.
        /// </summary>
        /// <returns>ScreenInfo containing the current DPI scale factor</returns>
        ScreenInfo? IRenderWebBrowser.GetScreenInfo()
        {
            return GetScreenInfo();
        }

        /// <summary>
        /// Gets the ScreenInfo - currently used to get the DPI scale factor.
        /// </summary>
        /// <returns>ScreenInfo containing the current DPI scale factor</returns>
        protected virtual ScreenInfo? GetScreenInfo()
        {
            var screenInfo = new ScreenInfo(scaleFactor: (float)DpiScaleFactor);

            return screenInfo;
        }

        /// <summary>
        /// Gets the view rect (width, height)
        /// </summary>
        /// <returns>ViewRect.</returns>
        ViewRect? IRenderWebBrowser.GetViewRect()
        {
            return GetViewRect();
        }

        /// <summary>
        /// Gets the view rect (width, height)
        /// </summary>
        /// <returns>ViewRect.</returns>
        protected virtual ViewRect? GetViewRect()
        {
            //NOTE: Previous we used Math.Ceiling to round the sizing up, we
            //now set UseLayoutRounding = true; on the control so the sizes are
            //already rounded to a whole number for us.
            var viewRect = new ViewRect((int)ActualWidth, (int)ActualHeight);

            return viewRect;
        }

        bool IRenderWebBrowser.GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY)
        {
            screenX = 0;
            screenY = 0;

            //We manually claculate the screen point as calling PointToScreen can only be called on the UI thread
            // in a sync fashion and it's easy for users to get themselves into a deadlock.
            if(DpiScaleFactor > 1)
            {
                screenX = (int)(browserScreenLocation.X + (viewX * DpiScaleFactor));
                screenY = (int)(browserScreenLocation.Y + (viewY * DpiScaleFactor));
            }
            else
            {
                screenX = (int)(browserScreenLocation.X + viewX);
                screenY = (int)(browserScreenLocation.Y + viewY);
            }

            return true;
        }

        /// <summary>
        /// Called when the user starts dragging content in the web view. 
        /// OS APIs that run a system message loop may be used within the StartDragging call.
        /// Don't call any of IBrowserHost::DragSource*Ended* methods after returning false.
        /// Call IBrowserHost.DragSourceEndedAt and DragSourceSystemDragEnded either synchronously or asynchronously to inform the web view that the drag operation has ended. 
        /// </summary>
        /// <param name="dragData"> Contextual information about the dragged content</param>
        /// <param name="allowedOps">allowed operations</param>
        /// <param name="x">is the drag start location in screen coordinates</param>
        /// <param name="y">is the drag start location in screen coordinates</param>
        /// <returns>Return true to handle the drag operation.</returns>
        bool IRenderWebBrowser.StartDragging(IDragData dragData, DragOperationsMask allowedOps, int x, int y)
        {
            var dataObject = new DataObject();

            dataObject.SetText(dragData.FragmentText, TextDataFormat.Text);
            dataObject.SetText(dragData.FragmentText, TextDataFormat.UnicodeText);
            dataObject.SetText(dragData.FragmentHtml, TextDataFormat.Html);

            //Clone dragData for use in OnDragEnter event handler
            currentDragData = dragData.Clone();
            currentDragData.ResetFileContents();

            // TODO: The following code block *should* handle images, but GetFileContents is
            // not yet implemented.
            //if (dragData.IsFile)
            //{
            //    var bmi = new BitmapImage();
            //    bmi.BeginInit();
            //    bmi.StreamSource = dragData.GetFileContents();
            //    bmi.EndInit();
            //    dataObject.SetImage(bmi);
            //}

            UiThreadRunAsync(delegate
            {
                if(browser != null)
                {
                    //DoDragDrop will fire DragEnter event
                    var result = DragDrop.DoDragDrop(this, dataObject, GetDragEffects(allowedOps));

                    //DragData was stored so when DoDragDrop fires DragEnter we reuse a clone of the IDragData provided here
                    currentDragData = null;

                    //If result == DragDropEffects.None then we'll send DragOperationsMask.None
                    //effectively cancelling the drag operation
                    browser.GetHost().DragSourceEndedAt(x, y, GetDragOperationsMask(result));
                    browser.GetHost().DragSourceSystemDragEnded();
                }
            });

            return true;
        }

        void IRenderWebBrowser.UpdateDragCursor(DragOperationsMask operation)
        {
            UpdateDragCursor(operation);
        }

        protected virtual void UpdateDragCursor(DragOperationsMask operation)
        {
            //TODO: Someone should implement this
        }

        /// <summary>
        /// Called when an element should be painted.
        /// </summary>
        /// <param name="type">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="buffer">The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        void IRenderWebBrowser.OnPaint(PaintElementType type, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            OnPaint(type == PaintElementType.Popup, dirtyRect, buffer, width, height);
        }

        /// <summary>
        /// Called when an element should be painted. Pixel values passed to this method are scaled relative to view coordinates based on the
        /// value of <see cref="ScreenInfo.DeviceScaleFactor"/> returned from <see cref="IRenderWebBrowser.GetScreenInfo"/>. To override the default behaviour
        /// override this method or implement your own <see cref="IRenderHandler"/> and assign to <see cref="RenderHandler"/>
        /// Called on the CEF UI Thread
        /// </summary>
        /// <param name="type">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="buffer">The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        protected virtual void OnPaint(bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            var paint = Paint;
            if (paint != null)
            {
                var args = new PaintEventArgs(isPopup, dirtyRect, buffer, width, height);

                paint(this, args);

                if(args.Handled)
                {
                    return;
                }
            }

            var img = isPopup ? popupImage : image;

            RenderHandler?.OnPaint(isPopup, dirtyRect, buffer, width, height, img);
        }

        /// <summary>
        /// Sets the popup size and position.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        void IRenderWebBrowser.OnPopupSize(Rect rect)
        {
            UiThreadRunAsync(() => SetPopupSizeAndPositionImpl(rect));
        }

        /// <summary>
        /// Sets the popup is open.
        /// </summary>
        /// <param name="isOpen">if set to <c>true</c> [is open].</param>
        void IRenderWebBrowser.OnPopupShow(bool isOpen)
        {
            UiThreadRunAsync(() => { popup.IsOpen = isOpen; });
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="type">The type.</param>
        void IRenderWebBrowser.OnCursorChange(IntPtr handle, CursorType type, CursorInfo customCursorInfo)
        {
            //Custom cursors are handled differently, for now keep standard ones executing
            //in an async fashion
            if (type == CursorType.Custom)
            {
                //When using a custom it appears we need to update the cursor in a sync fashion
                //Likely the underlying handle/buffer is being released before the cursor
                // is created when executed in an async fashion. Doesn't seem to be a problem
                //for build in cursor types
                UiThreadRunSync(() =>
                {
                    Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false));
                });
            }
            else
            {
                UiThreadRunAsync(() =>
                {
                    Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false));
                });
            }
        }

        void IRenderWebBrowser.OnImeCompositionRangeChanged(Range selectedRange, Rect[] characterBounds)
        {
            OnImeCompositionRangeChanged(selectedRange, characterBounds);
        }

        protected virtual void OnImeCompositionRangeChanged(Range selectedRange, Rect[] characterBounds)
        {
            //TODO: Implement this
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
        /// Called when [after browser created].
        /// </summary>
        /// <param name="browser">The browser.</param>
        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            Interlocked.Exchange(ref browserInitialized, 1);
            this.browser = browser;

            UiThreadRunAsync(() =>
            {
                if (!IsDisposed)
                {
                    SetCurrentValue(IsBrowserInitializedProperty, true);

                    // If Address was previously set, only now can we actually do the load
                    if (!string.IsNullOrEmpty(Address))
                    {
                        Load(Address);
                    }
                }
            });
        }

        #region CanGoBack dependency property

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
        public static DependencyProperty CanGoBackProperty = DependencyProperty.Register("CanGoBack", typeof(bool), typeof(ChromiumWebBrowser));

        #endregion

        #region CanGoForward dependency property

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
        public static DependencyProperty CanGoForwardProperty = DependencyProperty.Register("CanGoForward", typeof(bool), typeof(ChromiumWebBrowser));

        #endregion

        #region Address dependency property

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
            DependencyProperty.Register("Address", typeof(string), typeof(ChromiumWebBrowser),
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

        #endregion Address dependency property

        #region IsLoading dependency property

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
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(ChromiumWebBrowser), new PropertyMetadata(false));

        #endregion IsLoading dependency property

        #region IsBrowserInitialized dependency property

        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool IsBrowserInitialized
        {
            get { return (bool)GetValue(IsBrowserInitializedProperty); }
        }

        /// <summary>
        /// The is browser initialized property
        /// </summary>
        public static readonly DependencyProperty IsBrowserInitializedProperty =
            DependencyProperty.Register("IsBrowserInitialized", typeof(bool), typeof(ChromiumWebBrowser), new PropertyMetadata(false, OnIsBrowserInitializedChanged));

        /// <summary>
        /// Event handler that will get called when the browser has finished initializing
        /// </summary>
        public event DependencyPropertyChangedEventHandler IsBrowserInitializedChanged;

        /// <summary>
        /// Handles the <see cref="E:IsBrowserInitializedChanged" /> event.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsBrowserInitializedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (ChromiumWebBrowser)d;
            var oldValue = (bool)e.OldValue;
            var newValue = (bool)e.NewValue;

            owner.OnIsBrowserInitializedChanged(oldValue, newValue);

            owner.IsBrowserInitializedChanged?.Invoke(owner, e);
        }

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

        #endregion IsInitialized dependency property

        #region Title dependency property

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
            DependencyProperty.Register("Title", typeof(string), typeof(ChromiumWebBrowser), new PropertyMetadata(null, OnTitleChanged));

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

        #endregion Title dependency property

        #region ZoomLevel dependency property

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
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(ChromiumWebBrowser),
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

        #endregion ZoomLevel dependency property

        #region ZoomLevelIncrement dependency property

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
            DependencyProperty.Register("ZoomLevelIncrement", typeof(double), typeof(ChromiumWebBrowser), new PropertyMetadata(0.10));

        #endregion ZoomLevelIncrement dependency property

        #region CleanupElement dependency property

        /// <summary>
        /// The CleanupElement Controls when the BrowserResources will be cleaned up.
        /// The ChromiumWebBrowser will register on Unloaded of the provided Element and dispose all resources when that handler is called.
        /// By default the cleanup element is the Window that contains the ChromiumWebBrowser.
        /// if you want cleanup to happen earlier provide another FrameworkElement.
        /// Be aware that this Control is not usable anymore after cleanup is done.
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
            DependencyProperty.Register("CleanupElement", typeof(FrameworkElement), typeof(ChromiumWebBrowser), new PropertyMetadata(null, OnCleanupElementChanged));

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
                newValue.Unloaded -= OnCleanupElementUnloaded;
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

        #endregion CleanupElement dependency property

        #region TooltipText dependency property

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
            DependencyProperty.Register("TooltipText", typeof(string), typeof(ChromiumWebBrowser), new PropertyMetadata(null, (sender, e) => ((ChromiumWebBrowser)sender).OnTooltipTextChanged()));

        /// <summary>
        /// Called when [tooltip text changed].
        /// </summary>
        private void OnTooltipTextChanged()
        {
            var timer = tooltipTimer;
            if (timer == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(TooltipText))
            {
                UiThreadRunAsync(() => UpdateTooltip(null), DispatcherPriority.Render);

                if(timer.IsEnabled)
                {
                    timer.Stop();
                }
            }
            else if(!timer.IsEnabled)
            {
                timer.Start();
            }
        }

        #endregion

        #region WebBrowser dependency property

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
            DependencyProperty.Register("WebBrowser", typeof(IWebBrowser), typeof(ChromiumWebBrowser), new UIPropertyMetadata(defaultValue: null));

        #endregion WebBrowser dependency property

        /// <summary>
        /// Handles the <see cref="E:Drop" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void OnDrop(object sender, DragEventArgs e)
        {
            if(browser != null)
            {
                var mouseEvent = GetMouseEvent(e);
                var effect = GetDragOperationsMask(e.AllowedEffects);

                browser.GetHost().DragTargetDragOver(mouseEvent, effect);
                browser.GetHost().DragTargetDragDrop(mouseEvent);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:DragLeave" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void OnDragLeave(object sender, DragEventArgs e)
        {
            if(browser != null)
            {
                browser.GetHost().DragTargetDragLeave();
            }
        }

        /// <summary>
        /// Handles the <see cref="E:DragOver" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void OnDragOver(object sender, DragEventArgs e)
        {
            if(browser != null)
            {
                browser.GetHost().DragTargetDragOver(GetMouseEvent(e), GetDragOperationsMask(e.AllowedEffects));
            }
        }

        /// <summary>
        /// Handles the <see cref="E:DragEnter" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if(browser != null)
            {
                var mouseEvent = GetMouseEvent(e);
                var effect = GetDragOperationsMask(e.AllowedEffects);

                //DoDragDrop will fire this handler for internally sourced Drag/Drop operations
                //we use the existing IDragData (cloned copy)
                var dragData = currentDragData ?? e.GetDragDataWrapper();

                browser.GetHost().DragTargetDragEnter(dragData, mouseEvent, effect);
                browser.GetHost().DragTargetDragOver(mouseEvent, effect);
            }
        }

        /// <summary>
        /// Converts .NET drag drop effects to CEF Drag Operations
        /// </summary>
        /// <param name="dragDropEffects">The drag drop effects.</param>
        /// <returns>DragOperationsMask.</returns>
        /// s
        private static DragOperationsMask GetDragOperationsMask(DragDropEffects dragDropEffects)
        {
            var operations = DragOperationsMask.None;

            if (dragDropEffects.HasFlag(DragDropEffects.All))
            {
                operations |= DragOperationsMask.Every;
            }
            if (dragDropEffects.HasFlag(DragDropEffects.Copy))
            {
                operations |= DragOperationsMask.Copy;
            }
            if (dragDropEffects.HasFlag(DragDropEffects.Move))
            {
                operations |= DragOperationsMask.Move;
            }
            if (dragDropEffects.HasFlag(DragDropEffects.Link))
            {
                operations |= DragOperationsMask.Link;
            }

            return operations;
        }

        /// <summary>
        /// Gets the drag effects.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>DragDropEffects.</returns>
        private static DragDropEffects GetDragEffects(DragOperationsMask mask)
        {
            if ((mask & DragOperationsMask.Every) == DragOperationsMask.Every)
            {
                return DragDropEffects.All;
            }
            if ((mask & DragOperationsMask.Copy) == DragOperationsMask.Copy)
            {
                return DragDropEffects.Copy;
            }
            if ((mask & DragOperationsMask.Move) == DragOperationsMask.Move)
            {
                return DragDropEffects.Move;
            }
            if ((mask & DragOperationsMask.Link) == DragOperationsMask.Link)
            {
                return DragDropEffects.Link;
            }
            return DragDropEffects.None;
        }

        /// <summary>
        /// PresentationSource changed handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SourceChangedEventArgs"/> instance containing the event data.</param>
        private void PresentationSourceChangedHandler(object sender, SourceChangedEventArgs args)
        {
            if (args.NewSource != null)
            {
                var newSource = (HwndSource)args.NewSource;

                source = newSource;

                if (source != null)
                {
                    var matrix = source.CompositionTarget.TransformToDevice;
                    var notifyDpiChanged = DpiScaleFactor > 0 && !DpiScaleFactor.Equals(matrix.M11);

                    DpiScaleFactor = source.CompositionTarget.TransformToDevice.M11;

                    WpfKeyboardHandler.Setup(source);

                    if (notifyDpiChanged && browser != null)
                    {
                        browser.GetHost().NotifyScreenInfoChanged();
                    }

                    //Ignore this for custom bitmap factories
                    if (RenderHandler != null && (RenderHandler.GetType() == typeof(WritableBitmapRenderHandler) || RenderHandler.GetType() == typeof(InteropBitmapRenderHandler)))
                    {
                        if (DpiScaleFactor > 1.0 && RenderHandler.GetType() != typeof(WritableBitmapRenderHandler))
                        {
                            const int DefaultDpi = 96;
                            var scale = DefaultDpi * DpiScaleFactor;

                            RenderHandler = new WritableBitmapRenderHandler(scale, scale);
                        }
                        else if (DpiScaleFactor == 1.0 && RenderHandler.GetType() != typeof(InteropBitmapRenderHandler))
                        {
                            RenderHandler = new InteropBitmapRenderHandler();
                        }
                    }

                    var window = source.RootVisual as Window;
                    if(window != null)
                    {
                        window.StateChanged += OnWindowStateChanged;
                        window.LocationChanged += OnWindowLocationChanged;
                        sourceWindow = window;
                    }

                    browserScreenLocation = GetBrowserScreenLocation();
                }
            }
            else if (args.OldSource != null)
            {
                WpfKeyboardHandler.Dispose();

                var window = args.OldSource.RootVisual as Window;
                if (window != null)
                {
                    window.StateChanged -= OnWindowStateChanged;
                    window.LocationChanged -= OnWindowLocationChanged;
                    sourceWindow = null;
                }
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
                    if (browser != null)
                    {
                        browser.GetHost().WasHidden(false);
                    }
                    break;
                }
                case WindowState.Minimized:
                {
                    if (browser != null)
                    {
                        browser.GetHost().WasHidden(true);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Called when the Window Location Changes, the PresentationSource changes
        /// and the page loads. We manually track the position as CEF makes calls
        /// on a non-UI thread and calling Invoke in IRenderWebBrowser.GetScreenPoint
        /// makes it very easy to deadlock the browser.
        /// </summary>
        /// <returns>Returns screen coordinates of the browsers location</returns>
        protected virtual Point GetBrowserScreenLocation()
        {
            if (source != null && PresentationSource.FromVisual(this) != null)
            {
                return PointToScreen(new Point());
            }
            else
            {
                return new Point();
            }
        }

        private void OnWindowLocationChanged(object sender, EventArgs e)
        {
            //We maintain a manual reference to the controls screen location
            //(relative to top/left of the screen)
            browserScreenLocation = GetBrowserScreenLocation();
        }

        /// <summary>
        /// Create the underlying Browser instance, can be overriden to defer control creation
        /// The browser will only be created when size &gt; Size(0,0). If you specify a positive
        /// size then the browser will be created, if the ActualWidth and ActualHeight
        /// properties are in reality still 0 then you'll likely end up with a browser that
        /// won't render.
        /// </summary>
        /// <param name="size">size of the current control, must be greater than Size(0, 0)</param>
        /// <returns>bool to indicate if browser was created. If the browser has already been created then this will return false.</returns>
        protected virtual bool CreateOffscreenBrowser(Size size)
        {
            if (browserCreated || size.IsEmpty || size.Equals(new Size(0, 0)))
            {
                return false;
            }

            var webBrowserInternal = this as IWebBrowserInternal;
            if (!webBrowserInternal.HasParent)
            {
                managedCefBrowserAdapter.CreateOffscreenBrowser(source == null ? IntPtr.Zero : source.Handle, BrowserSettings, (RequestContext)RequestContext, Address);
            }
            browserCreated = true;

            return true;
        }

        /// <summary>
        /// Runs the specific Action on the Dispatcher in an async fashion
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        private void UiThreadRunAsync(Action action, DispatcherPriority priority = DispatcherPriority.DataBind)
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
        /// Runs the specific Action on the Dispatcher in an sync fashion
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        private void UiThreadRunSync(Action action, DispatcherPriority priority = DispatcherPriority.DataBind)
        {
            if (Dispatcher.CheckAccess())
            {
                action();
            }
            else if (!Dispatcher.HasShutdownStarted)
            {
                Dispatcher.Invoke(action, priority);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ActualSizeChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void OnActualSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Initialize RenderClientAdapter when WPF has calculated the actual size of current content.
            CreateOffscreenBrowser(e.NewSize);

            if (browser != null)
            {
                browser.GetHost().WasResized();
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

            if (browser != null)
            {
                var host = browser.GetHost();
                host.WasHidden(!isVisible);

                if (isVisible)
                {
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
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ApplicationExit" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExitEventArgs"/> instance containing the event data.</param>
        private static void OnApplicationExit(object sender, ExitEventArgs e)
        {
            if (!designMode)
            {
                CefShutdown();
            }
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
        /// Handles the <see cref="E:Loaded" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (CleanupElement == null)
            {
                CleanupElement = Window.GetWindow(this);
            }

            // TODO: Consider making the delay here configurable.
            tooltipTimer = new DispatcherTimer(
                TimeSpan.FromSeconds(0.5),
                DispatcherPriority.Render,
                OnTooltipTimerTick,
                Dispatcher
                );
            tooltipTimer.IsEnabled = false;

            //Initial value for screen location
            browserScreenLocation = GetBrowserScreenLocation();
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call
        /// <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (image == null)
            {
                // Create main window
                Content = image = CreateImage();
            }

            if (popup == null)
            {
                popup = CreatePopup();
            }
        }

        /// <summary>
        /// Creates the image.
        /// </summary>
        /// <returns>Image.</returns>
        private Image CreateImage()
        {
            var img = new Image();

            BindingOperations.SetBinding(img, RenderOptions.BitmapScalingModeProperty, new Binding
            {
                Path = new PropertyPath(RenderOptions.BitmapScalingModeProperty),
                Source = this,
            });

            img.Stretch = Stretch.None;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;

            return img;
        }

        /// <summary>
        /// Creates the popup.
        /// </summary>
        /// <returns>Popup.</returns>
        private Popup CreatePopup()
        {
            var newPopup = new Popup
            {
                Child = popupImage = CreateImage(),
                PlacementTarget = this,
                Placement = PlacementMode.Absolute,
                //Needs to allow transparency or only ScaleTransforms are allowed
                //https://referencesource.microsoft.com/#PresentationFramework/src/Framework/System/Windows/Controls/Primitives/Popup.cs,1713
                AllowsTransparency = true
            };

            BindingOperations.SetBinding(newPopup, LayoutTransformProperty, new Binding
            {
                Path = new PropertyPath(LayoutTransformProperty),
                Source = this,
            });

            newPopup.Opened += PopupOpened;
            newPopup.Closed += PopupClosed;

            return newPopup;
        }

        /// Converts a .NET Drag event to a CefSharp MouseEvent
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        /// <returns>MouseEvent.</returns>
        private MouseEvent GetMouseEvent(DragEventArgs e)
        {
            var point = e.GetPosition(this);

            return new MouseEvent((int)point.X, (int)point.Y, CefEventFlags.None);
        }

        /// <summary>
        /// Sets the popup size and position implementation.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void SetPopupSizeAndPositionImpl(Rect rect)
        {
            popup.Width = rect.Width ;
            popup.Height = rect.Height;

            var popupOffset = new Point(rect.X, rect.Y);
            var locationFromScreen = PointToScreen(popupOffset);
            popup.HorizontalOffset = locationFromScreen.X / DpiScaleFactor;
            popup.VerticalOffset = locationFromScreen.Y / DpiScaleFactor;
        }

        /// <summary>
        /// Handles the <see cref="E:TooltipTimerTick" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTooltipTimerTick(object sender, EventArgs e)
        {
            // Checks to see if the control is disposed/disposing
            if (!IsDisposed)
            {
                tooltipTimer.Stop();

                UpdateTooltip(TooltipText);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:TooltipClosed" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnTooltipClosed(object sender, RoutedEventArgs e)
        {
            toolTip.Visibility = Visibility.Collapsed;

            // Set Placement to something other than PlacementMode.Mouse, so that when we re-show the tooltip in
            // UpdateTooltip(), the tooltip will be repositioned to the new mouse point.
            toolTip.Placement = PlacementMode.Absolute;
        }

        /// <summary>
        /// Updates the tooltip.
        /// </summary>
        /// <param name="text">The text.</param>
        private void UpdateTooltip(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                toolTip.IsOpen = false;
            }
            else
            {
                // hide old tooltip before showing the new one to update the position
                if (toolTip.IsOpen)
                { 
                    toolTip.IsOpen = false;
                }

                toolTip.Content = text;
                toolTip.Placement = PlacementMode.Mouse;
                toolTip.Visibility = Visibility.Visible;
                toolTip.IsOpen = true;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:GotKeyboardFocus" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyboardFocusChangedEventArgs"/> instance containing the event data.</param>
        private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (browser != null)
            {
                browser.GetHost().SendFocusEvent(true);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:LostKeyboardFocus" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyboardFocusChangedEventArgs"/> instance containing the event data.</param>
        private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (browser != null)
            {
                browser.GetHost().SendFocusEvent(false);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached event reaches an
        /// element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                WpfKeyboardHandler.HandleKeyPress(e);
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyUp" /> attached event reaches an
        /// element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                WpfKeyboardHandler.HandleKeyPress(e);
            }

            base.OnPreviewKeyUp(e);
        }

        /// <summary>
        /// Handles the <see cref="E:PreviewTextInput" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e) 
        {
            if (!e.Handled)
            {
                WpfKeyboardHandler.HandleTextInput(e);
            }

            base.OnPreviewTextInput(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!e.Handled && browser != null)
            {
                var point = e.GetPosition(this);
                var modifiers = e.GetModifiers();

                browser.GetHost().SendMouseMoveEvent((int)point.X, (int)point.Y, false, modifiers);
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseWheel" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (!e.Handled && browser != null)
            {
                var point = e.GetPosition(this);
                var modifiers = e.GetModifiers();
                var isShiftKeyDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                var pointX = (int)point.X;
                var pointY= (int)point.Y;

                browser.SendMouseWheelEvent(
                    pointX,
                    pointY,
                    deltaX: isShiftKeyDown ? e.Delta : 0,
                    deltaY: !isShiftKeyDown ? e.Delta : 0,
                    modifiers: modifiers);

                e.Handled = true;
            }

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Captures the mouse when the popup is opened.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PopupOpened(object sender, EventArgs e)
        {
            if (Mouse.Captured != this)
            {
                Mouse.Capture(this);
            }
        }

        /// <summary>
        /// Releases mouse capture when the popup is closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PopupClosed(object sender, EventArgs e)
        {
            if (Mouse.Captured == this)
            {
                Mouse.Capture(null);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached event reaches an
        /// element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data.
        /// This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            OnMouseButton(e);

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp" /> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the mouse button was released.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            OnMouseButton(e);

            //If we have a mouse capture from OnMouseLeave we release it now
            if(mouseCapturedInOnMouseLeave)
            {
                Mouse.Capture(null);
                mouseCapturedInOnMouseLeave = false;
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (!e.Handled && browser != null)
            {
                var modifiers = e.GetModifiers();
                var point = e.GetPosition(this);

                //When left mouse button is pressed and we leave, capture the mouse so scrolling outside the
                //bounds of the control works.
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Mouse.Capture(this);

                    mouseCapturedInOnMouseLeave = true;
                }
                else
                {
                    browser.GetHost().SendMouseMoveEvent((int)point.X, (int)point.Y, true, modifiers);
                }

                ((IWebBrowserInternal)this).SetTooltipText(null);
            }

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Handles the <see cref="E:MouseButton" /> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnMouseButton(MouseButtonEventArgs e)
        {
            if (!e.Handled && browser != null)
            {
                var modifiers = e.GetModifiers();
                var mouseUp = (e.ButtonState == MouseButtonState.Released);
                var point = e.GetPosition(this);

                if (e.ChangedButton == MouseButton.XButton1)
                {
                    if (CanGoBack && mouseUp)
                    {
                        this.Back();
                    }
                }
                else if (e.ChangedButton == MouseButton.XButton2)
                {
                    if (CanGoForward && mouseUp)
                    {
                        this.Forward();
                    }
                }
                else
                {
                    browser.GetHost().SendMouseClickEvent((int)point.X, (int)point.Y, (MouseButtonType)e.ChangedButton, mouseUp, e.ClickCount, modifiers);
                }

                e.Handled = true;
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

            // Added null check -> binding-triggered changes of Address will lead to a nullref after Dispose has been called
            // or before OnApplyTemplate has been called
            if (browser != null)
            {
                browser.MainFrame.LoadUrl(url);
            }
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
        /// Legacy keyboard handler uses WindowProc callback interceptor to forward keypress events
        /// the the browser. Use this method to revert to the previous keyboard handling behaviour
        /// </summary>
        public void UseLegacyKeyboardHandler()
        {
            if (!(WpfKeyboardHandler is WpfLegacyKeyboardHandler))
            {
                WpfKeyboardHandler.Dispose();
                WpfKeyboardHandler = new WpfLegacyKeyboardHandler(this);
                WpfKeyboardHandler.Setup(source);
            }
        }

        /// <summary>
        /// Registers a Javascript object in this specific browser instance.
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        /// <param name="options">binding options - camelCaseJavascriptNames default to true </param>
        /// <exception cref="Exception">Browser is already initialized. RegisterJsObject must be +
        ///                                     called before the underlying CEF browser is created.</exception>
        public void RegisterJsObject(string name, object objectToBind, BindingOptions options = null)
        {
            if(!CefSharpSettings.LegacyJavascriptBindingEnabled)
            {
                throw new Exception(@"CefSharpSettings.LegacyJavascriptBindingEnabled is currently false,
                                    for legacy binding you must set CefSharpSettings.LegacyJavascriptBindingEnabled = true
                                    before registering your first object see https://github.com/cefsharp/CefSharp/issues/2246
                                    for details on the new binding options. If you perform cross-site navigations bound objects will
                                    no longer be registered and you will have to migrate to the new method.");
            }

            if (InternalIsBrowserInitialized())
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }

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

            if (InternalIsBrowserInitialized())
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            var objectRepository = managedCefBrowserAdapter.JavascriptObjectRepository;

            if (objectRepository == null)
            {
                throw new Exception("Object Repository Null, Browser has likely been Disposed.");
            }

            objectRepository.Register(name, objectToBind, true, options);
        }

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
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed
        {
            get
            {
                // Use CompareExchange to read the current value - if disposeCount is 1, we set it to 1, effectively a no-op
                // Volatile.Read would likely use a memory barrier which I believe is unnecessary in this scenario
                return Interlocked.CompareExchange(ref disposeCount, 1, 1) == 1;
            }
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

        //protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        //{
        //   base.OnManipulationDelta(e);

        //	if (!e.Handled)
        //	{
        //		var point = e.ManipulationOrigin;

        //		if (browser != null)
        //		{
        //			browser.GetHost().SendMouseWheelEvent(
        //				(int)point.X,
        //				(int)point.Y,
        //				deltaX: (int)e.DeltaManipulation.Translation.X,
        //				deltaY: (int)e.DeltaManipulation.Translation.Y,
        //				modifiers: CefEventFlags.None);
        //		}
        //	}
        //}
    }
}
