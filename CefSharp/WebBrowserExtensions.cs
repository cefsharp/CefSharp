// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
        public const string BrowserNullExceptionString = "IBrowser instance is null. Browser has likely not finished initializing or is in the process of disposing.";
        public const string BrowserHostNullExceptionString = "IBrowserHost instance is null. Browser has likely not finished initializing or is in the process of disposing.";
        public const string FrameNullExceptionString = "IFrame instance is null. Browser has likely not finished initializing or is in the process of disposing.";

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
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        public static IFrame GetMainFrame(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            var cefBrowser = browser.BrowserCore;

            ThrowExceptionIfBrowserNull(cefBrowser);

            return cefBrowser.MainFrame;
        }

        /// <summary>
        /// Returns the focused frame for the browser window.
        /// </summary>
        /// <param name="browser">the ChromiumWebBrowser instance.</param>
        /// <returns>the focused frame.</returns>
        public static IFrame GetFocusedFrame(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            var cefBrowser = browser.BrowserCore;

            ThrowExceptionIfBrowserNull(cefBrowser);

            return cefBrowser.FocusedFrame;
        }

        /// <summary>
        /// Execute Undo on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Undo(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Undo();
        }

        /// <summary>
        /// Execute Undo on the focused frame.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Undo(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Undo();
            }
        }

        /// <summary>
        /// Execute Redo on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Redo(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Redo();
        }

        /// <summary>
        /// Execute Redo on the focused frame.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Redo(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Redo();
            }
        }

        /// <summary>
        /// Execute Cut on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Cut(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Cut();
        }

        /// <summary>
        /// Execute Cut on the focused frame.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Cut(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Cut();
            }
        }

        /// <summary>
        /// Execute Copy on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Copy(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Copy();
        }

        /// <summary>
        /// Execute Copy on the focused frame.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Copy(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Copy();
            }
        }

        /// <summary>
        /// Execute Paste on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Paste(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Paste();
        }

        /// <summary>
        /// Execute Paste on the focused frame.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Paste(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Paste();
            }
        }

        /// <summary>
        /// Execute Delete on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Delete(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Delete();
        }

        /// <summary>
        /// Execute Delete on the focused frame.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Delete(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Delete();
            }
        }

        /// <summary>
        /// Execute SelectAll on the focused frame.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void SelectAll(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.SelectAll();
        }

        /// <summary>
        /// Execute SelectAll on the focused frame.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void SelectAll(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
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
        public static void ViewSource(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.ViewSource();
        }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web page is
        /// shown.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void ViewSource(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.MainFrame)
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
        public static Task<string> GetSourceAsync(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            return browser.BrowserCore.GetSourceAsync();
        }

        /// <summary>
        /// Retrieve the main frame's HTML source using a <see cref="Task{String}"/>.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <returns>
        /// <see cref="Task{String}"/> that when executed returns the main frame source as a string.
        /// </returns>
        public static Task<string> GetSourceAsync(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
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
        public static Task<string> GetTextAsync(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            return browser.BrowserCore.GetTextAsync();
        }

        /// <summary>
        /// Retrieve the main frame's display text using a <see cref="Task{String}"/>.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <returns>
        /// <see cref="Task{String}"/> that when executed returns the main frame display text as a string.
        /// </returns>
        public static Task<string> GetTextAsync(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.FocusedFrame)
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
        public static void StartDownload(this IChromiumWebBrowserBase browser, string url)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.StartDownload(url);
        }

        /// <summary>
        /// Download the file at url using <see cref="IDownloadHandler"/>. 
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <param name="url">url to download</param>
        public static void StartDownload(this IBrowser browser, string url)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.StartDownload(url);
        }

        /// <summary>
        /// See <see cref="IChromiumWebBrowserBase.LoadUrlAsync(string)"/> for details
        /// </summary>
        /// <param name="chromiumWebBrowser">ChromiumWebBrowser instance (cannot be null)</param>
        /// <summary>
        /// Load the <paramref name="url"/> in the main frame of the browser
        /// </summary>
        /// <param name="url">url to load</param>
        /// <returns>See <see cref="IChromiumWebBrowserBase.LoadUrlAsync(string)"/> for details</returns>
        public static Task<LoadUrlAsyncResponse> LoadUrlAsync(IChromiumWebBrowserBase chromiumWebBrowser, string url)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(chromiumWebBrowser);

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var tcs = new TaskCompletionSource<LoadUrlAsyncResponse>();

            EventHandler<LoadErrorEventArgs> loadErrorHandler = null;
            EventHandler<LoadingStateChangedEventArgs> loadingStateChangeHandler = null;

            loadErrorHandler = (sender, args) =>
            {
                //Actions that trigger a download will raise an aborted error.
                //Generally speaking Aborted is safe to ignore
                if (args.ErrorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                //If LoadError was called then we'll remove both our handlers
                //as we won't need to capture LoadingStateChanged, we know there
                //was an error
                chromiumWebBrowser.LoadError -= loadErrorHandler;
                chromiumWebBrowser.LoadingStateChanged -= loadingStateChangeHandler;

                //Ensure our continuation is executed on the ThreadPool
                //For the .Net Core implementation we could use
                //TaskCreationOptions.RunContinuationsAsynchronously
                tcs.TrySetResultAsync(new LoadUrlAsyncResponse(args.ErrorCode, -1));
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

                    //Ensure our continuation is executed on the ThreadPool
                    //For the .Net Core implementation we could use
                    //TaskCreationOptions.RunContinuationsAsynchronously
                    tcs.TrySetResultAsync(new LoadUrlAsyncResponse(statusCode == -1 ? CefErrorCode.Failed : CefErrorCode.None, statusCode));
                }
            };

            chromiumWebBrowser.LoadError += loadErrorHandler;
            chromiumWebBrowser.LoadingStateChanged += loadingStateChangeHandler;

            chromiumWebBrowser.LoadUrl(url);

            return tcs.Task;
        }

        /// <summary>
        /// This resolves when the browser navigates to a new URL or reloads.
        /// It is useful for when you run code which will indirectly cause the browser to navigate.
        /// A common use case would be when executing javascript that results in a navigation. e.g. clicks a link
        /// This must be called before executing the action that navigates the browser. It may not resolve correctly
        /// if called after.
        /// </summary>
        /// <remarks>
        /// Usage of the <c>History API</c> <see href="https://developer.mozilla.org/en-US/docs/Web/API/History_API"/> to change the URL is considered a navigation
        /// </remarks>
        /// <param name="chromiumWebBrowser">ChromiumWebBrowser instance (cannot be null)</param>
        /// <param name="timeout">optional timeout, if not specified defaults to five(5) seconds.</param>
        /// <param name="cancellationToken">optional CancellationToken</param>
        /// <returns>Task which resolves when <see cref="IChromiumWebBrowserBase.LoadingStateChanged"/> has been called with <see cref="LoadingStateChangedEventArgs.IsLoading"/> false.
        /// or when <see cref="IChromiumWebBrowserBase.LoadError"/> is called to signify a load failure.
        /// </returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string script = "document.getElementsByTagName('a')[0].click();";
        /// await Task.WhenAll(
        ///     chromiumWebBrowser.WaitForNavigationAsync(),
        ///     chromiumWebBrowser.EvaluateScriptAsync(script));
        /// ]]>
        /// </code>
        /// </example>
        public static async Task<WaitForNavigationAsyncResponse> WaitForNavigationAsync(IChromiumWebBrowserBase chromiumWebBrowser, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(chromiumWebBrowser);

            var tcs = new TaskCompletionSource<WaitForNavigationAsyncResponse>();

            EventHandler<LoadErrorEventArgs> loadErrorHandler = null;
            EventHandler<LoadingStateChangedEventArgs> loadingStateChangeHandler = null;

            loadErrorHandler = (sender, args) =>
            {
                //Actions that trigger a download will raise an aborted error.
                //Generally speaking Aborted is safe to ignore
                if (args.ErrorCode == CefErrorCode.Aborted)
                {
                    return;
                }

                //If LoadError was called then we'll remove both our handlers
                //as we won't need to capture LoadingStateChanged, we know there
                //was an error
                chromiumWebBrowser.LoadError -= loadErrorHandler;
                chromiumWebBrowser.LoadingStateChanged -= loadingStateChangeHandler;

                //Ensure our continuation is executed on the ThreadPool
                //For the .Net Core implementation we could use
                //TaskCreationOptions.RunContinuationsAsynchronously
                tcs.TrySetResultAsync(new WaitForNavigationAsyncResponse(args.ErrorCode, -1));
            };

            loadingStateChangeHandler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
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

                    //Ensure our continuation is executed on the ThreadPool
                    //For the .Net Core implementation we could use
                    //TaskCreationOptions.RunContinuationsAsynchronously
                    tcs.TrySetResultAsync(new WaitForNavigationAsyncResponse(statusCode == -1 ? CefErrorCode.Failed : CefErrorCode.None, statusCode));
                }
            };

            chromiumWebBrowser.LoadError += loadErrorHandler;
            chromiumWebBrowser.LoadingStateChanged += loadingStateChangeHandler;

            try
            {
                return await TaskTimeoutExtensions.WaitAsync(tcs.Task, timeout ?? TimeSpan.FromSeconds(5), cancellationToken).ConfigureAwait(false);
            }
            catch (Exception)
            {
                chromiumWebBrowser.LoadError -= loadErrorHandler;
                chromiumWebBrowser.LoadingStateChanged -= loadingStateChangeHandler;

                throw;
            }
        }

        /// <summary>
        /// Waits for a DOM element specified by the <paramref name="selector"/> string to be added to or removed from the DOM.
        /// A simplified version of Puppeteer WaitForSelector. Uses a MutationObserver to wait for the element to become added or removed.
        /// </summary>
        /// <param name="chromiumWebBrowser">ChromiumWebBrowser instance (cannot be null)</param>
        /// <param name="selector">querySelector for the element e.g. #idOfMyElement</param>
        /// <param name="timeout">timeout</param>
        /// <param name="removed">
        /// (Optional) if true  waits for element to be removed from the DOM. If the querySelector immediately resolves
        /// to null then the element is considered removed. If false (default) waits for the element to be added to the DOM.
        /// </param>
        /// <returns>A Task that resolves when element specified by selector string is added to or removed from the DOM.</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string script = "const newDiv = document.createElement('div'); newDiv.id = 'myElement'; document.body.append(newDiv);";
        /// await Task.WhenAll(
        ///     browser.WaitForSelectorAsync("#myElement");,
        ///     chromiumWebBrowser.EvaluateScriptAsync(script));
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This function is typically used in conjunction with javascript that directly or indirectly adds/removes an element from the DOM.
        /// Unlike the puppeteer version navigations aren't handled internally, the method will throw a <see cref="TimeoutException"/> if a navigation
        /// occurs whilst waiting to resolve.
        /// </remarks>
        public static async Task<WaitForSelectorAsyncResponse> WaitForSelectorAsync(this IWebBrowser chromiumWebBrowser, string selector, TimeSpan? timeout = null, bool removed = false)
        {
            const string waitForSelectorFunction = @"
            async function waitForSelectorFunction(timeout, selector, waitForRemoved)
            {
                let timedOut = false;
                if (timeout)
                    setTimeout(() => (timedOut = true), timeout);

                return await pollMutation();

                async function pollMutation() {
                    const success = await mutationSelector(selector, waitForRemoved);
                    if (success)
                        return Promise.resolve(success);
                    let fulfill;
                    const result = new Promise((x) => (fulfill = x));
                    const observer = new MutationObserver(async () => {
                        if (timedOut) {
                            observer.disconnect();
                            fulfill();
                        }
                        const success = await mutationSelector(selector, waitForRemoved);
                        if (success) {
                            observer.disconnect();
                            fulfill(success);
                        }
                    });
                    observer.observe(document, {
                        childList: true,
                        subtree: true,
                        attributes: false,
                    });
                    return result;
                }

                async function mutationSelector(selector, waitForRemoved)
                {
                    const element = document.querySelector(selector);

                    if (!element)
                        return waitForRemoved;

                    if(waitForRemoved && element)
                        return null;
                    
                    let obj = {};
                    obj.id = element.id;
                    obj.nodeValue = element.nodeValue;
                    obj.localName = element.localName;
                    obj.tagName = element.tagName;

                    return obj;
                }
            };";

            if(chromiumWebBrowser == null)
            {
                throw new ArgumentNullException(nameof(chromiumWebBrowser));
            }

            if(string.IsNullOrEmpty(selector))
            {
                throw new ArgumentException($"{nameof(selector)} cannot be null or empty.");
            }

            var execute = GetScriptForJavascriptMethodWithArgs("waitForSelectorFunction", new object[] { timeout.HasValue ? timeout.Value.Milliseconds : 5000, selector, removed });
            var query = @"return (async () => {" + Environment.NewLine + waitForSelectorFunction + Environment.NewLine + "return " + execute + Environment.NewLine + "})(); ";

            var response = chromiumWebBrowser.EvaluateScriptAsPromiseAsync(query);

            var timeoutResponse = await  TaskTimeoutExtensions.WaitAsync(response, timeout ?? TimeSpan.FromSeconds(5)).ConfigureAwait(continueOnCapturedContext:false);

            if(timeoutResponse.Success)
            {
                if(removed)
                {
                    if ((bool)timeoutResponse.Result)
                    {
                        return new WaitForSelectorAsyncResponse(string.Empty, string.Empty, false);
                    }

                    return new WaitForSelectorAsyncResponse(false, $"Failed to detect DOM change for removed element via selector {selector}");
                }

                var element = (IDictionary<string, object>)timeoutResponse.Result;
                var id = element["id"].ToString();
                var tagName = element["tagName"].ToString();

                return new WaitForSelectorAsyncResponse(id, tagName, true);
            }

            return new WaitForSelectorAsyncResponse(false, timeoutResponse.Message);
        }

        /// <summary>
        /// Execute Javascript code in the context of this Browser. As the method name implies, the script will be executed
        /// asynchronously, and the method therefore returns before the script has actually been executed. This simple helper extension
        /// will encapsulate params in single quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="methodName">The javascript method name to execute.</param>
        /// <param name="args">the arguments to be passed as params to the method. Args are encoded using
        /// <see cref="EncodeScriptParam"/>, you can provide a custom implementation if you require one.</param>
        public static void ExecuteScriptAsync(this IChromiumWebBrowserBase browser, string methodName, params object[] args)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.ExecuteScriptAsync(methodName, args);
        }

        /// <summary>
        /// Execute Javascript code in the context of this WebBrowser. As the method name implies, the script will be executed
        /// asynchronously, and the method therefore returns before the script has actually been executed. This simple helper extension
        /// will encapsulate params in single quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <param name="methodName">The javascript method name to execute.</param>
        /// <param name="args">the arguments to be passed as params to the method. Args are encoded using
        /// <see cref="EncodeScriptParam"/>, you can provide a custom implementation if you require one.</param>
        public static void ExecuteScriptAsync(this IBrowser browser, string methodName, params object[] args)
        {
            ThrowExceptionIfBrowserNull(browser);

            var script = GetScriptForJavascriptMethodWithArgs(methodName, args);

            browser.ExecuteScriptAsync(script);
        }

        /// <summary>
        /// Execute Javascript in the context of this Browsers Main Frame. As the method name implies, the script will be executed
        /// asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        public static void ExecuteScriptAsync(this IChromiumWebBrowserBase browser, string script)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.ExecuteJavaScriptAsync(script);
            }
        }

        /// <summary>
        /// Execute Javascript in the context of this Browser Main Frame. As the method name implies, the script will be executed
        /// asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        public static void ExecuteScriptAsync(this IBrowser browser, string script)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.MainFrame)
            {
                ThrowExceptionIfFrameNull(frame);

                frame.ExecuteJavaScriptAsync(script);
            }
        }

        /// <summary>
        /// Execute Javascript code in the context of this Browsers Main Frame. This extension method uses the LoadingStateChanged event. As the
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
        public static void ExecuteScriptAsyncWhenPageLoaded(this IChromiumWebBrowserBase webBrowser, string script, bool oneTime = true)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(webBrowser);

            var useLoadingStateChangedEventHandler = webBrowser.IsBrowserInitialized == false || oneTime == false;

            //Browser has been initialized, we check if there is a valid document and we're not loading
            if (webBrowser.IsBrowserInitialized)
            {
                //CefBrowser wrapper
                var browser = webBrowser.BrowserCore;
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
        /// </summary>
        /// <param name="browser">browser this method extends</param>
        /// <param name="url">url to load</param>
        /// <param name="postDataBytes">post data as byte array</param>
        /// <param name="contentType">(Optional) if set the Content-Type header will be set</param>
        public static void LoadUrlWithPostData(this IChromiumWebBrowserBase browser, string url, byte[] postDataBytes, string contentType = null)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.LoadUrlWithPostData(url, postDataBytes, contentType);
        }

        /// <summary>
        /// Creates a new instance of IRequest with the specified Url and Method = POST and then calls
        /// <see cref="IFrame.LoadRequest(IRequest)"/>.
        /// </summary>
        /// <param name="browser">browser this method extends</param>
        /// <param name="url">url to load</param>
        /// <param name="postDataBytes">post data as byte array</param>
        /// <param name="contentType">(Optional) if set the Content-Type header will be set</param>
        public static void LoadUrlWithPostData(this IBrowser browser, string url, byte[] postDataBytes, string contentType = null)
        {
            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.MainFrame)
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
        public static void LoadHtml(this IChromiumWebBrowserBase browser, string html, bool base64Encode = false)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            var htmlString = new HtmlString(html, base64Encode);

            browser.LoadUrl(htmlString.ToDataUriString());
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
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

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
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

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
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

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
        public static void Stop(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Stop();
        }

        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Stop(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            browser.StopLoad();
        }

        /// <summary>
        /// Navigates back, must check <see cref="IChromiumWebBrowserBase.CanGoBack"/> before calling this method.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Back(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Back();
        }

        /// <summary>
        /// Navigates back, must check <see cref="IBrowser.CanGoBack"/> before calling this method.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Back(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            browser.GoBack();
        }

        /// <summary>
        /// Navigates forward, must check <see cref="IChromiumWebBrowserBase.CanGoForward"/> before calling this method.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Forward(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Forward();
        }

        /// <summary>
        /// Navigates forward, must check <see cref="IBrowser.CanGoForward"/> before calling this method.
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        public static void Forward(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            browser.GoForward();
        }

        /// <summary>
        /// Reloads the page being displayed. This method will use data from the browser's cache, if available.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Reload(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.Reload(false);
        }

        /// <summary>
        /// Reloads the page being displayed, optionally ignoring the cache (which means the whole page including all .css, .js etc.
        /// resources will be re-fetched).
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring browser cache; <c>false</c> A reload is performed using
        /// files from the browser cache, if available.</param>
        public static void Reload(this IChromiumWebBrowserBase browser, bool ignoreCache)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Reload(ignoreCache);
        }

        /// <summary>
        /// Reloads the page being displayed, optionally ignoring the cache (which means the whole page including all .css, .js etc.
        /// resources will be re-fetched).
        /// </summary>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring browser cache; <c>false</c> A reload is performed using
        /// files from the browser cache, if available.</param>
        public static void Reload(this IBrowser browser, bool ignoreCache = false)
        {
            ThrowExceptionIfBrowserNull(browser);

            browser.Reload(ignoreCache);
        }

        /// <summary>
        /// Gets the default cookie manager associated with the <see cref="IChromiumWebBrowserBase"/> instance.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="callback">(Optional) If not null it will be executed asynchronously on the CEF IO thread after the manager's
        /// storage has been initialized.</param>
        /// <returns>
        /// Cookie Manager.
        /// </returns>
        public static ICookieManager GetCookieManager(this IChromiumWebBrowserBase browser, ICompletionCallback callback = null)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

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
        /// Gets the RequestContext associated with the <see cref="IChromiumWebBrowserBase"/> instance.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// RequestContext
        /// </returns>
        public static IRequestContext GetRequestContext(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            var host = browser.GetBrowserHost();

            ThrowExceptionIfBrowserHostNull(host);

            return host.RequestContext;
        }

        /// <summary>
        /// Asynchronously gets the current Zoom Level.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// An asynchronous result that yields the zoom level.
        /// </returns>
        public static Task<double> GetZoomLevelAsync(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
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
        public static Task<double> GetZoomLevelAsync(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            return browser.BrowserCore.GetZoomLevelAsync();
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
        public static void SetZoomLevel(this IBrowser browser, double zoomLevel)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
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
        public static void SetZoomLevel(this IChromiumWebBrowserBase browser, double zoomLevel)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> instance this method extends.</param>
        /// <param name="searchText">text to search for</param>
        /// <param name="forward">indicates whether to search forward or backward within the page</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive</param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up</param>
        /// <remarks>The <see cref="IFindHandler"/> instance, if any, will be called to report find results.</remarks>
        public static void Find(this IBrowser browser, string searchText, bool forward, bool matchCase, bool findNext)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.Find(searchText, forward, matchCase, findNext);
        }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="searchText">text to search for</param>
        /// <param name="forward">indicates whether to search forward or backward within the page</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive</param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up</param>
        /// <remarks>The <see cref="IFindHandler"/> instance, if any, will be called to report find results.</remarks>
        public static void Find(this IChromiumWebBrowserBase browser, string searchText, bool forward, bool matchCase, bool findNext)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Find(searchText, forward, matchCase, findNext);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> instance this method extends.</param>
        /// <param name="clearSelection">clear the current search selection.</param>
        public static void StopFinding(this IBrowser browser, bool clearSelection)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.StopFinding(clearSelection);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="clearSelection">clear the current search selection.</param>
        public static void StopFinding(this IChromiumWebBrowserBase browser, bool clearSelection)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.StopFinding(clearSelection);
        }

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        /// <param name="browser">The browser instance this method extends.</param>
        public static void Print(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.Print();
        }

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void Print(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.Print();
        }

        /// <summary>
        /// Asynchronously prints the current browser contents to the PDF file specified. The caller is responsible for deleting the file
        /// when done.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> object this method extends.</param>
        /// <param name="path">Output file location.</param>
        /// <param name="settings">(Optional) Print Settings.</param>
        /// <returns>
        /// A task that represents the asynchronous print operation. The result is true on success or false on failure to generate the
        /// Pdf.
        /// </returns>
        public static Task<bool> PrintToPdfAsync(this IBrowser browser, string path, PdfPrintSettings settings = null)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            var callback = new TaskPrintToPdfCallback();
            host.PrintToPdf(path, settings, callback);

            return callback.Task;
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
        public static Task<bool> PrintToPdfAsync(this IChromiumWebBrowserBase browser, string path, PdfPrintSettings settings = null)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            return browser.BrowserCore.PrintToPdfAsync(path, settings);
        }

        /// <summary>
        /// Open developer tools in its own window.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> instance this method extends.</param>
        /// <param name="windowInfo">(Optional) window info used for showing dev tools.</param>
        /// <param name="inspectElementAtX">(Optional) x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">(Optional) y coordinate (used for inspectElement)</param>
        public static void ShowDevTools(this IBrowser browser, IWindowInfo windowInfo = null, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
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
        public static void ShowDevTools(this IChromiumWebBrowserBase browser, IWindowInfo windowInfo = null, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.ShowDevTools(windowInfo, inspectElementAtX, inspectElementAtY);
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> instance this method extends.</param>
        public static void CloseDevTools(this IBrowser browser)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.CloseDevTools();
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void CloseDevTools(this IChromiumWebBrowserBase browser)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.CloseDevTools();
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling this method will replace it with the specified word.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> instance this method extends.</param>
        /// <param name="word">The new word that will replace the currently selected word.</param>
        public static void ReplaceMisspelling(this IBrowser browser, string word)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.ReplaceMisspelling(word);
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling this method will replace it with the specified word.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="word">The new word that will replace the currently selected word.</param>
        public static void ReplaceMisspelling(this IChromiumWebBrowserBase browser, string word)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.ReplaceMisspelling(word);
        }

        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> instance this method extends.</param>
        /// <param name="word">The new word that will be added to the dictionary.</param>
        public static void AddWordToDictionary(this IBrowser browser, string word)
        {
            ThrowExceptionIfBrowserNull(browser);

            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.AddWordToDictionary(word);
        }

        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <param name="word">The new word that will be added to the dictionary.</param>
        public static void AddWordToDictionary(this IChromiumWebBrowserBase browser, string word)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.AddWordToDictionary(word);
        }

        /// <summary>
        /// Shortcut method to get the browser IBrowserHost.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        /// <returns>
        /// browserHost or null.
        /// </returns>
        public static IBrowserHost GetBrowserHost(this IChromiumWebBrowserBase browser)
        {
            return browser.BrowserCore?.GetHost();
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
        public static void SendMouseWheelEvent(this IChromiumWebBrowserBase browser, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            browser.BrowserCore.SendMouseWheelEvent(x, y, deltaX, deltaY, modifiers);
        }

        /// <summary>
        /// Send a mouse wheel event to the browser.
        /// </summary>
        /// <param name="browser">The <see cref="IBrowser"/> instance this method extends.</param>
        /// <param name="x">The x coordinate relative to upper-left corner of view.</param>
        /// <param name="y">The y coordinate relative to upper-left corner of view.</param>
        /// <param name="deltaX">The delta x coordinate.</param>
        /// <param name="deltaY">The delta y coordinate.</param>
        /// <param name="modifiers">The modifiers.</param>
        public static void SendMouseWheelEvent(this IBrowser browser, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            ThrowExceptionIfBrowserNull(browser);

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
        /// Evaluate Javascript in the context of the MainFrame of the ChromiumWebBrowser. The script will be executed
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
            ThrowExceptionIfChromiumWebBrowserDisposed(chromiumWebBrowser);

            var jsbSettings = chromiumWebBrowser.JavascriptObjectRepository.Settings;

            var promiseHandlerScript = GetPromiseHandlerScript(script, jsbSettings.JavascriptBindingApiGlobalObjectName);

            return chromiumWebBrowser.EvaluateScriptAsync(promiseHandlerScript, timeout: timeout, useImmediatelyInvokedFuncExpression: true);
        }

        /// <summary>
        /// Evaluate Javascript in the context of this Browsers Main Frame. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript. The result of the script execution
        /// in javascript is Promise.resolve so even no promise values will be treated as a promise. Your javascript should return a value.
        /// The javascript will be wrapped in an Immediately Invoked Function Expression.
        /// When the promise either trigger then/catch this returned Task will be completed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsPromiseAsync(this IBrowser browser, string script, TimeSpan? timeout = null)
        {
            var promiseHandlerScript = GetPromiseHandlerScript(script, null);

            return browser.EvaluateScriptAsync(promiseHandlerScript, timeout: timeout, useImmediatelyInvokedFuncExpression: true);
        }

        /// <summary>
        /// Evaluate Javascript in the context of this Browsers Main Frame. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript. The result of the script execution
        /// in javascript is Promise.resolve so even no promise values will be treated as a promise. Your javascript should return a value.
        /// The javascript will be wrapped in an Immediately Invoked Function Expression.
        /// When the promise either trigger then/catch this returned Task will be completed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <param name="frame">The <seealso cref="IFrame"/> instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <param name="javascriptBindingApiGlobalObjectName">
        /// Only required if a custom value was specified for <see cref="JavascriptBinding.JavascriptBindingSettings.JavascriptBindingApiGlobalObjectName"/>
        /// then this param must match that value. Otherwise exclude passing a value for this param or pass in null.
        /// </param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsPromiseAsync(this IFrame frame, string script, TimeSpan? timeout = null, string javascriptBindingApiGlobalObjectName = null)
        {
            var promiseHandlerScript = GetPromiseHandlerScript(script, javascriptBindingApiGlobalObjectName);

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
        /// Evaluate Javascript in the context of this Browsers Main Frame. The script will be executed
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
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to obtain the result of the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IChromiumWebBrowserBase browser, string script, TimeSpan? timeout = null, bool useImmediatelyInvokedFuncExpression = false)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            if (browser is IWebBrowser b)
            {
                if (b.CanExecuteJavascriptInMainFrame == false)
                {
                    ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse();
                }
            }

            return browser.BrowserCore.EvaluateScriptAsync(script, timeout, useImmediatelyInvokedFuncExpression);
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of the MainFrame of the ChromiumWebBrowser. The script will be executed
        /// asynchronously and the method returns a Task encapsulating the response from the Javascript
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
        /// <param name="browser">The IBrowser instance this method extends.</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        /// <param name="timeout">(Optional) The timeout after which the Javascript code execution should be aborted.</param>
        /// <param name="useImmediatelyInvokedFuncExpression">When true the script is wrapped in a self executing function.
        /// Make sure to use a return statement in your javascript. e.g. (function () { return 42; })();
        /// When false don't include a return statement e.g. 42;
        /// </param>
        /// <returns>
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to obtain the result of the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IBrowser browser, string script, TimeSpan? timeout = null, bool useImmediatelyInvokedFuncExpression = false)
        {
            if (timeout.HasValue && timeout.Value.TotalMilliseconds > UInt32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout greater than Maximum allowable value of " + UInt32.MaxValue);
            }

            ThrowExceptionIfBrowserNull(browser);

            using (var frame = browser.MainFrame)
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
        /// <see cref="Task{JavascriptResponse}"/> that can be awaited to obtain the result of the script execution.
        /// </returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IChromiumWebBrowserBase browser, string methodName, params object[] args)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

            return browser.EvaluateScriptAsync(null, methodName, args);
        }

        /// <summary>
        /// Evaluate Javascript code in the context of this WebBrowser using the specified timeout. The script will be executed
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
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IChromiumWebBrowserBase browser, TimeSpan? timeout, string methodName, params object[] args)
        {
            ThrowExceptionIfChromiumWebBrowserDisposed(browser);

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
        /// Dispose of the DevToolsContext (if any). Used in conjunction with CefSharp.Dom
        /// </summary>
        /// <param name="webBrowserInternal">ChromiumWebBrowser instance</param>
        public static void DisposeDevToolsContext(this IWebBrowserInternal webBrowserInternal)
        {
            if(webBrowserInternal == null)
            {
                return;
            }

            webBrowserInternal.DevToolsContext?.Dispose();
            webBrowserInternal.DevToolsContext = null;
        }

        /// <summary>
        /// Set the <see cref="IWebBrowserInternal.DevToolsContext"/> property to null. Used in conjunction with CefSharp.Dom
        /// </summary>
        /// <param name="webBrowserInternal">ChromiumWebBrowser instance</param>
        public static void FreeDevToolsContext(this IWebBrowserInternal webBrowserInternal)
        {
            if (webBrowserInternal == null)
            {
                return;
            }

            webBrowserInternal.DevToolsContext = null;
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

        public static void ThrowExceptionIfChromiumWebBrowserDisposed(IChromiumWebBrowserBase browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException(nameof(browser));
            }

            if (browser.IsDisposed)
            {
                // Provide a more meaningful message for WinForms ChromiumHostControl
                // should be ChromiumWebBrowser/ChromiumHostControl
                var type = browser.GetType();

                throw new ObjectDisposedException(type.Name);
            }
        }

        /// <summary>
        /// Throw exception if frame null.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="frame">The <seealso cref="IFrame"/> instance this method extends.</param>
        public static void ThrowExceptionIfFrameNull(IFrame frame)
        {
            if (frame == null)
            {
                throw new Exception(FrameNullExceptionString);
            }
        }

        /// <summary>
        /// An IBrowser extension method that throw exception if browser null.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends.</param>
        public static void ThrowExceptionIfBrowserNull(IBrowser browser)
        {
            if (browser == null)
            {
                throw new Exception(BrowserNullExceptionString);
            }
        }

        /// <summary>
        /// Throw exception if browser host null.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="browserHost">The browser host.</param>
        public static void ThrowExceptionIfBrowserHostNull(IBrowserHost browserHost)
        {
            if (browserHost == null)
            {
                throw new Exception(BrowserHostNullExceptionString);
            }
        }

        /// <summary>
        /// Throw exception if can execute javascript in main frame false.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        public static void ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse()
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
