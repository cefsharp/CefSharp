﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Internals;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// An offscreen instance of Chromium that you can use to take
    /// snapshots or evaluate JavaScript.
    /// </summary>
    public class ChromiumWebBrowser : IRenderWebBrowser
    {
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;

        /// <summary>
        /// Contains the last rendering from Chromium.
        /// </summary>
        private Bitmap bitmap;

        /// <summary>
        /// Need a lock because the caller may be asking for the bitmap
        /// while Chromium async rendering has returned on another thread.
        /// </summary>
        private readonly object bitmapLock = new object();

        /// <summary>
        /// Size of the Chromium viewport.
        /// This must be set to something other than 0x0 otherwise Chromium will not render,
        /// and the ScreenshotAsync task will deadlock.
        /// </summary>
        private Size size = new Size(1366, 768);

        public bool IsBrowserInitialized { get; private set; }
        public bool IsLoading { get; set; }
        public string Title { get; set; }
        public string TooltipText { get; set; }
        public bool CanReload { get; private set; }
        public string Address { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanGoForward { get; private set; }
        public BrowserSettings BrowserSettings { get; private set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IDialogHandler DialogHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IPopupHandler PopupHandler { get; set; }
        public IMenuHandler MenuHandler { get; set; }
        public IFocusHandler FocusHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDragHandler DragHandler { get; set; }
        public IResourceHandlerFactory ResourceHandlerFactory { get; set; }
        public IGeolocationHandler GeolocationHandler { get; set; }

        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        public event EventHandler BrowserInitialized;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<AddressChangedEventArgs> AddressChanged;

        /// <summary>
        /// Fired by a separate thread when Chrome has re-rendered.
        /// This means that a Bitmap will be returned by ScreenshotOrNull().
        /// </summary>
        public event EventHandler NewScreenshot;

        /// <summary>
        /// Create a new OffScreen Chromium Browser
        /// </summary>
        /// <param name="address">Initial address (url) to load</param>
        /// <param name="browserSettings">The browser settings to use. If null, the default settings are used.</param>
        public ChromiumWebBrowser(string address = "", BrowserSettings browserSettings = null)
        {
            if (!Cef.IsInitialized && !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            ResourceHandlerFactory = new DefaultResourceHandlerFactory();
            BrowserSettings = browserSettings ?? new BrowserSettings();

            Cef.AddDisposable(this);
            Address = address;

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, true);
            managedCefBrowserAdapter.CreateOffscreenBrowser(IntPtr.Zero, BrowserSettings, address);
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

        protected virtual void Dispose(bool disposing)
        {
            // Don't reference handlers any longer:
            this.SetHandlersToNull();

            // Don't reference event listeners any longer:
            LoadError = null;
            FrameLoadStart = null;
            FrameLoadEnd = null;
            ConsoleMessage = null;
            BrowserInitialized = null;
            StatusMessage = null;
            LoadingStateChanged = null;
            AddressChanged = null;

            Cef.RemoveDisposable(this);

            if (disposing)
            {
                IsBrowserInitialized = false;

                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                if (BrowserSettings != null)
                {
                    BrowserSettings.Dispose();
                    BrowserSettings = null;
                }

                if (managedCefBrowserAdapter != null)
                {
                    if (!managedCefBrowserAdapter.IsDisposed)
                    {
                        managedCefBrowserAdapter.Dispose();
                    }
                    managedCefBrowserAdapter = null;
                }
            }
        }

        /// <summary>
        /// Get/set the size of the Chromium viewport, in pixels.
        /// 
        /// This also changes the size of the next screenshot.
        /// </summary>
        public Size Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    managedCefBrowserAdapter.WasResized();
                }
            }
        }

        /// <summary>
        /// Immediately returns a copy of the last rendering from Chrome,
        /// or null if no rendering has occurred yet.
        /// 
        /// Chrome also renders the page loading, so if you want to see a complete rendering,
        /// only start this task once your page is loaded (which you can detect via FrameLoadEnd
        /// or your own heuristics based on evaluating JavaScript).
        ///
        /// It is your responsibility to dispose the returned Bitmap.
        /// 
        /// The bitmap size is determined by the Size property set earlier.
        /// </summary>
        public Bitmap ScreenshotOrNull()
        {
            lock (bitmapLock)
            {
                return bitmap == null ? null : new Bitmap(bitmap);
            }
        }

        /// <summary>
        /// Starts a task that waits for the next rendering from Chrome.
        /// 
        /// Chrome also renders the page loading, so if you want to see a complete rendering,
        /// only start this task once your page is loaded (which you can detect via FrameLoadEnd
        /// or your own heuristics based on evaluating JavaScript).
        /// 
        /// It is your responsibility to dispose the returned Bitmap.
        /// 
        /// The bitmap size is determined by the Size property set earlier.
        /// </summary>
        public Task<Bitmap> ScreenshotAsync()
        {
            // Try our luck and see if there is already a screenshot, to save us creating a new thread for nothing.
            var screenshot = ScreenshotOrNull();

            var completionSource = new TaskCompletionSource<Bitmap>();

            if (screenshot == null)
            {
                EventHandler newScreenshot = null; // otherwise we cannot reference ourselves in the anonymous method below

                newScreenshot = (sender, e) =>
                {
                    // Chromium has rendered.  Tell the task about it.
                    NewScreenshot -= newScreenshot;

                    completionSource.SetResult(ScreenshotOrNull());
                };

                NewScreenshot += newScreenshot;
            }
            else
            {
                completionSource.SetResult(screenshot);
            }

            return completionSource.Task;
        }

        public Task<JavascriptResponse> EvaluateScriptAsync(string script, TimeSpan? timeout = null)
        {
            return managedCefBrowserAdapter.EvaluateScriptAsync(script, timeout);
        }

        public void Load(string url)
        {
            Address = url;
            managedCefBrowserAdapter.LoadUrl(Address);
        }

        public void RegisterJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind, camelCaseJavascriptNames);
        }

        /// <summary>
        /// Has Focus - Always False
        /// </summary>
        /// <returns>returns false</returns>
        bool IWebBrowser.Focus()
        {
            // no control to focus for offscreen browser
            return false;
        }

        public void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY)
        {
            managedCefBrowserAdapter.OnMouseWheel(x, y, deltaX, deltaY);
        }

        public IFrame GetMainFrame()
        {
            return IsBrowserInitialized ? managedCefBrowserAdapter.GetMainFrame() : null;
        }

        public IFrame GetFocusedFrame()
        {
            return IsBrowserInitialized ? managedCefBrowserAdapter.GetFocusedFrame() : null;
        }

        public IBrowser GetBrowser()
        {
            return IsBrowserInitialized ? managedCefBrowserAdapter.GetBrowser() : null;
        }

        #region IRenderWebBrowser (rendering to bitmap; derived from CefSharp.Wpf.ChromiumWebBrowser)

        ScreenInfo IRenderWebBrowser.GetScreenInfo()
        {
            var screenInfo = new ScreenInfo();

            screenInfo.Width = size.Width;
            screenInfo.Height = size.Height;
            //TODO: Expose NotifyScreenInfoChanged and allow user to specify their own scale factor
            screenInfo.ScaleFactor = 1.0F;

            return screenInfo;
        }

        BitmapInfo IRenderWebBrowser.CreateBitmapInfo(bool isPopup)
        {
            //The bitmap buffer is 32 BPP
            return new GdiBitmapInfo { IsPopup = isPopup, BitmapLock = bitmapLock };
        }

        /// <summary>
        /// Invoked from CefRenderHandler.OnPaint
        /// Locking provided by OnPaint as this method is called in it's lock scope
        /// </summary>
        /// <param name="bitmapInfo">information about the bitmap to be rendered</param>
        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            var gdiBitmapInfo = (GdiBitmapInfo)bitmapInfo;
            if (bitmapInfo.CreateNewBitmap)
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                bitmap = gdiBitmapInfo.CreateBitmap();
            }

            var handler = NewScreenshot;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        void IRenderWebBrowser.SetCursor(IntPtr handle, CefCursorType type)
        {
        }

        void IRenderWebBrowser.SetPopupIsOpen(bool show)
        {
        }

        void IRenderWebBrowser.SetPopupSizeAndPosition(int width, int height, int x, int y)
        {
        }
        #endregion

        #region IWebBrowserInternal (notifications from CEF to C#; derived from CefSharp.Wpf.ChromiumWebBrowser)

        void IWebBrowserInternal.OnConsoleMessage(string message, string source, int line)
        {
            var handler = ConsoleMessage;
            if (handler != null)
            {
                handler(this, new ConsoleMessageEventArgs(message, source, line));
            }
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

        void IWebBrowserInternal.OnInitialized()
        {
            IsBrowserInitialized = true;

            var handler = BrowserInitialized;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
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

        void IWebBrowserInternal.OnStatusMessage(string value)
        {
            var handler = StatusMessage;
            if (handler != null)
            {
                handler(this, new StatusMessageEventArgs(value));
            }
        }

        void IWebBrowserInternal.SetAddress(string address)
        {
            Address = address;

            var handler = AddressChanged;
            if (handler != null)
            {
                handler(this, new AddressChangedEventArgs(address));
            }
        }

        void IWebBrowserInternal.SetLoadingStateChange(bool canGoBack, bool canGoForward, bool isLoading)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
            CanReload = !isLoading;
            IsLoading = isLoading;

            var handler = LoadingStateChanged;
            if (handler != null)
            {
                handler(this, new LoadingStateChangedEventArgs(canGoBack, canGoForward, isLoading));
            }
        }

        void IWebBrowserInternal.SetTitle(string title)
        {
            Title = title;
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }
        #endregion
    }
}
