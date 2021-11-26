// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.Web;

namespace CefSharp.WinForms.Host
{
    /// <summary>
    /// <see cref="ChromiumWebBrowser"/> and <see cref="ChromiumHostControl"/> extensions
    /// </summary>
    public static class HostControlExtensions
    {
        /// <summary>
        /// Open DevTools using <paramref name="parentControl"/> as the parent control. If inspectElementAtX and/or inspectElementAtY are specified then
        /// the element at the specified (x,y) location will be inspected.
        /// For resize/moving to work correctly you will need to use the <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/> implementation.
        /// (Set <see cref="ChromiumWebBrowser.LifeSpanHandler"/> to an instance of <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/>)
        /// </summary>
        /// <param name="hostControl"><see cref="ChromiumHostControlBase"/> instance</param>
        /// <param name="parentControl">Control used as the parent for DevTools (a custom control will be added to the <see cref="Control.Controls"/> collection)</param>
        /// <param name="inspectElementAtX">x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">y coordinate (used for inspectElement)</param>
        /// <returns>Returns the <see cref="Control"/> that hosts the DevTools instance if successful, otherwise returns null on error.</returns>
        public static Control ShowDevToolsDocked(this IChromiumHostControl hostControl, Control parentControl, string controlName = nameof(ChromiumHostControl) + "DevTools", DockStyle dockStyle = DockStyle.Fill, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            if (hostControl.IsDisposed || parentControl == null || parentControl.IsDisposed)
            {
                return null;
            }

            return hostControl.ShowDevToolsDocked((ctrl) => { parentControl.Controls.Add(ctrl); }, controlName, dockStyle, inspectElementAtX, inspectElementAtY);
        }

        /// <summary>
        /// Open DevTools using your own Control as the parent. If inspectElementAtX and/or inspectElementAtY are specified then
        /// the element at the specified (x,y) location will be inspected.
        /// For resize/moving to work correctly you will need to use the <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/> implementation.
        /// (Set <see cref="ChromiumWebBrowser.LifeSpanHandler"/> to an instance of <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/>)
        /// </summary>
        /// <param name="hostControl"><see cref="ChromiumHostControlBase"/> instance</param>
        /// <param name="addParentControl">
        /// Action that is Invoked when the DevTools Host Control has been created and needs to be added to it's parent.
        /// It's important the control is added to it's intended parent at this point so the <see cref="Control.ClientRectangle"/>
        /// can be calculated to set the initial display size.</param>
        /// <param name="inspectElementAtX">x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">y coordinate (used for inspectElement)</param>
        /// <returns>Returns the <see cref="Control"/> that hosts the DevTools instance if successful, otherwise returns null on error.</returns>
        public static Control ShowDevToolsDocked(this IChromiumHostControl hostControl, Action<ChromiumHostControl> addParentControl, string controlName = nameof(ChromiumHostControl) + "DevTools", DockStyle dockStyle = DockStyle.Fill, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            if (hostControl.IsDisposed || addParentControl == null)
            {
                return null;
            }

            var host = hostControl.GetBrowserHost();
            if (host == null)
            {
                return null;
            }

            var control = new ChromiumHostControl()
            {
                Name = controlName,
                Dock = dockStyle
            };

            control.CreateControl();

            //It's now time for the user to add the control to it's parent
            addParentControl(control);

            //Devtools will be a child of the ChromiumHostControl
            var rect = control.ClientRectangle;
            var windowInfo = new WindowInfo();
            var windowBounds = new CefSharp.Structs.Rect(rect.X, rect.Y, rect.Width, rect.Height);
            windowInfo.SetAsChild(control.Handle, windowBounds);
            host.ShowDevTools(windowInfo, inspectElementAtX, inspectElementAtY);

            return control;
        }

        /// <summary>
        /// Asynchronously gets the current Zoom Level.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <returns>
        /// An asynchronous result that yields the zoom level.
        /// </returns>
        public static Task<double> GetZoomLevelAsync(this IChromiumHostControl hostControl)
        {
            return hostControl.BrowserCore.GetZoomLevelAsync();
        }

        /// <summary>
        /// Change the ZoomLevel to the specified value. Can be set to 0.0 to clear the zoom level.
        /// </summary>
        /// <remarks>
        /// If called on the CEF UI thread the change will be applied immediately. Otherwise, the change will be applied asynchronously
        /// on the CEF UI thread. The CEF UI thread is different to the WPF/WinForms UI Thread.
        /// </remarks>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="zoomLevel">zoom level.</param>
        public static void SetZoomLevel(this IChromiumHostControl hostControl, double zoomLevel)
        {
            hostControl.BrowserCore.SetZoomLevel(zoomLevel);
        }

        /// <summary>
        /// Search for text within the current page.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="identifier">Can be used in can conjunction with searchText to have multiple searches running simultaneously.</param>
        /// <param name="searchText">search text.</param>
        /// <param name="forward">indicates whether to search forward or backward within the page.</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive.</param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up.</param>
        public static void Find(this IChromiumHostControl hostControl, int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            hostControl.BrowserCore.Find(identifier, searchText, forward, matchCase, findNext);
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="clearSelection">clear the current search selection.</param>
        public static void StopFinding(this IChromiumHostControl hostControl, bool clearSelection)
        {
            hostControl.BrowserCore.StopFinding(clearSelection);
        }

        /// <summary>
        /// Opens a Print Dialog which if used (can be user cancelled) will print the browser contents.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void Print(this IChromiumHostControl hostControl)
        {
            hostControl.BrowserCore.Print();
        }

        /// <summary>
        /// Stops loading the current page.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void StopLoad(this IChromiumHostControl hostControl)
        {
            hostControl.BrowserCore.StopLoad();
        }

        /// <summary>
        /// Navigates back, must check <see cref="IWebBrowser.CanGoBack"/> before calling this method.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void GoBack(this IChromiumHostControl hostControl)
        {
            hostControl.BrowserCore.GoBack();
        }

        /// <summary>
        /// Navigates forward, must check <see cref="IWebBrowser.CanGoForward"/> before calling this method.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void GoForward(this IChromiumHostControl hostControl)
        {
            hostControl.BrowserCore.GoForward();
        }

        /// <summary>
        /// Reloads the page being displayed, optionally ignoring the cache (which means the whole page including all .css, .js etc.
        /// resources will be re-fetched).
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="ignoreCache"><c>true</c> A reload is performed ignoring browser cache; <c>false</c> A reload is performed using
        /// files from the browser cache, if available.</param>
        public static void Reload(this IChromiumHostControl hostControl, bool ignoreCache = false)
        {
            hostControl.BrowserCore.Reload(ignoreCache);
        }

        /// <summary>
        /// Retrieve the main frame's HTML source using a <see cref="Task{String}"/>.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <returns>
        /// <see cref="Task{String}"/> that when executed returns the main frame source as a string.
        /// </returns>
        public static Task<string> GetSourceAsync(this IChromiumHostControl hostControl)
        {
            return hostControl.BrowserCore.GetSourceAsync();
        }

        /// <summary>
        /// Shortcut method to get the browser <see cref="IBrowserHost"/>.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <returns>IBrowserHost instance or null</returns>
        public static IBrowserHost GetBrowserHost(this IChromiumHostControl hostControl)
        {
            var cefBrowser = hostControl.BrowserCore;

            return cefBrowser == null ? null : cefBrowser.GetHost();
        }

        /// <summary>
        /// Loads the specified <paramref name="url"/> in the Main Frame.
        /// If <see cref="IsDisposed"/> is true then the method call will be ignored.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="url">url to load in Main Frame.</param>
        public static void LoadUrl(this IChromiumHostControl hostControl, string url)
        {
            if (hostControl.IsDisposed)
            {
                return;
            }

            var browser = hostControl.BrowserCore;

            if (browser == null)
            {
                throw new Exception(CefSharp.WebBrowserExtensions.BrowserNullExceptionString);
            }

            using (var mainFrame = browser.MainFrame)
            {
                mainFrame.LoadUrl(url);
            }
        }

        /// <summary>
        /// Returns the focused frame for the browser window.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <returns>the focused frame.</returns>
        public static IFrame GetFocusedFrame(this IChromiumHostControl hostControl)
        {
            var browser = hostControl.BrowserCore;

            if (browser == null)
            {
                throw new Exception(CefSharp.WebBrowserExtensions.BrowserNullExceptionString);
            }

            return browser.FocusedFrame;
        }

        /// <summary>
        /// Returns the main (top-level) frame for the browser window.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <returns> the main frame. </returns>
        public static IFrame GetMainFrame(this IChromiumHostControl hostControl)
        {
            var browser = hostControl.BrowserCore;

            if (browser == null)
            {
                throw new Exception(CefSharp.WebBrowserExtensions.BrowserNullExceptionString);
            }

            return browser.MainFrame;
        }

        /// <summary>
        /// Execute Undo on the focused frame.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void Undo(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Undo();
            }
        }

        /// <summary>
        /// Execute Redo on the focused frame.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void Redo(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Redo();
            }
        }

        /// <summary>
        /// Execute Cut on the focused frame.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void Cut(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Cut();
            }
        }

        /// <summary>
        /// Execute Copy on the focused frame.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void Copy(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Copy();
            }
        }

        /// <summary>
        /// Execute Paste on the focused frame.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void Paste(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Paste();
            }
        }

        /// <summary>
        /// Execute Delete on the focused frame.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void Delete(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.Delete();
            }
        }

        /// <summary>
        /// Execute SelectAll on the focused frame.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void SelectAll(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetFocusedFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.SelectAll();
            }
        }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web page is
        /// shown.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void ViewSource(this IChromiumHostControl hostControl)
        {
            using (var frame = hostControl.GetMainFrame())
            {
                ThrowExceptionIfFrameNull(frame);

                frame.ViewSource();
            }
        }

        /// <summary>
        /// Open developer tools in its own window.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="windowInfo">(Optional) window info used for showing dev tools.</param>
        /// <param name="inspectElementAtX">(Optional) x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">(Optional) y coordinate (used for inspectElement)</param>
        public static void ShowDevTools(this IChromiumHostControl hostControl, IWindowInfo windowInfo = null, int inspectElementAtX = 0, int inspectElementAtY = 0)
        {
            var host = hostControl.GetBrowserHost();
            CefSharp.WebBrowserExtensions.ThrowExceptionIfBrowserHostNull(host);

            host.ShowDevTools(windowInfo, inspectElementAtX, inspectElementAtY);
        }

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        public static void CloseDevTools(this IChromiumHostControl hostControl)
        {
            var host = hostControl.GetBrowserHost();
            CefSharp.WebBrowserExtensions.ThrowExceptionIfBrowserHostNull(host);

            host.CloseDevTools();
        }

        /// <summary>
        /// Asynchronously prints the current browser contents to the PDF file specified. The caller is responsible for deleting the file
        /// when done.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="path">Output file location.</param>
        /// <param name="settings">(Optional) Print Settings.</param>
        /// <returns>
        /// A task that represents the asynchronous print operation. The result is true on success or false on failure to generate the
        /// Pdf.
        /// </returns>
        public static Task<bool> PrintToPdfAsync(this IChromiumHostControl hostControl, string path, PdfPrintSettings settings = null)
        {
            var host = hostControl.GetBrowserHost();
            CefSharp.WebBrowserExtensions.ThrowExceptionIfBrowserHostNull(host);

            var callback = new TaskPrintToPdfCallback();
            host.PrintToPdf(path, settings, callback);

            return callback.Task;
        }

        /// <summary>
        /// Loads html as Data Uri See https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Data_URIs for details If
        /// base64Encode is false then html will be Uri encoded.
        /// </summary>
        /// <param name="hostControl">The ChromiumWebBrowser/ChromiumHostControl instance this method extends.</param>
        /// <param name="html">Html to load as data uri.</param>
        /// <param name="base64Encode">(Optional) if true the html string will be base64 encoded using UTF8 encoding.</param>
        public static void LoadHtml(this IChromiumHostControl hostControl, string html, bool base64Encode = false)
        {
            var htmlString = new HtmlString(html, base64Encode);

            hostControl.LoadUrl(htmlString.ToDataUriString());
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
                throw new Exception(CefSharp.WebBrowserExtensions.FrameNullExceptionString);
            }
        }
    }
}
