// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly Key[] KeysToSendtoBrowser =
        {
            Key.Tab,
            Key.Home, Key.End,
            Key.Left, Key.Right,
            Key.Up, Key.Down
        };

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
        private ScaleTransform dpiTransform;
        private readonly List<IDisposable> disposables = new List<IDisposable>();

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
        public IResourceHandler ResourceHandler { get; set; }

        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<NavStateChangedEventArgs> NavStateChanged;

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

        int IRenderWebBrowser.BytesPerPixel
        {
            get { return PixelFormat.BitsPerPixel / 8; }
        }

        int IRenderWebBrowser.Width
        {
            get { return (int)matrix.Transform(new Point(ActualWidth, ActualHeight)).X; }
        }

        int IRenderWebBrowser.Height
        {
            get { return (int)matrix.Transform(new Point(ActualWidth, ActualHeight)).Y; }
        }

        private static PixelFormat PixelFormat
        {
            get { return PixelFormats.Bgra32; }
        }

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
            if (!Cef.IsInitialized)
            {
                throw new InvalidOperationException("Cef::IsInitialized is false");
            }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isdisposing)
        {
            ResourceHandler = null;

            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;

            GotKeyboardFocus -= OnGotKeyboardFocus;
            LostKeyboardFocus -= OnLostKeyboardFocus;

            IsVisibleChanged -= OnIsVisibleChanged;

            Cef.RemoveDisposable(this);

            RemoveSourceHook();

            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
            disposables.Clear();

            UiThreadRunAsync(() => WebBrowser = null);
            managedCefBrowserAdapter = null;
            ConsoleMessage = null;
            FrameLoadStart = null;
            FrameLoadEnd = null;
            LoadError = null;
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
            Cef.AddDisposable(this);
            Focusable = true;
            FocusVisualStyle = null;
            IsTabStop = true;

            Dispatcher.BeginInvoke((Action)(() => WebBrowser = this));

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            GotKeyboardFocus += OnGotKeyboardFocus;
            LostKeyboardFocus += OnLostKeyboardFocus;

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

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this);

            disposables.Add(managedCefBrowserAdapter);
            disposables.Add(new DisposableEventWrapper(this, ActualHeightProperty, OnActualSizeChanged));
            disposables.Add(new DisposableEventWrapper(this, ActualWidthProperty, OnActualSizeChanged));

            ResourceHandler = new DefaultResourceHandler();
        }

        ~ChromiumWebBrowser()
        {
            Dispose(false);
        }

        private void CreateOffscreenBrowserWhenActualSizeChanged()
        {
            if (browserCreated || System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            managedCefBrowserAdapter.CreateOffscreenBrowser(BrowserSettings ?? new BrowserSettings());
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
            // If the control was not rendered yet when we tried to set up the source hook, it may have failed (since it couldn't
            // lookup the HwndSource), so we need to retry it whenever visibility changes.
            AddSourceHookIfNotAlreadyPresent();
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

            AddSourceHookIfNotAlreadyPresent();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            RemoveSourceHook();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AddSourceHookIfNotAlreadyPresent();

            CheckIsNonStandardDpi();

            // Create main window
            Content = image = CreateImage();
            Transform(image);

            popup = CreatePopup();
            Transform(popup);
        }

        private static Image CreateImage()
        {
            var temp = new Image();

            RenderOptions.SetBitmapScalingMode(temp, BitmapScalingMode.NearestNeighbor);

            temp.Stretch = Stretch.None;
            temp.HorizontalAlignment = HorizontalAlignment.Left;
            temp.VerticalAlignment = VerticalAlignment.Top;
            return temp;
        }

        private Popup CreatePopup()
        {
            var newPopup = new Popup
            {
                Child = popupImage = CreateImage(),
                PlacementTarget = this,
                Placement = PlacementMode.Relative,
            };

            newPopup.MouseEnter += PopupMouseEnter;
            newPopup.MouseLeave += PopupMouseLeave;

            return newPopup;
        }

        private void Transform(FrameworkElement element)
        {
            if (dpiTransform != null)
            {
                element.LayoutTransform = dpiTransform;
            }
        }

        private void CheckIsNonStandardDpi()
        {
            dpiTransform = new ScaleTransform(
                   1 / matrix.M11,
                   1 / matrix.M22
               );
        }

        private void AddSourceHookIfNotAlreadyPresent()
        {
            if (source != null)
            {
                return;
            }

            source = (HwndSource)PresentationSource.FromVisual(this);

            if (source != null)
            {
                matrix = source.CompositionTarget.TransformToDevice;
                sourceHook = SourceHook;
                source.AddHook(sourceHook);
            }
        }

        private void RemoveSourceHook()
        {
            if (source != null &&
                sourceHook != null)
            {
                source.RemoveHook(sourceHook);
                source = null;
            }
        }

        private IntPtr SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;

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

                    if (managedCefBrowserAdapter.SendKeyEvent(message, wParam.ToInt32(), 0))
                    {
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
        }

        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            IRenderWebBrowser renderer = this;
            UiThreadRunAsync(() => renderer.SetBitmap(bitmapInfo), DispatcherPriority.Render);
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
            UiThreadRunAsync(() => SetCurrentValue(IsLoadingProperty, isLoading));
        }

        void IWebBrowserInternal.SetNavState(bool canGoBack, bool canGoForward, bool canReload)
        {
            UiThreadRunAsync(() =>
            {
                SetCurrentValue(CanGoBackProperty, canGoBack);
                SetCurrentValue(CanGoForwardProperty, canGoForward);
                SetCurrentValue(CanReloadProperty, canReload);

                RaiseCommandsCanExecuteChanged();
            });

            var handler = NavStateChanged;
            if (handler != null)
            {
                handler(this, new NavStateChangedEventArgs(canGoBack, canGoForward, canReload));
            }
        }

        private void RaiseCommandsCanExecuteChanged()
        {
            ((DelegateCommand)BackCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ForwardCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ReloadCommand).RaiseCanExecuteChanged();
        }

        void IWebBrowserInternal.SetTitle(string title)
        {
            UiThreadRunAsync(() => SetCurrentValue(TitleProperty, title));
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            UiThreadRunAsync(() => SetCurrentValue(TooltipTextProperty, tooltipText));
        }

        void IRenderWebBrowser.SetPopupSizeAndPosition(int width, int height, int x, int y)
        {
            UiThreadRunAsync(() => SetPopupSizeAndPositionImpl(width, height, x, y));
        }

        void IRenderWebBrowser.SetPopupIsOpen(bool isOpen)
        {
            UiThreadRunAsync(() => { popup.IsOpen = isOpen; });
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
            popup.Width = width;
            popup.Height = height;

            var popupOffset = new Point(x, y);
            popup.HorizontalOffset = popupOffset.X / matrix.M11;
            popup.VerticalOffset = popupOffset.Y / matrix.M22;
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
            OnPreviewKey(e);
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            OnPreviewKey(e);
        }

        private void OnPreviewKey(KeyEventArgs e)
        {
            // For some reason, not all kinds of keypresses triggers the appropriate WM_ messages handled by our SourceHook, so
            // we have to handle these extra keys here. Hooking the Tab key like this makes the tab focusing in essence work like
            // KeyboardNavigation.TabNavigation="Cycle"; you will never be able to Tab out of the web browser control.
            var modifiers = GetModifiers(e);

            if (KeysToSendtoBrowser.Contains(e.Key) || modifiers > 0)
            {
                var message = (int)(e.IsDown ? WM.KEYDOWN : WM.KEYUP);
                var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);

                managedCefBrowserAdapter.SendKeyEvent(message, virtualKey, modifiers);

                if (KeysToSendtoBrowser.Contains(e.Key))
                {
                    e.Handled = true;
                }

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
            MouseButtonType mouseButtonType;

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    mouseButtonType = MouseButtonType.Left;
                    break;

                case MouseButton.Middle:
                    mouseButtonType = MouseButtonType.Middle;
                    break;

                case MouseButton.Right:
                    mouseButtonType = MouseButtonType.Right;
                    break;

                default:
                    return;
            }

            var modifiers = GetModifiers(e);
            var mouseUp = (e.ButtonState == MouseButtonState.Released);
            var point = GetPixelPosition(e);

            if (managedCefBrowserAdapter != null)
            {
                managedCefBrowserAdapter.OnMouseButton((int)point.X, (int)point.Y, mouseButtonType, mouseUp, e.ClickCount, modifiers);
            }
        }

        void IWebBrowserInternal.OnInitialized()
        {
            UiThreadRunAsync(() => SetCurrentValue(IsBrowserInitializedProperty, true));
        }

        public void Load(string url)
        {
            if (!Cef.IsInitialized &&
                !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            //Added null check -> binding-triggered changes of Address will lead to a nullref after Dispose has been called.
            if (managedCefBrowserAdapter != null)
            {
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
            var handler = ResourceHandler;
            if (handler == null)
            {
                throw new Exception("Implement IResourceHandler and assign to the ResourceHandler property to use this feature");
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

        public void RegisterJsObject(string name, object objectToBind)
        {
            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind);
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

        void IRenderWebBrowser.SetCursor(IntPtr handle)
        {
            UiThreadRunAsync(() =>
            {
                Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false));
            });
        }

        void IRenderWebBrowser.ClearBitmap(BitmapInfo bitmapInfo)
        {
            bitmapInfo.InteropBitmap = null;
        }

        void IRenderWebBrowser.SetBitmap(BitmapInfo bitmapInfo)
        {
            lock (bitmapInfo.BitmapLock)
            {
                if (bitmapInfo.IsPopup)
                {
                    bitmapInfo.InteropBitmap = SetBitmapHelper(bitmapInfo,
                        (InteropBitmap)bitmapInfo.InteropBitmap, bitmap => popupImage.Source = bitmap);
                }
                else
                {
                    bitmapInfo.InteropBitmap = SetBitmapHelper(bitmapInfo,
                        (InteropBitmap)bitmapInfo.InteropBitmap, bitmap => image.Source = bitmap);
                }
            }
        }

        private Point GetPixelPosition(MouseEventArgs e)
        {
            var deviceIndependentPosition = e.GetPosition(this);
            var pixelPosition = matrix.Transform(deviceIndependentPosition);

            return pixelPosition;
        }

        private object SetBitmapHelper(BitmapInfo bitmapInfo, InteropBitmap bitmap, Action<InteropBitmap> imageSourceSetter)
        {
            if (bitmap == null)
            {
                imageSourceSetter(null);
                GC.Collect(1);

                var stride = bitmapInfo.Width * ((IRenderWebBrowser)this).BytesPerPixel;

                bitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(bitmapInfo.FileMappingHandle,
                    bitmapInfo.Width, bitmapInfo.Height, PixelFormat, stride, 0);
                imageSourceSetter(bitmap);
            }

            bitmap.Invalidate();

            return bitmap;
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
