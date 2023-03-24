// Copyright © 2013 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using CefSharp.Internals;
using CefSharp.Web;
using CefSharp.WinForms.Internals;
using CefSharp.WinForms.Host;
using CefSharp.DevTools.Page;
using System.Threading.Tasks;

namespace CefSharp.WinForms
{
    /// <summary>
    /// ChromiumWebBrowser is the WinForms web browser control
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Control" />
    /// <seealso cref="CefSharp.WinForms.IWinFormsWebBrowser" />
    [Docking(DockingBehavior.AutoDock), DefaultEvent("LoadingStateChanged"), ToolboxBitmap(typeof(ChromiumWebBrowser)),
    Description("CefSharp ChromiumWebBrowser - Chromium Embedded Framework .Net wrapper. https://github.com/cefsharp/CefSharp"),
    Designer(typeof(ChromiumWebBrowserDesigner))]
    public partial class ChromiumWebBrowser : ChromiumHostControlBase, IWebBrowserInternal, IWinFormsWebBrowser
    {
        //TODO: If we start adding more consts then extract them into a common class
        //Possibly in the CefSharp assembly and move the WPF ones into there as well.
        private const uint WS_EX_NOACTIVATE = 0x08000000;

        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private IBrowserAdapter managedCefBrowserAdapter;
        /// <summary>
        /// The parent form message interceptor
        /// </summary>
        private ParentFormMessageInterceptor parentFormMessageInterceptor;
        /// <summary>
        /// A flag that indicates whether or not the designer is active
        /// NOTE: DesignMode becomes false by the time we get to the destructor/dispose so it gets stored here
        /// </summary>
        private bool designMode;
        /// <summary>
        /// A flag that indicates whether or not <see cref="InitializeFieldsAndCefIfRequired"/> has been called.
        /// </summary>
        private bool initialized;
        /// <summary>
        /// Has the underlying Cef Browser been created (slightly different to initialized in that
        /// the browser is initialized in an async fashion)
        /// </summary>
        private bool browserCreated;
        /// <summary>
        /// A flag indicating if the <see cref="Address"/> was used when calling CreateBrowser
        /// If false and <see cref="Address"/> contains a non empty string Load will be called
        /// on the main frame
        /// </summary>
        private bool initialAddressLoaded;
        /// <summary>
        /// If true the the WS_EX_NOACTIVATE style will be removed so that future mouse clicks
        /// inside the browser correctly activate and focus the window.
        /// </summary>
        private bool removeExNoActivateStyle;
        /// <summary>
        /// Browser initialization settings
        /// </summary>
        private IBrowserSettings browserSettings;
        /// <summary>
        /// The request context (we deliberately use a private variable so we can throw an exception if
        /// user attempts to set after browser created)
        /// </summary>
        private IRequestContext requestContext;

        /// <summary>
        /// Parking control used to temporarily host the CefBrowser instance
        /// when <see cref="Control.RecreatingHandle"/> is <c>true</c>.
        /// </summary>
        private Control parkingControl;
        /// <summary>
        /// This flag is set when the browser gets focus before the underlying CEF browser
        /// has been initialized.
        /// </summary>
        private bool initialFocus;

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><see langword="true" /> if this instance is disposed; otherwise, <see langword="false" />.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public new bool IsDisposed
        {
            get
            {
                return Interlocked.CompareExchange(ref disposeSignaled, 1, 1) == 1;
            }
        }

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IBrowserSettings BrowserSettings
        {
            get
            {
                //We keep a reference to the browserSettings for the case where
                //the Control Handle is destroyed then Created see https://github.com/cefsharp/CefSharp/issues/2840
                //As it's not possible to change settings after the browser has been
                //created, and changing browserSettings then creating a new handle will
                //give a subtle different user experience if you aren't expecting it we
                //return null here even though we still have a reference.
                if (browserCreated)
                {
                    return null;
                }
                return browserSettings;
            }
            set
            {
                if (browserCreated)
                {
                    throw new Exception("Browser has already been created. BrowserSettings must be " +
                                        "set before the underlying CEF browser is created.");
                }
                if (value != null && !Core.ObjectFactory.BrowserSetingsType.IsAssignableFrom(value.UnWrap().GetType()))
                {
                    throw new Exception(string.Format("BrowserSettings can only be of type {0} or null", Core.ObjectFactory.BrowserSetingsType));
                }
                browserSettings = value;
            }
        }
        /// <summary>
        /// Activates browser upon creation, the default value is false. Prior to version 73
        /// the default behaviour was to activate browser on creation (Equivalent of setting this property to true).
        /// To restore this behaviour set this value to true immediately after you create the <see cref="ChromiumWebBrowser"/> instance.
        /// https://github.com/chromiumembedded/cef/issues/1856
        /// </summary>
        public bool ActivateBrowserOnCreation { get; set; }
        /// <summary>
        /// Gets or sets the request context.
        /// </summary>
        /// <value>The request context.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public IRequestContext RequestContext
        {
            get { return requestContext; }
            set
            {
                if (browserCreated)
                {
                    throw new Exception("Browser has already been created. RequestContext must be " +
                                        "set before the underlying CEF browser is created.");
                }
                if (value != null && !Core.ObjectFactory.RequestContextType.IsAssignableFrom(value.UnWrap().GetType()))
                {
                    throw new Exception(string.Format("RequestContext can only be of type {0} or null", Core.ObjectFactory.RequestContextType));
                }
                requestContext = value;
            }
        }
        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is loading; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsLoading { get; private set; }
        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        /// <value>The tooltip text.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public string TooltipText { get; private set; }
        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <value>The address.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public string Address { get; private set; }

        /// <summary>
        /// Occurs when the browser address changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<AddressChangedEventArgs> AddressChanged;
        /// <summary>
        /// Occurs when the browser title changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<TitleChangedEventArgs> TitleChanged;

        /// <summary>
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool CanGoForward { get; private set; }
        /// <summary>
        /// A flag that indicates whether the state of the control current supports the GoBack action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool CanGoBack { get; private set; }
        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsBrowserInitialized
        {
            get { return InternalIsBrowserInitialized(); }
        }

        /// <summary>
        /// ParentFormMessageInterceptor hooks the Form handle and forwards
        /// the move/active messages to the browser, the default is true
        /// and should only be required when using <see cref="CefSettingsBase.MultiThreadedMessageLoop"/>
        /// set to true.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(true)]
        public bool UseParentFormMessageInterceptor { get; set; } = true;

        /// <summary>
        /// By default when <see cref="Control.OnHandleDestroyed(EventArgs)"/> is called
        /// the underlying Browser Hwnd is only parked (moved to a temp parent) 
        /// when <see cref="Control.RecreatingHandle"/> is <c>true</c>, there are a few other
        /// cases where parking of the control is desired, you can force parking by setting
        /// this property to <c>true</c>.
        /// </summary>
        /// <remarks>
        /// You may wish to set this property to <c>true</c> when using the browser in conjunction
        /// with https://github.com/dockpanelsuite/dockpanelsuite
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(true)]
        public bool ParkControlOnHandleDestroyed { get; set; } = false;

        /// <summary>
        /// Initializes static members of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        static ChromiumWebBrowser()
        {
            if (CefSharpSettings.ShutdownOnExit)
            {
                Application.ApplicationExit += OnApplicationExit;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ApplicationExit" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            Cef.Shutdown();
        }

        /// <summary>
        /// To control how <see cref="Cef.Shutdown"/> is called, this method will
        /// unsubscribe from <see cref="Application.ApplicationExit"/>,
        /// </summary>
        public static void UnregisterShutdownHandler()
        {
            Application.ApplicationExit -= OnApplicationExit;
        }

        /// <summary>
        /// <strong>Important!!!</strong>
        /// This constructor exists as the WinForms designer requires a parameterless constructor, if you are instantiating
        /// an instance of this class in code then use the <see cref="ChromiumWebBrowser(string, IRequestContext)"/>
        /// constructor overload instead. Using this constructor in code is unsupported and you may experience <see cref="NullReferenceException"/>'s
        /// when attempting to access some of the properties immediately after instantiation. 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ChromiumWebBrowser()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// **Important** - When using this constructor the <see cref="Control.Dock"/> property
        /// will default to <see cref="DockStyle.Fill"/>.
        /// </summary>
        /// <param name="html">html string to be initially loaded in the browser.</param>
        /// <param name="requestContext">(Optional) Request context that will be used for this browser instance, if null the Global
        /// Request Context will be used.</param>
        public ChromiumWebBrowser(HtmlString html, IRequestContext requestContext = null) : this(html.ToDataUriString(), requestContext)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// **Important** - When using this constructor the <see cref="Control.Dock"/> property
        /// will default to <see cref="DockStyle.Fill"/>.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="requestContext">(Optional) Request context that will be used for this browser instance, if null the Global
        /// Request Context will be used.</param>
        public ChromiumWebBrowser(string address, IRequestContext requestContext = null)
        {
            Dock = DockStyle.Fill;
            Address = address;
            RequestContext = requestContext;

            InitializeFieldsAndCefIfRequired();
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsequently throw an exception.
        /// TODO: Still not happy with this method name, need something better
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void InitializeFieldsAndCefIfRequired()
        {
            if (!initialized)
            {
                if (!Cef.IsInitialized && !Cef.Initialize(new CefSettings()))
                {
                    throw new InvalidOperationException(CefInitializeFailedErrorMessage);
                }

                Cef.AddDisposable(this);

                if (FocusHandler == null)
                {
                    //If the WinForms UI thread and the CEF UI thread are one in the same
                    //then we don't need the FocusHandler, it's only required when using
                    //MultiThreadedMessageLoop (the default)
                    if (!Cef.CurrentlyOnThread(CefThreadIds.TID_UI))
                    {
                        FocusHandler = new DefaultFocusHandler();
                    }
                }

                if (browserSettings == null)
                {
                    browserSettings = Core.ObjectFactory.CreateBrowserSettings(autoDispose: true);
                }

                managedCefBrowserAdapter = ManagedCefBrowserAdapter.Create(this, false);

                initialized = true;
            }
        }

        /// <summary>
        /// If not in design mode; Releases unmanaged and - optionally - managed resources for the <see cref="ChromiumWebBrowser"/>
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Attempt to move the disposeSignaled state from 0 to 1. If successful, we can be assured that
            // this thread is the first thread to do so, and can safely dispose of the object.
            if (Interlocked.CompareExchange(ref disposeSignaled, 1, 0) != 0)
            {
                return;
            }

            if (!designMode)
            {
                InternalDispose(disposing);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources for the <see cref="ChromiumWebBrowser"/>
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        /// <remarks>
        /// This method cannot be inlined as the designer will attempt to load libcef.dll and will subsequently throw an exception.
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void InternalDispose(bool disposing)
        {
            if (disposing)
            {
                Interlocked.Exchange(ref browserInitialized, 0);
                CanExecuteJavascriptInMainFrame = false;

                // Don't maintain a reference to event listeners anylonger:
                AddressChanged = null;
                ConsoleMessage = null;
                FrameLoadEnd = null;
                FrameLoadStart = null;
                LoadError = null;
                LoadingStateChanged = null;
                StatusMessage = null;
                TitleChanged = null;
                JavascriptMessageReceived = null;

                // Release reference to handlers, except LifeSpanHandler which is done after Disposing
                // ManagedCefBrowserAdapter otherwise the ILifeSpanHandler.DoClose will not be invoked.
                // We also leave FocusHandler and override with a NoFocusHandler implementation as
                // it so we can block taking Focus (we're dispoing afterall). Issue #3715
                FreeHandlersExceptLifeSpanAndFocus();

                FocusHandler = new NoFocusHandler();

                browser = null;
                BrowserCore = null;

                if (parentFormMessageInterceptor != null)
                {
                    parentFormMessageInterceptor.Dispose();
                    parentFormMessageInterceptor = null;
                }

                if (managedCefBrowserAdapter != null)
                {
                    managedCefBrowserAdapter.Dispose();
                    managedCefBrowserAdapter = null;
                }

                //Dispose of BrowserSettings if we created it, if user created then they're responsible
                if (browserSettings != null && browserSettings.AutoDispose)
                {
                    browserSettings.Dispose();
                }

                browserSettings = null;

                parkingControl?.Dispose();
                parkingControl = null;

                // LifeSpanHandler is set to null after managedCefBrowserAdapter.Dispose so ILifeSpanHandler.DoClose
                // is called.
                LifeSpanHandler = null;
            }

            Cef.RemoveDisposable(this);
        }

        /// <inheritdoc/>
        public void Load(string url)
        {
            if (IsDisposed)
            {
                return;
            }

            var browserCore = BrowserCore;

            //There's a small window here between CreateBrowser
            //and OnAfterBrowserCreated where the Address prop
            //will be updated, no MainFrame.LoadUrl call will be made.
            if (browserCore == null)
            {
                Address = url;
            }
            else
            {
                if(browserCore.IsDisposed)
                {
                    return;
                }

                if(!browserCore.IsValid)
                {
                    throw new InvalidOperationException("IBrowser instance is no longer valid. Control.Handle was likely destroyed.");
                }

                using (var frame = browserCore.MainFrame)
                {
                    //Only attempt to call load if frame is valid
                    //I've seen so far one case where the MainFrame is invalid.
                    //As yet unable to reproduce
                    if (frame.IsValid)
                    {
                        frame.LoadUrl(url);
                    }
                }
            }
        }

        /// <summary>
        /// Capture page screenshot.
        /// </summary>
        /// <param name="format">Image compression format (defaults to png).</param>
        /// <param name="quality">Compression quality from range [0..100] (jpeg only).</param>
        /// <param name="viewPort">Capture the screenshot of a given region only.</param>
        /// <param name="fromSurface">Capture the screenshot from the surface, rather than the view. Defaults to true.</param>
        /// <param name="captureBeyondViewport">Capture the screenshot beyond the viewport. Defaults to false.</param>
        /// <returns>A task that can be awaited to obtain the screenshot as a byte[].</returns>
        public async Task<byte[]> CaptureScreenshotAsync(CaptureScreenshotFormat format = CaptureScreenshotFormat.Png, int? quality = null, Viewport viewPort = null, bool fromSurface = true, bool captureBeyondViewport = false)
        {
            ThrowExceptionIfDisposed();
            ThrowExceptionIfBrowserNotInitialized();

            if(viewPort != null && viewPort.Scale <= 0)
            {
                throw new ArgumentException($"{nameof(viewPort)}.{nameof(viewPort.Scale)} must be greater than 0.");
            }

            using (var devToolsClient = browser.GetDevToolsClient())
            {
                var screenShot = await devToolsClient.Page.CaptureScreenshotAsync(format, quality, viewPort, fromSurface, captureBeyondViewport).ConfigureAwait(continueOnCapturedContext: false);

                return screenShot.Data;
            }
        }

        /// <summary>
        /// The javascript object repository, one repository per ChromiumWebBrowser instance.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IJavascriptObjectRepository JavascriptObjectRepository
        {
            get
            {
                InitializeFieldsAndCefIfRequired();
                return managedCefBrowserAdapter == null ? null : managedCefBrowserAdapter.JavascriptObjectRepository;
            }
        }

        
        /// <summary>
        ///  Indicates if one of the Ancestors of this control is sited
        ///  and that site in DesignMode.
        /// </summary>
        // Roughly based on https://github.com/dotnet/winforms/pull/5375
        private bool IsParentInDesignMode(Control control)
        {
            if(control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            //Check if our Site is in DesignMode
            //If not then walk up the tree
            //Until we find a Site that is or our parent is null
            if(control.Site?.DesignMode ?? false)
            {
                return true;
            }

            if(control.Parent == null)
            {
                return false;
            }

            return IsParentInDesignMode(control.Parent);
        }


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            designMode = DesignMode;

            //Check if our Parent is in design mode.
            if (!designMode)
            {
                try
                {
                    designMode = IsParentInDesignMode(this);
                }
                catch (Exception)
                {
                    //TODO: We should log the exception
                    //Need to provide a wrapper around CEF Log first
                }
            }

            if(designMode)
            {
                //For design mode only we remove our custom ApplicationExit event handler
                //As we must avoid making all unmanaged calls
                Application.ApplicationExit -= OnApplicationExit;
            }
            else
            {
                InitializeFieldsAndCefIfRequired();

                // NOTE: Had to move the code out of this function otherwise the designer would crash
                CreateBrowser();

                ResizeBrowser(Width, Height);
            }

            base.OnHandleCreated(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!designMode)
            {
                // NOTE: Had to move the code out of this function otherwise the designer would crash
                OnHandleDestroyedInternal();
            }

            base.OnHandleDestroyed(e);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void OnHandleDestroyedInternal()
        {
            //When the Control is being Recreated then we'll park
            //the browser (set to a temp parent) and assign to
            //our new handle when it's ready.
            if (RecreatingHandle || ParkControlOnHandleDestroyed)
            {
                parkingControl = new Control();
                parkingControl.CreateControl();

                var host = this.GetBrowserHost();

                // Possible host is null
                // https://github.com/cefsharp/CefSharp/issues/3931
                if (host != null)
                {
                    var hwnd = host.GetWindowHandle();

                    NativeMethodWrapper.SetWindowParent(hwnd, parkingControl.Handle);
                }
            }
        }

        /// <summary>
        /// Override this method to handle creation of WindowInfo. This method can be used to customise aspects of
        /// browser creation including configuration of settings such as <see cref="IWindowInfo.ExStyle"/>.
        /// Window Activation is disabled by default, you can re-enable it by overriding and removing the
        /// WS_EX_NOACTIVATE style from <see cref="IWindowInfo.ExStyle"/>.
        /// </summary>
        /// <param name="handle">Window handle for the Control</param>
        /// <returns>Window Info</returns>
        /// <example>
        /// To re-enable Window Activation then remove WS_EX_NOACTIVATE from ExStyle
        /// <code>
        /// const uint WS_EX_NOACTIVATE = 0x08000000;
        /// windowInfo.ExStyle &amp;= ~WS_EX_NOACTIVATE;
        ///</code>
        /// </example>
        protected virtual IWindowInfo CreateBrowserWindowInfo(IntPtr handle)
        {
            var windowInfo = Core.ObjectFactory.CreateWindowInfo();
            windowInfo.SetAsChild(handle);

            if (!ActivateBrowserOnCreation)
            {
                //Disable Window activation by default
                //https://github.com/chromiumembedded/cef/issues/1856/branch-2526-cef-activates-browser-window
                windowInfo.ExStyle |= WS_EX_NOACTIVATE;
            }

            return windowInfo;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void CreateBrowser()
        {
            browserCreated = true;

            if (((IWebBrowserInternal)this).HasParent == false)
            {
                //If we are Recreating our handle we will have re-parented our
                //browser to parkingControl. We'll assign the browser to our newly
                //created handle now.
                if ((RecreatingHandle || ParkControlOnHandleDestroyed) && IsBrowserInitialized && browser != null)
                {
                    var host = this.GetBrowserHost();
                    var hwnd = host.GetWindowHandle();

                    NativeMethodWrapper.SetWindowParent(hwnd, Handle);

                    parkingControl.Dispose();
                    parkingControl = null;
                }
                else
                {
                    var windowInfo = CreateBrowserWindowInfo(Handle);

                    //We actually check if WS_EX_NOACTIVATE was set for instances
                    //the user has override CreateBrowserWindowInfo and not called base.CreateBrowserWindowInfo
                    removeExNoActivateStyle = (windowInfo.ExStyle & WS_EX_NOACTIVATE) == WS_EX_NOACTIVATE;

                    initialAddressLoaded = !string.IsNullOrEmpty(Address);

                    managedCefBrowserAdapter.CreateBrowser(windowInfo, browserSettings, requestContext, Address);
                }
            }
        }

        /// <summary>
        /// Called from <see cref="OnAfterBrowserCreated(IBrowser)"/> when we set focus
        /// to the CefBrowser instance via <see cref="IBrowserHost.SetFocus(bool)"/>.
        /// Method is only called if the browser got focus via <see cref="OnGotFocus(EventArgs)"/>
        /// before the call to <see cref="OnAfterBrowserCreated(IBrowser)"/>.
        /// Can be overridden to provide custom behaviour.
        /// </summary>
        protected virtual void OnSetBrowserInitialFocus()
        {
            // MultiThreadedMessageLoop = true
            // Starting in M104 CEF changes mean that calling CefBrowserHost::SetFocus(true)
            // directly in OnAfterCreated result in the browser focus being in a strange state when using
            // MultiThreadedMessageLoop. Dealaying the SetFocus call results in the correct behaviour.
            // Here we Invoke back into the WinForms UI Thread, check if we have Focus then call
            // SetFocus (which will call back onto the CEF UI Thread).
            // It's possible to use Cef.PostAction to invoke directly on the CEF UI Thread,
            // this also seems to work as expected, using the WinForms UI Thread allows
            // us to check the Focused property to determine if we actully have focus
            // https://github.com/chromiumembedded/cef/issues/3436/chromium-based-browser-loses-focus-when
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() =>
                {
                    if (Disposing || IsDisposed || browser?.IsDisposed == true)
                    {
                        return;
                    }

                    if (Focused)
                    {
                        browser?.GetHost()?.SetFocus(true);
                    }
                }));
            }
        }

        /// <summary>
        /// Called after browser created.
        /// </summary>
        /// <param name="browser">The browser.</param>
        partial void OnAfterBrowserCreated(IBrowser browser)
        {
            BrowserHwnd = browser.GetHost().GetWindowHandle();

            // By the time this callback gets called, this control
            // is most likely hooked into a browser Form of some sort. 
            // (Which is what ParentFormMessageInterceptor relies on.)
            // Ensure the ParentFormMessageInterceptor construction occurs on the WinForms UI thread:
            if (UseParentFormMessageInterceptor)
            {
                this.InvokeOnUiThreadIfRequired(() =>
                {
                    parentFormMessageInterceptor = new ParentFormMessageInterceptor(this);
                    parentFormMessageInterceptor.Moving += (sender, args) =>
                    {
                        if (IsBrowserInitialized && !IsDisposed)
                        {
                            browser?.GetHost()?.NotifyMoveOrResizeStarted();
                        }
                    };
                });
            }

            ResizeBrowser(Width, Height);

            //If Load was called after the call to CreateBrowser we'll call Load
            //on the MainFrame
            if (!initialAddressLoaded && !string.IsNullOrEmpty(Address))
            {
                browser.MainFrame.LoadUrl(Address);
            }

            if(initialFocus)
            {
                OnSetBrowserInitialFocus();
            }            

            RaiseIsBrowserInitializedChangedEvent();
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
        partial void SetLoadingStateChange(LoadingStateChangedEventArgs args)
        {
            CanGoBack = args.CanGoBack;
            CanGoForward = args.CanGoForward;
            IsLoading = args.IsLoading;

            if (removeExNoActivateStyle && browser != null)
            {
                removeExNoActivateStyle = false;

                var host = this.GetBrowserHost();
                var hwnd = host.GetWindowHandle();
                //Remove the WS_EX_NOACTIVATE style so that future mouse clicks inside the
                //browser correctly activate and focus the browser. 
                //https://github.com/chromiumembedded/cef/blob/9df4a54308a88fd80c5774d91c62da35afb5fd1b/tests/cefclient/browser/root_window_win.cc#L1088
                NativeMethodWrapper.RemoveExNoActivateStyle(hwnd);
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
        /// Manually implement Focused because cef does not implement it.
        /// </summary>
        /// <value><c>true</c> if focused; otherwise, <c>false</c>.</value>
        /// <remarks>This is also how the Microsoft's WebBrowserControl implements the Focused property.</remarks>
        public override bool Focused
        {
            get
            {
                if (base.Focused)
                {
                    return true;
                }

                if (!IsHandleCreated)
                {
                    return false;
                }

                return NativeMethodWrapper.IsFocused(Handle);
            }
        }

        /// <inheritdoc/>
        protected override void ResizeBrowser(int width, int height)
        {
            if (!designMode && IsBrowserInitialized)
            {
                managedCefBrowserAdapter.Resize(width, height);
            }
        }

        /// <summary>
        /// When minimized set the browser window size to 0x0 to reduce resource usage.
        /// https://github.com/chromiumembedded/cef/blob/c7701b8a6168f105f2c2d6b239ce3958da3e3f13/tests/cefclient/browser/browser_window_std_win.cc#L87
        /// </summary>
        internal override void HideInternal()
        {
            ResizeBrowser(0, 0);
        }

        /// <summary>
        /// Show the browser (called after previous minimised)
        /// </summary>
        internal override void ShowInternal()
        {
            ResizeBrowser(Width, Height);
        }

        /// <inheritdoc/>
        protected override void OnGotFocus(EventArgs e)
        {
            if (IsBrowserInitialized)
            {
                browser.GetHost().SetFocus(true);
            }
            else
            {
                initialFocus = true;
            }

            base.OnGotFocus(e);
        }

        /// <summary>
        /// Returns the current IBrowser Instance
        /// </summary>
        /// <returns>browser instance</returns>
        public IBrowser GetBrowser()
        {
            ThrowExceptionIfDisposed();
            ThrowExceptionIfBrowserNotInitialized();

            return browser;
        }

        /// <summary>
        /// Gets the <see cref="ChromiumWebBrowser"/> associated with
        /// a specific <see cref="IBrowser"/> instance. 
        /// </summary>
        /// <param name="browser">browser</param>
        /// <returns>returns the assocaited <see cref="ChromiumWebBrowser"/> or null if Disposed or no host found.</returns>
        public static ChromiumWebBrowser FromBrowser(IBrowser browser)
        {
            return FromBrowser<ChromiumWebBrowser>(browser);
        }
    }
}
