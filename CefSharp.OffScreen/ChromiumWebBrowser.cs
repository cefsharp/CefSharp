// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using CefSharp.Internals;
using CefSharp.ModelBinding;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// An offscreen instance of Chromium that you can use to take
    /// snapshots or evaluate JavaScript.
    /// </summary>
    /// <seealso cref="CefSharp.Internals.IRenderWebBrowser" />
    public class ChromiumWebBrowser : IRenderWebBrowser
    {
        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;

        /// <summary>
        /// Contains the last rendering from Chromium. Direct access
        /// to the underlying Bitmap - there is no locking when trying
        /// to access directly, use <see cref="BitmapLock" /> where appropriate.
        /// A new bitmap is only created when it's size changes, otherwise
        /// the back buffer for the bitmap is constantly updated.
        /// Read the <see cref="InvokeRenderAsync" /> doco for more info.
        /// </summary>
        /// <value>The bitmap.</value>
        public Bitmap Bitmap { get; protected set; }

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

        /// <summary>
        /// The browser
        /// </summary>
        private IBrowser browser;

        /// <summary>
        /// Flag to guard the creation of the underlying offscreen browser - only one instance can be created
        /// </summary>
        private bool browserCreated;

        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool IsBrowserInitialized { get; private set; }
        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is loading; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool IsLoading { get; private set; }
        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        /// <value>The tooltip text.</value>
        public string TooltipText { get; private set; }
        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <value>The address.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public string Address { get; private set; }
        /// <summary>
        /// A flag that indicates whether the state of the control current supports the GoBack action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool CanGoBack { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether the popup is open.
        /// </summary>
        /// <value>
        /// <c>true</c> if popup is opened; otherwise, <c>false</c>.
        /// </value>
        public bool PopupOpen { get; protected set; }
        /// <summary>
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool CanGoForward { get; private set; }
        /// <summary>
        /// Gets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        public BrowserSettings BrowserSettings { get; private set; }
        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public RequestContext RequestContext { get; private set; }

        /// <summary>
        /// Implement <see cref="IJsDialogHandler" /> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        /// <value>The js dialog handler.</value>
        public IJsDialogHandler JsDialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDialogHandler" /> and assign to handle dialog events.
        /// </summary>
        /// <value>The dialog handler.</value>
        public IDialogHandler DialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDownloadHandler" /> and assign to handle events related to downloading files.
        /// </summary>
        /// <value>The download handler.</value>
        public IDownloadHandler DownloadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IKeyboardHandler" /> and assign to handle events related to key press.
        /// </summary>
        /// <value>The keyboard handler.</value>
        public IKeyboardHandler KeyboardHandler { get; set; }
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
        /// Implement <see cref="IRequestHandler" /> and assign to handle events related to browser requests.
        /// </summary>
        /// <value>The request handler.</value>
        public IRequestHandler RequestHandler { get; set; }
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
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> LoadError;
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
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        /// <summary>
        /// Occurs when [browser initialized].
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler BrowserInitialized;
        /// <summary>
        /// Event handler for changes to the status message.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang.
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
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
        /// Occurs when [address changed].
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        /// <summary>
        /// Occurs when [title changed].
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<TitleChangedEventArgs> TitleChanged;

        /// <summary>
        /// Fired by a separate thread when Chrome has re-rendered.
        /// This means that a Bitmap will be returned by ScreenshotOrNull().
        /// </summary>
        public event EventHandler NewScreenshot;

        /// <summary>
        /// Top Left position of the popup.
        /// </summary>
        private Point popupPosition;

        /// <summary>
        ///  Size of the popup.
        /// </summary>
        private Size popupSize;

        /// <summary>
        /// The popup Bitmap.
        /// </summary>
        public Bitmap Popup { get; protected set; }

        /// <summary>
        /// Create a new OffScreen Chromium Browser
        /// </summary>
        /// <param name="address">Initial address (url) to load</param>
        /// <param name="browserSettings">The browser settings to use. If null, the default settings are used.</param>
        /// <param name="requestContext">See <see cref="RequestContext" /> for more details. Defaults to null</param>
        /// <param name="automaticallyCreateBrowser">automatically create the underlying Browser</param>
        /// <param name="blendPopup">Should the popup be blended in the background in the rendering</param>
        /// <exception cref="System.InvalidOperationException">Cef::Initialize() failed</exception>
        public ChromiumWebBrowser(string address = "", BrowserSettings browserSettings = null,
            RequestContext requestContext = null, bool automaticallyCreateBrowser = true)
        {
            if (!Cef.IsInitialized && !Cef.Initialize())
            {
                throw new InvalidOperationException("Cef::Initialize() failed");
            }

            BitmapFactory = new BitmapFactory(BitmapLock);

            ResourceHandlerFactory = new DefaultResourceHandlerFactory();
            BrowserSettings = browserSettings ?? new BrowserSettings();
            RequestContext = requestContext;

            Cef.AddDisposable(this);
            Address = address;

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, true);

            if (automaticallyCreateBrowser)
            {
                CreateBrowser(IntPtr.Zero);
            }

            popupPosition = new Point();
            popupSize = new Size();
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
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
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

            // Release reference to handlers, make sure this is done after we dispose managedCefBrowserAdapter
            // otherwise the ILifeSpanHandler.DoClose will not be invoked. (More important in the WinForms version,
            // we do it here for consistency)
            this.SetHandlersToNull();
        }

        /// <summary>
        /// Gets the size of the popup.
        /// </summary>
        /// <param name="size">Size of the popup.</param>
        public Size PopupSize
        {
            get { return popupSize; }
        }

        /// <summary>
        /// Gets the popup position.
        /// </summary>
        /// <param name="popupPos">The popup position.</param>
        public Point PopupPosition
        {
            get { return popupPosition; }
        }

        /// <summary>
        /// Create the underlying browser. The instance address, browser settings and request context will be used.
        /// </summary>
        /// <param name="windowHandle">Window handle if any, IntPtr.Zero is the default</param>
        /// <exception cref="System.Exception">An instance of the underlying offscreen browser has already been created, this method can only be called once.</exception>

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
        /// This also changes the size of the next screenshot.
        /// </summary>
        /// <value>The size.</value>
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
        /// Chrome also renders the page loading, so if you want to see a complete rendering,
        /// only start this task once your page is loaded (which you can detect via FrameLoadEnd
        /// or your own heuristics based on evaluating JavaScript).
        /// It is your responsibility to dispose the returned Bitmap.
        /// The bitmap size is determined by the Size property set earlier.
        /// </summary>
        /// <param name="blend">Choose which bitmap to retrieve, choose <see cref="PopupBlending.Blend"/> for a merged bitmap.</param>
        /// <returns>Bitmap.</returns>
        public Bitmap ScreenshotOrNull(PopupBlending blend = PopupBlending.Main)
        {
            lock (BitmapLock)
            {
                if (blend == PopupBlending.Blend)
                {
                    if (PopupOpen && Bitmap != null && Popup != null)
                    {
                        return MergeBitmaps(Bitmap, Popup);
                    }

                    return Bitmap == null ? null : new Bitmap(Bitmap);
                }
                else if(blend == PopupBlending.Popup)
                {
                    if (PopupOpen)
                    {
                        return Popup == null ? null : new Bitmap(Popup);
                    }

                    return null;
                }

                return Bitmap == null ? null : new Bitmap(Bitmap);
            }
        }

        /// <summary>
        /// Sets the Bitmap to render with a copy of bitmap in parameter.
        /// </summary>
        /// <param name="bitmap">The bitmap which will be copied</param>
        protected void SetBitmap(Bitmap bitmap)
        {
            Bitmap = (Bitmap)bitmap.Clone();
        }

        /// <summary>
        /// Starts a task that waits for the next rendering from Chrome.
        /// Chrome also renders the page loading, so if you want to see a complete rendering,
        /// only start this task once your page is loaded (which you can detect via FrameLoadEnd
        /// or your own heuristics based on evaluating JavaScript).
        /// It is your responsibility to dispose the returned Bitmap.
        /// The bitmap size is determined by the Size property set earlier.
        /// </summary>
        /// <param name="ignoreExistingScreenshot">Ignore existing bitmap (if any) and return the next avaliable bitmap</param>
        /// /// <param name="blend">Choose which bitmap to retrieve, choose <see cref="PopupBlending.Blend"/> for a merged bitmap.</param>
        /// <returns>Task&lt;Bitmap&gt;.</returns>
        public Task<Bitmap> ScreenshotAsync(bool ignoreExistingScreenshot = false, PopupBlending blend = PopupBlending.Main)
        {
            // Try our luck and see if there is already a screenshot, to save us creating a new thread for nothing.
            var screenshot = ScreenshotOrNull(blend);

            var completionSource = new TaskCompletionSource<Bitmap>();

            if (screenshot == null || ignoreExistingScreenshot)
            {
                EventHandler newScreenshot = null; // otherwise we cannot reference ourselves in the anonymous method below

                newScreenshot = (sender, e) =>
                {
                    // Chromium has rendered.  Tell the task about it.
                    NewScreenshot -= newScreenshot;

                    completionSource.TrySetResultAsync(ScreenshotOrNull());
                };

                NewScreenshot += newScreenshot;
            }
            else
            {
                completionSource.TrySetResultAsync(screenshot);
            }

            return completionSource.Task;
        }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        public void Load(string url)
        {
            Address = url;

            //Destroy the frame wrapper when we're done
            using (var frame = this.GetMainFrame())
            {
                frame.LoadUrl(url);
            }
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
            if (IsBrowserInitialized)
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
            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }
            managedCefBrowserAdapter.RegisterAsyncJsObject(name, objectToBind, options);
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

        /// <summary>
        /// Returns the current CEF Browser Instance
        /// </summary>
        /// <returns>browser instance or null</returns>
        public IBrowser GetBrowser()
        {
            this.ThrowExceptionIfBrowserNotInitialized();

            return browser;
        }

        /// <summary>
        /// Gets the screen information.
        /// </summary>
        /// <returns>ScreenInfo.</returns>
        ScreenInfo IRenderWebBrowser.GetScreenInfo()
        {
            //TODO: Expose NotifyScreenInfoChanged and allow user to specify their own scale factor
            var screenInfo = new ScreenInfo
            {
                ScaleFactor = 1.0F
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
                Width = size.Width,
                Height = size.Height
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
            return BitmapFactory.CreateBitmap(isPopup, 1.0F);
        }

        /// <summary>
        /// Invoked from CefRenderHandler.OnPaint
        /// A new <see cref="Bitmap" /> is only created when <see cref="BitmapInfo.CreateNewBitmap" />
        /// is true, otherwise the new buffer is simply copied into the backBuffer of the existing
        /// <see cref="Bitmap" /> for efficiency. Locking provided by OnPaint as this method is called
        /// in it's lock scope.
        /// </summary>
        /// <param name="bitmapInfo">information about the bitmap to be rendered</param>
        void IRenderWebBrowser.InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            InvokeRenderAsync(bitmapInfo);

            var handler = NewScreenshot;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Invoked from CefRenderHandler.OnPaint
        /// A new <see cref="Bitmap" /> is only created when <see cref="BitmapInfo.CreateNewBitmap" />
        /// is true, otherwise the new buffer is simply copied into the backBuffer of the existing
        /// <see cref="Bitmap" /> for efficiency. Locking provided by OnPaint as this method is called
        /// in it's lock scope.
        /// </summary>
        /// <param name="bitmapInfo">information about the bitmap to be rendered</param>
        public virtual void InvokeRenderAsync(BitmapInfo bitmapInfo)
        {
            var gdiBitmapInfo = (GdiBitmapInfo)bitmapInfo;

            if (bitmapInfo.CreateNewBitmap)
            {
                if (gdiBitmapInfo.IsPopup)
                {
                    if (Popup != null)
                    {
                        Popup.Dispose();
                        Popup = null;
                    }

                    Popup = gdiBitmapInfo.CreateBitmap();
                }
                else
                {
                    if (Bitmap != null)
                    {
                        Bitmap.Dispose();
                        Bitmap = null;
                    }

                    Bitmap = gdiBitmapInfo.CreateBitmap();
                }
            }
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="type">The type.</param>
        void IRenderWebBrowser.SetCursor(IntPtr handle, CefCursorType type)
        {
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
            return false;
        }

        /// <summary>
        /// Sets the popup is open.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        void IRenderWebBrowser.SetPopupIsOpen(bool show)
        {
            PopupOpen = show;

            //Cleanup the old popup now that's it's not open
            if (!PopupOpen && Popup != null)
            {
                Popup.Dispose();
                Popup = null;
            }
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
            popupPosition.X = x;
            popupPosition.Y = y;
            popupSize.Width = width;
            popupSize.Height = height;
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
        /// Called when [after browser created].
        /// </summary>
        /// <param name="browser">The browser.</param>
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
        /// Sets the address.
        /// </summary>
        /// <param name="args">The <see cref="AddressChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetAddress(AddressChangedEventArgs args)
        {
            Address = args.Address;

            var handler = AddressChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Sets the loading state change.
        /// </summary>
        /// <param name="args">The <see cref="LoadingStateChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="args">The <see cref="TitleChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetTitle(TitleChangedEventArgs args)
        {
            var handler = TitleChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="tooltipText">The tooltip text.</param>
        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        /// <summary>
        /// Creates a new bitmap with the dimensions of firstBitmap, then
        /// draws the firstBitmap, then overlays the secondBitmap
        /// </summary>
        /// <param name="firstBitmap">First bitmap, this will be the first image drawn</param>
        /// <param name="secondBitmap">Second bitmap, this image will be drawn on the first</param>
        /// <returns>The merged bitmap, size of firstBitmap</returns>
        private Bitmap MergeBitmaps(Bitmap firstBitmap, Bitmap secondBitmap)
        {
            var mergedBitmap = new Bitmap(firstBitmap.Width, firstBitmap.Height, PixelFormat.Format32bppPArgb);
            using (var g = Graphics.FromImage(mergedBitmap))
            {
                g.DrawImage(firstBitmap, new Rectangle(0, 0, firstBitmap.Width, firstBitmap.Height));
                g.DrawImage(secondBitmap, new Rectangle((int)popupPosition.X, (int)popupPosition.Y, secondBitmap.Width, secondBitmap.Height));
            }
            return mergedBitmap;
        }
    }
}
