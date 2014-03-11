// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

using CefSharp.Internals;
using System.Windows.Data;

namespace CefSharp.Wpf
{
    public class WebView : ContentControl, IRenderWebBrowser, IWpfWebBrowser
    {
        private HwndSource _source;
        private HwndSourceHook _sourceHook;
        private DispatcherTimer _tooltipTimer;
        private readonly ToolTip _toolTip;
        private ManagedCefBrowserAdapter _managedCefBrowserAdapter;
        private bool _isOffscreenBrowserCreated;
        private bool _ignoreUriChange;

        private Image _image;
        private Image _popupImage;
        private Popup _popup;


        public BrowserSettings BrowserSettings { get; set; }
        public bool IsBrowserInitialized { get; private set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }


        public int BytesPerPixel
        {
            get { return PixelFormat.BitsPerPixel / 8; }
        }

        private static PixelFormat PixelFormat
        {
            get { return PixelFormats.Bgra32; }
        }

        #region Commands

        public ICommand BackCommand { get; private set; }

        private void Back()
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.GoBack();
            }
        }

        public ICommand ForwardCommand { get; private set; }

        private void Forward()
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.GoForward();
            }
        }

        public ICommand ReloadCommand { get; private set; }

        private void Reload()
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.Reload();
            }
        }

        public ICommand HomeCommand { get; private set; }

        private void GoHome()
        {
            SetCurrentValue( AddressProperty, HomeAddress );
        }

        #endregion

        #region Address dependency property

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(WebView),
                                        new UIPropertyMetadata(null, OnAdressChanged));

        private static void OnAdressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WebView owner = (WebView)sender;
            string oldValue = (string)args.OldValue;
            string newValue = (string)args.NewValue;

            owner.OnAddressChanged(oldValue, newValue);
        }

        protected virtual void OnAddressChanged(string oldValue, string newValue)
        {
            if (_ignoreUriChange)
            {
                return;
            }

            if (!Cef.Instance.IsInitialized &&
                !Cef.Instance.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            // TODO: Consider making the delay here configurable.
            _tooltipTimer = new DispatcherTimer(
                TimeSpan.FromSeconds(0.5),
                DispatcherPriority.Render,
                OnTooltipTimerTick,
                Dispatcher
            );

            if (_isOffscreenBrowserCreated && Address != null )
            {
                _managedCefBrowserAdapter.LoadUrl(Address);
            }
            else
            {
                InitializeCefAdapter();
            }
        }

        #endregion Address dependency property

        #region IsLoading dependency property

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(WebView), new PropertyMetadata(false));

        #endregion IsLoading dependency property

        #region Title dependency property

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WebView), new PropertyMetadata(defaultValue: null));

        #endregion Title dependency property

        #region TooltipText dependency property

        public string TooltipText
        {
            get { return (string)GetValue(TooltipTextProperty); }
            set { SetValue(TooltipTextProperty, value); }
        }

        public static readonly DependencyProperty TooltipTextProperty =
            DependencyProperty.Register("TooltipText", typeof(string), typeof(WebView), new PropertyMetadata(null, (sender, e) => ((WebView)sender).OnTooltipTextChanged()));

        private void OnTooltipTextChanged()
        {
            _tooltipTimer.Stop();

            if (String.IsNullOrEmpty(TooltipText))
            {
                Dispatcher.BeginInvoke((Action)(() => UpdateTooltip(null)), DispatcherPriority.Render);
            }
            else
            {
                _tooltipTimer.Start();
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
            DependencyProperty.Register("WebBrowser", typeof(IWebBrowser), typeof(WebView), new UIPropertyMetadata(defaultValue: null));

        #endregion WebBrowser dependency property

        #region HomeAddress dependency property

        public static readonly DependencyProperty HomeAddressProperty = DependencyProperty.Register(
            "HomeAddress", typeof (string), typeof (WebView), new PropertyMetadata(default(string), OnHomeAdressPropertyChanged));
        
        public string HomeAddress
        {
            get { return (string) GetValue(HomeAddressProperty); }
            set { SetValue(HomeAddressProperty, value); }
        }

        private static void OnHomeAdressPropertyChanged( DependencyObject sender, DependencyPropertyChangedEventArgs args )
        {
            WebView owner = (WebView) sender;
            string oldvalue = (string) args.OldValue;
            string newvalue = (string) args.NewValue;

            owner.OnHomeAdressPropertyChanged( oldvalue, newvalue );
        }

        protected virtual void OnHomeAdressPropertyChanged( string oldValue, string newValue )
        {
            if (!string.IsNullOrEmpty(newValue) && string.IsNullOrEmpty(Address))
            {
                SetCurrentValue( AddressProperty, newValue );
            }
        }

        #endregion

        #region CanGoBack

        public static DependencyProperty CanGoBackProperty = DependencyProperty.Register( "CanGoBack", typeof(bool), typeof(WebView) );
        public bool CanGoBack 
        {
            get { return (bool)GetValue(CanGoBackProperty); }
            private set { SetValue((DependencyProperty) CanGoBackProperty, value); }
        }

        #endregion

        #region CanGoForward

        public static DependencyProperty CanGoForwardProperty = DependencyProperty.Register("CanGoForward", typeof(bool), typeof(WebView));
        public bool CanGoForward
        {
            get { return (bool)GetValue(CanGoForwardProperty); }
            private set { SetValue((DependencyProperty) CanGoForwardProperty, value); }
        }

        #endregion

        #region CanReload

        public static DependencyProperty CanReloadProperty = DependencyProperty.Register("CanReload", typeof(bool), typeof(WebView));
        public bool CanReload
        {
            get { return (bool)GetValue(CanReloadProperty); }
            private set { SetValue((DependencyProperty) CanReloadProperty, value); }
        }

        #endregion

        #region lifetime

        static WebView()
        {
            Application.Current.Exit += OnApplicationExit;
        }
        
        public WebView()
        {
            Focusable = true;
            FocusVisualStyle = null;
            IsTabStop = true;

            WebBrowser = (IWebBrowser)this;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            this.IsVisibleChanged += OnIsVisibleChanged;

            ToolTip = _toolTip = new ToolTip();
            _toolTip.StaysOpen = true;
            _toolTip.Visibility = Visibility.Collapsed;
            _toolTip.Closed += OnTooltipClosed;

            BrowserSettings = new BrowserSettingsWrapper();

            var backCommand = new DelegateCommand(Back);
            var forwardCommand = new DelegateCommand(Forward);
            var reloadCommand = new DelegateCommand(Reload);
            var homeCommand = new ParameterCommand(GoHome);

            backCommand.SetBinding(DelegateCommand.CanExecuteValueProperty, new Binding(CanGoBackProperty.Name) { Source = this });
            forwardCommand.SetBinding(DelegateCommand.CanExecuteValueProperty, new Binding(CanGoForwardProperty.Name) { Source = this });
            reloadCommand.SetBinding(DelegateCommand.CanExecuteValueProperty, new Binding(CanReloadProperty.Name) { Source = this });
            homeCommand.SetBinding( ParameterCommand.ParameterProperty, new Binding( HomeAddressProperty.Name ) { Source = this } );

            var cangohomemapping = new MappingConverter();
            cangohomemapping.FallbackValue = null;
            cangohomemapping.Mappings.Add( new Mapping() { From = string.Empty, To = false } );
            cangohomemapping.Mappings.Add( new Mapping() { From = null, To = false } );

            homeCommand.SetBinding( DelegateCommand.CanExecuteValueProperty, new Binding( HomeAddressProperty.Name ) { Source = this, Converter = cangohomemapping } );

            BackCommand = backCommand;
            ForwardCommand = forwardCommand;
            ReloadCommand = reloadCommand;
            HomeCommand = homeCommand;

            Application.Current.MainWindow.Closed += OnInstanceApplicationExit;
        }

        private void OnInstanceApplicationExit(object sender, EventArgs e)
        {
            ShutdownManagedCefBrowserAdapter();
        }

        private static void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Cef.Instance.Dispose();
        }


        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            //(HwndSource) PresentationSource.FromVisual( this ); will fail if the control was not rendered yet so we need to try initialize if visibility changes 
            InitializeCefAdapter();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            InitializeCefAdapter();
        }

        public void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ShutdownManagedCefBrowserAdapter();
        }

        private void InitializeCefAdapter()
        {
            if (_isOffscreenBrowserCreated)
            {
                return;
            }

            if (!AddSourceHook())
            {
                return;
            }

            if (!CreateOffscreenBrowser())
            {
                return;
            }

            _isOffscreenBrowserCreated = true;
        }

        private void ShutdownManagedCefBrowserAdapter()
        {
            RemoveSourceHook();

            var temp = _managedCefBrowserAdapter;

            if (temp == null)
            {
                return;
            }

            _managedCefBrowserAdapter = null;
            _isOffscreenBrowserCreated = false;
            temp.Dispose();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeCefAdapter();

            Content = _image = new Image();
            _popup = CreatePopup();

            RenderOptions.SetBitmapScalingMode(_image, BitmapScalingMode.NearestNeighbor);

            _image.Stretch = Stretch.None;
            _image.HorizontalAlignment = HorizontalAlignment.Left;
            _image.VerticalAlignment = VerticalAlignment.Top;
        }

        private Popup CreatePopup()
        {
            var popup = new Popup
            {
                Child = _popupImage = new Image(),
                PlacementTarget = this,
                Placement = PlacementMode.Relative
            };

            return popup;
        }

        private bool CreateOffscreenBrowser()
        {
            if (Address == null || _source == null)
            {
                return false;
            }

            if (_isOffscreenBrowserCreated)
            {
                return true;
            }

            _managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this);
            _managedCefBrowserAdapter.CreateOffscreenBrowser(BrowserSettings, _source.Handle, Address);

            return true;
        }

        private bool AddSourceHook()
        {
            if (_source != null)
            {
                return true;
            }

            _source = (HwndSource)PresentationSource.FromVisual(this);

            if (_source != null)
            {
                _sourceHook = SourceHook;
                _source.AddHook(_sourceHook);
                return true;
            }

            return false;
        }

        private void RemoveSourceHook()
        {
            if (_source != null &&
                _sourceHook != null)
            {
                _source.RemoveHook(_sourceHook);
                _source = null;
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
                    if (!IsFocused)
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

                    if (_managedCefBrowserAdapter.SendKeyEvent(message, wParam.ToInt32()))
                    {
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var size = base.ArrangeOverride(arrangeBounds);
            var newWidth = size.Width;
            var newHeight = size.Height;

            if (newWidth > 0 && newHeight > 0 && _isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.WasResized();
            }

            return size;
        }

        #endregion

        #region IWebBrowser

        public event ConsoleMessageEventHandler ConsoleMessage;
        public event LoadCompletedEventHandler LoadCompleted;
        public event LoadErrorEventHandler LoadError;

        public void Load(string url)
        {
            throw new NotImplementedException();
        }

        public void LoadHtml(string html, string url)
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.LoadHtml(html, url);
            }
        }

        public void RegisterJsObject(string name, object objectToBind)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> BoundObjects { get; private set; }

        public void ExecuteScriptAsync(string script)
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.ExecuteScriptAsync(script);
            }
        }

        public object EvaluateScript(string script)
        {
            return EvaluateScript(script, timeout: null);
        }

        public object EvaluateScript(string script, TimeSpan? timeout)
        {
            if (timeout == null)
            {
                timeout = TimeSpan.MaxValue;
            }

            if (_isOffscreenBrowserCreated)
            {
                return _managedCefBrowserAdapter.EvaluateScript(script, timeout.Value);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region IRenderWebBrowser

        int IRenderWebBrowser.Width
        {
            get { return (int)ActualWidth; }
        }

        int IRenderWebBrowser.Height
        {
            get { return (int)ActualHeight; }
        }

        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            DoInUi(() => SetBitmap(bitmapInfo), DispatcherPriority.Render);
        }

        void IWebBrowserInternal.SetIsLoading( bool isLoading )
        {
            DoInUi( () => IsLoading = isLoading );
        }

        void IWebBrowserInternal.SetNavState( bool canGoBack, bool canGoForward, bool canReload )
        {
            DoInUi( () =>
            {
                CanGoBack = canGoBack;
                CanGoForward = canGoForward;
                CanReload = canReload;
            } );
        }
        
        void IRenderWebBrowser.SetPopupIsOpen( bool isOpen )
        {
            DoInUi( () => _popup.IsOpen = isOpen );
        }

        void IRenderWebBrowser.SetPopupSizeAndPosition( int width, int height, int x, int y )
        {
            DoInUi( () =>
            {
                _popup.Width = width;
                _popup.Height = height;

                var popupOffset = new Point( x, y );
                // TODO: Port over this from CefSharp1.
                //if (popupOffsetTransform != null) 
                //{
                //    popupOffset = popupOffsetTransform->GeneralTransform::Transform(popupOffset);
                //}

                _popup.HorizontalOffset = popupOffset.X;
                _popup.VerticalOffset = popupOffset.Y;
            } );
        }

        public void SetCursor(IntPtr handle)
        {
            DoInUi(() => Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false)));
        }

        public void ClearBitmap(BitmapInfo bitmapInfo)
        {
            lock (bitmapInfo._bitmapLock)
            {
                bitmapInfo.InteropBitmap = null;
            }
        }

        public void SetBitmap(BitmapInfo bitmapInfo)
        {
            lock (bitmapInfo._bitmapLock)
            {
                if (bitmapInfo.IsPopup)
                {
                    bitmapInfo.InteropBitmap = SetBitmapHelper(bitmapInfo, (InteropBitmap)bitmapInfo.InteropBitmap, bitmap => _popupImage.Source = bitmap);
                }
                else
                {
                    bitmapInfo.InteropBitmap = SetBitmapHelper(bitmapInfo, (InteropBitmap)bitmapInfo.InteropBitmap, bitmap => _image.Source = bitmap);
                }
            }
        }

        #endregion

        #region WebBrowserInternal

        void IWebBrowserInternal.SetAddress( string address )
        {
            DoInUi(() =>
            {
                _ignoreUriChange = true;
                Address = address;
                _ignoreUriChange = false;

                // The tooltip should obviously also be reset (and hidden) when the address changes.
                TooltipText = null;
            });
        }

        void IWebBrowserInternal.SetTitle( string title )
        {
            DoInUi( () => Title = title );
        }

        void IWebBrowserInternal.SetTooltipText( string tooltipText )
        {
            DoInUi( () => TooltipText = tooltipText );
        }

        void IWebBrowserInternal.OnInitialized()
        {
        }

        void IWebBrowserInternal.ShowDevTools()
        {
            // TODO: Do something about this one.
            var devToolsUrl = _managedCefBrowserAdapter.DevToolsUrl;
            throw new NotImplementedException();
        }

        void IWebBrowserInternal.CloseDevTools()
        {
            throw new NotImplementedException();
        }

        void IWebBrowserInternal.OnFrameLoadStart( string url )
        {
        }

        void IWebBrowserInternal.OnFrameLoadEnd( string url )
        {
            if ( LoadCompleted != null )
            {
                LoadCompleted( this, new LoadCompletedEventArgs( url ) );
            }
        }

        void IWebBrowserInternal.OnTakeFocus( bool next )
        {
            throw new NotImplementedException();
        }

        void IWebBrowserInternal.OnConsoleMessage( string message, string source, int line )
        {
            if ( ConsoleMessage != null )
            {
                ConsoleMessage( this, new ConsoleMessageEventArgs( message, source, line ) );
            }
        }

        public void OnLoadError( string url, CefErrorCode errorCode, string errorText )
        {
            if ( LoadError != null )
            {
                LoadError( url, errorCode, errorText );
            }
        }

        #endregion

        #region helpers

        private void DoInUi(Action action, DispatcherPriority priority = DispatcherPriority.DataBind)
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

        private void OnTooltipTimerTick( object sender, EventArgs e )
        {
            _tooltipTimer.Stop();

            UpdateTooltip( TooltipText );
        }

        private void OnTooltipClosed( object sender, RoutedEventArgs e )
        {
            _toolTip.Visibility = Visibility.Collapsed;

            // Set Placement to something other than PlacementMode.Mouse, so that when we re-show the tooltip in
            // UpdateTooltip(), the tooltip will be repositioned to the new mouse point.
            _toolTip.Placement = PlacementMode.Absolute;
        }

        private object SetBitmapHelper(BitmapInfo bitmapInfo, InteropBitmap bitmap, Action<InteropBitmap> imageSourceSetter)
        {
            if (bitmap == null)
            {
                imageSourceSetter(null);
                GC.Collect(1);

                var stride = bitmapInfo.Width * BytesPerPixel;

                bitmap = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(bitmapInfo.FileMappingHandle,
                    bitmapInfo.Width, bitmapInfo.Height, PixelFormat, stride, 0);
                imageSourceSetter(bitmap);
            }

            bitmap.Invalidate();

            return bitmap;
        }

        private void UpdateTooltip(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                _toolTip.IsOpen = false;
            }
            else
            {
                _toolTip.Content = text;
                _toolTip.Placement = PlacementMode.Mouse;
                _toolTip.Visibility = Visibility.Visible;
                _toolTip.IsOpen = true;
            }
        }

        #endregion

        #region event hooks

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.SendFocusEvent(true);
            }

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.SendFocusEvent(false);
            }

            base.OnLostFocus(e);
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

            if (e.Key == Key.Tab ||
                new[] { Key.Left, Key.Right, Key.Up, Key.Down }.Contains(e.Key))
            {
                var message = (int)(e.IsDown ? WM.KEYDOWN : WM.KEYUP);
                var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
                _managedCefBrowserAdapter.SendKeyEvent(message, virtualKey);
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            _managedCefBrowserAdapter.OnMouseMove((int)point.X, (int)point.Y, mouseLeave: false);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var point = e.GetPosition(this);

            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.OnMouseWheel(
                    (int)point.X,
                    (int)point.Y,
                    deltaX: 0,
                    deltaY: e.Delta
                );
            }
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
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.OnMouseMove(0, 0, mouseLeave: true);
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

            var mouseUp = (e.ButtonState == MouseButtonState.Released);

            var point = e.GetPosition(this);

            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.OnMouseButton((int)point.X, (int)point.Y, mouseButtonType, mouseUp, e.ClickCount);
            }
        }

        #endregion

        public void ViewSource()
        {
            if (_isOffscreenBrowserCreated)
            {
                _managedCefBrowserAdapter.ViewSource();
            }
        }
    }
}
