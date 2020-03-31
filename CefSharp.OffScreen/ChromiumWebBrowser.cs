// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Enums;
using CefSharp.Internals;
using CefSharp.Structs;
using CefSharp.Web;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace CefSharp.OffScreen
{
    /// <summary>
    /// An offscreen instance of Chromium that you can use to take
    /// snapshots or evaluate JavaScript.
    /// </summary>
    public class ChromiumWebBrowser : IRenderWebBrowser
    {
        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private ManagedCefBrowserAdapter managedCefBrowserAdapter;

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
        /// The value for disposal, if it's 1 (one) then this instance is either disposed
        /// or in the process of getting disposed
        /// </summary>
        private int disposeSignaled;

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><see langword="true" /> if this instance is disposed; otherwise, <see langword="false" />.</value>
        public bool IsDisposed
        {
            get
            {
                return Interlocked.CompareExchange(ref disposeSignaled, 1, 1) == 1;
            }
        }

        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
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
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        public bool CanGoForward { get; private set; }
        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public IRequestContext RequestContext { get; private set; }
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
        /// Implement <see cref="IRenderHandler" /> and assign to handle events related to browser rendering.
        /// </summary>
        /// <value>The render handler.</value>
        public IRenderHandler RenderHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDragHandler" /> and assign to handle events related to dragging.
        /// </summary>
        /// <value>The drag handler.</value>
        public IDragHandler DragHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IResourceRequestHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        public IResourceRequestHandlerFactory ResourceRequestHandlerFactory { get; set; }
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
        /// Implement <see cref="IAccessibilityHandler" /> to handle events related to accessibility.
        /// </summary>
        /// <value>The accessibility handler.</value>
        public IAccessibilityHandler AccessibilityHandler { get; set; }

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
        /// </remarks>
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
        /// Event called after the underlying CEF browser instance has been created. 
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
        /// Fired on the CEF UI thread, which by default is not the same as your application main thread.
        /// Called when an element should be painted. Pixel values passed to this method are scaled relative to view coordinates
        /// based on the value of ScreenInfo.DeviceScaleFactor returned from GetScreenInfo. 
        /// </summary>
        public event EventHandler<OnPaintEventArgs> Paint;

        /// <summary>
        /// Used by <see cref="ScreenshotAsync(bool, PopupBlending)"/> as the <see cref="Paint"/>
        /// event happens before the bitmap buffer is updated. We do this to allow users to mark <see cref="OnPaintEventArgs.Handled"/>
        /// to true and stop the buffer from being updated.
        /// </summary>
        private event EventHandler<OnPaintEventArgs> AfterPaint;

        /// <summary>
        /// Event handler that will get called when the message that originates from CefSharp.PostMessage
        /// </summary>
        public event EventHandler<JavascriptMessageReceivedEventArgs> JavascriptMessageReceived;

        /// <summary>
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        public bool CanExecuteJavascriptInMainFrame { get; private set; }

        /// <summary>
        /// Create a new OffScreen Chromium Browser. If you use <see cref="CefSharpSettings.LegacyJavascriptBindingEnabled"/> = true then you must
        /// set <paramref name="automaticallyCreateBrowser"/> to false and call <see cref="CreateBrowser"/> after the objects are registered.
        /// </summary>
        /// <param name="html">html string to be initially loaded in the browser.</param>
        /// <param name="browserSettings">The browser settings to use. If null, the default settings are used.</param>
        /// <param name="requestContext">See <see cref="RequestContext" /> for more details. Defaults to null</param>
        /// <param name="automaticallyCreateBrowser">automatically create the underlying Browser</param>
        /// <exception cref="System.InvalidOperationException">Cef::Initialize() failed</exception>
        public ChromiumWebBrowser(HtmlString html, BrowserSettings browserSettings = null,
            IRequestContext requestContext = null, bool automaticallyCreateBrowser = true) : this(html.ToDataUriString(), browserSettings, requestContext, automaticallyCreateBrowser)
        {
        }

        /// <summary>
        /// Create a new OffScreen Chromium Browser. If you use <see cref="CefSharpSettings.LegacyJavascriptBindingEnabled"/> = true then you must
        /// set <paramref name="automaticallyCreateBrowser"/> to false and call <see cref="CreateBrowser"/> after the objects are registered.
        /// </summary>
        /// <param name="address">Initial address (url) to load</param>
        /// <param name="browserSettings">The browser settings to use. If null, the default settings are used.</param>
        /// <param name="requestContext">See <see cref="RequestContext" /> for more details. Defaults to null</param>
        /// <param name="automaticallyCreateBrowser">automatically create the underlying Browser</param>
        /// <exception cref="System.InvalidOperationException">Cef::Initialize() failed</exception>
        public ChromiumWebBrowser(string address = "", BrowserSettings browserSettings = null,
            IRequestContext requestContext = null, bool automaticallyCreateBrowser = true)
        {
            if (!Cef.IsInitialized)
            {
                var settings = new CefSettings();

                if (!Cef.Initialize(settings))
                {
                    throw new InvalidOperationException("Cef::Initialize() failed");
                }
            }

            RequestContext = requestContext;

            Cef.AddDisposable(this);
            Address = address;

            managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, true);

            if (automaticallyCreateBrowser)
            {
                CreateBrowser(null, browserSettings);
            }

            RenderHandler = new DefaultRenderHandler(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        ~ChromiumWebBrowser()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ChromiumWebBrowser"/> object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources for the <see cref="ChromiumWebBrowser"/>
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Attempt to move the disposeSignaled state from 0 to 1. If successful, we can be assured that
            // this thread is the first thread to do so, and can safely dispose of the object.
            if (Interlocked.CompareExchange(ref disposeSignaled, 1, 0) != 0)
            {
                return;
            }

            if (disposing)
            {
                CanExecuteJavascriptInMainFrame = false;
                IsBrowserInitialized = false;

                // Don't reference event listeners any longer:
                AddressChanged = null;
                BrowserInitialized = null;
                ConsoleMessage = null;
                FrameLoadEnd = null;
                FrameLoadStart = null;
                LoadError = null;
                LoadingStateChanged = null;
                Paint = null;
                AfterPaint = null;
                StatusMessage = null;
                TitleChanged = null;
                JavascriptMessageReceived = null;

                // Release reference to handlers, except LifeSpanHandler which is done after Disposing
                // ManagedCefBrowserAdapter otherwise the ILifeSpanHandler.DoClose will not be invoked.
                this.SetHandlersToNullExceptLifeSpan();

                browser = null;

                if (managedCefBrowserAdapter != null)
                {
                    managedCefBrowserAdapter.Dispose();
                    managedCefBrowserAdapter = null;
                }

                // LifeSpanHandler is set to null after managedCefBrowserAdapter.Dispose so ILifeSpanHandler.DoClose
                // is called.
                LifeSpanHandler = null;
            }

            Cef.RemoveDisposable(this);
        }

        /// <summary>
        /// Create the underlying browser. The instance address, browser settings and request context will be used.
        /// </summary>
        /// <param name="windowInfo">Window information used when creating the browser</param>
        /// <param name="browserSettings">Browser initialization settings</param>
        /// <exception cref="System.Exception">An instance of the underlying offscreen browser has already been created, this method can only be called once.</exception>
        public void CreateBrowser(IWindowInfo windowInfo = null, BrowserSettings browserSettings = null)
        {
            if (browserCreated)
            {
                throw new Exception("An instance of the underlying offscreen browser has already been created, this method can only be called once.");
            }

            browserCreated = true;

            if (browserSettings == null)
            {
                browserSettings = new BrowserSettings(frameworkCreated: true);
            }

            if (windowInfo == null)
            {
                windowInfo = new WindowInfo();
                windowInfo.SetAsWindowless(IntPtr.Zero);
            }

            managedCefBrowserAdapter.CreateBrowser(windowInfo, browserSettings, (RequestContext)RequestContext, Address);

            //Dispose of BrowserSettings if we created it, if user created then they're responsible
            if (browserSettings.FrameworkCreated)
            {
                browserSettings.Dispose();
            }
            browserSettings = null;
        }

        /// <summary>
        /// Get/set the size of the Chromium viewport, in pixels.
        /// This also changes the size of the next rendered bitmap.
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
            if (RenderHandler == null)
            {
                throw new NullReferenceException("RenderHandler cannot be null. Use DefaultRenderHandler unless implementing your own");
            }

            var renderHandler = RenderHandler as DefaultRenderHandler;

            if (renderHandler == null)
            {
                throw new Exception("ScreenshotOrNull and ScreenshotAsync can only be used in combination with the DefaultRenderHandler");
            }

            lock (renderHandler.BitmapLock)
            {
                if (blend == PopupBlending.Main)
                {
                    return renderHandler.BitmapBuffer.CreateBitmap();
                }

                if (blend == PopupBlending.Popup)
                {
                    return renderHandler.PopupOpen ? renderHandler.PopupBuffer.CreateBitmap() : null;
                }


                var bitmap = renderHandler.BitmapBuffer.CreateBitmap();

                if (renderHandler.PopupOpen && bitmap != null)
                {
                    var popup = renderHandler.PopupBuffer.CreateBitmap();
                    if (popup == null)
                    {
                        return bitmap;
                    }
                    return MergeBitmaps(bitmap, popup, renderHandler.PopupPosition);
                }

                return bitmap;
            }
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
        /// <param name="blend">Choose which bitmap to retrieve, choose <see cref="PopupBlending.Blend"/> for a merged bitmap.</param>
        /// <returns>Task&lt;Bitmap&gt;.</returns>
        public Task<Bitmap> ScreenshotAsync(bool ignoreExistingScreenshot = false, PopupBlending blend = PopupBlending.Main)
        {
            // Try our luck and see if there is already a screenshot, to save us creating a new thread for nothing.
            var screenshot = ScreenshotOrNull(blend);

            var completionSource = new TaskCompletionSource<Bitmap>();

            if (screenshot == null || ignoreExistingScreenshot)
            {
                EventHandler<OnPaintEventArgs> afterPaint = null; // otherwise we cannot reference ourselves in the anonymous method below

                afterPaint = (sender, e) =>
                {
                    // Chromium has rendered.  Tell the task about it.
                    AfterPaint -= afterPaint;

                    //If the user handled the Paint event then we'll throw an exception here
                    //as it's not possible to use ScreenShotAsync as the buffer wasn't updated.
                    if (e.Handled)
                    {
                        completionSource.TrySetException(new InvalidOperationException("OnPaintEventArgs.Handled = true, unable to process request. The buffer has not been updated"));
                    }
                    else
                    {
                        completionSource.TrySetResultAsync(ScreenshotOrNull(blend));
                    }
                };

                AfterPaint += afterPaint;
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
        /// The javascript object repository, one repository per ChromiumWebBrowser instance.
        /// </summary>
        public IJavascriptObjectRepository JavascriptObjectRepository
        {
            get { return managedCefBrowserAdapter?.JavascriptObjectRepository; }
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
            this.ThrowExceptionIfDisposed();
            this.ThrowExceptionIfBrowserNotInitialized();

            return browser;
        }

        /// <summary>
        /// Gets the screen information (scale factor).
        /// </summary>
        /// <returns>ScreenInfo.</returns>
        ScreenInfo? IRenderWebBrowser.GetScreenInfo()
        {
            return GetScreenInfo();
        }

        /// <summary>
        /// Gets the screen information (scale factor).
        /// </summary>
        /// <returns>ScreenInfo.</returns>
        protected virtual ScreenInfo? GetScreenInfo()
        {
            return RenderHandler?.GetScreenInfo();
        }

        /// <summary>
        /// Gets the view rect (width, height)
        /// </summary>
        /// <returns>ViewRect.</returns>
        Rect IRenderWebBrowser.GetViewRect()
        {
            return GetViewRect();
        }

        /// <summary>
        /// Gets the view rect (width, height)
        /// </summary>
        /// <returns>ViewRect.</returns>
        protected virtual Rect GetViewRect()
        {
            if (RenderHandler == null)
            {
                return new Rect(0, 0, 640, 480);
            }

            return RenderHandler.GetViewRect();
        }

        /// <summary>
        /// Called to retrieve the translation from view coordinates to actual screen coordinates. 
        /// </summary>
        /// <param name="viewX">x</param>
        /// <param name="viewY">y</param>
        /// <param name="screenX">screen x</param>
        /// <param name="screenY">screen y</param>
        /// <returns>Return true if the screen coordinates were provided.</returns>
        bool IRenderWebBrowser.GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY)
        {
            return GetScreenPoint(viewX, viewY, out screenX, out screenY);
        }

        /// <summary>
        /// Called to retrieve the translation from view coordinates to actual screen coordinates. 
        /// </summary>
        /// <param name="viewX">x</param>
        /// <param name="viewY">y</param>
        /// <param name="screenX">screen x</param>
        /// <param name="screenY">screen y</param>
        /// <returns>Return true if the screen coordinates were provided.</returns>
        protected virtual bool GetScreenPoint(int viewX, int viewY, out int screenX, out int screenY)
        {
            screenX = 0;
            screenY = 0;

            return RenderHandler?.GetScreenPoint(viewX, viewY, out screenX, out screenY) ?? false;
        }

        /// <summary>
        /// Called when an element has been rendered to the shared texture handle.
        /// This method is only called when <see cref="IWindowInfo.SharedTextureEnabled"/> is set to true
        /// </summary>
        /// <param name="type">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="sharedHandle">is the handle for a D3D11 Texture2D that can be accessed via ID3D11Device using the OpenSharedResource method.</param>
        void IRenderWebBrowser.OnAcceleratedPaint(PaintElementType type, Rect dirtyRect, IntPtr sharedHandle)
        {
            RenderHandler?.OnAcceleratedPaint(type, dirtyRect, sharedHandle);
        }

        /// <summary>
        /// Called when an element should be painted. (Invoked from CefRenderHandler.OnPaint)
        /// </summary>
        /// <param name="type">indicates whether the element is the view or the popup widget.</param>
        /// <param name="dirtyRect">contains the set of rectangles in pixel coordinates that need to be repainted</param>
        /// <param name="buffer">The bitmap will be will be  width * height *4 bytes in size and represents a BGRA image with an upper-left origin</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        void IRenderWebBrowser.OnPaint(PaintElementType type, Rect dirtyRect, IntPtr buffer, int width, int height)
        {
            var handled = false;

            var args = new OnPaintEventArgs(type == PaintElementType.Popup, dirtyRect, buffer, width, height);

            var handler = Paint;
            if (handler != null)
            {
                handler(this, args);
                handled = args.Handled;
            }

            if (!handled)
            {
                RenderHandler?.OnPaint(type, dirtyRect, buffer, width, height);
            }

            var afterHandler = AfterPaint;
            if (afterHandler != null)
            {
                afterHandler(this, args);
            }
        }

        /// <summary>
        /// Called when the browser's cursor has changed. . 
        /// </summary>
        /// <param name="cursor">If type is Custom then customCursorInfo will be populated with the custom cursor information</param>
        /// <param name="type">cursor type</param>
        /// <param name="customCursorInfo">custom cursor Information</param>
        void IRenderWebBrowser.OnCursorChange(IntPtr cursor, CursorType type, CursorInfo customCursorInfo)
        {
            RenderHandler?.OnCursorChange(cursor, type, customCursorInfo);
        }

        /// <summary>
        /// Starts dragging.
        /// </summary>
        /// <param name="dragData">The drag data.</param>
        /// <param name="mask">The mask.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool IRenderWebBrowser.StartDragging(IDragData dragData, DragOperationsMask mask, int x, int y)
        {
            return RenderHandler?.StartDragging(dragData, mask, x, y) ?? false;
        }

        void IRenderWebBrowser.UpdateDragCursor(DragOperationsMask operation)
        {
            RenderHandler?.UpdateDragCursor(operation);
        }

        /// <summary>
        /// Sets the popup is open.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        void IRenderWebBrowser.OnPopupShow(bool show)
        {
            RenderHandler?.OnPopupShow(show);
        }

        /// <summary>
        /// Called when the browser wants to move or resize the popup widget. 
        /// </summary>
        /// <param name="rect">contains the new location and size in view coordinates. </param>
        void IRenderWebBrowser.OnPopupSize(Rect rect)
        {
            RenderHandler?.OnPopupSize(rect);
        }

        void IRenderWebBrowser.OnImeCompositionRangeChanged(Range selectedRange, Rect[] characterBounds)
        {
            RenderHandler?.OnImeCompositionRangeChanged(selectedRange, characterBounds);
        }

        void IRenderWebBrowser.OnVirtualKeyboardRequested(IBrowser browser, TextInputMode inputMode)
        {
            RenderHandler?.OnVirtualKeyboardRequested(browser, inputMode);
        }

        /// <summary>
        /// Handles the <see cref="E:ConsoleMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ConsoleMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnConsoleMessage(ConsoleMessageEventArgs args)
        {
            ConsoleMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadStart" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadStartEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadStart(FrameLoadStartEventArgs args)
        {
            FrameLoadStart?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadEnd" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadEndEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadEnd(FrameLoadEndEventArgs args)
        {
            FrameLoadEnd?.Invoke(this, args);
        }

        /// <summary>
        /// Called when [after browser created].
        /// </summary>
        /// <param name="browser">The browser.</param>
        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            this.browser = browser;

            IsBrowserInitialized = true;

            BrowserInitialized?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the <see cref="E:LoadError" /> event.
        /// </summary>
        /// <param name="args">The <see cref="LoadErrorEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnLoadError(LoadErrorEventArgs args)
        {
            LoadError?.Invoke(this, args);
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
            StatusMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="args">The <see cref="AddressChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetAddress(AddressChangedEventArgs args)
        {
            Address = args.Address;

            AddressChanged?.Invoke(this, args);
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

            LoadingStateChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="args">The <see cref="TitleChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetTitle(TitleChangedEventArgs args)
        {
            TitleChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="tooltipText">The tooltip text.</param>
        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        void IWebBrowserInternal.SetCanExecuteJavascriptOnMainFrame(bool canExecute)
        {
            CanExecuteJavascriptInMainFrame = canExecute;
        }

        void IWebBrowserInternal.SetJavascriptMessageReceived(JavascriptMessageReceivedEventArgs args)
        {
            JavascriptMessageReceived?.Invoke(this, args);
        }

        /// <summary>
        /// Creates a new bitmap with the dimensions of firstBitmap, then
        /// draws the firstBitmap, then overlays the secondBitmap
        /// </summary>
        /// <param name="firstBitmap">First bitmap, this will be the first image drawn</param>
        /// <param name="secondBitmap">Second bitmap, this image will be drawn on the first</param>
        /// <param name="secondBitmapPosition">Position of the second bitmap</param>
        /// <returns>The merged bitmap, size of firstBitmap</returns>
        private Bitmap MergeBitmaps(Bitmap firstBitmap, Bitmap secondBitmap, Point secondBitmapPosition)
        {
            var mergedBitmap = new Bitmap(firstBitmap.Width, firstBitmap.Height, PixelFormat.Format32bppPArgb);
            using (var g = Graphics.FromImage(mergedBitmap))
            {
                g.DrawImage(firstBitmap, new Rectangle(0, 0, firstBitmap.Width, firstBitmap.Height));
                g.DrawImage(secondBitmap, new Rectangle(secondBitmapPosition.X, secondBitmapPosition.Y, secondBitmap.Width, secondBitmap.Height));
            }
            return mergedBitmap;
        }
    }
}
