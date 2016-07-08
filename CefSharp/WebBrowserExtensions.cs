// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using CefSharp.Internals;

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
        /// <param name="args">the arguments to be passed as params to the method</param>
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
            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.ExecuteJavaScriptAsync(script);
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
        public static void LoadHtml(this IWebBrowser browser, string html, string url)
        {
            browser.LoadHtml(html, url, Encoding.UTF8);
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
        public static void LoadHtml(this IWebBrowser browser, string html, string url, Encoding encoding)
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

            resourceHandler.RegisterHandler(url, ResourceHandler.FromString(html, encoding, true));

            browser.Load(url);
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

            return host.PrintToPdfAsync(path, settings);
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

            host.SendMouseWheelEvent(x, y, deltaX, deltaY, modifiers);
        }

        public static Task<JavascriptResponse> EvaluateScriptAsync(this IWebBrowser browser, string script, TimeSpan? timeout = null)
        {
            if (timeout.HasValue && timeout.Value.TotalMilliseconds > UInt32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timeout", "Timeout greater than Maximum allowable value of " + UInt32.MaxValue);
            }

            using (var frame = browser.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                return frame.EvaluateScriptAsync(script, timeout);
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
        /// This simple helper extension will encapsulate params in single quotes (unless int, uint, etc)
        /// </summary>
        /// <param name="browser">The ChromiumWebBrowser instance this method extends</param>
        /// <param name="timeout">The timeout after which the Javascript code execution should be aborted.</param>
        /// <param name="methodName">The javascript method name to execute</param>
        /// <param name="args">the arguments to be passed as params to the method</param>
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
                throw new Exception("Browser Is Not yet initialized. Use the IsBrowserInitializedChanged event and check" +
                                    "the IsBrowserInitialized property to determine when the browser has been intialized.");
            }
        }

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
                        stringBuilder.Append(args[i].ToString().Replace("'", "\\'"));
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
    }
}
