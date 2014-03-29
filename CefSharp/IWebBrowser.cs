using System;

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
        /// Event handler for receiving JavaScript console messages being sent from web pages.
        /// </summary>
        event ConsoleMessageEventHandler ConsoleMessage;

        /// <summary>
        /// Event handler that will get called whenever page loading is complete.
        /// </summary>        
        event LoadCompletedEventHandler LoadCompleted;

        /// <summary>
        /// Event handler that will get called whenever a load error occurs.
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
        /// Registers a JavaScript object in this specific browser instance.
        /// </summary>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to JavaScript.</param>
        void RegisterJsObject(string name, object objectToBind);

        /// <summary>
        /// Execute some JavaScript code in the context of this WebBrowser. As the method name implies, the script will be
        /// executed asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="script">The JavaScript code that should be executed.</param>
        void ExecuteScriptAsync(string script);

        /// <summary>
        /// Execute some JavaScript code in the context of this WebBrowser, and return the result of the evaluation.
        /// </summary>
        /// <param name="script">The JavaScript code that should be executed.</param>
        /// <param name="timeout">The timeout after which the JavaScript code execution should be aborted.</param>
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
    }
}
