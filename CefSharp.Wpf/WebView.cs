// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace CefSharp.Wpf
{
    public class WebView : ContentControl, IRenderWebBrowser, IDisposable
    {
        private readonly object sync;
        private HwndSource source;
        private BrowserCore browserCore;
        private DispatcherTimer timer;
        private readonly ToolTip toolTip;
        private RenderClientAdapter renderClientAdapter;

        private Image image;
        private InteropBitmap interopBitmap;
        private InteropBitmap popupInteropBitmap;

        public IJsDialogHandler JsDialogHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public IMenuHandler MenuHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public ILoadHandler LoadHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public string TooltipText { get; set; }
        public string Title { get; set; }
        public bool CanGoForward { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool IsLoading { get; private set; }
        public bool IsBrowserInitialized { get; private set; }
        public event ConsoleMessageEventHandler ConsoleMessage;
        public event PropertyChangedEventHandler PropertyChanged;
        public event LoadCompletedEventHandler LoadCompleted;

        public IntPtr FileMappingHandle { get; set; }
        public IntPtr PopupFileMappingHandle { get; set; }

        public int BytesPerPixel
        {
            get { return PixelFormats.Bgr32.BitsPerPixel / 8; }
        }

        int IRenderWebBrowser.Width
        {
            get { return (int) ActualWidth; }
        }

        int IRenderWebBrowser.Height
        {
            get { return (int) ActualHeight; }
        }

        public Uri Uri
        {
            get { return (Uri) GetValue(UriProperty); }
            set
            {
                // TODO: Not-so-orthodox WPF, but the caller is unmanaged code which has no notion of the Dispatcher or anything.
                // Should perhaps be solved by adding an exra property to IWebBrowser for invoking stuff on the UI thread.
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke((Action) (() => Uri = value));
                    return;
                }

                SetValue(UriProperty, value);
            }
        }

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(Uri), typeof(WebView), new UIPropertyMetadata(null, OnUriChanged));

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webView = (WebView) d;

            if (!Cef.IsInitialized &&
                !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            if (webView.browserCore == null)
            {
                webView.browserCore = new BrowserCore(webView.Uri);
                webView.browserCore.PropertyChanged += webView.OnBrowserCorePropertyChanged;

                webView.timer = new DispatcherTimer(
                    TimeSpan.FromSeconds(0.5),
                    DispatcherPriority.Render,
                    webView.OnTimerTick,
                    webView.Dispatcher
                );
            }

            webView.browserCore.Address = webView.Uri;
        }

        public WebView()
        {
            Focusable = true;
            FocusVisualStyle = null;
            IsTabStop = true;

            sync = new Object();

            //_scriptCore = new ScriptCore();
            //_paintPopupDelegate = gcnew ActionHandler(this, &WebView::SetPopupBitmap);
            //_resizePopupDelegate = gcnew ActionHandler(this, &WebView::SetPopupSizeAndPositionImpl);

            ToolTip = toolTip = new ToolTip();
            toolTip.StaysOpen = true;
            toolTip.Visibility = Visibility.Collapsed;
            toolTip.Closed += OnTooltipClosed;

            //this->Loaded +=	gcnew RoutedEventHandler(this, &WebView::OnLoaded);	
            //this->Unloaded += gcnew RoutedEventHandler(this, &WebView::OnUnloaded);	
        }

        public void Dispose()
        {
            // TODO: This is crazy and essentially makes it only possible to ever have one WebView in an application...
            Cef.Shutdown();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            renderClientAdapter = new RenderClientAdapter(this);

            AddSourceHook();

            // TODO: Make it possible to override the BrowserSettings using a dependency property.
            renderClientAdapter.CreateOffscreenBrowser(new BrowserSettings(), source.Handle, Uri);

            Content = image = new Image();
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);

            //_popup = gcnew Popup();
            //_popup->Child = _popupImage = gcnew Image();

            //_popup->MouseDown += gcnew MouseButtonEventHandler(this, &WebView::OnPopupMouseDown);
            //_popup->MouseUp += gcnew MouseButtonEventHandler(this, &WebView::OnPopupMouseUp);
            //_popup->MouseMove += gcnew MouseEventHandler(this, &WebView::OnPopupMouseMove);
            //_popup->MouseLeave += gcnew MouseEventHandler(this, &WebView::OnPopupMouseLeave);
            //_popup->MouseWheel += gcnew MouseWheelEventHandler(this, &WebView::OnPopupMouseWheel);

            //_popup->PlacementTarget = this;
            //_popup->Placement = PlacementMode::Relative;

            image.Stretch = Stretch.None;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;

            //_popupImage->Stretch = Stretch::None;
            //_popupImage->HorizontalAlignment = ::HorizontalAlignment::Left;
            //_popupImage->VerticalAlignment = ::VerticalAlignment::Top;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var size = base.ArrangeOverride(arrangeBounds);
            var newWidth = size.Width;
            var newHeight = size.Height;

            if (newWidth > 0 &&
                newHeight > 0)
            {
                renderClientAdapter.WasResized();
            }

            return size;
        }

        public void InvokeRenderAsync(Action callback)
        {
            if (!Dispatcher.HasShutdownStarted)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Render, callback);
            }
        }

        private void OnBrowserCorePropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TooltipText")
            {
                HandleTooltipUpdate();
            }
            else if (e.PropertyName == "Address")
            {
                Uri = browserCore.Address;
            }
        }

        private void HandleTooltipUpdate()
        {
            timer.Stop();

            if (String.IsNullOrEmpty(browserCore.TooltipText))
            {
                Dispatcher.BeginInvoke((Action) (() => SetTooltipText(null)), DispatcherPriority.Render);
            }
            else
            {
                timer.Start();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            timer.Stop();
            SetTooltipText(browserCore.TooltipText);
        }

        private void OnTooltipClosed(object sender, RoutedEventArgs e)
        {
            toolTip.Visibility = Visibility.Collapsed;

            // Set Placement to something other than PlacementMode::Mouse, so that when we re-show the tooltip in
            // SetTooltipText(), the tooltip will be repositioned to the new mouse point.
            toolTip.Placement = PlacementMode.Absolute;
        }

        void AddSourceHook()
        {
            if (source != null)
            {
                return;
            }

            source = (HwndSource) PresentationSource.FromVisual(this);

            if (source != null)
            {
                //source.AddHook(SourceHook);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            renderClientAdapter.OnMouseMove((int) point.X, (int) point.Y, mouseLeave: false);
        }

        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            OnMouseButton(e);
            Mouse.Capture(this);
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            OnMouseButton(e);
            Mouse.Capture(null);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            renderClientAdapter.OnMouseMove(0, 0, mouseLeave: true);
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
            renderClientAdapter.OnMouseButton((int) point.X, (int) point.Y, mouseButtonType, mouseUp, e.ClickCount);
        }

        // TODO: Will likely have to be done in the C++ part.
        //private IntPtr SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    handled = false;

        //    switch(message)
        //    {
        //    case WM_KEYDOWN:
        //    case WM_KEYUP:
        //    case WM_SYSKEYDOWN:
        //    case WM_SYSKEYUP:
        //    case WM_CHAR:
        //    case WM_SYSCHAR:
        //    case WM_IME_CHAR:
        //        CefRefPtr<CefBrowser> browser;
        //        if (!IsFocused ||
        //            !TryGetCefBrowser(browser))
        //        {
        //            break;
        //        }

        //        CefBrowser::KeyType type;
        //        if (message == WM_CHAR)
        //            type = KT_CHAR;
        //        else if (message == WM_KEYDOWN || message == WM_SYSKEYDOWN)
        //            type = KT_KEYDOWN;
        //        else if (message == WM_KEYUP || message == WM_SYSKEYUP)
        //            type = KT_KEYUP;

        //        CefKeyInfo keyInfo;
        //        keyInfo.key =
        //            wParam.ToInt32();
        //        keyInfo.sysChar =
        //            message == WM_SYSKEYDOWN ||
        //            message == WM_SYSKEYUP ||
        //            message == WM_SYSCHAR;
        //        keyInfo.imeChar =
        //            message == WM_IME_CHAR;

        //        browser->SendKeyEvent(type, keyInfo, lParam.ToInt32());
        //        handled = true;
        //    }

        //    return IntPtr.Zero;
        //}

        private void SetTooltipText(String text)
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

        public void OnInitialized()
        {
            browserCore.OnInitialized();
        }

        public void Load(string address)
        {
            throw new NotImplementedException();
        }

        public void Load(Uri url)
        {
            throw new NotImplementedException();
        }

        public void LoadHtml(string html)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Back()
        {
            throw new NotImplementedException();
        }

        public void Forward()
        {
            throw new NotImplementedException();
        }

        public void Reload(bool ignoreCache)
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public void ClearHistory()
        {
            throw new NotImplementedException();
        }

        public void ShowDevTools()
        {
            throw new NotImplementedException();
        }

        public void CloseDevTools()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void Cut()
        {
            throw new NotImplementedException();
        }

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public void Paste()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void SelectAll()
        {
            throw new NotImplementedException();
        }

        public void Print()
        {
            throw new NotImplementedException();
        }

        public void SetNavState(bool isLoading, bool canGoBack, bool canGoForward)
        {
            browserCore.SetNavState(isLoading, canGoBack, canGoForward);
        }

        public void OnFrameLoadStart(string url)
        {
            browserCore.OnFrameLoadStart();
        }

        public void OnFrameLoadEnd(string url)
        {
            browserCore.OnFrameLoadEnd();

            if (LoadCompleted != null)
            {
                LoadCompleted(this, new LoadCompletedEventArgs(url));
            }
        }

        public void OnTakeFocus(bool next)
        {
            throw new NotImplementedException();
        }

        public void OnConsoleMessage(string message, string source, int line)
        {
            throw new NotImplementedException();
        }

        public void RegisterJsObject(string name, object objectToBind)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> GetBoundObjects()
        {
            throw new NotImplementedException();
        }

        public void ExecuteScript(string script)
        {
            browserCore.CheckBrowserInitialization();

            // Not yet ported to CEF3/C#-based WebView.
            throw new NotImplementedException();
            //CefRefPtr<CefBrowser> browser;
            //if (TryGetCefBrowser(browser))
            //{
            //    _scriptCore->Execute(browser, toNative(script));
            //}
        }

        public object EvaluateScript(string script, TimeSpan? timeout = null)
        {
            if (timeout == null)
            {
                timeout = TimeSpan.MaxValue;
            }

            // Not supported at the moment. Can be done, but will not be able to support a return value here like we're supposed
            // to, because of CEF3:s asynchronous nature.
            throw new NotImplementedException();
            //_browserCore->CheckBrowserInitialization();

            //CefRefPtr<CefBrowser> browser;
            //if (TryGetCefBrowser(browser))
            //{
            //    return _scriptCore->Evaluate(browser, toNative(script), timeout.TotalMilliseconds);
            //}
            //else
            //{
            //    return nullptr;
            //}
        }

        public void SetCursor(IntPtr handle)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke((Action<IntPtr>) SetCursor, handle);
                return;
            }

            Cursor = CursorInteropHelper.Create(new SafeFileHandle(handle, ownsHandle: false));
        }

        public void SetPopupIsOpen(bool isOpen)
        {
            //    if(!Dispatcher->HasShutdownStarted) {
            //        Dispatcher->BeginInvoke(gcnew Action<bool>(this, &WebView::ShowHidePopup), DispatcherPriority::Render, isOpen);
            //    }
        }

        public void SetPopupSizeAndPosition(IntPtr rect)
        {
            //    auto cefRect = (const CefRect&) rect;

            //    _popupX = cefRect.x;
            //    _popupY = cefRect.y;
            //    _popupWidth = cefRect.width;
            //    _popupHeight = cefRect.height;

            //    if(!Dispatcher->HasShutdownStarted) {
            //        Dispatcher->BeginInvoke(DispatcherPriority::Render, _resizePopupDelegate);
            //    }
        }

        //void WebView::SetPopupSizeAndPositionImpl()
        //{
        //    _popup->Width = _popupWidth;
        //    _popup->Height = _popupHeight;

        //    _popup->HorizontalOffset = _popupX;
        //    _popup->VerticalOffset = _popupY;
        //}

        public void ClearBitmap()
        {
            lock (sync)
            {
                interopBitmap = null;
            }
        }

        public void SetBitmap()
        {
            lock (sync)
            {
                if (interopBitmap == null)
                {
                    image.Source = null;
                    GC.Collect(1);

                    var stride = (int) ActualWidth * BytesPerPixel;

                    // TODO: Throws Access Denied exceptions sometimes like below. Could this be because
                    // RenderClientAdapterInternal has closed the FileMappingHandle, because it gets called reentrantly? At first
                    // glance, when looking at the list of threads, this should not be the case but I wouldn't bet on it...
                    // "Additional information: Access is denied. (Exception from HRESULT: 0x80070005 (E_ACCESSDENIED))"
                    var bitmap = (InteropBitmap) Imaging.CreateBitmapSourceFromMemorySection(FileMappingHandle, (int) ActualWidth,
                        (int) ActualHeight, PixelFormats.Bgr32, stride, 0);
                    image.Source = bitmap;
                    interopBitmap = bitmap;
                }

                interopBitmap.Invalidate();
            }
        }

        public void SetPopupBitmap()
        {
            throw new NotImplementedException();
            //if(popupInteropBitmap == null) 
            //{
            //    _popupImage->Source = nullptr;
            //    GC::Collect(1);

            //    int stride = _popupImageWidth * PixelFormats::Bgr32.BitsPerPixel / 8;
            //    bitmap = (InteropBitmap^)Interop::Imaging::CreateBitmapSourceFromMemorySection(
            //        (IntPtr)_popupFileMappingHandle, _popupImageWidth, _popupImageHeight, PixelFormats::Bgr32, stride, 0);
            //    _popupImage->Source = bitmap;
            //    _popupIbitmap = bitmap;
            //}

            //bitmap->Invalidate();
        }
    }
}
