// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;
using System.Threading.Tasks;

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
        /// </summary>
        event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;

        /// <summary>
        /// Event handler that will get called when the browser is done loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method will always be called
        /// for all frames irrespective of whether the request completes successfully.
        /// </summary>
        event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;

        /// <summary>
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// </summary>
        event EventHandler<LoadErrorEventArgs> LoadError;

        /// <summary>
        /// Event handler that will get called when the Navigation state has changed (Maps to OnLoadingStateChange in Cef).
        /// This event will be fired twice. Once when loading is initiated either programmatically or
        /// by user action, and once when loading is terminated due to completion, cancellation of failure. 
        /// </summary>
        event EventHandler<NavStateChangedEventArgs> NavStateChanged;

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        void Load(string url);

        /// <summary>
        /// Registers and loads a <see cref="ResourceHandler"/> that represents the HTML content.
        /// </summary>
        /// <remarks>
        /// `Cef` Native `LoadHtml` is unpredictable and only works sometimes, this method wraps
        /// the provided HTML in a <see cref="ResourceHandler"/> and loads the provided url using
        /// the <see cref="Load"/> method.
        /// Defaults to using <see cref="Encoding.UTF8"/> for character encoding 
        /// </remarks>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        void LoadHtml(string html, string url);

        /// <summary>
        /// Registers and loads a <see cref="ResourceHandler"/> that represents the HTML content.
        /// </summary>
        /// <remarks>
        /// `Cef` Native `LoadHtml` is unpredictable and only works sometimes, this method wraps
        /// the provided HTML in a <see cref="ResourceHandler"/> and loads the provided url using
        /// the <see cref="Load"/> method.
        /// </remarks>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        /// <param name="encoding">Character Encoding</param>
        void LoadHtml(string html, string url, Encoding encoding);

        /// <summary>
        /// Registers a Javascript object in this specific browser instance.
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        void RegisterJsObject(string name, object objectToBind);

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser. As the method name implies, the script will be
        /// executed asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="script">The Javascript code that should be executed.</param>
        void ExecuteScriptAsync(string script);

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser, and return the result of the evaluation
        /// in an Async fashion
        /// </summary>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">The timeout after which the Javascript code execution should be aborted.</param>
        /// /// <returns>A Task that can be awaited to perform the script execution</returns>
        Task<JavascriptResponse> EvaluateScriptAsync(string script, TimeSpan? timeout = null);

        /// <summary>
        /// Implement <see cref="IDialogHandler"/> and assign to handle dialog events.
        /// </summary>
        IDialogHandler DialogHandler { get; set; }
        
        /// <summary>
        /// Implement <see cref="IRequestHandler"/> and assign to handle events related to browser requests.
        /// </summary>
        IRequestHandler RequestHandler { get; set; }
        
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
        /// Implement <see cref="IMenuHandler"/> and assign to handle events related to the browser context menu
        /// </summary>
        IMenuHandler MenuHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFocusHandler"/> and assign to handle events related to the browser component's focus
        /// </summary>
        IFocusHandler FocusHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IResourceHandler"/> and control the loading of resources
        /// </summary>
        IResourceHandler ResourceHandler { get; set; }

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
        /// A flag that indicates whether the state of the control currently supports the Reload action (true) or not (false).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        bool CanReload { get; }

        /// <summary>
        /// The address (URL) which the browser control is currently displaying.
        /// Will automatically be updated as the user navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        string Address { get; }

        /// <summary>
        /// The title of the web page being currently displayed.
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        string Title { get; }

        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        string TooltipText { get; }

        /// <summary>
        /// The zoom level at which the browser control is currently displaying. Can be set to 0 to clear the zoom level (resets to
        /// default zoom level).
        /// </summary>
        /// <remarks>In the WPF control, this property is implemented as a Dependency Property and fully supports data
        /// binding.</remarks>
        double ZoomLevel { get; set; }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
        /// <param name="identifier">Can be used in can conjunction with searchText to have multiple
        /// searches running simultaneously.</param>
        /// <param name="searchText">search text</param>
        /// <param name="forward">indicates whether to search forward or backward within the page.</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive. </param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up.</param>
        void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext);

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="clearSelection">clear the current search selection</param>
        void StopFinding(bool clearSelection);

        /// <summary>
        /// Navigates back, must check <see cref="CanGoBack"/> before calling this method.
        /// </summary>
        void Back();

        /// <summary>
        /// Navigates forward, must check <see cref="CanGoForward"/> before calling this method.
        /// </summary>
        void Forward();

        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        void Stop();

        /// <summary>
        /// Retrieve the main frame's HTML source using a <see cref="Task{String}"/>.
        /// </summary>
        /// <returns><see cref="Task{String}"/> that when executed returns the frame source as a string</returns>
        Task<string> GetSourceAsync();

        /// <summary>
        /// Retrieve the main frame's display text using a <see cref="Task{String}"/>.
        /// </summary>
        /// <returns><see cref="Task{String}"/> that when executed returns the frame display text as a string.</returns>
        Task<string> GetTextAsync();

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        void ViewSource();

        /// <summary>
        /// Attempts to give focus to the IWpfWebBrowser control.
        /// </summary>
        /// <returns><c>true</c> if keyboard focus and logical focus were set to this element; <c>false</c> if only logical focus
        /// was set to this element, or if the call to this method did not force the focus to change.</returns>
        bool Focus();

        /// <summary>
        /// Reloads the page being displayed. This method will use data from the browser's cache, if available.
        /// </summary>
        void Reload();

        /// <summary>
        /// Reloads the page being displayed, optionally ignoring the cache (which means the whole page including all .css, .js
        /// etc. resources will be re-fetched).
        /// </summary>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring browser cache; <c>false</c> A reload is
        /// performed using files from the browser cache, if available.</param>
        void Reload(bool ignoreCache);

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        void Print();

        /// <summary>
        /// Open developer tools in its own window. 
        /// </summary>
        void ShowDevTools();

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        void CloseDevTools();

        /// <summary>
        /// Send a mouse wheel event to the browser.
        /// </summary>
        /// <param name="x">X-Axis coordinate relative to the upper-left corner of the view.</param>
        /// <param name="y">Y-Axis coordinate relative to the upper-left corner of the view.</param>
        /// <param name="deltaX">Movement delta for X direction.</param>
        /// <param name="deltaY">movement delta for Y direction.</param>
        void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY);
    }
}
