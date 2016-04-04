// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
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
        /// Contains the last rendering from Chromium. Direct access
        /// to the underlying Bitmap - there is no locking when trying
        /// to access directly, use <see cref="BitmapLock"/> where appropriate.
        /// A new bitmap is only created when it's size changes, otherwise
        /// the back buffer for the bitmap is constantly updated.
        /// Read the <see cref="InvokeRenderAsync"/> doco for more info.
        /// </summary>
        public Bitmap Bitmap { get; private set; }

        /// <summary>
        /// Need a lock because the caller may be asking for the bitmap
        /// while Chromium async rendering has returned on another thread.
        /// </summary>
        public readonly object BitmapLock = new object();

        /// <summary>
        /// Size of the Chromium viewport.
        /// This must be set to something other than 0x0 otherwise Chromium will not render,
        /// and the ScreenshotAsync task will deadlock.
        /// </summary>
        private Size size = new Size(1366, 768);

        private IBrowser browser;

        /// <summary>
        /// Flag to guard the creation of the underlying offscreen browser - only one instance can be created
        /// </summary>
        private bool browserCreated;

        public bool IsBrowserInitialized { get; private set; }
        public bool IsLoading { get; set; }
        public string TooltipText { get; set; }
        public string Address { get; private set; }
        public bool CanGoBack { get; private set; }
        public bool CanGoForward { get; private set; }
        public BrowserSettings BrowserSettings { get; private set; }
        public RequestContext RequestContext { get; private set; }
        public IJsDialogHandler JsDialogHandler { get; set; }
        public IDialogHandler DialogHandler { get; set; }
        public IDownloadHandler DownloadHandler { get; set; }
        public IKeyboardHandler KeyboardHandler { get; set; }
        public ILoadHandler LoadHandler { get; set; }
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        public IDisplayHandler DisplayHandler { get; set; }
        public IContextMenuHandler MenuHandler { get; set; }
        public IFocusHandler FocusHandler { get; set; }
        public IRequestHandler RequestHandler { get; set; }
        public IDragHandler DragHandler { get; set; }
        public IResourceHandlerFactory ResourceHandlerFactory { get; set; }
        public IGeolocationHandler GeolocationHandler { get; set; }
        public IBitmapFactory BitmapFactory { get; set; }
        public IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }
        public IFindHandler FindHandler { get; set; }

        public event EventHandler<LoadErrorEventArgs> LoadError;
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        public event EventHandler BrowserInitialized;
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        public event EventHandler<TitleChangedEventArgs> TitleChanged;

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
        /// <param name="requestcontext">See <see cref="RequestContext"/> for more details. Defaults to null</param>
        /// <param name="automaticallyCreateBrowser">automatically create the underlying Browser</param>
        public ChromiumWebBrowser(string address = "", BrowserSettings browserSettings = null,
            RequestContext requestcontext = null, bool automaticallyCreateBrowser = true)
        {
            if (!Cef.IsInitialized && !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            BitmapFactory = new BitmapFactory(BitmapLock);

            ResourceHandlerFactory = new DefaultResourceHandlerFactory();
            BrowserSettings = browserSettings ?? new BrowserSettings();
            RequestContext = requestcontext;

            Cef.AddDisposable(this);
            Address = address;

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, true);

            if(automaticallyCreateBrowser)
            {
                CreateBrowser(IntPtr.Zero);
            }
            
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
                browser = null;
                IsBrowserInitialized = false;

                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                    Bitmap = null;
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
        /// Create the underlying browser. The instance address, browser settings and request context will be used.
        /// </summary>
        /// <param name="windowHandle">Window handle if any, IntPtr.Zero is the default</param>
        
        public void CreateBrowser(IntPtr windowHandle)
        {
            if (browserCreated)
            {
                throw new Exception("An instance of the underlying offscreen browser has already been created, this method can only be called once.");
            }

            browserCreated = true;

            managedCefBrowserAdapter.CreateOffscreenBrowser(windowHandle, BrowserSettings, RequestContext, Address);
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

                    if (IsBrowserInitialized)
                    {
                        browser.GetHost().WasResized();
                    }
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
            lock (BitmapLock)
            {
                return Bitmap == null ? null : new Bitmap(Bitmap);
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
        /// <param name="ignoreExistingScreenshot">Ignore existing bitmap (if any) and return the next avaliable bitmap</param>
        public Task<Bitmap> ScreenshotAsync(bool ignoreExistingScreenshot = false)
        {
            // Try our luck and see if there is already a screenshot, to save us creating a new thread for nothing.
            var screenshot = ScreenshotOrNull();

            var completionSource = new TaskCompletionSource<Bitmap>();

            if (screenshot == null || ignoreExistingScreenshot)
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

        public void Load(string url)
        {
            Address = url;

            //Destroy the frame wrapper when we're done
            using (var frame = this.GetMainFrame())
            {
                frame.LoadUrl(url);
            }
        }

        public void RegisterJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }

            //Enable WCF if not already enabled
            CefSharpSettings.WcfEnabled = true;

            managedCefBrowserAdapter.RegisterJsObject(name, objectToBind, camelCaseJavascriptNames);
        }

        public void RegisterAsyncJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true)
        {
            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            managedCefBrowserAdapter.RegisterAsyncJsObject(name, objectToBind, camelCaseJavascriptNames);
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

        public IBrowser GetBrowser()
        {
            this.ThrowExceptionIfBrowserNotInitialized();

            return browser;
        }

        ScreenInfo IRenderWebBrowser.GetScreenInfo()
        {
            //TODO: Expose NotifyScreenInfoChanged and allow user to specify their own scale factor
            var screenInfo = new ScreenInfo
            {
                ScaleFactor = 1.0F
            };

            return screenInfo;
        }

        ViewRect IRenderWebBrowser.GetViewRect()
        {
            var viewRect = new ViewRect
            {
                Width = size.Width,
                Height = size.Height
            };

            return viewRect;
        }

        BitmapInfo IRenderWebBrowser.CreateBitmapInfo(bool isPopup)
        {
            if (BitmapFactory == null)
            {
                throw new Exception("BitmapFactory cannot be null");
            }
            return BitmapFactory.CreateBitmap(isPopup, 1.0F);
        }

        /// <summary>
        /// Invoked from CefRenderHandler.OnPaint
        /// A new <see cref="Bitmap"/> is only created when <see cref="BitmapInfo.CreateNewBitmap"/>
        /// is true, otherwise the new buffer is simply copied into the backBuffer of the existing
        /// <see cref="Bitmap"/> for efficency. Locking provided by OnPaint as this method is called
        /// in it's lock scope.
        /// </summary>
        /// <param name="bitmapInfo">information about the bitmap to be rendered</param>
        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            InvokeRenderAsync(bitmapInfo);
        }

        /// <summary>
        /// Invoked from CefRenderHandler.OnPaint
        /// A new <see cref="Bitmap"/> is only created when <see cref="BitmapInfo.CreateNewBitmap"/>
        /// is true, otherwise the new buffer is simply copied into the backBuffer of the existing
        /// <see cref="Bitmap"/> for efficency. Locking provided by OnPaint as this method is called
        /// in it's lock scope.
        /// </summary>
        /// <param name="bitmapInfo">information about the bitmap to be rendered</param>
        public virtual void InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            var gdiBitmapInfo = (GdiBitmapInfo)bitmapInfo;
            if (bitmapInfo.CreateNewBitmap)
            {
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                    Bitmap = null;
                }

                Bitmap = gdiBitmapInfo.CreateBitmap();
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

        bool IRenderWebBrowser.StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y)
        {
            return false;
        }

        void IRenderWebBrowser.SetPopupIsOpen(bool show)
        {
        }

        void IRenderWebBrowser.SetPopupSizeAndPosition(int width, int height, int x, int y)
        {
        }

        void IWebBrowserInternal.OnConsoleMessage(ConsoleMessageEventArgs args)
        {
            var handler = ConsoleMessage;
            if (handler != null)
            {
                handler(this, args);
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

        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            this.browser = browser;

            IsBrowserInitialized = true;

            var handler = BrowserInitialized;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        void IWebBrowserInternal.OnLoadError(LoadErrorEventArgs args)
        {
            var handler = LoadError;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        IBrowserAdapter IWebBrowserInternal.BrowserAdapter
        {
            get { return managedCefBrowserAdapter;}
        }

        bool IWebBrowserInternal.HasParent { get; set; }
        
        IntPtr IWebBrowserInternal.ControlHandle
        {
            get { return IntPtr.Zero; }
        }

        void IWebBrowserInternal.OnStatusMessage(StatusMessageEventArgs args)
        {
            var handler = StatusMessage;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetAddress(AddressChangedEventArgs args)
        {
            Address = args.Address;

            var handler = AddressChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetLoadingStateChange(LoadingStateChangedEventArgs args)
        {
            CanGoBack = args.CanGoBack;
            CanGoForward = args.CanGoForward;
            IsLoading = args.IsLoading;

            var handler = LoadingStateChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetTitle(TitleChangedEventArgs args)
        {
            var handler = TitleChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }
    }
}
