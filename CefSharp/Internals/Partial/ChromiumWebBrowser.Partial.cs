// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Internals;

#if OFFSCREEN
namespace CefSharp.OffScreen
#elif WPF
namespace CefSharp.Wpf
#elif WINFORMS
namespace CefSharp.WinForms
#endif
{
    //ChromiumWebBrowser Partial class implementation shared between the 
    //WPF, Winforms and Offscreen
    public partial class ChromiumWebBrowser
    {
        public const string BrowserNotInitializedExceptionErrorMessage =
            "The ChromiumWebBrowser instance creates the underlying Chromium Embedded Framework (CEF) browser instance in an async fashion. " +
            "The undelying CefBrowser instance is not yet initialized. Use the IsBrowserInitializedChanged event and check " +
            "the IsBrowserInitialized property to determine when the browser has been initialized.";

        private const string CefInitializeFailedErrorMessage = "Cef.Initialize() failed.Check the log file see https://github.com/cefsharp/CefSharp/wiki/Trouble-Shooting#log-file for details.";

        /// <summary>
        /// Used as workaround for issue https://github.com/cefsharp/CefSharp/issues/3021
        /// </summary>
        private int canExecuteJavascriptInMainFrameChildProcessId;

        /// <summary>
        /// The browser initialized - boolean represented as 0 (false) and 1(true) as we use Interlocker to increment/reset
        /// </summary>
        private int browserInitialized;

        /// <summary>
        /// The value for disposal, if it's 1 (one) then this instance is either disposed
        /// or in the process of getting disposed
        /// </summary>
        private int disposeSignaled;

        /// <summary>
        /// The browser
        /// </summary>
        private IBrowser browser;

        /// <summary>
        /// Initial browser load task complection source
        /// </summary>
        private TaskCompletionSource<LoadUrlAsyncResponse> initialLoadTaskCompletionSource = new TaskCompletionSource<LoadUrlAsyncResponse>(TaskCreationOptions.RunContinuationsAsynchronously);

        /// <summary>
        /// Initial browser load action
        /// </summary>
        private Action<bool?, CefErrorCode?> initialLoadAction;

        /// <summary>
        /// Get access to the core <see cref="IBrowser"/> instance.
        /// Maybe null if the underlying CEF Browser has not yet been
        /// created or if this control has been disposed. Check
        /// <see cref="IBrowser.IsDisposed"/> before accessing.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IBrowser BrowserCore { get; internal set; }

        /// <summary>
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool CanExecuteJavascriptInMainFrame { get; private set; }
        /// <summary>
        /// Implement <see cref="IDialogHandler" /> and assign to handle dialog events.
        /// </summary>
        /// <value>The dialog handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDialogHandler DialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IJsDialogHandler" /> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        /// <value>The js dialog handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IJsDialogHandler JsDialogHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IKeyboardHandler" /> and assign to handle events related to key press.
        /// </summary>
        /// <value>The keyboard handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IKeyboardHandler KeyboardHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IRequestHandler" /> and assign to handle events related to browser requests.
        /// </summary>
        /// <value>The request handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IRequestHandler RequestHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDownloadHandler" /> and assign to handle events related to downloading files.
        /// </summary>
        /// <value>The download handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDownloadHandler DownloadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILoadHandler" /> and assign to handle events related to browser load status.
        /// </summary>
        /// <value>The load handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public ILoadHandler LoadHandler { get; set; }
        /// <summary>
        /// Implement <see cref="ILifeSpanHandler" /> and assign to handle events related to popups.
        /// </summary>
        /// <value>The life span handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public ILifeSpanHandler LifeSpanHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDisplayHandler" /> and assign to handle events related to browser display state.
        /// </summary>
        /// <value>The display handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDisplayHandler DisplayHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IContextMenuHandler" /> and assign to handle events related to the browser context menu
        /// </summary>
        /// <value>The menu handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IContextMenuHandler MenuHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IRenderProcessMessageHandler" /> and assign to handle messages from the render process.
        /// </summary>
        /// <value>The render process message handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IFindHandler" /> to handle events related to find results.
        /// </summary>
        /// <value>The find handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IFindHandler FindHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IAudioHandler" /> to handle audio events.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IAudioHandler AudioHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IFrameHandler" /> to handle frame events.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IFrameHandler FrameHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IPermissionHandler" /> to handle events related to permission requests.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IPermissionHandler PermissionHandler { get; set; }
        /// <summary>
        /// The <see cref="IFocusHandler" /> for this ChromiumWebBrowser.
        /// </summary>
        /// <value>The focus handler.</value>
        /// <remarks>If you need customized focus handling behavior for WinForms, the suggested
        /// best practice would be to inherit from DefaultFocusHandler and try to avoid
        /// needing to override the logic in OnGotFocus. The implementation in
        /// DefaultFocusHandler relies on very detailed behavior of how WinForms and
        /// Windows interact during window activation.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IFocusHandler FocusHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IDragHandler" /> and assign to handle events related to dragging.
        /// </summary>
        /// <value>The drag handler.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IDragHandler DragHandler { get; set; }
        /// <summary>
        /// Implement <see cref="IResourceRequestHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IResourceRequestHandlerFactory ResourceRequestHandlerFactory { get; set; }

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
        /// Event handler that will get called when the Loading state has changed.
        /// This event will be fired twice. Once when loading is initiated either programmatically or
        /// by user action, and once when loading is terminated due to completion, cancellation of failure.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;
        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when you're running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;
        /// <summary>
        /// Event handler for changes to the status message.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang.
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when you're running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<StatusMessageEventArgs> StatusMessage;
        /// <summary>
        /// Event handler that will get called when the message that originates from CefSharp.PostMessage
        /// </summary>
        public event EventHandler<JavascriptMessageReceivedEventArgs> JavascriptMessageReceived;

        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        bool IChromiumWebBrowserBase.IsBrowserInitialized
        {
            get { return InternalIsBrowserInitialized(); }
        }

        void IWebBrowserInternal.SetCanExecuteJavascriptOnMainFrame(string frameId, bool canExecute)
        {
            //When loading pages of a different origin the frameId changes
            //For the first loading of a new origin the messages from the render process
            //Arrive in a different order than expected, the OnContextCreated message
            //arrives before the OnContextReleased, then the message for OnContextReleased
            //incorrectly overrides the value
            //https://github.com/cefsharp/CefSharp/issues/3021

            var chromiumChildProcessId = GetChromiumChildProcessId(frameId);

            if (chromiumChildProcessId > canExecuteJavascriptInMainFrameChildProcessId && !canExecute)
            {
                return;
            }

            canExecuteJavascriptInMainFrameChildProcessId = chromiumChildProcessId;
            CanExecuteJavascriptInMainFrame = canExecute;
        }

        void IWebBrowserInternal.SetJavascriptMessageReceived(JavascriptMessageReceivedEventArgs args)
        {
            //Run the event on the ThreadPool (rather than the CEF Thread we are currently on).
            Task.Run(() => JavascriptMessageReceived?.Invoke(this, args));
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
        /// Handles the <see cref="E:ConsoleMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ConsoleMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnConsoleMessage(ConsoleMessageEventArgs args)
        {
            ConsoleMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:StatusMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="StatusMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnStatusMessage(StatusMessageEventArgs args)
        {
            StatusMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:LoadError" /> event.
        /// </summary>
        /// <param name="args">The <see cref="LoadErrorEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnLoadError(LoadErrorEventArgs args)
        {
            LoadError?.Invoke(this, args);

            initialLoadAction?.Invoke(null, args.ErrorCode);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has parent.
        /// </summary>
        /// <value><c>true</c> if this instance has parent; otherwise, <c>false</c>.</value>
        bool IWebBrowserInternal.HasParent { get; set; }

        /// <summary>
        /// Used by CefSharp.Puppeteer to associate a single DevToolsContext with a ChromiumWebBrowser instance.
        /// </summary>
        IDisposable IWebBrowserInternal.DevToolsContext { get; set; }

        /// <summary>
        /// Gets the browser adapter.
        /// </summary>
        /// <value>The browser adapter.</value>
        IBrowserAdapter IWebBrowserInternal.BrowserAdapter
        {
            get { return managedCefBrowserAdapter; }
        }

        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            if (IsDisposed || browser.IsDisposed)
            {
                return;
            }

            this.browser = browser;
            BrowserCore = browser;
            initialLoadAction = InitialLoad;
            Interlocked.Exchange(ref browserInitialized, 1);

            OnAfterBrowserCreated(browser);
        }

        /// <summary>
        /// Sets the loading state change.
        /// </summary>
        /// <param name="args">The <see cref="LoadingStateChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetLoadingStateChange(LoadingStateChangedEventArgs args)
        {
            SetLoadingStateChange(args);

            LoadingStateChanged?.Invoke(this, args);

            initialLoadAction?.Invoke(args.IsLoading, null);
        }

        /// <inheritdoc/>
        public void LoadUrl(string url)
        {
            Load(url);
        }

        /// <inheritdoc/>
        public Task<LoadUrlAsyncResponse> LoadUrlAsync(string url)
        {
            //LoadUrlAsync is actually a static method so that CefSharp.Wpf.HwndHost can reuse the code
            return CefSharp.WebBrowserExtensions.LoadUrlAsync(this, url);
        }

        /// <inheritdoc/>
        public Task<WaitForNavigationAsyncResponse> WaitForNavigationAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            //WaitForNavigationAsync is actually a static method so that CefSharp.Wpf.HwndHost can reuse the code
            return CefSharp.WebBrowserExtensions.WaitForNavigationAsync(this, timeout, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<LoadUrlAsyncResponse> WaitForInitialLoadAsync()
        {
            return initialLoadTaskCompletionSource.Task;
        }

        /// <inheritdoc/>
        public bool TryGetBrowserCoreById(int browserId, out IBrowser browser)
        {
            var browserAdapter = managedCefBrowserAdapter;

            if (IsDisposed || browserAdapter == null || browserAdapter.IsDisposed)
            {
                browser = null;

                return false;
            }

            browser = browserAdapter.GetBrowser(browserId);

            return browser != null;
        }

        /// <inheritdoc/>
        public async Task<CefSharp.Structs.DomRect> GetContentSizeAsync()
        {
            ThrowExceptionIfDisposed();
            ThrowExceptionIfBrowserNotInitialized();

            using (var devToolsClient = browser.GetDevToolsClient())
            {
                //Get the content size
                var layoutMetricsResponse = await devToolsClient.Page.GetLayoutMetricsAsync().ConfigureAwait(continueOnCapturedContext: false);
                var rect = layoutMetricsResponse.CssContentSize;

                return new Structs.DomRect(rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        private void InitialLoad(bool? isLoading, CefErrorCode? errorCode)
        {
            if (IsDisposed)
            {
                initialLoadAction = null;

                initialLoadTaskCompletionSource.TrySetCanceled();

                return;
            }

            if (isLoading.HasValue)
            {
                if (isLoading.Value)
                {
                    return;
                }

                initialLoadAction = null;

                var host = browser?.GetHost();

                var navEntry = host?.GetVisibleNavigationEntry();

                int statusCode = navEntry?.HttpStatusCode ?? -1;

                //By default 0 is some sort of error, we map that to -1
                //so that it's clearer that something failed.
                if (statusCode == 0)
                {
                    statusCode = -1;
                }

                initialLoadTaskCompletionSource.TrySetResult(new LoadUrlAsyncResponse(CefErrorCode.None, statusCode));
            }
            else if (errorCode.HasValue)
            {
                //Actions that trigger a download will raise an aborted error.
                //Generally speaking Aborted is safe to ignore
                if (errorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                initialLoadAction = null;

                initialLoadTaskCompletionSource.TrySetResult(new LoadUrlAsyncResponse(errorCode.Value, -1));
            }
        }

        partial void OnAfterBrowserCreated(IBrowser browser);

        partial void SetLoadingStateChange(LoadingStateChangedEventArgs args);

        /// <summary>
        /// Sets the handler references to null.
        /// Where required also calls Dispose().
        /// </summary>
        private void FreeHandlersExceptLifeSpanAndFocus()
        {
            AudioHandler?.Dispose();
            AudioHandler = null;
            DialogHandler = null;
            FindHandler = null;
            RequestHandler = null;
            DisplayHandler = null;
            LoadHandler = null;
            KeyboardHandler = null;
            JsDialogHandler = null;
            DragHandler = null;
            DownloadHandler = null;
            MenuHandler = null;
            ResourceRequestHandlerFactory = null;
            RenderProcessMessageHandler = null;

            this.FreeDevToolsContext();
        }

        /// <summary>
        /// Check is browser is initialized
        /// </summary>
        /// <returns>true if browser is initialized</returns>
        private bool InternalIsBrowserInitialized()
        {
            // Use CompareExchange to read the current value - if disposeCount is 1, we set it to 1, effectively a no-op
            // Volatile.Read would likely use a memory barrier which I believe is unnecessary in this scenario
            return Interlocked.CompareExchange(ref browserInitialized, 0, 0) == 1;
        }

        /// <summary>
        /// Throw exception if browser not initialized.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        private void ThrowExceptionIfBrowserNotInitialized()
        {
            if (!InternalIsBrowserInitialized())
            {
                throw new Exception(BrowserNotInitializedExceptionErrorMessage);
            }
        }

        /// <summary>
        /// Throw exception if disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when a supplied object has been disposed.</exception>
        private void ThrowExceptionIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("ChromiumWebBrowser");
            }
        }

        private int GetChromiumChildProcessId(string frameIdentifier)
        {
            try
            {
                var parts = frameIdentifier.Split('-');

                if (int.TryParse(parts[0], out var childProcessId))
                    return childProcessId;
            }
            catch
            {

            }

            return -1;
        }
    }
}
