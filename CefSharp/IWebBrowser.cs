﻿using System;

namespace CefSharp
{
    /// <summary>
    /// A delegate type used to listen to LoadError messages.
    /// </summary>
    /// <param name="failedUrl">The URL that failed to load.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorText">The error text.</param>
    public delegate void LoadErrorEventHandler(string failedUrl, CefErrorCode errorCode, string errorText);

    public interface IWebBrowser
    {
        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// </summary>
        event ConsoleMessageEventHandler ConsoleMessage;

        /// <summary>
        /// Event handler that will get called when the browser begins loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method may not be called for a
        /// particular frame if the load request for that frame fails. For notification of overall browser load status use
        /// OnLoadingStateChange instead.
        /// </summary>        
        event FrameLoadStartEventHandler FrameLoadStart;
        
        /// <summary>
        /// Event handler that will get called when the browser is done loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method will always be called
        /// for all frames irrespective of whether the request completes successfully. 
        /// </summary>        
        event FrameLoadEndEventHandler FrameLoadEnd;

        /// <summary>
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// </summary>        
        event LoadErrorEventHandler LoadError;

        void Load(string url);

        /// <summary>
        /// Loads custom HTML content into the web browser.
        /// </summary>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        void LoadHtml(string html, string url);

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
        /// Execute some Javascript code in the context of this WebBrowser, and return the result of the evaluation.
        /// </summary>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">The timeout after which the Javascript code execution should be aborted.</param>
        object EvaluateScript(string script, TimeSpan? timeout = null);

        IRequestHandler RequestHandler { get; set; }
        bool IsBrowserInitialized { get; }
        
        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        bool IsLoading { get; set; }
        bool CanGoBack { get; }
        bool CanGoForward { get; }

        /// <summary>
        /// The address (URL) which the browser control is currently displaying. Can be set to a simplified URL
        /// (e.g. www.google.com) or a full URL (e.g. http://www.google.com). Will automatically be updated as the user
        /// navigates to another page (e.g. by clicking on a link).
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        string Address { get; set; }
        
        /// <summary>
        /// The title of the web page being currently displayed.
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        string Title { get; }

        string TooltipText { get; set; }

        /// <summary>
        /// Search for text within the current page
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
    }
}
