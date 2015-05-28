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
        }

        public static void Redo(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Redo();
        }

        public static void Cut(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Cut();
        }

        public static void Copy(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Copy();
        }

        public static void Paste(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.Paste();
        }

        public static void Delete(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);
            
            frame.Delete();
        }

        public static void SelectAll(this IWebBrowser browser)
        {
            var frame = browser.GetFocusedFrame();

            ThrowExceptionIfFrameNull(frame);

            frame.SelectAll();
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
        }

        public static void LoadString(this IWebBrowser browser, string html, string url)
        {
            var frame = browser.GetMainFrame();

            ThrowExceptionIfFrameNull(frame);
            
            frame.LoadStringForUrl(html, url);
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
            var internalBrowser = browser.GetBrowser();

            internalBrowser.StopLoad();
        }

        /// <summary>
        /// Navigates back, must check <see cref="IWebBrowser.CanGoBack"/> before calling this method.
        /// </summary>
        public static void Back(this IWebBrowser browser)
        {
            var internalBrowser = browser.GetBrowser();

            internalBrowser.GoBack();
        }

        /// <summary>
        /// Navigates forward, must check <see cref="IWebBrowser.CanGoForward"/> before calling this method.
        /// </summary>
        public static void Forward(this IWebBrowser browser)
        {
            var internalBrowser = browser.GetBrowser();

            internalBrowser.GoForward();
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
            var internalBrowser = browser.GetBrowser();

            if(ignoreCache)
            {
                internalBrowser.ReloadIgnoreCache();
            }
            else
            {
                internalBrowser.Reload();
            }
        }

        private static void ThrowExceptionIfFrameNull(IFrame frame)
        {
            if (frame == null)
            {
                throw new Exception("IFrame instance is null. Browser has likely not finished initializing or is in the process of disposing.");
            }
        }
    }
}
