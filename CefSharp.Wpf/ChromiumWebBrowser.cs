// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using CefSharp.Wpf.Internals;
using CefSharp.Wpf.Rendering;
using Microsoft.Win32.SafeHandles;
using System;
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
using CefSharp.ModelBinding;

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
        /// The source hook
        /// </summary>
        private HwndSourceHook sourceHook;
        /// <summary>
        /// The tooltip timer
        /// </summary>
        private DispatcherTimer tooltipTimer;
        /// <summary>
        /// The tool tip
        /// </summary>
        private readonly ToolTip toolTip;
        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;
        /// <summary>
        /// The ignore URI change
        /// </summary>
        private bool ignoreUriChange;
        /// <summary>
        /// The browser created
        /// </summary>
        private bool browserCreated;
        /// <summary>
        /// The browser initialized
        /// </summary>
        private volatile bool browserInitialized;
        /// <summary>
        /// The matrix
        /// </summary>
        private Matrix matrix;
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
        private volatile int disposeCount;

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        public BrowserSettings BrowserSettings { get; set; }
        /// <summary>
        /// Gets or sets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public RequestContext RequestContext { get; set; }
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
        /// Implement <see cref="IGeolocationHandler" /> and assign to handle requests for permission to use geolocation.
        /// </summary>
        /// <value>The geolocation handler.</value>
        public IGeolocationHandler GeolocationHandler { get; set; }
        /// <summary>
        /// Gets or sets the bitmap factory.
        /// </summary>
        /// <value>The bitmap factory.</value>
        public IBitmapFactory BitmapFactory { get; set; }
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
        /// Raised before each render cycle, and allows you to adjust the bitmap before it's rendered/applied
        /// </summary>
        public event EventHandler<RenderingEventArgs> Rendering;

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
            if (!Cef.IsInitialized && !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            BitmapFactory = new BitmapFactory();

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

            PresentationSource.AddSourceChangedHandler(this, PresentationSourceChangedHandler);

            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        ~ChromiumWebBrowser()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            //Check if already disposed
            if (Interlocked.Increment(ref disposeCount) == 1)
            { 
                // No longer reference event listeners:
                ConsoleMessage = null;
                FrameLoadStart = null;
                FrameLoadEnd = null;
                LoadError = null;
                LoadingStateChanged = null;
                Rendering = null;

                if (isDisposing)
                {
                    browser = null;
                    if (BrowserSettings != null)
                    {
                        BrowserSettings.Dispose();
                        BrowserSettings = null;
                    }

                    PresentationSource.RemoveSourceChangedHandler(this, PresentationSourceChangedHandler);

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

                    if (tooltipTimer != null)
                    {
                        tooltipTimer.Tick -= OnTooltipTimerTick;
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


                    browserInitialized = false;
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

                RemoveSourceHook();
            }
        }

        /// <summary>
        /// Gets the screen information.
        /// </summary>
        /// <returns>ScreenInfo.</returns>
        ScreenInfo IRenderWebBrowser.GetScreenInfo()
        {
            var screenInfo = new ScreenInfo
            {
                ScaleFactor = (float)matrix.M11
            };

            return screenInfo;
        }

        /// <summary>
        /// Gets the view rect.
        /// </summary>
        /// <returns>ViewRect.</returns>
        ViewRect IRenderWebBrowser.GetViewRect()
        {
            var viewRect = new ViewRect
            {
                Width = (int)ActualWidth,
                Height = (int)ActualHeight
            };

            return viewRect;
        }

        /// <summary>
        /// Creates the bitmap information.
        /// </summary>
        /// <param name="isPopup">if set to <c>true</c> [is popup].</param>
        /// <returns>BitmapInfo.</returns>
        /// <exception cref="System.Exception">BitmapFactory cannot be null</exception>
        BitmapInfo IRenderWebBrowser.CreateBitmapInfo(bool isPopup)
        {
            if (BitmapFactory == null)
            {
                throw new Exception("BitmapFactory cannot be null");
            }
            return BitmapFactory.CreateBitmap(isPopup, matrix.M11);
        }

        /// <summary>
        /// Starts the dragging.
        /// </summary>
        /// <param name="dragData">The drag data.</param>
        /// <param name="mask">The mask.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool IRenderWebBrowser.StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y)
        {
            var dataObject = new DataObject();

            dataObject.SetText(dragData.FragmentText, TextDataFormat.Text);
            dataObject.SetText(dragData.FragmentText, TextDataFormat.UnicodeText);
            dataObject.SetText(dragData.FragmentHtml, TextDataFormat.Html);

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
                    var results = DragDrop.DoDragDrop(this, dataObject, GetDragEffects(mask));
                    browser.GetHost().DragSourceEndedAt(0, 0, GetDragOperationsMask(results));
                    browser.GetHost().DragSourceSystemDragEnded();
                }
            });

            return true;
        }

        /// <summary>
        /// Invokes the render asynchronous.
        /// </summary>
        /// <param name="bitmapInfo">The bitmap information.</param>
        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            UiThreadRunAsync(delegate
            {
                lock (bitmapInfo.BitmapLock)
                {
                    var wpfBitmapInfo = (WpfBitmapInfo)bitmapInfo;
                    // Inform parents that the browser rendering is updating
                    OnRendering(this, wpfBitmapInfo);

                    // Now update the WPF image
                    if (wpfBitmapInfo.CreateNewBitmap)
                    {
                        var img = bitmapInfo.IsPopup ? popupImage : image;

                        img.Source = null;
                        GC.Collect(1);

                        img.Source = wpfBitmapInfo.CreateBitmap();
                    }

                    wpfBitmapInfo.Invalidate();
                }
            },
            DispatcherPriority.Render);
        }

        /// <summary>
        /// Sets the popup size and position.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        void IRenderWebBrowser.SetPopupSizeAndPosition(int width, int height, int x, int y)
        {
            UiThreadRunAsync(() => SetPopupSizeAndPositionImpl(width, height, x, y));
        }

        /// <summary>
        /// Sets the popup is open.
        /// </summary>
        /// <param name="isOpen">if set to <c>true</c> [is open].</param>
        void IRenderWebBrowser.SetPopupIsOpen(bool isOpen)
        {
            UiThreadRunAsync(() => { popup.IsOpen = isOpen; });
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="type">The type.</param>
        void IRenderWebBrowser.SetCursor(IntPtr handle, CefCursorType type)
        {
            UiThreadRunAsync(() =>
            {
                Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false));
            });
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
            browserInitialized = true;
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
            if (ignoreUriChange || newValue == null || !browserInitialized)
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

            var handlers = owner.IsBrowserInitializedChanged;

            if (handlers != null)
            {
                handlers(owner, e);
            }
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

        public event DependencyPropertyChangedEventHandler TitleChanged;

        /// <summary>
        /// Handles the <see cref="E:TitleChanged" /> event.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (ChromiumWebBrowser)d;

            var handlers = owner.TitleChanged;

            if (handlers != null)
            {
                handlers(owner, e);
            }
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

            timer.Stop();

            if (String.IsNullOrEmpty(TooltipText))
            {
                UiThreadRunAsync(() => UpdateTooltip(null), DispatcherPriority.Render);
            }
            else
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
                browser.GetHost().DragTargetDragDrop(GetMouseEvent(e));
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
                browser.GetHost().DragTargetDragEnter(e.GetDragDataWrapper(), GetMouseEvent(e), GetDragOperationsMask(e.AllowedEffects));
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
                    var notifyDpiChanged = !matrix.Equals(source.CompositionTarget.TransformToDevice);

                    matrix = source.CompositionTarget.TransformToDevice;
                    sourceHook = SourceHook;
                    source.AddHook(sourceHook);

                    if (notifyDpiChanged)
                    {
                        if(browser != null)
                        {
                            browser.GetHost().NotifyScreenInfoChanged();
                        }
                    }
                }
            }
            else if (args.OldSource != null)
            {
                RemoveSourceHook();
            }
        }

        /// <summary>
        /// Removes the source hook.
        /// </summary>
        private void RemoveSourceHook()
        {
            if (source != null && sourceHook != null)
            {
                source.RemoveHook(sourceHook);
                source = null;
            }
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
            if (browserCreated || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) || size.IsEmpty || size.Equals(new Size(0, 0)))
            {
                return false;
            }

            var webBrowserInternal = this as IWebBrowserInternal;
            if (!webBrowserInternal.HasParent)
            {
                managedCefBrowserAdapter.CreateOffscreenBrowser(source == null ? IntPtr.Zero : source.Handle, BrowserSettings, RequestContext, Address);
            }
            browserCreated = true;

            return true;
        }

        /// <summary>
        /// UIs the thread run asynchronous.
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
                browser.GetHost().WasHidden(!isVisible);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ApplicationExit" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExitEventArgs"/> instance containing the event data.</param>
        private static void OnApplicationExit(object sender, ExitEventArgs e)
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
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call
        /// <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Create main window
            Content = image = CreateImage();

            popup = CreatePopup();
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
            };

            newPopup.MouseEnter += PopupMouseEnter;
            newPopup.MouseLeave += PopupMouseLeave;

            return newPopup;
        }

        /// <summary>
        /// Sources the hook.
        /// </summary>
        /// <param name="hWnd">The hWnd.</param>
        /// <param name="message">The message.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <param name="handled">if set to <c>true</c> [handled].</param>
        /// <returns>IntPtr.</returns>
        protected virtual IntPtr SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (handled)
            {
                return IntPtr.Zero;
            }

            switch ((WM)message)
            {
                case WM.SYSCHAR:
                case WM.SYSKEYDOWN:
                case WM.SYSKEYUP:
                case WM.KEYDOWN:
                case WM.KEYUP:
                case WM.CHAR:
                case WM.IME_CHAR:
                { 
                    if (!IsKeyboardFocused)
                    {
                        break;
                    }

                    if (message == (int)WM.SYSKEYDOWN &&
                        wParam.ToInt32() == KeyInterop.VirtualKeyFromKey(Key.F4))
                    {
                        // We don't want CEF to receive this event (and mark it as handled), since that makes it impossible to
                        // shut down a CefSharp-based app by pressing Alt-F4, which is kind of bad.
                        return IntPtr.Zero;
                    }

                    if (browser != null)
                    {
                        browser.GetHost().SendKeyEvent(message, wParam.CastToInt32(), lParam.CastToInt32());    
                        handled = true;
                    }

                    break;
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Converts a .NET Drag event to a CefSharp MouseEvent
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        /// <returns>MouseEvent.</returns>
        private MouseEvent GetMouseEvent(DragEventArgs e)
        {
            var point = e.GetPosition(this);

            return new MouseEvent
            {
                X = (int)point.X,
                Y = (int)point.Y,
                //Modifiers = modifiers // TODO: Add support for modifiers in drag events (might not be need as it can be accessed via the mouse events)
            };
        }

        /// <summary>
        /// Sets the popup size and position implementation.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void SetPopupSizeAndPositionImpl(int width, int height, int x, int y)
        {
            popup.Width = width ;
            popup.Height = height;

            var popupOffset = new Point(x, y);
            var locationFromScreen = PointToScreen(popupOffset);
            popup.HorizontalOffset = locationFromScreen.X / matrix.M11;
            popup.VerticalOffset = locationFromScreen.Y / matrix.M22;
        }

        /// <summary>
        /// Handles the <see cref="E:TooltipTimerTick" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTooltipTimerTick(object sender, EventArgs e)
        {
            tooltipTimer.Stop();

            UpdateTooltip(TooltipText);
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
                OnPreviewKey(e);
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
                OnPreviewKey(e);
            }

            base.OnPreviewKeyUp(e);
        }

        /// <summary>
        /// Handles the <see cref="E:PreviewKey" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnPreviewKey(KeyEventArgs e)
        {
            // As KeyDown and KeyUp bubble, it appears they're being handled before they get a chance to
            // trigger the appropriate WM_ messages handled by our SourceHook, so we have to handle these extra keys here.
            // Hooking the Tab key like this makes the tab focusing in essence work like
            // KeyboardNavigation.TabNavigation="Cycle"; you will never be able to Tab out of the web browser control.
            // We also add the condition to allow ctrl+a to work when the web browser control is put inside listbox.
            if (e.Key == Key.Tab || e.Key == Key.Home || e.Key == Key.End || e.Key == Key.Up
                                 || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right
                                 || (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control))
            {
                var modifiers = e.GetModifiers();
                var message = (int)(e.IsDown ? WM.KEYDOWN : WM.KEYUP);
                var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);

                if(browser != null)
                {
                    browser.GetHost().SendKeyEvent(message, virtualKey, (int)modifiers);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (browser != null)
            {
                var point = e.GetPosition(this);
                var modifiers = e.GetModifiers();

                browser.GetHost().SendMouseMoveEvent((int)point.X, (int)point.Y, false, modifiers);
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseWheel" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (browser != null)
            {
                var point = e.GetPosition(this);
                var modifiers = e.GetModifiers();

                browser.SendMouseWheelEvent(
                    (int)point.X,
                    (int)point.Y,
                    deltaX: 0,
                    deltaY: e.Delta,
                    modifiers: modifiers);
            }
        }

        /// <summary>
        /// Popups the mouse enter.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected void PopupMouseEnter(object sender, MouseEventArgs e)
        {
            Focus();
            Mouse.Capture(this);
        }

        /// <summary>
        /// Popups the mouse leave.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected void PopupMouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.Capture(null);
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
            Mouse.Capture(this);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp" /> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the mouse button was released.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            OnMouseButton(e);
            Mouse.Capture(null);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> attached event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (browser != null)
            {
                var modifiers = e.GetModifiers();

                browser.GetHost().SendMouseMoveEvent(-1, -1, true, modifiers);

                ((IWebBrowserInternal)this).SetTooltipText(null);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:MouseButton" /> event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnMouseButton(MouseButtonEventArgs e)
        {
            // Cef currently only supports Left, Middle and Right button presses.
            if (e.ChangedButton > MouseButton.Right)
            {
                return;
            }

            if (browser != null)
            {
                var modifiers = e.GetModifiers();
                var mouseUp = (e.ButtonState == MouseButtonState.Released);
                var point = e.GetPosition(this);

                browser.GetHost().SendMouseClickEvent((int)point.X, (int)point.Y, (MouseButtonType)e.ChangedButton, mouseUp, e.ClickCount, modifiers);

                e.Handled = true;
            }
        }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        public void Load(string url)
        {
            // Added null check -> binding-triggered changes of Address will lead to a nullref after Dispose has been called
            // or before OnApplyTemplate has been called
            if (browser != null)
            {
                if (tooltipTimer != null)
                {
                    tooltipTimer.Tick -= OnTooltipTimerTick;
                }

                // TODO: Consider making the delay here configurable.
                tooltipTimer = new DispatcherTimer(
                    TimeSpan.FromSeconds(0.5),
                    DispatcherPriority.Render,
                    OnTooltipTimerTick,
                    Dispatcher
                    );

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
        /// Zooms the browser reset.
        /// </summary>
        private void ZoomReset()
        {
            UiThreadRunAsync(() =>
            {
                ZoomLevel = 0;
            });
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
            if (browserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }

            //Enable WCF if not already enabled
            CefSharpSettings.WcfEnabled = true;

            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind, options);
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
            if (browserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            managedCefBrowserAdapter.RegisterAsyncJsObject(name, objectToBind, options);
        }

        /// <summary>
        /// Raises Rendering event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="bitmapInfo">The bitmap information.</param>
        protected virtual void OnRendering(object sender, WpfBitmapInfo bitmapInfo)
        {
            var rendering = Rendering;
            if (rendering != null)
            {
                rendering(sender, new RenderingEventArgs(bitmapInfo));
            }
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
            get { return disposeCount > 0; }
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
