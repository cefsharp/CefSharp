// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefSharp.Internals;
using CefSharp.Web;

namespace CefSharp
{
    /// <summary>
    /// WebBrowser extensions - These methods make performing common tasks easier.
    /// </summary>
    public static class WebBrowserExtensions
    {
        #region Legacy Javascript Binding
        /// <summary>
        /// Registers a Javascript object in this specific browser instance.
        /// </summary>
        /// <param name="webBrowser">The browser to perform the registering on.</param>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        /// <param name="options">(Optional) binding options - camelCaseJavascriptNames default to true.</param>
        /// <exception cref="Exception">Browser is already initialized. RegisterJsObject must be +
        ///                                     called before the underlying CEF browser is created.</exception>
        [Obsolete("This method has been removed, see https://github.com/cefsharp/CefSharp/issues/2990 for details on migrating your code.")]
        public static void RegisterJsObject(this IWebBrowser webBrowser, string name, object objectToBind, BindingOptions options = null)
        {
            throw new NotImplementedException("This method has been removed, see https://github.com/cefsharp/CefSharp/issues/2990 for details on migrating your code.");
        }

        /// <summary>
        /// <para>Asynchronously registers a Javascript object in this specific browser instance.</para>
        /// <para>Only methods of the object will be availabe.</para>
        /// </summary>
        /// <param name="webBrowser">The browser to perform the registering on</param>
        /// <param name="name">The name of the object. (e.g. "foo", if you want the object to be accessible as window.foo).</param>
        /// <param name="objectToBind">The object to be made accessible to Javascript.</param>
        /// <param name="options">binding options - camelCaseJavascriptNames default to true </param>
        /// <exception cref="Exception">Browser is already initialized. RegisterJsObject must be +
        ///                                     called before the underlying CEF browser is created.</exception>
        /// <remarks>The registered methods can only be called in an async way, they will all return immediately and the resulting
        /// object will be a standard javascript Promise object which is usable to wait for completion or failure.</remarks>
        [Obsolete("This method has been removed, see https://github.com/cefsharp/CefSharp/issues/2990 for details on migrating your code.")]
        public static void RegisterAsyncJsObject(this IWebBrowser webBrowser, string name, object objectToBind, BindingOptions options = null)
        {
            throw new NotImplementedException("This method has been removed, see https://github.com/cefsharp/CefSharp/issues/2990 for details on migrating your code.");
        }
        #endregion

        /// <summary>
        /// Returns the main (top-level) frame for the browser window.
        /// </summary>
        /// <param name="browser">the ChromiumWebBrowser instance.</param>
        /// <returns> the main frame. </returns>
        public static IFrame GetMainFrame(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            cefBrowser.ThrowExceptionIfBrowserNull();

            return cefBrowser.MainFrame;
        }

        /// <summary>
        /// Returns the focused frame for the browser window.
        /// </summary>
        /// <param name="browser">the ChromiumWebBrowser instance.</param>
        /// <returns>the focused frame.</returns>
        public static IFrame GetFocusedFrame(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            cefBrowser.ThrowExceptionIfBrowserNull();

            return cefBrowser.FocusedFrame;
        }

        /// <summary>
        /// Execute Undo on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Undo(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Undo();
            }
        }

        /// <summary>
        /// Execute Redo on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Redo(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Redo();
            }
        }

        /// <summary>
        /// Execute Cut on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Cut(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Cut();
            }
        }

        /// <summary>
        /// Execute Copy on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Copy(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Copy();
            }
        }

        /// <summary>
        /// Execute Paste on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Paste(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Paste();
            }
        }

        /// <summary>
        /// Execute Delete on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Delete(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Delete();
            }
        }

        /// <summary>
        /// Execute SelectAll on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void SelectAll(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.SelectAll();
            }
        }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web page is
        /// shown.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void ViewSource(this IWebBrowser browser)
        {
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.ViewSource();
            }
        }

        /// <summary>
        /// Retrieve the main frame's HTML source using a <see cref="Task{String}"/>.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// <see cref="Task{String}"/> that when executed returns the main frame source as a string.
        /// </returns>
        public static Task<string> GetSourceAsync(this IWebBrowser browser)
        {
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                return frame.GetSourceAsync();
            }
        }

        /// <summary>
        /// Retrieve the main frame's display text using a <see cref="Task{String}"/>.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// <see cref="Task{String}"/> that when executed returns the main frame display text as a string.
        /// </returns>
        public static Task<string> GetTextAsync(this IWebBrowser browser)
        {
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                return frame.GetTextAsync();
            }
        }

        /// <summary>
        /// Download the file at url using <see cref="IDownloadHandler"/>. 
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="url">url to download</param>
        public static void StartDownload(this IWebBrowser browser, string url)
        {
            var host = browser.GetBrowserHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.StartDownload(url);
        }

        /// <summary>
        /// See <see cref="IWebBrowser.LoadUrlAsync(string, SynchronizationContext)"/> for details
        /// </summary>
        /// <param name="chromiumWebBrowser">ChromiumWebBrowser instance (cannot be null)</param>
        /// <summary>
        /// Load the <paramref name="url"/> in the main frame of the browser
        /// </summary>
        /// <param name="url">url to load</param>
        /// <param name="ctx">SynchronizationContext to execute the continuation on, if null then the ThreadPool will be used.</param>
        /// <returns>See <see cref="IWebBrowser.LoadUrlAsync(string, SynchronizationContext)"/> for details</returns>
        public static Task<LoadUrlAsyncResponse> LoadUrlAsync(IWebBrowser chromiumWebBrowser, string url = null, SynchronizationContext ctx = null)
        {
            var tcs = new TaskCompletionSource<LoadUrlAsyncResponse>();

            EventHandler<LoadErrorEventArgs> loadErrorHandler = null;
            EventHandler<LoadingStateChangedEventArgs> loadingStateChangeHandler = null;

            loadErrorHandler = (sender, args) =>
            {
                //Ignore Aborted
                //Currently invalid SSL certificates which aren't explicitly allowed
                //end up with CefErrorCode.Aborted, I've created the following PR
                //in the hopes of getting this fixed.
                //https://bitbucket.org/chromiumembedded/cef/pull-requests/373
                if (args.ErrorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                //If LoadError was called then we'll remove both our handlers
                //as we won't need to capture LoadingStateChanged, we know there
                //was an error
                chromiumWebBrowser.LoadError -= loadErrorHandler;
                chromiumWebBrowser.LoadingStateChanged -= loadingStateChangeHandler;

                if (ctx == null)
                {
                    //Ensure our continuation is executed on the ThreadPool
                    //For the .Net Core implementation we could use
                    //TaskCreationOptions.RunContinuationsAsynchronously
                    tcs.TrySetResultAsync(new LoadUrlAsyncResponse(args.ErrorCode, -1));
                }
                else
                {
                    ctx.Post(new SendOrPostCallback((o) =>
                    {
                        tcs.TrySetResult(new LoadUrlAsyncResponse(args.ErrorCode, -1));
                    }), null);
                }
            };

            loadingStateChangeHandler = (sender, args) =>
            {
                //Wait for IsLoading = false
                if (!args.IsLoading)
                {
                    //If LoadingStateChanged was called then we'll remove both our handlers
                    //as LoadError won't be called, our site has loaded with a valid HttpStatusCode
                    //HttpStatusCodes can still be for example 404, this is considered a successful request,
                    //the server responded, it just didn't have the page you were after.
                    chromiumWebBrowser.LoadError -= loadErrorHandler;
                    chromiumWebBrowser.LoadingStateChanged -= loadingStateChangeHandler;

                    var host = args.Browser.GetHost();

                    var navEntry = host?.GetVisibleNavigationEntry();

                    int statusCode = navEntry?.HttpStatusCode ?? -1;

                    //By default 0 is some sort of error, we map that to -1
                    //so that it's clearer that something failed.
                    if (statusCode == 0)
                    {
                        statusCode = -1;
                    }

                    if (ctx == null)
                    {
                        //Ensure our continuation is executed on the ThreadPool
                        //For the .Net Core implementation we could use
                        //TaskCreationOptions.RunContinuationsAsynchronously
                        tcs.TrySetResultAsync(new LoadUrlAsyncResponse(CefErrorCode.None, statusCode));
                    }
                    else
                    {
                        ctx.Post(new SendOrPostCallback((o) =>
                        {
                            tcs.TrySetResult(new LoadUrlAsyncResponse(CefErrorCode.None, statusCode));
                        }), null);
                    }
                }
            };

            chromiumWebBrowser.LoadError += loadErrorHandler;
            chromiumWebBrowser.LoadingStateChanged += loadingStateChangeHandler;

            if (!string.IsNullOrEmpty(url))
            {
                chromiumWebBrowser.Load(url);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser. As the method name implies, the script will be executed
        /// asynchronously, and the method therefore returns before the script has actually been executed. This simple helper extension
        /// will encapsulate params in single quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="methodName">The javascript method name to execute.</param>
        /// <param name="args">the arguments to be passed as params to the method. Args are encoded using
        /// <see cref="EncodeScriptParam"/>, you can provide a custom implementation if you require one.</param>
        public static void ExecuteScriptAsync(this IWebBrowser browser, string methodName, params object[] args)
        {
            var script = GetScriptForJavascriptMethodWithArgs(methodName, args);

            browser.ExecuteScriptAsync(script);
        }

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser. As the method name implies, the script will be executed
        /// asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        public static void ExecuteScriptAsync(this IWebBrowser browser, string script)
        {
            if (browser.CanExecuteJavascriptInMainFrame == false)
            {
                ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse();
            }

            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.ExecuteJavaScriptAsync(script);
            }
        }

        /// <summary>
        /// Execute Javascript code in the context of this WebBrowser. This extension method uses the LoadingStateChanged event. As the
        /// method name implies, the script will be executed asynchronously, and the method therefore returns before the script has
        /// actually been executed.
        /// </summary>
        /// <remarks>
        /// Best effort is made to make sure the script is executed, there are likely a few edge cases where the script won't be executed,
        /// if you suspect your script isn't being executed, then try executing in the LoadingStateChanged event handler to confirm that
        /// it does indeed get executed.
        /// </remarks>
        /// <param name="webBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="oneTime">(Optional) The script will only be executed on first page load, subsequent page loads will be ignored.</param>
        public static void ExecuteScriptAsyncWhenPageLoaded(this IWebBrowser webBrowser, string script, bool oneTime = true)
        {
            var useLoadingStateChangedEventHandler = webBrowser.IsBrowserInitialized == false || oneTime == false;

            //Browser has been initialized, we check if there is a valid document and we're not loading
            if (webBrowser.IsBrowserInitialized)
            {
                //CefBrowser wrapper
                var browser = webBrowser.GetBrowser();
                if (browser.HasDocument && browser.IsLoading == false)
                {
                    webBrowser.ExecuteScriptAsync(script);
                }
                else
                {
                    useLoadingStateChangedEventHandler = true;
                }
            }

            //If the browser hasn't been initialized we can just wire up the LoadingStateChanged event
            //If the script has already been executed and oneTime is false will be hooked up next page load.
            if (useLoadingStateChangedEventHandler)
            {
                EventHandler<LoadingStateChangedEventArgs> handler = null;

                handler = (sender, args) =>
                {
                    //Wait for while page to finish loading not just the first frame
                    if (!args.IsLoading)
                    {
                        if (oneTime)
                        {
                            webBrowser.LoadingStateChanged -= handler;
                        }

                        webBrowser.ExecuteScriptAsync(script);
                    }
                };

                webBrowser.LoadingStateChanged += handler;
            }
        }

        /// <summary>
        /// Creates a new instance of IRequest with the specified Url and Method = POST and then calls
        /// <see cref="IFrame.LoadRequest(IRequest)"/>.
        /// <see cref="IFrame.LoadRequest(IRequest)"/> can only be used if a renderer process already exists.
        /// In newer versions initially loading about:blank no longer creates a renderer process. You can load a Data Uri initially then
        /// call this method. https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs.
        /// </summary>
        /// <param name="browser">browser this method extends</param>
        /// <param name="url">url to load</param>
        /// <param name="postDataBytes">post data as byte array</param>
        /// <param name="contentType">(Optional) if set the Content-Type header will be set</param>
        public static void LoadUrlWithPostData(this IWebBrowser browser, string url, byte[] postDataBytes, string contentType = null)
        {
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                //Initialize Request with PostData
                var request = frame.CreateRequest(initializePostData: true);

                request.Url = url;
                request.Method = "POST";
                //Add AllowStoredCredentials as per suggestion linked in
                //https://github.com/cefsharp/CefSharp/issues/2705#issuecomment-476819788
                request.Flags = UrlRequestFlags.AllowStoredCredentials;

                request.PostData.AddData(postDataBytes);

                if (!string.IsNullOrEmpty(contentType))
                {
                    var headers = new NameValueCollection();
                    headers.Add("Content-Type", contentType);
                    request.Headers = headers;
                }

                frame.LoadRequest(request);
            }
        }

        /// <summary>
        /// Registers and loads a <see cref="ResourceHandler"/> that represents the HTML content.
        /// </summary>
        /// <remarks>
        /// `Cef` Native `LoadHtml` is unpredictable and only works sometimes, this method wraps the provided HTML in a
        /// <see cref="ResourceHandler"/> and loads the provided url using the <see cref="IWebBrowser.Load"/> method. Defaults to using
        /// <see cref="Encoding.UTF8"/> for character encoding The url must start with a valid schema, other uri's such as about:blank
        /// are invalid A valid example looks like http://test/page.
        /// </remarks>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        /// <returns>
        /// returns false if the Url was not successfully parsed into a Uri.
        /// </returns>
        public static bool LoadHtml(this IWebBrowser browser, string html, string url)
        {
            return browser.LoadHtml(html, url, Encoding.UTF8);
        }

        /// <summary>
        /// Loads html as Data Uri See https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs for details If
        /// base64Encode is false then html will be Uri encoded.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="html">Html to load as data uri.</param>
        /// <param name="base64Encode">(Optional) if true the html string will be base64 encoded using UTF8 encoding.</param>
        public static void LoadHtml(this IWebBrowser browser, string html, bool base64Encode = false)
        {
            var htmlString = new HtmlString(html, base64Encode);

            browser.Load(htmlString.ToDataUriString());
        }

        /// <summary>
        /// Loads html as Data Uri See https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs for details If
        /// base64Encode is false then html will be Uri encoded.
        /// </summary>
        /// <param name="frame">The <seealso cref="IFrame"/> instance this method extends.</param>
        /// <param name="html">Html to load as data uri.</param>
        /// <param name="base64Encode">(Optional) if true the html string will be base64 encoded using UTF8 encoding.</param>
        public static void LoadHtml(this IFrame frame, string html, bool base64Encode = false)
        {
            var htmlString = new HtmlString(html, base64Encode);

            frame.LoadUrl(htmlString.ToDataUriString());
        }

        /// <summary>
        /// Registers and loads a <see cref="ResourceHandler"/> that represents the HTML content.
        /// </summary>
        /// <remarks>
        /// `Cef` Native `LoadHtml` is unpredictable and only works sometimes, this method wraps the provided HTML in a
        /// <see cref="ResourceHandler"/> and loads the provided url using the <see cref="IWebBrowser.Load"/> method.
        /// </remarks>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        /// <param name="encoding">Character Encoding.</param>
        /// <param name="oneTimeUse">(Optional) Whether or not the handler should be used once (true) or until manually unregistered
        /// (false)</param>
        /// <returns>
        /// returns false if the Url was not successfully parsed into a Uri.
        /// </returns>
        public static bool LoadHtml(this IWebBrowser browser, string html, string url, Encoding encoding, bool oneTimeUse = false)
        {
            if (browser.ResourceRequestHandlerFactory == null)
            {
                browser.ResourceRequestHandlerFactory = new ResourceRequestHandlerFactory();
            }

            var handler = browser.ResourceRequestHandlerFactory as ResourceRequestHandlerFactory;

            if (handler == null)
            {
                throw new Exception("LoadHtml can only be used with the default IResourceRequestHandlerFactory(DefaultResourceRequestHandlerFactory) implementation");
            }

            if (handler.RegisterHandler(url, ResourceHandler.GetByteArray(html, encoding, true), ResourceHandler.DefaultMimeType, oneTimeUse))
            {
                browser.Load(url);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Register a ResourceHandler. Can only be used when browser.ResourceHandlerFactory is an instance of
        /// DefaultResourceHandlerFactory.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="url">the url of the resource to unregister.</param>
        /// <param name="stream">Stream to be registered, the stream should not be shared with any other instances of
        /// DefaultResourceHandlerFactory.</param>
        /// <param name="mimeType">(Optional) the mimeType.</param>
        /// <param name="oneTimeUse">(Optional) Whether or not the handler should be used once (true) or until manually unregistered
        /// (false). If true the Stream will be Diposed of when finished.</param>
        public static void RegisterResourceHandler(this IWebBrowser browser, string url, Stream stream, string mimeType = ResourceHandler.DefaultMimeType,
            bool oneTimeUse = false)
        {
            if (browser.ResourceRequestHandlerFactory == null)
            {
                browser.ResourceRequestHandlerFactory = new ResourceRequestHandlerFactory();
            }

            var handler = browser.ResourceRequestHandlerFactory as ResourceRequestHandlerFactory;

            if (handler == null)
            {
                throw new Exception("RegisterResourceHandler can only be used with the default IResourceRequestHandlerFactory(DefaultResourceRequestHandlerFactory) implementation");
            }

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);

                handler.RegisterHandler(url, ms.ToArray(), mimeType, oneTimeUse);
            }
        }

        /// <summary>
        /// Unregister a ResourceHandler. Can only be used when browser.ResourceHandlerFactory is an instance of
        /// DefaultResourceHandlerFactory.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="url">the url of the resource to unregister.</param>
        public static void UnRegisterResourceHandler(this IWebBrowser browser, string url)
        {
            var handler = browser.ResourceRequestHandlerFactory as ResourceRequestHandlerFactory;

            if (handler == null)
            {
                throw new Exception("UnRegisterResourceHandler can only be used with the default IResourceRequestHandlerFactory(DefaultResourceRequestHandlerFactory) implementation");
            }

            handler.UnregisterHandler(url);
        }

        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Stop(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.StopLoad();
        }

        /// <summary>
        /// Navigates back, must check <see cref="IWebBrowser.CanGoBack"/> before calling this method.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Back(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.GoBack();
        }

        /// <summary>
        /// Navigates forward, must check <see cref="IWebBrowser.CanGoForward"/> before calling this method.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Forward(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.GoForward();
        }

        /// <summary>
        /// Reloads the page being displayed. This method will use data from the browser's cache, if available.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Reload(this IWebBrowser browser)
        {
            browser.Reload(false);
        }

        /// <summary>
        /// Reloads the page being displayed, optionally ignoring the cache (which means the whole page including all .css, .js etc.
        /// resources will be re-fetched).
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring browser cache; <c>false</c> A reload is performed using
        /// files from the browser cache, if available.</param>
        public static void Reload(this IWebBrowser browser, bool ignoreCache)
        {
            var cefBrowser = browser.GetBrowser();

            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.Reload(ignoreCache);
        }

        /// <summary>
        /// Gets the default cookie manager associated with the IWebBrowser.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="callback">(Optional) If not null it will be executed asynchronously on the CEF IO thread after the manager's
        /// storage has been initialized.</param>
        /// <returns>
        /// Cookie Manager.
        /// </returns>
        public static ICookieManager GetCookieManager(this IWebBrowser browser, ICompletionCallback callback = null)
        {
            var host = browser.GetBrowserHost();

            ThrowExceptionIfBrowserHostNull(host);

            var requestContext = host.RequestContext;

            if (requestContext == null)
            {
                throw new Exception("RequestContext is null, unable to obtain cookie manager");
            }

            return requestContext.GetCookieManager(callback);
        }

        /// <summary>
        /// Asynchronously gets the current Zoom Level.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// An asynchronous result that yields the zoom level.
        /// </returns>
        public static Task<double> GetZoomLevelAsync(this IBrowser cefBrowser)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            return host.GetZoomLevelAsync();
        }

        /// <summary>
        /// Asynchronously gets the current Zoom Level.
        /// </summary>
        /// <param name="browser">the ChromiumWebBrowser instance.</param>
        /// <returns>
        /// An asynchronous result that yields the zoom level.
        /// </returns>
        public static Task<double> GetZoomLevelAsync(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();
            return cefBrowser.GetZoomLevelAsync();
        }

        /// <summary>
        /// Change the ZoomLevel to the specified value. Can be set to 0.0 to clear the zoom level.
        /// </summary>
        /// <remarks>
        /// If called on the CEF UI thread the change will be applied immediately. Otherwise, the change will be applied asynchronously
        /// on the CEF UI thread. The CEF UI thread is different to the WPF/WinForms UI Thread.
        /// </remarks>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="zoomLevel">zoom level.</param>
        public static void SetZoomLevel(this IBrowser cefBrowser, double zoomLevel)
        {
            cefBrowser.ThrowExceptionIfBrowserNull();

            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Change the ZoomLevel to the specified value. Can be set to 0.0 to clear the zoom level.
        /// </summary>
        /// <remarks>
        /// If called on the CEF UI thread the change will be applied immediately. Otherwise, the change will be applied asynchronously
        /// on the CEF UI thread. The CEF UI thread is different to the WPF/WinForms UI Thread.
        /// </remarks>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="zoomLevel">zoom level.</param>
        public static void SetZoomLevel(this IWebBrowser browser, double zoomLevel)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="identifier">Can be used in can conjunction with searchText to have multiple searches running simultaneously.</param>
        /// <param name="searchText">search text.</param>
        /// <param name="forward">indicates whether to search forward or backward within the page.</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive.</param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up.</param>
        public static void Find(this IBrowser cefBrowser, int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.Find(identifier, searchText, forward, matchCase, findNext);
        }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="identifier">Can be used in can conjunction with searchText to have multiple searches running simultaneously.</param>
        /// <param name="searchText">search text.</param>
        /// <param name="forward">indicates whether to search forward or backward within the page.</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive.</param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up.</param>
        public static void Find(this IWebBrowser browser, int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.Find(identifier, searchText, forward, matchCase, findNext);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="clearSelection">clear the current search selection.</param>
        public static void StopFinding(this IBrowser cefBrowser, bool clearSelection)
        {
            cefBrowser.ThrowExceptionIfBrowserNull();

            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.StopFinding(clearSelection);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="clearSelection">clear the current search selection.</param>
        public static void StopFinding(this IWebBrowser browser, bool clearSelection)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.StopFinding(clearSelection);
        }

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Print(this IBrowser cefBrowser)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.Print();
        }

        /// <summary>
        /// Asynchronously prints the current browser contents to the PDF file specified. The caller is responsible for deleting the file
        /// when done.
        /// </summary>
        /// <param name="cefBrowser">The <see cref="IBrowser"/> object this method extends.</param>
        /// <param name="path">Output file location.</param>
        /// <param name="settings">(Optional) Print Settings.</param>
        /// <returns>
        /// A task that represents the asynchronous print operation. The result is true on success or false on failure to generate the
        /// Pdf.
        /// </returns>
        public static Task<bool> PrintToPdfAsync(this IBrowser cefBrowser, string path, PdfPrintSettings settings = null)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            var callback = new TaskPrintToPdfCallback();
            host.PrintToPdf(path, settings, callback);

            return callback.Task;
        }

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Print(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.Print();
        }

        /// <summary>
        /// Asynchronously prints the current browser contents to the PDF file specified. The caller is responsible for deleting the file
        /// when done.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="path">Output file location.</param>
        /// <param name="settings">(Optional) Print Settings.</param>
        /// <returns>
        /// A task that represents the asynchronous print operation. The result is true on success or false on failure to generate the
        /// Pdf.
        /// </returns>
        public static Task<bool> PrintToPdfAsync(this IWebBrowser browser, string path, PdfPrintSettings settings = null)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();

            return cefBrowser.PrintToPdfAsync(path, settings);
        }

        /// <summary>
        /// Open developer tools in its own window.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="windowInfo">(Optional) window info used for showing dev tools.</param>
        /// <param name="inspectElementAtX">(Optional) x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">(Optional) y coordinate (used for inspectElement)</param>
        public static void ShowDevTools(this IBrowser cefBrowser, IWindowInfo windowInfo = null, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.ShowDevTools(windowInfo, inspectElementAtX, inspectElementAtY);
        }

        /// <summary>
        /// Open developer tools in its own window.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="windowInfo">(Optional) window info used for showing dev tools.</param>
        /// <param name="inspectElementAtX">(Optional) x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">(Optional) y coordinate (used for inspectElement)</param>
        public static void ShowDevTools(this IWebBrowser browser, IWindowInfo windowInfo = null, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();
            cefBrowser.ShowDevTools(windowInfo, inspectElementAtX, inspectElementAtY);
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        public static void CloseDevTools(this IBrowser cefBrowser)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.CloseDevTools();
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void CloseDevTools(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();
            cefBrowser.CloseDevTools();
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling this method will replace it with the specified word.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="word">The new word that will replace the currently selected word.</param>
        public static void ReplaceMisspelling(this IBrowser cefBrowser, string word)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.ReplaceMisspelling(word);
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling this method will replace it with the specified word.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="word">The new word that will replace the currently selected word.</param>
        public static void ReplaceMisspelling(this IWebBrowser browser, string word)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.ReplaceMisspelling(word);
        }

        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="word">The new word that will be added to the dictionary.</param>
        public static void AddWordToDictionary(this IBrowser cefBrowser, string word)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.AddWordToDictionary(word);
        }

        /// <summary>
        /// Shortcut method to get the browser IBrowserHost.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// browserHost or null.
        /// </returns>
        public static IBrowserHost GetBrowserHost(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            return cefBrowser == null ? null : cefBrowser.GetHost();
        }

        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="word">The new word that will be added to the dictionary.</param>
        public static void AddWordToDictionary(this IWebBrowser browser, string word)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.AddWordToDictionary(word);
        }

        /// <summary>
        /// Send a mouse wheel event to the browser.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="x">The x coordinate relative to upper-left corner of view.</param>
        /// <param name="y">The y coordinate relative to upper-left corner of view.</param>
        /// <param name="deltaX">The delta x coordinate.</param>
        /// <param name="deltaY">The delta y coordinate.</param>
        /// <param name="modifiers">The modifiers.</param>
        public static void SendMouseWheelEvent(this IWebBrowser browser, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.ThrowExceptionIfBrowserNull();

            cefBrowser.SendMouseWheelEvent(x, y, deltaX, deltaY, modifiers);
        }

        /// <summary>
        /// Send a mouse wheel event to the browser.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="x">The x coordinate relative to upper-left corner of view.</param>
        /// <param name="y">The y coordinate relative to upper-left corner of view.</param>
        /// <param name="deltaX">The delta x coordinate.</param>
        /// <param name="deltaY">The delta y coordinate.</param>
        /// <param name="modifiers">The modifiers.</param>
        public static void SendMouseWheelEvent(this IBrowser browser, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            browser.ThrowExceptionIfBrowserNull();

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseWheelEvent(new MouseEvent(x, y, modifiers), deltaX, deltaY);
        }

        /// <summary>
        /// Send a mouse wheel event to the browser.
        /// </summary>
        /// <param name="host">browserHost.</param>
        /// <param name="x">The x coordinate relative to upper-left corner of view.</param>
        /// <param name="y">The y coordinate relative to upper-left corner of view.</param>
        /// <param name="deltaX">The delta x coordinate.</param>
        /// <param name="deltaY">The delta y coordinate.</param>
        /// <param name="modifiers">The modifiers.</param>
        public static void SendMouseWheelEvent(this IBrowserHost host, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseWheelEvent(new MouseEvent(x, y, modifiers), deltaX, deltaY);
        }

        /// <summary>
        /// Send a mouse click event to the browser.
        /// </summary>
        /// <param name="host">browserHost.</param>
        /// <param name="x">The x coordinate relative to upper-left corner of view.</param>
        /// <param name="y">The y coordinate relative to upper-left corner of view.</param>
        /// <param name="mouseButtonType">Type of the mouse button.</param>
        /// <param name="mouseUp">True to mouse up.</param>
        /// <param name="clickCount">Number of clicks.</param>
        /// <param name="modifiers">The modifiers.</param>
        public static void SendMouseClickEvent(this IBrowserHost host, int x, int y, MouseButtonType mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers)
        {
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseClickEvent(new MouseEvent(x, y, modifiers), mouseButtonType, mouseUp, clickCount);
        }

        /// <summary>
        /// Send a mouse move event to the browser.
        /// </summary>
        /// <param name="host">browserHost.</param>
        /// <param name="x">The x coordinate relative to upper-left corner of view.</param>
        /// <param name="y">The y coordinate relative to upper-left corner of view.</param>
        /// <param name="mouseLeave">mouse leave.</param>
        /// <param name="modifiers">The modifiers.</param>
        public static void SendMouseMoveEvent(this IBrowserHost host, int x, int y, bool mouseLeave, CefEventFlags modifiers)
        {
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseMoveEvent(new MouseEvent(x, y, modifiers), mouseLeave);
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of the MainFrame of the ChromiumWebBrowser. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript. The result of the script execution
        /// in javascript is Promise.resolve so even no promise values will be treated as a promise. Your javascript should return a value.
        /// The javascript will be wrapped in an Immediately Invoked Function Expression.
        /// When the promise either trigger then/catch this returned Task will be completed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <param name="chromiumWebBrowser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsPromiseAsync(this IWebBrowser chromiumWebBrowser, string script, TimeSpan? timeout = null)
        {
            var jsbSettings = chromiumWebBrowser.JavascriptObjectRepository.Settings;

            var promiseHandlerScript = GetPromiseHandlerScript(script, jsbSettings.JavascriptBindingApiGlobalObjectName);

            return chromiumWebBrowser.EvaluateScriptAsync(promiseHandlerScript, timeout: timeout, useImmediatelyInvokedFuncExpression: true);
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of the MainFrame of the ChromiumWebBrowser. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript. The result of the script execution
        /// in javascript is Promise.resolve so even no promise values will be treated as a promise. Your javascript should return a value.
        /// The javascript will be wrapped in an Immediately Invoked Function Expression.
        /// When the promise either trigger then/catch this returned Task will be completed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <param name="frame">The <seealso cref="IFrame"/> instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsPromiseAsync(this IFrame frame, string script, TimeSpan? timeout = null)
        {
            var promiseHandlerScript = GetPromiseHandlerScript(script, null);

            return frame.EvaluateScriptAsync(promiseHandlerScript, timeout: timeout, useImmediatelyInvokedFuncExpression: true);
        }

        private static string GetPromiseHandlerScript(string script, string javascriptBindingApiGlobalObjectName)
        {
            var internalJsFunctionName = "cefSharp.sendEvalScriptResponse";

            //If the user chose to customise the name of the object CefSharp
            //creates in Javascript then we'll workout what the name should be.
            if (!string.IsNullOrWhiteSpace(javascriptBindingApiGlobalObjectName))
            {
                internalJsFunctionName = javascriptBindingApiGlobalObjectName;

                if (char.IsLower(internalJsFunctionName[0]))
                {
                    internalJsFunctionName += ".sendEvalScriptResponse";
                }
                else
                {
                    internalJsFunctionName += ".SendEvalScriptResponse";
                }
            }
            var promiseHandlerScript = "let innerImmediatelyInvokedFuncExpression = (async function() { " + script + " })(); Promise.resolve(innerImmediatelyInvokedFuncExpression).then((val) => " + internalJsFunctionName + "(cefSharpInternalCallbackId, true, val, false)).catch ((reason) => " + internalJsFunctionName + "(cefSharpInternalCallbackId, false, String(reason), false)); return 'CefSharpDefEvalScriptRes';";

            return promiseHandlerScript;
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of the MainFrame of the ChromiumWebBrowser. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <param name="useImmediatelyInvokedFuncExpression">When true the script is wrapped in a self executing function.
        /// Make sure to use a return statement in your javascript. e.g. (function () { return 42; })();
        /// When false don't include a return statement e.g. 42;
        /// </param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IWebBrowser browser, string script, TimeSpan? timeout = null, bool useImmediatelyInvokedFuncExpression = false)
        {
            if (timeout.HasValue && timeout.Value.TotalMilliseconds > UInt32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout greater than Maximum allowable value of " + UInt32.MaxValue);
            }

            if (browser.CanExecuteJavascriptInMainFrame == false)
            {
                ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse();
            }

            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                return frame.EvaluateScriptAsync(script, timeout: timeout, useImmediatelyInvokedFuncExpression: useImmediatelyInvokedFuncExpression);
            }
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of this WebBrowser. The script will be executed asynchronously and the method
        /// returns a Task encapsulating the response from the Javascript This simple helper extension will encapsulate params in single
        /// quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="methodName">The javascript method name to execute.</param>
        /// <param name="args">the arguments to be passed as params to the method.</param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IWebBrowser browser, string methodName, params object[] args)
        {
            return browser.EvaluateScriptAsync(null, methodName, args);
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of this WebBrowser using the specified timeout. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript This simple helper extension will
        /// encapsulate params in single quotes (unless int, uint, etc).
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="timeout">The timeout after which the Javascript code execution should be aborted.</param>
        /// <param name="methodName">The javascript method name to execute.</param>
        /// <param name="args">the arguments to be passed as params to the method. Args are encoded using
        /// <see cref="EncodeScriptParam"/>, you can provide a custom implementation if you require a custom implementation.</param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IWebBrowser browser, TimeSpan? timeout, string methodName, params object[] args)
        {
            var script = GetScriptForJavascriptMethodWithArgs(methodName, args);

            return browser.EvaluateScriptAsync(script, timeout);
        }

        /// <summary>
        /// An IWebBrowser extension method that sets the <see cref="IWebBrowserInternal.HasParent"/>
        /// property used when passing a ChromiumWebBrowser instance to <see cref="ILifeSpanHandler.OnBeforePopup"/>
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void SetAsPopup(this IWebBrowser browser)
        {
            var internalBrowser = (IWebBrowserInternal)browser;

            internalBrowser.HasParent = true;
        }

        /// <summary>
        /// Function used to encode the params passed to <see cref="ExecuteScriptAsync(IWebBrowser, string, object[])"/>,
        /// <see cref="EvaluateScriptAsync(IWebBrowser, string, object[])"/> and
        /// <see cref="EvaluateScriptAsync(IWebBrowser, TimeSpan?, string, object[])"/>
        /// Provide your own custom function to perform custom encoding. You can use your choice of JSON encoder here if you should so
        /// choose.
        /// </summary>
        /// <value>
        /// A function delegate that yields a string.
        /// </value>
        public static Func<string, string> EncodeScriptParam { get; set; } = (str) =>
        {
            return str.Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\t", "\\t")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        };

        /// <summary>
        /// Checks if the given object is a numerical object.
        /// </summary>
        /// <param name="value">The object to check.</param>
        /// <returns>
        /// True if numeric, otherwise false.
        /// </returns>
        private static bool IsNumeric(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        /// <summary>
        /// Transforms the methodName and arguments into valid Javascript code. Will encapsulate params in single quotes (unless int,
        /// uint, etc)
        /// </summary>
        /// <param name="methodName">The javascript method name to execute.</param>
        /// <param name="args">the arguments to be passed as params to the method.</param>
        /// <returns>
        /// The Javascript code.
        /// </returns>
        public static string GetScriptForJavascriptMethodWithArgs(string methodName, object[] args)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(methodName);
            stringBuilder.Append("(");

            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    var obj = args[i];
                    if (obj == null)
                    {
                        stringBuilder.Append("null");
                    }
                    else if (obj.IsNumeric())
                    {
                        stringBuilder.Append(Convert.ToString(args[i], CultureInfo.InvariantCulture));
                    }
                    else if (obj is bool)
                    {
                        stringBuilder.Append(args[i].ToString().ToLowerInvariant());
                    }
                    else
                    {
                        stringBuilder.Append("'");
                        stringBuilder.Append(EncodeScriptParam(obj.ToString()));
                        stringBuilder.Append("'");
                    }

                    stringBuilder.Append(", ");
                }

                //Remove the trailing comma
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            stringBuilder.Append(");");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Throw exception if frame null.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="frame">The <seealso cref="IFrame"/> instance this method extends.</param>
        private static void ThrowExceptionIfFrameNull(IFrame frame)
        {
            if (frame == null)
            {
                throw new Exception("IFrame instance is null. Browser has likely not finished initializing or is in the process of disposing.");
            }
        }

        /// <summary>
        /// An IBrowser extension method that throw exception if browser null.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        internal static void ThrowExceptionIfBrowserNull(this IBrowser browser)
        {
            if (browser == null)
            {
                throw new Exception("IBrowser instance is null. Browser has likely not finished initializing or is in the process of disposing.");
            }
        }

        /// <summary>
        /// Throw exception if browser host null.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browserHost">The browser host.</param>
        internal static void ThrowExceptionIfBrowserHostNull(IBrowserHost browserHost)
        {
            if (browserHost == null)
            {
                throw new Exception("IBrowserHost instance is null. Browser has likely not finished initializing or is in the process of disposing.");
            }
        }

        /// <summary>
        /// Throw exception if can execute javascript in main frame false.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        private static void ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse()
        {
            throw new Exception("Unable to execute javascript at this time, scripts can only be executed within a V8Context. " +
                                    "Use the IWebBrowser.CanExecuteJavascriptInMainFrame property to guard against this exception. " +
                                    "See https://github.com/cefsharp/CefSharp/wiki/General-Usage#when-can-i-start-executing-javascript " +
                                    "for more details on when you can execute javascript. For frames that do not contain Javascript then no " +
                                    "V8Context will be created. Executing a script once the frame has loaded it's possible to create a V8Context. " +
                                    "You can use browser.GetMainFrame().ExecuteJavaScriptAsync(script) or browser.GetMainFrame().EvaluateScriptAsync " +
                                    "to bypass these checks (advanced users only).");
        }
    }
}
