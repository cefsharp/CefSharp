﻿// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Internals;
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

namespace CefSharp.Wpf
{
    public class WebView : ContentControl, IRenderWebBrowser, IWpfWebBrowser
    {
        #region private members

        private HwndSource source;
        private HwndSourceHook sourceHook;
        private DispatcherTimer tooltipTimer;
        private readonly ToolTip toolTip;
        private readonly ManagedCefBrowserAdapter managedCefBrowserAdapter;
        private bool ignoreUriChange;
        private Matrix matrix;

        private Image image;
        private Image popupImage;
        private Popup popup;
        
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        #endregion

        public BrowserSettings BrowserSettings { get; set; }
        public bool IsBrowserInitialized { get; private set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }

        public event ConsoleMessageEventHandler ConsoleMessage;
        public event LoadCompletedEventHandler LoadCompleted;
        public event LoadErrorEventHandler LoadError;

        public bool CanGoBack { get; private set; }
        public bool CanGoForward { get; private set; }
        public bool CanReload { get; private set; }

        private static PixelFormat PixelFormat
        {
            get { return PixelFormats.Bgra32; }
        }


        #region CleanupElement dependency property

        /// <summary>
        /// The CleanupElement Controls when the BrowserResources will be cleand up. 
        /// The WebView will register on Unloaded of the provided Element and dispose all resources when that handler is called.
        /// By default the cleanup element is the Window that contains the WebView. 
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
            DependencyProperty.Register("CleanupElement", typeof(FrameworkElement), typeof(WebView), new PropertyMetadata(null, OnCleanupElementChanged));
       
        private static void OnCleanupElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var owner = (WebView)sender;
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
            Cleanup();
        }

        protected virtual void Cleanup()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }

        #endregion CleanupElement dependency property

        #region IWpfWebBrowser implementation
        
        #region Address dependency property

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(WebView),
                                        new UIPropertyMetadata(null, OnAddressChanged));

        private static void OnAddressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            WebView owner = (WebView)sender;
            string oldValue = (string)args.OldValue;
            string newValue = (string)args.NewValue;

            owner.OnAddressChanged(oldValue, newValue);
        }

        protected virtual void OnAddressChanged(string oldValue, string newValue)
        {
            if (ignoreUriChange)
            {
                return;
            }

            if (!Cef.IsInitialized &&
                !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            // TODO: Consider making the delay here configurable.
            tooltipTimer = new DispatcherTimer(
                TimeSpan.FromSeconds(0.5),
                DispatcherPriority.Render,
                OnTooltipTimerTick,
                Dispatcher
            );

            managedCefBrowserAdapter.LoadUrl(Address);
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

        private void Back()
        {
            managedCefBrowserAdapter.GoBack();
        }

        private void Forward()
        {
            managedCefBrowserAdapter.GoForward();
        }

        public void Reload()
        {
            managedCefBrowserAdapter.Reload();
        }

        #endregion

        public ICommand BackCommand { get; private set; }
        public ICommand ForwardCommand { get; private set; }
        public ICommand ReloadCommand { get; private set; }

        public void ViewSource()
        {
            managedCefBrowserAdapter.ViewSource();
        }

        #endregion

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
            tooltipTimer.Stop();

            if (String.IsNullOrEmpty(TooltipText))
            {
                Dispatcher.BeginInvoke((Action)(() => UpdateTooltip(null)), DispatcherPriority.Render);
            }
            else
            {
                tooltipTimer.Start();
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

        static WebView()
        {
            Application.Current.Exit += OnApplicationExit;
        }

        public WebView()
        {
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

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this);
            managedCefBrowserAdapter.CreateOffscreenBrowser(BrowserSettings ?? new BrowserSettings());

            disposables.Add(managedCefBrowserAdapter);

            disposables.Add(new DisposableEventWrapper(this, ActualHeightProperty, OnActualSizeChanged));
            disposables.Add(new DisposableEventWrapper(this, ActualWidthProperty, OnActualSizeChanged));
        }

        private class DisposableEventWrapper : IDisposable
        {
            public DependencyObject Source { get; private set; }
            public DependencyProperty Property { get; private set; }
            public EventHandler Handler { get; private set; }

            public DisposableEventWrapper(DependencyObject source, DependencyProperty property, EventHandler handler)
            {
                Source = source;
                Property = property;
                Handler = handler;
                DependencyPropertyDescriptor.FromProperty(Property, Source.GetType()).AddValueChanged(Source, Handler);
            }

            public void Dispose()
            {
                DependencyPropertyDescriptor.FromProperty(Property, Source.GetType()).RemoveValueChanged(Source, Handler);
            }
        }

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

        #region event handlers

        private void OnActualSizeChanged(object sender, EventArgs e)
        {
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
            // TODO: This prevents AccessViolation on shutdown, but it would be better handled by the Cef class; the WebView control
            // should not explicitly have to perform this.
            if (Cef.IsInitialized)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Cef.Shutdown();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            CleanupElement = Window.GetWindow(this);
            AddSourceHookIfNotAlreadyPresent();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            RemoveSourceHook();
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

        private void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            managedCefBrowserAdapter.SendFocusEvent(true);
        }

        private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            managedCefBrowserAdapter.SendFocusEvent(false);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Content = image = new Image();

            // If the display properties is set to 125%, M11 and M22 will be 1.25.
            var factorX = matrix.M11;
            var factorY = matrix.M22;
            var scaleX = 1 / factorX;
            var scaleY = 1 / factorY;
            image.LayoutTransform = new ScaleTransform(scaleX, scaleY);

            popup = CreatePopup();

            AddSourceHookIfNotAlreadyPresent();

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);

            image.Stretch = Stretch.None;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;
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
                managedCefBrowserAdapter.SendKeyEvent(message, virtualKey);
                e.Handled = true;
            }

            if (e.IsDown && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.C)
                {
                    managedCefBrowserAdapter.Copy();
                }
                else if (e.Key == Key.V)
                {
                    managedCefBrowserAdapter.Paste();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = GetPixelPosition(e);
            var modifiers = GetModifiers(e);

            managedCefBrowserAdapter.OnMouseMove((int)point.X, (int)point.Y, false, modifiers);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var point = GetPixelPosition(e);

            managedCefBrowserAdapter.OnMouseWheel(
                (int)point.X,
                (int)point.Y,
                deltaX: 0,
                deltaY: e.Delta
                );
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
            managedCefBrowserAdapter.OnMouseMove(0, 0, true, modifiers);
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

            managedCefBrowserAdapter.OnMouseButton((int)point.X, (int)point.Y, mouseButtonType, mouseUp, e.ClickCount, modifiers);
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

                    if (managedCefBrowserAdapter.SendKeyEvent(message, wParam.ToInt32()))
                    {
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
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
            return modifiers;
        }

        private Point GetPixelPosition(MouseEventArgs e)
        {
            var deviceIndependentPosition = e.GetPosition(this);
            var pixelPosition = matrix.Transform(deviceIndependentPosition);

            return pixelPosition;
        }

        private Popup CreatePopup()
        {
            var popup = new Popup
            {
                Child = popupImage = new Image(),
                PlacementTarget = this,
                Placement = PlacementMode.Relative
            };

            return popup;
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

        #endregion

        #region IWebBrowser 

        public void Load(string url)
        {
            throw new NotImplementedException();
        }

        public void LoadHtml(string html, string url)
        {
            managedCefBrowserAdapter.LoadHtml(html, url);
        }

        public void RegisterJsObject(string name, object objectToBind)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> BoundObjects { get; private set; }

        public void ExecuteScriptAsync(string script)
        {
            managedCefBrowserAdapter.ExecuteScriptAsync(script);
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

            return managedCefBrowserAdapter.EvaluateScript(script, timeout.Value);
        }

        #endregion

        #region IWebBrowserInternal Implementation

        void IWebBrowserInternal.OnInitialized()
        {
            //browserCore.OnInitialized();
        }

        void IWebBrowserInternal.SetAddress(string address)
        {
            DoInUi(() =>
            {
                ignoreUriChange = true;
                Address = address;
                ignoreUriChange = false;

                // The tooltip should obviously also be reset (and hidden) when the address changes.
                TooltipText = null;
            });
        }

        void IWebBrowserInternal.SetIsLoading(bool isLoading)
        {
            DoInUi(() => IsLoading = isLoading);
        }

        void IWebBrowserInternal.SetNavState(bool canGoBack, bool canGoForward, bool canReload)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = canReload;

            RaiseCommandsCanExecuteChanged();
        }

        void IWebBrowserInternal.SetTitle(string title)
        {
            DoInUi(() =>
            {
                Title = title;
            });
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            DoInUi(() =>
            {
                TooltipText = tooltipText;
            });
        }

        void IWebBrowserInternal.ShowDevTools()
        {
            // TODO: Do something about this one.
            var devToolsUrl = managedCefBrowserAdapter.DevToolsUrl;
            throw new NotImplementedException();
        }

        void IWebBrowserInternal.CloseDevTools()
        {
            throw new NotImplementedException();
        }

        void IWebBrowserInternal.OnFrameLoadStart(string url)
        {
            //browserCore.OnFrameLoadStart();
        }

        void IWebBrowserInternal.OnFrameLoadEnd(string url)
        {
            //browserCore.OnFrameLoadEnd();

            if (LoadCompleted != null)
            {
                LoadCompleted(this, new LoadCompletedEventArgs(url));
            }
        }

        void IWebBrowserInternal.OnTakeFocus(bool next)
        {
            throw new NotImplementedException();
        }

        void IWebBrowserInternal.OnConsoleMessage(string message, string source, int line)
        {
            if (ConsoleMessage != null)
            {
                ConsoleMessage(this, new ConsoleMessageEventArgs(message, source, line));
            }
        }

        void IWebBrowserInternal.OnLoadError(string url, CefErrorCode errorCode, string errorText)
        {
            if (LoadError != null)
            {
                LoadError(url, errorCode, errorText);
            }
        }

        private void RaiseCommandsCanExecuteChanged()
        {
            ((DelegateCommand)BackCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ForwardCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ReloadCommand).RaiseCanExecuteChanged();
        }

        #endregion

        #region IRenderWebBrowser Implementation

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

        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            DoInUi(() => ((IRenderWebBrowser)this).SetBitmap(bitmapInfo), DispatcherPriority.Render);
        }

        void IRenderWebBrowser.SetCursor(IntPtr handle)
        {
            DoInUi(() =>
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
                    bitmapInfo.InteropBitmap = SetBitmapHelper(bitmapInfo, (InteropBitmap)bitmapInfo.InteropBitmap, bitmap => popupImage.Source = bitmap);
                }
                else
                {
                    bitmapInfo.InteropBitmap = SetBitmapHelper(bitmapInfo, (InteropBitmap)bitmapInfo.InteropBitmap, bitmap => image.Source = bitmap);
                }
            }
        }

        void IRenderWebBrowser.SetPopupSizeAndPosition(int width, int height, int x, int y)
        {
            DoInUi(() =>
            {
                popup.Width = width;
                popup.Height = height;

                var popupOffset = new Point(x, y);
                // TODO: Port over this from CefSharp1.
                //if (popupOffsetTransform != null)
                //{
                //    popupOffset = popupOffsetTransform->GeneralTransform::Transform(popupOffset);
                //}

                popup.HorizontalOffset = popupOffset.X;
                popup.VerticalOffset = popupOffset.Y;
            });
        }

        void IRenderWebBrowser.SetPopupIsOpen(bool isOpen)
        {
            DoInUi(() =>
            {
                popup.IsOpen = isOpen;
            });
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

        #endregion
    }
}
