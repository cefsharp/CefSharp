﻿// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using CefSharp.Internals;
using System.IO;
using System.Collections.Specialized;

namespace CefSharp
{
    public static class WebBrowserExtensions
    {
        private static Type[] numberTypes = new Type[] { typeof(int), typeof(uint), typeof(double), typeof(decimal), typeof(float), typeof(Int64), typeof(Int16) };

        /// <summary>
        /// Returns the main (top-level) frame for the browser window.
        /// </summary>
        /// <returns>Frame</returns>
        public static IFrame GetMainFrame(this IWebBrowser webBrowser)
        {
            var browser = webBrowser.GetBrowser();

            ThrowExceptionIfBrowserNull(browser);

            return browser.MainFrame;
        }

        /// <summary>
        /// Returns the focused frame for the browser window.
        /// </summary>
        /// <returns>Frame</returns>
        public static IFrame GetFocusedFrame(this IWebBrowser webBrowser)
        {
            var browser = webBrowser.GetBrowser();

            ThrowExceptionIfBrowserNull(browser);

            return browser.FocusedFrame;
        }

        /// <summary>
        /// Execute Undo on the focused frame
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void Undo(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            { 
                ThrowExceptionIfFrameNull(frame);

                frame.Undo();
            }
        }

        /// <summary>
        /// Execute Redo on the focused frame
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void Redo(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Redo();
            }
        }

        /// <summary>
        /// Execute Cut on the focused frame
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void Cut(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Cut();
            }
        }

        /// <summary>
        /// Execute Copy on the focused frame
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void Copy(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Copy();
            }
        }

        /// <summary>
        /// Execute Paste on the focused frame
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void Paste(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Paste();
            }
        }

        /// <summary>
        /// Execute Delete on the focused frame
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void Delete(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Delete();
            }
        }

        /// <summary>
        /// Execute SelectAll on the focused frame
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void SelectAll(this IWebBrowser browser)
        {
            using (var frame = browser.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.SelectAll();
            }
        }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
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
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <returns><see cref="Task{String}"/> that when executed returns the frame source as a string</returns>
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
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <returns><see cref="Task{String}"/> that when executed returns the frame display text as a string.</returns>
        public static Task<string> GetTextAsync(this IWebBrowser browser)
        {
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                return frame.GetTextAsync();
            }
        }

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser. As the method name implies, the script will be
        /// executed asynchronously, and the method therefore returns before the script has actually been executed.
        /// This simple helper extension will encapsulate params in single quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="methodName">The javascript method name to execute</param>
        /// <param name="args">the arguments to be passed as params to the method. Args are encoded using <see cref="EncodeScriptParam"/>,
        /// you can provide a custom implementation if you require a custom implementation</param>
        public static void ExecuteScriptAsync(this IWebBrowser browser, string methodName, params object[] args)
        {
            var script = GetScript(methodName, args);

            browser.ExecuteScriptAsync(script);
        }

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser. As the method name implies, the script will be
        /// executed asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        public static void ExecuteScriptAsync(this IWebBrowser browser, string script)
        {
            //TODO: Re-enable when Native IPC issue resolved.
            //if (browser.CanExecuteJavascriptInMainFrame == false)
            //{
            //	ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse();
            //}

            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.ExecuteJavaScriptAsync(script);
            }
        }

        /// <summary>
        /// Loads 
        /// Creates a new instance of IRequest with the specified Url and Method = POST
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="url"></param>
        /// <param name="postDataBytes"></param>
        /// <param name="contentType"></param>
        /// <remarks>This is an extension method</remarks>
        public static void LoadUrlWithPostData(this IWebBrowser browser, string url, byte[] postDataBytes, string contentType = null)
        {
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                //Initialize Request with PostData
                var request = frame.CreateRequest(initializePostData:true);

                request.Url = url;
                request.Method = "POST";

                request.PostData.AddData(postDataBytes);

                if(!string.IsNullOrEmpty(contentType))
                { 
                    var headers = new NameValueCollection();
                    headers.Add("Content-Type", contentType);
                    request.Headers = headers;
                }

                frame.LoadRequest(request);
            }
        }

        /// <summary>
        /// Load the string contents with the specified dummy url. Web security restrictions may not behave as expected. 
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="html">html string to load</param>
        /// <param name="url">the url should have a standard scheme (for example, http scheme) or behaviors like link clicks</param>
        public static void LoadString(this IWebBrowser browser, string html, string url)
        {
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.LoadStringForUrl(html, url);
            }
        }

        /// <summary>
        /// Registers and loads a <see cref="ResourceHandler"/> that represents the HTML content.
        /// </summary>
        /// <remarks>
        /// `Cef` Native `LoadHtml` is unpredictable and only works sometimes, this method wraps
        /// the provided HTML in a <see cref="ResourceHandler"/> and loads the provided url using
        /// the <see cref="IWebBrowser.Load"/> method.
        /// Defaults to using <see cref="Encoding.UTF8"/> for character encoding 
        /// The url must start with a valid schema, other uri's such as about:blank are invalid
        /// A valid example looks like http://test/page
        /// </remarks>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        /// <returns>returns false if the Url was not successfully parsed into a Uri</returns>
        public static bool LoadHtml(this IWebBrowser browser, string html, string url)
        {
            return browser.LoadHtml(html, url, Encoding.UTF8);
        }

        /// <summary>
        /// Loads html as Data Uri
        /// See https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs for details
        /// If base64Encode is false then html will be Uri encoded
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="html">Html to load as data uri.</param>
        /// <param name="base64Encode">if true the html string will be base64 encoded using UTF8 encoding.</param>
        public static void LoadHtml(this IWebBrowser browser, string html, bool base64Encode = false)
        {
            if(base64Encode)
            { 
                var base64EncodedHtml = Convert.ToBase64String(Encoding.UTF8.GetBytes(html));
                browser.Load("data:text/html;base64," + base64EncodedHtml);
            }
            else
            { 
                var uriEncodedHtml = Uri.EscapeDataString(html);
                browser.Load("data:text/html," + uriEncodedHtml);
            }
        }

        /// <summary>
        /// Registers and loads a <see cref="ResourceHandler"/> that represents the HTML content.
        /// </summary>
        /// <remarks>
        /// `Cef` Native `LoadHtml` is unpredictable and only works sometimes, this method wraps
        /// the provided HTML in a <see cref="ResourceHandler"/> and loads the provided url using
        /// the <see cref="IWebBrowser.Load"/> method.
        /// </remarks>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="html">The HTML content.</param>
        /// <param name="url">The URL that will be treated as the address of the content.</param>
        /// <param name="encoding">Character Encoding</param>
        /// <param name="oneTimeUse">Whether or not the handler should be used once (true) or until manually unregistered (false)</param>
        /// <returns>returns false if the Url was not successfully parsed into a Uri</returns>
        public static bool LoadHtml(this IWebBrowser browser, string html, string url, Encoding encoding, bool oneTimeUse = false)
        {
            var handler = browser.ResourceHandlerFactory;
            if (handler == null)
            {
                throw new Exception("Implement IResourceHandlerFactory and assign to the ResourceHandlerFactory property to use this feature");
            }

            var resourceHandler = handler as DefaultResourceHandlerFactory;

            if(resourceHandler == null)
            {
                throw new Exception("LoadHtml can only be used with the default IResourceHandlerFactory(DefaultResourceHandlerFactory) implementation");
            }

            if (resourceHandler.RegisterHandler(url, ResourceHandler.GetByteArray(html, encoding, true), ResourceHandler.DefaultMimeType, oneTimeUse))
            {
                browser.Load(url);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Register a ResourceHandler. Can only be used when browser.ResourceHandlerFactory is an instance of DefaultResourceHandlerFactory
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="url">the url of the resource to unregister</param>
        /// <param name="stream">Stream to be registered, the stream should not be shared with any other instances of DefaultResourceHandlerFactory</param>
        /// <param name="mimeType">the mimeType</param>
        /// <param name="oneTimeUse">Whether or not the handler should be used once (true) or until manually unregistered (false). If true the Stream
        /// will be Diposed of when finished.</param>
        public static void RegisterResourceHandler(this IWebBrowser browser, string url, Stream stream, string mimeType = ResourceHandler.DefaultMimeType,
            bool oneTimeUse = false)
        {
            var handler = browser.ResourceHandlerFactory as DefaultResourceHandlerFactory;
            if (handler == null)
            {
                throw new Exception("RegisterResourceHandler can only be used with the default IResourceHandlerFactory(DefaultResourceHandlerFactory) implementation");
            }

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);

                handler.RegisterHandler(url, ms.ToArray(), mimeType, oneTimeUse);
            }
        }

        /// <summary>
        /// Unregister a ResourceHandler. Can only be used when browser.ResourceHandlerFactory is an instance of DefaultResourceHandlerFactory
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="url">the url of the resource to unregister</param>
        public static void UnRegisterResourceHandler(this IWebBrowser browser, string url)
        {
            var handler = browser.ResourceHandlerFactory as DefaultResourceHandlerFactory;
            if (handler == null)
            {
                throw new Exception("UnRegisterResourceHandler can only be used with the default IResourceHandlerFactory(DefaultResourceHandlerFactory) implementation");
            }

            handler.UnregisterHandler(url);
        }

        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        public static void Stop(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.StopLoad();
        }

        /// <summary>
        /// Navigates back, must check <see cref="IWebBrowser.CanGoBack"/> before calling this method.
        /// </summary>
        public static void Back(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.GoBack();
        }

        /// <summary>
        /// Navigates forward, must check <see cref="IWebBrowser.CanGoForward"/> before calling this method.
        /// </summary>
        public static void Forward(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.GoForward();
        }

        /// <summary>
        /// Reloads the page being displayed. This method will use data from the browser's cache, if available.
        /// </summary>
        public static void Reload(this IWebBrowser browser)
        {
            browser.Reload(false);
        }

        /// <summary>
        /// Reloads the page being displayed, optionally ignoring the cache (which means the whole page including all .css, .js
        /// etc. resources will be re-fetched).
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring browser cache; <c>false</c> A reload is
        /// performed using files from the browser cache, if available.</param>
        public static void Reload(this IWebBrowser browser, bool ignoreCache)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.Reload(ignoreCache);
        }

        /// <summary>
        /// Gets the default cookie manager associated with the IWebBrowser
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="callback">If not null it will be executed asnychronously on the
        /// CEF IO thread after the manager's storage has been initialized.</param>
        /// <returns>Cookie Manager</returns>
        public static ICookieManager GetCookieManager(this IWebBrowser browser, ICompletionCallback callback = null)
        {
            var host = browser.GetBrowserHost();

            ThrowExceptionIfBrowserHostNull(host);

            var requestContext = host.RequestContext;

            if(requestContext == null)
            {
                throw new Exception("RequestContext is null, unable to obtain cookie manager");
            }

            return requestContext.GetDefaultCookieManager(callback);
        }

        /// <summary>
        /// Asynchronously gets the current Zoom Level.
        /// </summary>
        public static Task<double> GetZoomLevelAsync(this IBrowser cefBrowser)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            return host.GetZoomLevelAsync();
        }

        /// <summary>
        /// Asynchronously gets the current Zoom Level.
        /// </summary>
        public static Task<double> GetZoomLevelAsync(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();
            return cefBrowser.GetZoomLevelAsync();
        }

        /// <summary>
        /// Change the ZoomLevel to the specified value. Can be set to 0.0 to clear the zoom level.
        /// </summary>
        /// <remarks>
        /// If called on the CEF UI thread the change will be applied immediately.
        /// Otherwise, the change will be applied asynchronously on the CEF UI thread.
        /// The CEF UI thread is different to the WPF/WinForms UI Thread
        /// </remarks>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="zoomLevel">zoom level</param>
        public static void SetZoomLevel(this IBrowser cefBrowser, double zoomLevel)
        {
            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Change the ZoomLevel to the specified value. Can be set to 0.0 to clear the zoom level.
        /// </summary>
        /// <remarks>
        /// If called on the CEF UI thread the change will be applied immediately.
        /// Otherwise, the change will be applied asynchronously on the CEF UI thread.
        /// The CEF UI thread is different to the WPF/WinForms UI Thread
        /// </remarks>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="zoomLevel">zoom level</param>
        public static void SetZoomLevel(this IWebBrowser browser, double zoomLevel)
        {
            var cefBrowser = browser.GetBrowser();
            cefBrowser.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="identifier">Can be used in can conjunction with searchText to have multiple
        /// searches running simultaneously.</param>
        /// <param name="searchText">search text</param>
        /// <param name="forward">indicates whether to search forward or backward within the page.</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive. </param>
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
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="identifier">Can be used in can conjunction with searchText to have multiple
        /// searches running simultaneously.</param>
        /// <param name="searchText">search text</param>
        /// <param name="forward">indicates whether to search forward or backward within the page.</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive. </param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up.</param>
        public static void Find(this IWebBrowser browser, int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.Find(identifier, searchText, forward, matchCase, findNext);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="clearSelection">clear the current search selection</param>
        public static void StopFinding(this IBrowser cefBrowser, bool clearSelection)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.StopFinding(clearSelection);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="clearSelection">clear the current search selection</param>
        public static void StopFinding(this IWebBrowser browser, bool clearSelection)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.StopFinding(clearSelection);
        }

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        public static void Print(this IBrowser cefBrowser)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.Print();
        }

        /// <summary>
        /// Asynchronously prints the current browser contents to the PDF file specified.
        /// The caller is responsible for deleting the file when done.
        /// </summary>
        /// <param name="cefBrowser">The <see cref="IBrowser"/> object this method extends.</param>
        /// <param name="path">Output file location.</param>
        /// <param name="settings">Print Settings.</param>
        /// <returns>A task that represents the asynchronous print operation.
        /// The result is true on success or false on failure to generate the Pdf.</returns>
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
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void Print(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.Print();
        }

        /// <summary>
        /// Asynchronously prints the current browser contents to the PDF file specified.
        /// The caller is responsible for deleting the file when done.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="path">Output file location.</param>
        /// <param name="settings">Print Settings.</param>
        /// <returns>A task that represents the asynchronous print operation.
        /// The result is true on success or false on failure to generate the Pdf.</returns>
        public static Task<bool> PrintToPdfAsync(this IWebBrowser browser, string path, PdfPrintSettings settings = null)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);

            return cefBrowser.PrintToPdfAsync(path, settings);
        }

        /// <summary>
        /// Open developer tools in its own window. 
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        public static void ShowDevTools(this IBrowser cefBrowser)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.ShowDevTools();
        }

        /// <summary>
        /// Open developer tools in its own window. 
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void ShowDevTools(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);
            cefBrowser.ShowDevTools();
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        public static void CloseDevTools(this IBrowser cefBrowser)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.CloseDevTools();
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        public static void CloseDevTools(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);
            cefBrowser.CloseDevTools();
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling
        /// this method will replace it with the specified word. 
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="word">The new word that will replace the currently selected word.</param>
        public static void ReplaceMisspelling(this IBrowser cefBrowser, string word)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.ReplaceMisspelling(word);
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling
        /// this method will replace it with the specified word. 
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="word">The new word that will replace the currently selected word.</param>
        public static void ReplaceMisspelling(this IWebBrowser browser, string word)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.ReplaceMisspelling(word);
        }

        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="cefBrowser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="word">The new word that will be added to the dictionary.</param>
        public static void AddWordToDictionary(this IBrowser cefBrowser, string word)
        {
            var host = cefBrowser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.AddWordToDictionary(word);
        }

        /// <summary>
        /// Shortcut method to get the browser IBrowserHost
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <returns>browserHost or null</returns>
        public static IBrowserHost GetBrowserHost(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            return cefBrowser == null ? null : cefBrowser.GetHost();
        }

        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="word">The new word that will be added to the dictionary.</param>
        public static void AddWordToDictionary(this IWebBrowser browser, string word)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.AddWordToDictionary(word);
        }

        public static void SendMouseWheelEvent(this IWebBrowser browser, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            var cefBrowser = browser.GetBrowser();
            ThrowExceptionIfBrowserNull(cefBrowser);

            cefBrowser.SendMouseWheelEvent(x, y, deltaX, deltaY, modifiers);
        }

        public static void SendMouseWheelEvent(this IBrowser browser, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            var host = browser.GetHost();
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseWheelEvent(new MouseEvent(x, y, modifiers), deltaX, deltaY);
        }

        public static void SendMouseWheelEvent(this IBrowserHost host, int x, int y, int deltaX, int deltaY, CefEventFlags modifiers)
        {
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseWheelEvent(new MouseEvent(x, y, modifiers), deltaX, deltaY);
        }

        public static void SendMouseClickEvent(this IBrowserHost host, int x, int y, MouseButtonType mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers)
        {
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseClickEvent(new MouseEvent(x, y, modifiers), mouseButtonType, mouseUp, clickCount);
        }

        public static void SendMouseMoveEvent(this IBrowserHost host, int x, int y, bool mouseLeave, CefEventFlags modifiers)
        {
            ThrowExceptionIfBrowserHostNull(host);

            host.SendMouseMoveEvent(new MouseEvent(x, y, modifiers), mouseLeave);
        }
        
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IWebBrowser browser, string script, TimeSpan? timeout = null)
        {
            if (timeout.HasValue && timeout.Value.TotalMilliseconds > UInt32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout greater than Maximum allowable value of " + UInt32.MaxValue);
            }

            //TODO: Re-enable when Native IPC issue resolved.
            //if(browser.CanExecuteJavascriptInMainFrame == false)
            //{
            //	ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse();
            //}

            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                return frame.EvaluateScriptAsync(script, timeout: timeout);
            }
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of this WebBrowser. The script will be executed asynchronously and the
        /// method returns a Task encapsulating the response from the Javascript 
        /// This simple helper extension will encapsulate params in single quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="methodName">The javascript method name to execute</param>
        /// <param name="args">the arguments to be passed as params to the method</param>
        /// <returns><see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution</returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IWebBrowser browser, string methodName, params object[] args)
        {
            return browser.EvaluateScriptAsync(null, methodName, args);
        }

        /// <summary>
        /// Evaluate some Javascript code in the context of this WebBrowser using the specified timeout. The script will be executed asynchronously and the
        /// method returns a Task encapsulating the response from the Javascript 
        /// This simple helper extension will encapsulate params in single quotes (unless int, uint, etc).
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="timeout">The timeout after which the Javascript code execution should be aborted.</param>
        /// <param name="methodName">The javascript method name to execute</param>
        /// <param name="args">the arguments to be passed as params to the method. Args are encoded using <see cref="EncodeScriptParam"/>,
        /// you can provide a custom implementation if you require a custom implementation</param>
        /// <returns><see cref="Task{JavascriptResponse}"/> that can be awaited to perform the script execution</returns>
        public static Task<JavascriptResponse> EvaluateScriptAsync(this IWebBrowser browser, TimeSpan? timeout, string methodName, params object[] args)
        {
            var script = GetScript(methodName, args);

            return browser.EvaluateScriptAsync(script, timeout);
        }

        public static void SetAsPopup(this IWebBrowser browser)
        {
            var internalBrowser = (IWebBrowserInternal)browser;

            internalBrowser.HasParent = true;
        }

        public static void ThrowExceptionIfBrowserNotInitialized(this IWebBrowser browser)
        {
            if (!browser.IsBrowserInitialized)
            {
                throw new Exception("Browser is not yet initialized. Use the IsBrowserInitializedChanged event and check " +
                                    "the IsBrowserInitialized property to determine when the browser has been intialized.");
            }
        }

        /// <summary>
        /// Function used to encode the params passed to <see cref="ExecuteScriptAsync(IWebBrowser, string, object[])"/>,
        /// <see cref="EvaluateScriptAsync(IWebBrowser, string, object[])"/> and <see cref="EvaluateScriptAsync(IWebBrowser, TimeSpan?, string, object[])"/>
        /// Provide your own custom function to perform custom encoding. You can use your choice
        /// of JSON encoder here if you should so choose.
        /// </summary>
        public static Func<string, string> EncodeScriptParam { get; set; } = (str) =>
        {
            return str.Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\t", "\\t")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        };

        /// <summary>
        /// Transforms the methodName and arguments into valid Javascript code. Will encapsulate params in single quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="methodName">The javascript method name to execute</param>
        /// <param name="args">the arguments to be passed as params to the method</param>
        /// <returns>The Javascript code</returns>
        private static string GetScript(string methodName, object[] args)
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
                    else if (numberTypes.Contains(obj.GetType()))
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

        private static void ThrowExceptionIfFrameNull(IFrame frame)
        {
            if (frame == null)
            {
                throw new Exception("IFrame instance is null. Browser has likely not finished initializing or is in the process of disposing.");
            }
        }

        private static void ThrowExceptionIfBrowserNull(IBrowser browser)
        {
            if (browser == null)
            {
                throw new Exception("IBrowser instance is null. Browser has likely not finished initializing or is in the process of disposing.");
            }
        }

        private static void ThrowExceptionIfBrowserHostNull(IBrowserHost browserHost)
        {
            if (browserHost == null)
            {
                throw new Exception("IBrowserHost instance is null. Browser has likely not finished initializing or is in the process of disposing.");
            }
        }

        private static void ThrowExceptionIfCanExecuteJavascriptInMainFrameFalse()
        {
            throw new Exception("Unable to execute javascript at this time, scripts can only be executed within a V8Context." +
                                    "Use the IWebBrowser.CanExecuteJavascriptInMainFrame property to guard against this exception." +
                                    "See https://github.com/cefsharp/CefSharp/wiki/General-Usage#when-can-i-start-executing-javascript " +
                                    "for more details on when you can execute javascript. For frames that do not contain Javascript then no" +
                                    "V8Context will be created. Executing a script once the frame has loaded it's possible to create a V8Context. " +
                                    "You can use browser.GetMainFrame().ExecuteJavaScriptAsync(script) or browser.GetMainFrame().EvaluateScriptAsync " +
                                    "to bypass these checks (advanced users only).");
        }
    }
}
