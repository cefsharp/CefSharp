// Copyright © 2011 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// ChromiumWebBrowser implementations implement this interface. Can be cast to
    /// the concrete implementation to access UI specific features.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IWebBrowser : IDisposable
    {
        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread. 
        /// (The exception to this is when you're running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;

        /// <summary>
        /// Event handler for changes to the status message.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang.
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread. 
        /// (The exception to this is when you're running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        event EventHandler<StatusMessageEventArgs> StatusMessage;

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
        /// <see cref="IRenderProcessMessageHandler.OnContextCreated"/> as it's called when the underlying V8Context is created
        /// </remarks>
        event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;

        /// <summary>
        /// Event handler that will get called when the browser is done loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method will always be called
        /// for all frames irrespective of whether the request completes successfully.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread. 
        /// </summary>
        event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;

        /// <summary>
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread. 
        /// </summary>
        event EventHandler<LoadErrorEventArgs> LoadError;

        /// <summary>
        /// Event handler that will get called when the Loading state has changed.
        /// This event will be fired twice. Once when loading is initiated either programmatically or
        /// by user action, and once when loading is terminated due to completion, cancellation of failure. 
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread. 
        /// </summary>
        event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;

        /// <summary>
        /// Event handler that will get called when the message that originates from CefSharp.PostMessage
        /// </summary>
        event EventHandler<JavascriptMessageReceivedEventArgs> JavascriptMessageReceived;

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        void Load(string url);

        /// <summary>
        /// The javascript object repository, one repository per ChromiumWebBrowser instance.
        /// </summary>
        IJavascriptObjectRepository JavascriptObjectRepository { get; }

        /// <summary>
        /// Implement <see cref="IDialogHandler" /> and assign to handle dialog events.
        /// </summary>
        /// <value>The dialog handler.</value>
        IDialogHandler DialogHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IRequestHandler" /> and assign to handle events related to browser requests.
        /// </summary>
        /// <value>The request handler.</value>
        IRequestHandler RequestHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDisplayHandler" /> and assign to handle events related to browser display state.
        /// </summary>
        /// <value>The display handler.</value>
        IDisplayHandler DisplayHandler { get; set; }

        /// <summary>
        /// Implement <see cref="ILoadHandler" /> and assign to handle events related to browser load status.
        /// </summary>
        /// <value>The load handler.</value>
        ILoadHandler LoadHandler { get; set; }

        /// <summary>
        /// Implement <see cref="ILifeSpanHandler" /> and assign to handle events related to popups.
        /// </summary>
        /// <value>The life span handler.</value>
        ILifeSpanHandler LifeSpanHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IKeyboardHandler" /> and assign to handle events related to key press.
        /// </summary>
        /// <value>The keyboard handler.</value>
        IKeyboardHandler KeyboardHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IJsDialogHandler" /> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        /// <value>The js dialog handler.</value>
        IJsDialogHandler JsDialogHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDragHandler" /> and assign to handle events related to dragging.
        /// </summary>
        /// <value>The drag handler.</value>
        IDragHandler DragHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDownloadHandler" /> and assign to handle events related to downloading files.
        /// </summary>
        /// <value>The download handler.</value>
        IDownloadHandler DownloadHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IContextMenuHandler" /> and assign to handle events related to the browser context menu
        /// </summary>
        /// <value>The menu handler.</value>
        IContextMenuHandler MenuHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFocusHandler" /> and assign to handle events related to the browser component's focus
        /// </summary>
        /// <value>The focus handler.</value>
        IFocusHandler FocusHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IResourceRequestHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        IResourceRequestHandlerFactory ResourceRequestHandlerFactory { get; set; }

        /// <summary>
        /// Implement <see cref="IRenderProcessMessageHandler" /> and assign to handle messages from the render process.
        /// </summary>
        /// <value>The render process message handler.</value>
        IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFindHandler" /> to handle events related to find results.
        /// </summary>
        /// <value>The find handler.</value>
        IFindHandler FindHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IAudioHandler" /> to handle audio events.
        /// </summary>
        IAudioHandler AudioHandler { get; set; }

        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is browser initialized; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control there are two IsBrowserInitialized properties, the ChromiumWebBrowser.IsBrowserInitialized
        /// property is implemented as a Dependency Property and fully supports data binding. This property
        /// can only be called from the UI Thread. The explicit IWebBrowser.IsBrowserInitialized interface implementation that
        /// can be called from any Thread.</remarks>
        bool IsBrowserInitialized { get; }

        /// <summary>
        /// A flag that indicates whether the WebBrowser has been disposed (<see langword="true" />) or not (<see langword="false" />)
        /// </summary>
        /// <value><see langword="true" /> if this instance is disposed; otherwise, <see langword="false" /></value>
        bool IsDisposed { get; }

        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance is loading; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool IsLoading { get; }

        /// <summary>
        /// A flag that indicates whether the state of the control current supports the GoBack action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool CanGoBack { get; }

        /// <summary>
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <value><c>true</c> if this instance can go forward; otherwise, <c>false</c>.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool CanGoForward { get; }

        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <value>The address.</value>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        string Address { get; }

        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        /// <value>The tooltip text.</value>
        string TooltipText { get; }

        /// <summary>
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        bool CanExecuteJavascriptInMainFrame { get; }

        /// <summary>
        /// Gets the custom request context assigned to this browser instance
        /// If no instance was assigned this will be null and the global
        /// request context will have been used for this browser. 
        /// You can access the global request context through Cef.GetGlobalRequestContext()
        /// </summary>
        IRequestContext RequestContext { get; }

        /// <summary>
        /// Attempts to give focus to the IWebBrowser control.
        /// </summary>
        /// <returns><c>true</c> if keyboard focus and logical focus were set to this element; <c>false</c> if only logical focus
        /// was set to this element, or if the call to this method did not force the focus to change.</returns>
        bool Focus();

        /// <summary>
        /// Returns the current CEF Browser Instance
        /// </summary>
        /// <returns>browser instance or null</returns>
        IBrowser GetBrowser();
    }
}
