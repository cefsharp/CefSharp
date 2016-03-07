// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public interface IWebBrowser : IDisposable
    {
        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// </summary>
        event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;

        /// <summary>
        /// Event handler for changes to the status message.
        /// </summary>
        event EventHandler<StatusMessageEventArgs> StatusMessage;

        /// <summary>
        /// Event handler that will get called when the browser begins loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method may not be called for a
        /// particular frame if the load request for that frame fails. For notification of overall browser load status use
        /// OnLoadingStateChange instead.
        /// This event will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;

        /// <summary>
        /// Event handler that will get called when the browser is done loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method will always be called
        /// for all frames irrespective of whether the request completes successfully.
        /// This event will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;

        /// <summary>
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// This event will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        event EventHandler<LoadErrorEventArgs> LoadError;

        /// <summary>
        /// Event handler that will get called when the Loading state has changed.
        /// This event will be fired twice. Once when loading is initiated either programmatically or
        /// by user action, and once when loading is terminated due to completion, cancellation of failure. 
        /// This event will be called on the CEF UI thread.
        /// Blocking this thread will likely cause your UI to become unresponsive and/or hang.
        /// </summary>
        event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        void Load(string url);

        /// <summary>
        /// Registers a Javascript object in this specific browser instance.
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        /// <param name="camelCaseJavascriptNames">camel case the javascript names of properties/methods, defaults to true</param>
        void RegisterJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true);

        /// <summary>
        /// <para>Asynchronously registers a Javascript object in this specific browser instance.</para>
        /// <para>Only methods of the object will be availabe.</para>
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        /// <param name="camelCaseJavascriptNames">camel case the javascript names of methods, defaults to true</param>
        /// <remarks>
        /// The registered methods can only be called in an async way, they will all return immeditaly and the resulting
        /// object will be a standard javascript Promise object which is usable to wait for completion or failure.
        /// </remarks>
        void RegisterAsyncJsObject(string name, object objectToBind, bool camelCaseJavascriptNames = true);

        /// <summary>
        /// Implement <see cref="IDialogHandler"/> and assign to handle dialog events.
        /// </summary>
        IDialogHandler DialogHandler { get; set; }
        
        /// <summary>
        /// Implement <see cref="IRequestHandler"/> and assign to handle events related to browser requests.
        /// </summary>
        IRequestHandler RequestHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDisplayHandler"/> and assign to handle events related to browser display state.
        /// </summary>
        IDisplayHandler DisplayHandler { get; set; }

        /// <summary>
        /// Implement <see cref="ILoadHandler"/> and assign to handle events related to browser load status.
        /// </summary>
        ILoadHandler LoadHandler { get; set; }

        /// <summary>
        /// Implement <see cref="ILifeSpanHandler"/> and assign to handle events related to popups.
        /// </summary>
        ILifeSpanHandler LifeSpanHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IKeyboardHandler"/> and assign to handle events related to key press.
        /// </summary>
        IKeyboardHandler KeyboardHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IJsDialogHandler"/> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        IJsDialogHandler JsDialogHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDragHandler"/> and assign to handle events related to dragging.
        /// </summary>
        IDragHandler DragHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDownloadHandler"/> and assign to handle events related to downloading files.
        /// </summary>
        IDownloadHandler DownloadHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IContextMenuHandler"/> and assign to handle events related to the browser context menu
        /// </summary>
        IContextMenuHandler MenuHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFocusHandler"/> and assign to handle events related to the browser component's focus
        /// </summary>
        IFocusHandler FocusHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IResourceHandlerFactory"/> and control the loading of resources
        /// </summary>
        IResourceHandlerFactory ResourceHandlerFactory { get; set; }

        /// <summary>
        /// Implement <see cref="IGeolocationHandler"/> and assign to handle requests for permission to use geolocation.
        /// </summary>
        IGeolocationHandler GeolocationHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IRenderProcessMessageHandler"/> and assign to handle messages from the render process. 
        /// </summary>
        IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFindHandler"/> to handle events related to find results.
        /// </summary>
        IFindHandler FindHandler { get; set; }

        /// <summary>
        /// A flag that indicates whether the WebBrowser is initialized (true) or not (false).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool IsBrowserInitialized { get; }

        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool IsLoading { get; }

        /// <summary>
        /// A flag that indicates whether the state of the control current supports the GoBack action (true) or not (false).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool CanGoBack { get; }

        /// <summary>
        /// A flag that indicates whether the state of the control currently supports the GoForward action (true) or not (false).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool CanGoForward { get; }

        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        string Address { get; }

        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        string TooltipText { get; }

        /// <summary>
        /// Attempts to give focus to the IWpfWebBrowser control.
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
