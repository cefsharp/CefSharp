// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp
{
    public static class WebBrowserExtensions
    {
        public static void Undo(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);
            
            frame.Undo();

            frame.Dispose();
        }

        public static void Redo(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Redo();

            frame.Dispose();
        }

        public static void Cut(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Cut();

            frame.Dispose();
        }

        public static void Copy(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Copy();

            frame.Dispose();
        }

        public static void Paste(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Paste();

            frame.Dispose();
        }

        public static void Delete(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);
            
            frame.Delete();

            frame.Dispose();
        }

        public static void SelectAll(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.SelectAll();

            frame.Dispose();
        }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        public static void ViewSource(this IWebBrowser browser)
        {
            var frame = browser.GetMainFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.ViewSource();

            frame.Dispose();
        }

        /// <summary>
        /// Retrieve the main frame's HTML source using a <see cref="Task{String}"/>.
        /// </summary>
        /// <returns><see cref="Task{String}"/> that when executed returns the frame source as a string</returns>
        public static Task<string> GetSourceAsync(this IWebBrowser browser)
        {
            var frame = browser.GetMainFrame();

            ThrowExceptionIfFrameNull(frame);

            return frame.GetSourceAsync();
        }

        /// <summary>
        /// Retrieve the main frame's display text using a <see cref="Task{String}"/>.
        /// </summary>
        /// <returns><see cref="Task{String}"/> that when executed returns the frame display text as a string.</returns>
        public static Task<string> GetTextAsync(this IWebBrowser browser)
        {
            var frame = browser.GetMainFrame();

            ThrowExceptionIfFrameNull(frame);

            return frame.GetTextAsync();
        }

        /// <summary>
        /// Execute some Javascript code in the context of this WebBrowser. As the method name implies, the script will be
        /// executed asynchronously, and the method therefore returns before the script has actually been executed.
        /// </summary>
        /// <param name="browser">the browser</param>
        /// <param name="script">The Javascript code that should be executed.</param>
        public static void ExecuteScriptAsync(this IWebBrowser browser, string script)
        {
            var frame = browser.GetMainFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.ExecuteJavaScriptAsync(script);

            frame.Dispose();
        }

        public static void LoadString(this IWebBrowser browser, string html, string url)
        {
            var frame = browser.GetMainFrame();

            ThrowExceptionIfFrameNull(frame);
            
            frame.LoadStringForUrl(html, url);

            frame.Dispose();
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
        /// <param name="browser">The instance this method extends</param>
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
        /// <param name="browser">The instance this method extends</param>
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
        /// <param name="browser"></param>
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
        public static Task<double> GetZoomLevelAsync(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            return host.GetZoomLevelAsync();
        }

        /// <summary>
        /// Change the ZoomLevel to the specified value. Can be set to 0.0 to clear the zoom level.
        /// </summary>
        /// <remarks>
        /// If called on the CEF UI thread the change will be applied immediately.
        /// Otherwise, the change will be applied asynchronously on the CEF UI thread.
        /// The CEF UI thread is different to the WPF/WinForms UI Thread
        /// </remarks>
        /// <param name="zoomLevel">zoom level</param>
        public static void SetZoomLevel(this IWebBrowser browser, double zoomLevel)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
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

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.Find(identifier, searchText, forward, matchCase, findNext);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="clearSelection">clear the current search selection</param>
        public static void StopFinding(this IWebBrowser browser, bool clearSelection)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.StopFinding(clearSelection);
        }

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        public static void Print(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.Print();
        }

        /// <summary>
        /// Open developer tools in its own window. 
        /// </summary>
        public static void ShowDevTools(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.ShowDevTools();
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        public static void CloseDevTools(this IWebBrowser browser)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.CloseDevTools();
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling
        /// this method will replace it with the specified word. 
        /// </summary>
        /// <param name="word">The new word that will replace the currently selected word.</param>
        public static void ReplaceMisspelling(this IWebBrowser browser, string word)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.ReplaceMisspelling(word);
        }

        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="word">The new word that will be added to the dictionary.</param>
        public static void AddWordToDictionary(this IWebBrowser browser, string word)
        {
            var cefBrowser = browser.GetBrowser();

            ThrowExceptionIfBrowserNull(cefBrowser);

            var host = cefBrowser.GetHost();

            ThrowExceptionIfBrowserHostNull(host);

            host.AddWordToDictionary(word);
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
