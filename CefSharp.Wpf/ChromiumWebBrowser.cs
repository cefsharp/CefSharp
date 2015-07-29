﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using CefSharp.Wpf.Internals;
using CefSharp.Wpf.Rendering;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
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
        private volatile bool browserInitialized;
        private Matrix matrix;
        private Image image;
        private Image popupImage;
        private Popup popup;

        public BrowserSettings BrowserSettings { get; set; }
        public RequestContext RequestContext { get; set; }
        public IDialogHandler DialogHandler { get; set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IPopupHandler PopupHandler { get; set; }
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
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;

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

            BackCommand = new DelegateCommand(this.Back, () => CanGoBack);
            ForwardCommand = new DelegateCommand(this.Forward, () => CanGoForward);
            ReloadCommand = new DelegateCommand(this.Reload, () => CanReload);
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
            LoadingStateChanged = null;
            Rendering = null;

            // No longer reference handlers:
            this.SetHandlersToNull();

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

            var handler = LoadingStateChanged;
            if (handler != null)
            {
                handler(this, new LoadingStateChangedEventArgs(canGoBack, canGoForward, isLoading));
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

        void IWebBrowserInternal.OnLoadError(IFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            var handler = LoadError;
            if (handler != null)
            {
                handler(this, new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl));
            }
        }

        IBrowserAdapter IWebBrowserInternal.BrowserAdapter
        {
            get { return managedCefBrowserAdapter; }
        }

        bool IWebBrowserInternal.HasParent { get; set; }

        void IWebBrowserInternal.OnAfterBrowserCreated()
        {
            browserInitialized = true;

            UiThreadRunAsync(() =>
            {
                SetCurrentValue(IsBrowserInitializedProperty, true);

                // If Address was previously set, only now can we actually do the load
                if (!string.IsNullOrEmpty(Address))
                {
                    Load(Address);
                }
            });
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
            if (ignoreUriChange || newValue == null || !browserInitialized)
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
            var task = this.GetZoomLevelAsync();
            task.ContinueWith(previous =>
            {
                if (previous.IsCompleted)
                {
                    UiThreadRunAsync(() => SetCurrentValue(ZoomLevelProperty, previous.Result));
                }
                else
                {
                    throw new InvalidOperationException("Unexpected failure of calling CEF->GetZoomLevelAsync", previous.Exception);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
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
            this.SetZoomLevel(newValue);
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
            managedCefBrowserAdapter.OnDragTargetDragEnter(e.GetDragDataWrapper(), GetMouseEvent(e), GetDragOperationsMask(e.AllowedEffects));
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

            managedCefBrowserAdapter.CreateOffscreenBrowser(source == null ? IntPtr.Zero : source.Handle, BrowserSettings, RequestContext, Address);
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
                var modifiers = e.GetModifiers();
                var message = (int)(e.IsDown ? WM.KEYDOWN : WM.KEYUP);
                var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);

                e.Handled = managedCefBrowserAdapter.SendKeyEvent(message, virtualKey, (int)modifiers);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var browser = GetBrowser();

            if (browser != null)
            {
                var point = GetPixelPosition(e);
                var modifiers = e.GetModifiers();

                browser.GetHost().SendMouseMoveEvent((int)point.X, (int)point.Y, false, modifiers);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var point = GetPixelPosition(e);

            var browser = GetBrowser();

            if (browser != null)
            {
                browser.SendMouseWheelEvent(
                    (int)point.X,
                    (int)point.Y,
                    deltaX: 0,
                    deltaY: e.Delta,
                    modifiers: CefEventFlags.None);
            }
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
            var browser = GetBrowser();

            if (browser != null)
            {
                var modifiers = e.GetModifiers();

                browser.GetHost().SendMouseMoveEvent(-1, -1, true, modifiers);

                ((IWebBrowserInternal)this).SetTooltipText(null);
            }
        }

        private void OnMouseButton(MouseButtonEventArgs e)
        {
            // Cef currently only supports Left, Middle and Right button presses.
            if (e.ChangedButton > MouseButton.Right)
            {
                return;
            }

            var modifiers = e.GetModifiers();
            var mouseUp = (e.ButtonState == MouseButtonState.Released);
            var point = GetPixelPosition(e);

            var browser = GetBrowser();

            if (browser != null)
            {
                browser.GetHost().SendMouseClickEvent((int)point.X, (int)point.Y, (MouseButtonType)e.ChangedButton, mouseUp, e.ClickCount, modifiers);
            }
        }

        public void Load(string url)
        {
            // Added null check -> binding-triggered changes of Address will lead to a nullref after Dispose has been called
            // or before OnApplyTemplate has been called
            var browser = GetBrowser();
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

        public void RegisterJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            if (browserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind, camelCaseJavascriptNames);
        }

        public void RegisterAsyncJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            if (browserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            managedCefBrowserAdapter.RegisterAsyncJsObject(name, objectToBind, camelCaseJavascriptNames);
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

        public IBrowser GetBrowser()
        {
            return managedCefBrowserAdapter == null ? null : managedCefBrowserAdapter.GetBrowser();
        }
    }
}
