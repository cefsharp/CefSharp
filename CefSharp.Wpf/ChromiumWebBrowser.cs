// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using CefSharp.Wpf.Rendering;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace CefSharp.Wpf
{
    public class ChromiumWebBrowser : ContentControl, IRenderWebBrowser, IWpfWebBrowser
    {
        private readonly List<IDisposable> disposables = new List<IDisposable>();
        private readonly ScaleTransform imageTransform;

        private HwndSource source;
        private HwndSourceHook sourceHook;
        private DispatcherTimer tooltipTimer;
        private readonly ToolTip toolTip;
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;
        private bool ignoreUriChange;
        private bool browserCreated;
        private Matrix matrix;
        private Image image;
        private Image popupImage;
        private Popup popup;

        public BrowserSettings BrowserSettings { get; set; }
        public IDialogHandler DialogHandler { get; set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IMenuHandler MenuHandler { get; set; }
        public IFocusHandler FocusHandler { get; set; }
        public IDragHandler DragHandler { get; set; }
        public IResourceHandlerFactory ResourceHandlerFactory { get; set; }
        public IGeolocationHandler GeolocationHandler { get; set; }
        public IBitmapFactory BitmapFactory { get; set; }

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<NavStateChangedEventArgs> NavStateChanged;

        /// <summary>
        /// Raised before each render cycle, and allows you to adjust the bitmap before it's rendered/applied
        /// </summary>
        public event EventHandler<RenderingEventArgs> Rendering;

        public ICommand BackCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }
        public ICommand ReloadCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }
        public ICommand ZoomInCommand { get; private set; }
        public ICommand ZoomOutCommand { get; private set; }
        public ICommand ZoomResetCommand { get; private set; }
        public ICommand ViewSourceCommand { get; private set; }
        public ICommand CleanupCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand CutCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand SelectAllCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        static ChromiumWebBrowser()
        {
            var app = Application.Current;

            if (app != null)
            {
                app.Exit += OnApplicationExit;
            }
        }

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

            BackCommand = new DelegateCommand(Back, () => CanGoBack);
            ForwardCommand = new DelegateCommand(Forward, () => CanGoForward);
            ReloadCommand = new DelegateCommand(Reload, () => CanReload);
            PrintCommand = new DelegateCommand(Print);
            ZoomInCommand = new DelegateCommand(ZoomIn);
            ZoomOutCommand = new DelegateCommand(ZoomOut);
            ZoomResetCommand = new DelegateCommand(ZoomReset);
            ViewSourceCommand = new DelegateCommand(ViewSource);
            CleanupCommand = new DelegateCommand(Dispose);
            StopCommand = new DelegateCommand(Stop);
            CutCommand = new DelegateCommand(Cut);
            CopyCommand = new DelegateCommand(Copy);
            PasteCommand = new DelegateCommand(Paste);
            SelectAllCommand = new DelegateCommand(SelectAll);
            UndoCommand = new DelegateCommand(Undo);
            RedoCommand = new DelegateCommand(Redo);

            imageTransform = new ScaleTransform();
            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, true);

            disposables.Add(managedCefBrowserAdapter);
            disposables.Add(new DisposableEventWrapper(this, ActualHeightProperty, OnActualSizeChanged));
            disposables.Add(new DisposableEventWrapper(this, ActualWidthProperty, OnActualSizeChanged));

            ResourceHandlerFactory = new DefaultResourceHandlerFactory();
            BrowserSettings = new BrowserSettings();

            PresentationSource.AddSourceChangedHandler(this, PresentationSourceChangedHandler);
        }

        ~ChromiumWebBrowser()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isdisposing)
        {
            // No longer reference event listeners:
            ConsoleMessage = null;
            FrameLoadStart = null;
            FrameLoadEnd = null;
            LoadError = null;
            NavStateChanged = null;

            // No longer reference handlers:
            ResourceHandlerFactory = null;
            DialogHandler = null;
            JsDialogHandler = null;
            KeyboardHandler = null;
            RequestHandler = null;
            DownloadHandler = null;
            LifeSpanHandler = null;
            MenuHandler = null;
            FocusHandler = null;
            DragHandler = null;
            GeolocationHandler = null;
            Rendering = null;

            if (isdisposing)
            {
                if (BrowserSettings != null)
                {
                    BrowserSettings.Dispose();
                    BrowserSettings = null;
                }

                PresentationSource.RemoveSourceChangedHandler(this, PresentationSourceChangedHandler);

                // Release internal event listeners:
                Loaded -= OnLoaded;
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

                foreach (var disposable in disposables)
                {
                    disposable.Dispose();
                }
                disposables.Clear();
                UiThreadRunAsync(() => WebBrowser = null);
            }

            Cef.RemoveDisposable(this);

            RemoveSourceHook();

            managedCefBrowserAdapter = null;
        }

        ScreenInfo IRenderWebBrowser.GetScreenInfo()
        {
            var screenInfo = new ScreenInfo();

            var point = matrix.Transform(new Point(ActualWidth, ActualHeight));

            screenInfo.Width = (int)point.X;
            screenInfo.Height = (int)point.Y;
            screenInfo.ScaleFactor = (float)matrix.M11;

            return screenInfo;
        }

        BitmapInfo IRenderWebBrowser.CreateBitmapInfo(bool isPopup)
        {
            if (BitmapFactory == null)
            {
                throw new Exception("BitmapFactory cannot be null");
            }
            return BitmapFactory.CreateBitmap(isPopup);
        }

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

        void IRenderWebBrowser.SetPopupSizeAndPosition(int width, int height, int x, int y)
        {
            UiThreadRunAsync(() => SetPopupSizeAndPositionImpl(width, height, x, y));
        }

        void IRenderWebBrowser.SetPopupIsOpen(bool isOpen)
        {
            UiThreadRunAsync(() => { popup.IsOpen = isOpen; });
        }

        void IRenderWebBrowser.SetCursor(IntPtr handle, CefCursorType type)
        {
            UiThreadRunAsync(() =>
            {
                Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false));
            });
        }

        void IWebBrowserInternal.SetAddress(string address)
        {
            UiThreadRunAsync(() =>
            {
                ignoreUriChange = true;
                SetCurrentValue(AddressProperty, address);
                ignoreUriChange = false;

                // The tooltip should obviously also be reset (and hidden) when the address changes.
                SetCurrentValue(TooltipTextProperty, null);
            });
        }

        void IWebBrowserInternal.SetIsLoading(bool isLoading)
        {
            
        }

        void IWebBrowserInternal.SetLoadingStateChange(bool canGoBack, bool canGoForward, bool isLoading)
        {
            UiThreadRunAsync(() =>
            {
                SetCurrentValue(CanGoBackProperty, canGoBack);
                SetCurrentValue(CanGoForwardProperty, canGoForward);
                SetCurrentValue(CanReloadProperty, !isLoading);
                SetCurrentValue(IsLoadingProperty, isLoading);

                ((DelegateCommand)BackCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)ForwardCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)ReloadCommand).RaiseCanExecuteChanged();
            });

            var handler = NavStateChanged;
            if (handler != null)
            {
                handler(this, new NavStateChangedEventArgs(canGoBack, canGoForward, isLoading));
            }
        }

        void IWebBrowserInternal.SetTitle(string title)
        {
            UiThreadRunAsync(() => SetCurrentValue(TitleProperty, title));
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            UiThreadRunAsync(() => SetCurrentValue(TooltipTextProperty, tooltipText));
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

        void IWebBrowserInternal.OnInitialized()
        {
            UiThreadRunAsync(() => SetCurrentValue(IsBrowserInitializedProperty, true));
        }

        #region CanGoBack dependency property

        public bool CanGoBack
        {
            get { return (bool)GetValue(CanGoBackProperty); }
        }

        public static DependencyProperty CanGoBackProperty = DependencyProperty.Register("CanGoBack", typeof(bool), typeof(ChromiumWebBrowser));

        #endregion

        #region CanGoForward dependency property

        public bool CanGoForward
        {
            get { return (bool)GetValue(CanGoForwardProperty); }
        }

        public static DependencyProperty CanGoForwardProperty = DependencyProperty.Register("CanGoForward", typeof(bool), typeof(ChromiumWebBrowser));

        #endregion

        #region CanReload dependency property

        public bool CanReload
        {
            get { return (bool)GetValue(CanReloadProperty); }
        }

        public static DependencyProperty CanReloadProperty = DependencyProperty.Register("CanReload", typeof(bool), typeof(ChromiumWebBrowser));

        #endregion

        #region Address dependency property

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(ChromiumWebBrowser),
                                        new UIPropertyMetadata(null, OnAddressChanged));

        private static void OnAddressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var owner = (ChromiumWebBrowser)sender;
            var oldValue = (string)args.OldValue;
            var newValue = (string)args.NewValue;

            owner.OnAddressChanged(oldValue, newValue);
        }

        protected virtual void OnAddressChanged(string oldValue, string newValue)
        {
            if (ignoreUriChange)
            {
                return;
            }

            Load(newValue);
        }

        #endregion Address dependency property

        #region IsLoading dependency property

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(ChromiumWebBrowser), new PropertyMetadata(false));

        #endregion IsLoading dependency property

        #region IsBrowserInitialized dependency property

        public bool IsBrowserInitialized
        {
            get { return (bool)GetValue(IsBrowserInitializedProperty); }
        }

        public static readonly DependencyProperty IsBrowserInitializedProperty =
            DependencyProperty.Register("IsBrowserInitialized", typeof(bool), typeof(ChromiumWebBrowser), new PropertyMetadata(false, OnIsBrowserInitializedChanged));

        public event DependencyPropertyChangedEventHandler IsBrowserInitializedChanged;

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

        protected virtual void OnIsBrowserInitializedChanged(bool oldValue, bool newValue)
        {
        }

        #endregion IsInitialized dependency property

        #region Title dependency property

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ChromiumWebBrowser), new PropertyMetadata(null, OnTitleChanged));

        public event DependencyPropertyChangedEventHandler TitleChanged;

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

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(ChromiumWebBrowser),
                                        new UIPropertyMetadata(0d, OnZoomLevelChanged));

        private static void OnZoomLevelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var owner = (ChromiumWebBrowser)sender;
            var oldValue = (double)args.OldValue;
            var newValue = (double)args.NewValue;

            owner.OnZoomLevelChanged(oldValue, newValue);
        }

        protected virtual void OnZoomLevelChanged(double oldValue, double newValue)
        {
            managedCefBrowserAdapter.SetZoomLevel(newValue);
        }

        #endregion ZoomLevel dependency property

        #region ZoomLevelIncrement dependency property

        /// <summary>
        /// Specifies the amount used to increase/decrease to ZoomLevel by
        /// By Default this value is 0.10
        /// </summary>
        public double ZoomLevelIncrement
        {
            get { return (double)GetValue(ZoomLevelIncrementProperty); }
            set { SetValue(ZoomLevelIncrementProperty, value); }
        }

        public static readonly DependencyProperty ZoomLevelIncrementProperty =
            DependencyProperty.Register("ZoomLevelIncrement", typeof(double), typeof(ChromiumWebBrowser), new PropertyMetadata(0.10));

        #endregion ZoomLevelIncrement dependency property

        #region CleanupElement dependency property

        /// <summary>
        /// The CleanupElement Controls when the BrowserResources will be cleand up.
        /// The ChromiumWebBrowser will register on Unloaded of the provided Element and dispose all resources when that handler is called.
        /// By default the cleanup element is the Window that contains the ChromiumWebBrowser.
        /// if you want cleanup to happen earlier provide another FrameworkElement.
        /// Be aware that this Control is not usable anymore after cleanup is done.
        /// </summary>
        /// <value>
        /// The cleanup element.
        /// </value>
        public FrameworkElement CleanupElement
        {
            get { return (FrameworkElement)GetValue(CleanupElementProperty); }
            set { SetValue(CleanupElementProperty, value); }
        }

        public static readonly DependencyProperty CleanupElementProperty =
            DependencyProperty.Register("CleanupElement", typeof(FrameworkElement), typeof(ChromiumWebBrowser), new PropertyMetadata(null, OnCleanupElementChanged));

        private static void OnCleanupElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var owner = (ChromiumWebBrowser)sender;
            var oldValue = (FrameworkElement)args.OldValue;
            var newValue = (FrameworkElement)args.NewValue;

            owner.OnCleanupElementChanged(oldValue, newValue);
        }

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

        private void OnCleanupElementUnloaded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        #endregion CleanupElement dependency property

        #region TooltipText dependency property

        public string TooltipText
        {
            get { return (string)GetValue(TooltipTextProperty); }
        }

        public static readonly DependencyProperty TooltipTextProperty =
            DependencyProperty.Register("TooltipText", typeof(string), typeof(ChromiumWebBrowser), new PropertyMetadata(null, (sender, e) => ((ChromiumWebBrowser)sender).OnTooltipTextChanged()));

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

        public IWebBrowser WebBrowser
        {
            get { return (IWebBrowser)GetValue(WebBrowserProperty); }
            set { SetValue(WebBrowserProperty, value); }
        }

        public static readonly DependencyProperty WebBrowserProperty =
            DependencyProperty.Register("WebBrowser", typeof(IWebBrowser), typeof(ChromiumWebBrowser), new UIPropertyMetadata(defaultValue: null));

        #endregion WebBrowser dependency property

        private void OnDrop(object sender, DragEventArgs e)
        {
            managedCefBrowserAdapter.OnDragTargetDragDrop(GetMouseEvent(e));
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            managedCefBrowserAdapter.OnDragTargetDragLeave();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            managedCefBrowserAdapter.OnDragTargetDragOver(GetMouseEvent(e), GetDragOperationsMask(e.AllowedEffects));
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            managedCefBrowserAdapter.OnDragTargetDragEnter(GetDragDataWrapper(e), GetMouseEvent(e), GetDragOperationsMask(e.AllowedEffects));
        }

        /// <summary>
        /// Converts .NET drag drop effects to CEF Drag Operations
        /// </summary>s
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

        private static CefDragDataWrapper GetDragDataWrapper(DragEventArgs e)
        {
            // Convert Drag Data
            var dragData = CefDragDataWrapper.Create();

            // Files            
            dragData.IsFile = e.Data.GetDataPresent(DataFormats.FileDrop);
            if (dragData.IsFile)
            {
                // As per documentation, we only need to specify FileNames, not FileName, when dragging into the browser (http://magpcss.org/ceforum/apidocs3/projects/(default)/CefDragData.html)
                foreach (var filePath in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    var displayName = Path.GetFileName(filePath);

                    dragData.AddFile(filePath.Replace("\\", "/"), displayName);
                }
            }

            // Link/Url
            var link = GetLink(e.Data);
            dragData.IsLink = !string.IsNullOrEmpty(link);
            if (dragData.IsLink)
            {
                dragData.LinkUrl = link;
            }

            // Text/HTML
            dragData.IsFragment = e.Data.GetDataPresent(DataFormats.Text);
            if (dragData.IsFragment)
            {
                dragData.FragmentText = (string)e.Data.GetData(DataFormats.Text);
                dragData.FragmentHtml = (string)e.Data.GetData(DataFormats.Html);
            }

            return dragData;
        }

        private static string GetLink(IDataObject data)
        {
            const string asciiUrlDataFormatName = "UniformResourceLocator";
            const string unicodeUrlDataFormatName = "UniformResourceLocatorW";

            // Try Unicode
            if (data.GetDataPresent(unicodeUrlDataFormatName))
            {
                // Try to read a Unicode URL from the data
                var unicodeUrl = ReadUrlFromDragDropData(data, unicodeUrlDataFormatName, Encoding.Unicode);
                if (unicodeUrl != null)
                {
                    return unicodeUrl;
                }
            }

            // Try ASCII
            if (data.GetDataPresent(asciiUrlDataFormatName))
            {
                // Try to read an ASCII URL from the data
                return ReadUrlFromDragDropData(data, asciiUrlDataFormatName, Encoding.ASCII);
            }

            // Not a valid link
            return null;
        }

        /// <summary>Reads a URL using a particular text encoding from drag-and-drop data.</summary>
        /// <param name="data">The drag-and-drop data.</param>
        /// <param name="urlDataFormatName">The data format name of the URL type.</param>
        /// <param name="urlEncoding">The text encoding of the URL type.</param>
        /// <returns>A URL, or <see langword="null"/> if <paramref name="data"/> does not contain a URL
        /// of the correct type.</returns>
        private static string ReadUrlFromDragDropData(IDataObject data, string urlDataFormatName, Encoding urlEncoding)
        {
            // Read the URL from the data
            string url;
            using (var urlStream = (Stream)data.GetData(urlDataFormatName))
            {
                using (TextReader reader = new StreamReader(urlStream, urlEncoding))
                {
                    url = reader.ReadToEnd();
                }
            }

            // URLs in drag/drop data are often padded with null characters so remove these
            return url.TrimEnd('\0');
        }

        private void PresentationSourceChangedHandler(object sender, SourceChangedEventArgs args)
        {
            if (args.NewSource != null)
            {
                var newSource = (HwndSource)args.NewSource;

                source = newSource;

                if (source != null)
                {
                    matrix = source.CompositionTarget.TransformToDevice;
                    sourceHook = SourceHook;
                    source.AddHook(sourceHook);

                    managedCefBrowserAdapter.NotifyScreenInfoChanged();
                    imageTransform.ScaleX = 1 / matrix.M11;
                    imageTransform.ScaleY = 1 / matrix.M22;
                }
            }
            else if (args.OldSource != null)
            {
                RemoveSourceHook();
            }
        }

        private void RemoveSourceHook()
        {
            if (source != null && sourceHook != null)
            {
                source.RemoveHook(sourceHook);
                source = null;
            }
        }

        private void CreateOffscreenBrowserWhenActualSizeChanged()
        {
            if (browserCreated || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            managedCefBrowserAdapter.CreateOffscreenBrowser(source == null ? IntPtr.Zero : source.Handle, BrowserSettings, Address);
            browserCreated = true;
        }

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

        private void OnActualSizeChanged(object sender, EventArgs e)
        {
            // Initialize RenderClientAdapter when WPF has calculated the actual size of current content.
            CreateOffscreenBrowserWhenActualSizeChanged();
            managedCefBrowserAdapter.WasResized();
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var isVisible = (bool)args.NewValue;
            managedCefBrowserAdapter.WasHidden(!isVisible);
        }

        private static void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Cef.Shutdown();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (CleanupElement == null)
            {
                CleanupElement = Window.GetWindow(this);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Create main window
            Content = image = CreateImage();

            popup = CreatePopup();
        }

        private Image CreateImage()
        {
            var img = new Image();

            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);

            img.Stretch = Stretch.None;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            //Scale Image based on DPI settings
            img.LayoutTransform = imageTransform;

            return img;
        }

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

                    handled = managedCefBrowserAdapter.SendKeyEvent(message, wParam.CastToInt32(), lParam.CastToInt32());

                    break;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Converts a .NET Drag event to a CefSharp MouseEvent
        /// </summary>
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

        private static CefEventFlags GetModifiers(MouseEventArgs e)
        {
            CefEventFlags modifiers = 0;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                modifiers |= CefEventFlags.LeftMouseButton;
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                modifiers |= CefEventFlags.MiddleMouseButton;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                modifiers |= CefEventFlags.RightMouseButton;
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                modifiers |= CefEventFlags.ControlDown | CefEventFlags.IsLeft;
            }

            if (Keyboard.IsKeyDown(Key.RightCtrl))
            {
                modifiers |= CefEventFlags.ControlDown | CefEventFlags.IsRight;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                modifiers |= CefEventFlags.ShiftDown | CefEventFlags.IsLeft;
            }

            if (Keyboard.IsKeyDown(Key.RightShift))
            {
                modifiers |= CefEventFlags.ShiftDown | CefEventFlags.IsRight;
            }

            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                modifiers |= CefEventFlags.AltDown | CefEventFlags.IsLeft;
            }

            if (Keyboard.IsKeyDown(Key.RightAlt))
            {
                modifiers |= CefEventFlags.AltDown | CefEventFlags.IsRight;
            }

            return modifiers;
        }

        private static CefEventFlags GetModifiers(KeyEventArgs e)
        {
            CefEventFlags modifiers = 0;

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                modifiers |= CefEventFlags.ShiftDown;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                modifiers |= CefEventFlags.AltDown;
            }

            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                modifiers |= CefEventFlags.ControlDown;
            }

            return modifiers;
        }

        private void SetPopupSizeAndPositionImpl(int width, int height, int x, int y)
        {
            popup.Width = width / matrix.M11;
            popup.Height = height / matrix.M22;

            var popupOffset = new Point(x / matrix.M11, y / matrix.M22);
            var locationFromScreen = PointToScreen(popupOffset);
            popup.HorizontalOffset = locationFromScreen.X / matrix.M11;
            popup.VerticalOffset = locationFromScreen.Y / matrix.M22;
        }

        private void OnTooltipTimerTick(object sender, EventArgs e)
        {
            tooltipTimer.Stop();

            UpdateTooltip(TooltipText);
        }

        private void OnTooltipClosed(object sender, RoutedEventArgs e)
        {
            toolTip.Visibility = Visibility.Collapsed;

            // Set Placement to something other than PlacementMode.Mouse, so that when we re-show the tooltip in
            // UpdateTooltip(), the tooltip will be repositioned to the new mouse point.
            toolTip.Placement = PlacementMode.Absolute;
        }

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

        private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.SendFocusEvent(true);
            }
        }

        private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.SendFocusEvent(false);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                OnPreviewKey(e);
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                OnPreviewKey(e);
            }

            base.OnPreviewKeyUp(e);
        }

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
                var modifiers = GetModifiers(e);
                var message = (int)(e.IsDown ? WM.KEYDOWN : WM.KEYUP);
                var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);

                e.Handled = managedCefBrowserAdapter.SendKeyEvent(message, virtualKey, (int)modifiers);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = GetPixelPosition(e);
            var modifiers = GetModifiers(e);

            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.OnMouseMove((int)point.X, (int)point.Y, false, modifiers);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var point = GetPixelPosition(e);

            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.OnMouseWheel(
                    (int)point.X,
                    (int)point.Y,
                    deltaX: 0,
                    deltaY: e.Delta
                    );
            }
        }

        public void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY)
        {
            managedCefBrowserAdapter.OnMouseWheel(x, y, deltaX, deltaY);
        }

        protected void PopupMouseEnter(object sender, MouseEventArgs e)
        {
            Focus();
            Mouse.Capture(this);
        }

        protected void PopupMouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.Capture(null);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            OnMouseButton(e);
            Mouse.Capture(this);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            OnMouseButton(e);
            Mouse.Capture(null);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            var modifiers = GetModifiers(e);

            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.OnMouseMove(-1, -1, true, modifiers);
            }
        }

        private void OnMouseButton(MouseButtonEventArgs e)
        {
            // Cef currently only supports Left, Middle and Right button presses.
            if (e.ChangedButton > MouseButton.Right)
            {
                return;
            }

            var modifiers = GetModifiers(e);
            var mouseUp = (e.ButtonState == MouseButtonState.Released);
            var point = GetPixelPosition(e);

            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.OnMouseButton((int)point.X, (int)point.Y, (int)e.ChangedButton, mouseUp, e.ClickCount, modifiers);
            }
        }

        public void Load(string url)
        {
            //Added null check -> binding-triggered changes of Address will lead to a nullref after Dispose has been called.
            if (managedCefBrowserAdapter != null)
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

                managedCefBrowserAdapter.LoadUrl(url);
            }
        }

        public void LoadHtml(string html, string url)
        {
            LoadHtml(html, url, Encoding.UTF8);
        }

        public void LoadHtml(string html, string url, Encoding encoding)
        {
            var handler = ResourceHandlerFactory;
            if (handler == null)
            {
                throw new Exception("Implement IResourceHandlerFactory and assign to the ResourceHandlerFactory property to use this feature");
            }

            handler.RegisterHandler(url, CefSharp.ResourceHandler.FromString(html, encoding, true));

            Load(url);
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

        public void Print()
        {
            managedCefBrowserAdapter.Print();
        }

        public void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            managedCefBrowserAdapter.Find(identifier, searchText, forward, matchCase, findNext);
        }

        public void StopFinding(bool clearSelection)
        {
            managedCefBrowserAdapter.StopFinding(clearSelection);
        }

        private void ZoomIn()
        {
            UiThreadRunAsync(() =>
            {
                ZoomLevel = ZoomLevel + ZoomLevelIncrement;
            });
        }

        private void ZoomOut()
        {
            UiThreadRunAsync(() =>
            {
                ZoomLevel = ZoomLevel - ZoomLevelIncrement;
            });
        }

        private void ZoomReset()
        {
            UiThreadRunAsync(() =>
            {
                ZoomLevel = 0;
            });
        }

        public void ShowDevTools()
        {
            managedCefBrowserAdapter.ShowDevTools();
        }

        public void CloseDevTools()
        {
            managedCefBrowserAdapter.CloseDevTools();
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

        /// <summary>
        /// Raises Rendering event
        /// </summary>
        protected virtual void OnRendering(object sender, WpfBitmapInfo bitmapInfo)
        {
            var rendering = Rendering;
            if (rendering != null)
            {
                rendering(sender, new RenderingEventArgs(bitmapInfo));
            }
        }

        private Point GetPixelPosition(MouseEventArgs e)
        {
            var deviceIndependentPosition = e.GetPosition(this);
            var pixelPosition = matrix.Transform(deviceIndependentPosition);

            return pixelPosition;
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

        public void ReplaceMisspelling(string word)
        {
            managedCefBrowserAdapter.ReplaceMisspelling(word);
        }

        public void AddWordToDictionary(string word)
        {
            managedCefBrowserAdapter.AddWordToDictionary(word);
        }

        /// <summary>
        /// Invalidate the view. The browser will call CefRenderHandler::OnPaint asynchronously.
        /// </summary>
        /// <param name="type">indicates which surface to re-paint either View or Popup.</param>
        public void Invalidate(PaintElementType type)
        {
            managedCefBrowserAdapter.Invalidate(type);
        }

        /// <summary>
        /// Change the zoom level to the specified value. Specify 0.0 to reset the zoom level.
        /// </summary>
        /// <param name="zoomLevel">zoom level</param>
        public void SetZoomLevel(double zoomLevel)
        {
            managedCefBrowserAdapter.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Sends a Key Event directly to the underlying Browser (CEF).
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="wParam">The wParam</param>
        /// <param name="lParam">The lParam</param>
        /// <returns>bool</returns>
        public bool SendKeyEvent(int message, int wParam, int lParam)
        {
            return managedCefBrowserAdapter.SendKeyEvent(message, wParam, lParam);
        }
    }
}
