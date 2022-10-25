// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Handler
{
    /// <summary>
    /// Default implementation of <see cref="ILifeSpanHandler"/>. This class provides default implementations of the methods from
    /// <see cref="ILifeSpanHandler"/>, therefore providing a convenience base class for a custom implementation.
    /// You need only override the methods you require.
    /// IMPORTANT: <see cref="ILifeSpanHandler.DoClose(IWebBrowser, IBrowser)"/> behaviour of this implementation differs
    /// from the default, the WM_CLOSE message is only sent by default for popups (return false), for the main browser
    /// we return true to cancel this behaviour.
    /// </summary>
    /// <seealso cref="T:CefSharp.ILifeSpanHandler"/>
    public class LifeSpanHandler : ILifeSpanHandler
    {
        /// <inheritdoc/>
        bool ILifeSpanHandler.DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return DoClose(chromiumWebBrowser, browser);
        }

        /// <summary>
        /// Called when a browser has recieved a request to close. This may result
        /// directly from a call to CefBrowserHost::CloseBrowser() or indirectly if the
        /// browser is a top-level OS window created by CEF and the user attempts to
        /// close the window. This method will be called after the JavaScript
        /// 'onunload' event has been fired. It will not be called for browsers after
        /// the associated OS window has been destroyed (for those browsers it is no
        /// longer possible to cancel the close).
        ///
        /// If CEF created an OS window for the browser returning false will send an OS
        /// close notification to the browser window's top-level owner (e.g. WM_CLOSE
        /// on Windows). If no OS window exists (window rendering disabled) returning false
        /// will cause the browser object to be destroyed immediately. Return true if
        /// the browser is parented to another window and that other window needs to
        /// receive close notification via some non-standard technique.
        ///
        /// !!IMPORTANT!!: Behaviour of this implementation differs from the default,
        /// the WM_CLOSE message is only sent by default for popups (return false),
        /// for the main browser we return true to cancel this behaviour.
        ///
        /// If an application provides its own top-level window it should handle OS
        /// close notifications by calling CefBrowserHost::CloseBrowser(false) instead
        /// of immediately closing (see the example below). This gives CEF an
        /// opportunity to process the 'onbeforeunload' event and optionally cancel the
        /// close before DoClose() is called.
        ///
        /// The CefLifeSpanHandler::OnBeforeClose() method will be called immediately
        /// before the browser object is destroyed. The application should only exit
        /// after OnBeforeClose() has been called for all existing browsers.
        ///
        /// If the browser represents a modal window and a custom modal loop
        /// implementation was provided in CefLifeSpanHandler::RunModal() this callback
        /// should be used to restore the opener window to a usable state.
        ///
        /// By way of example consider what should happen during window close when the
        /// browser is parented to an application-provided top-level OS window.
        /// 1.  User clicks the window close button which sends an OS close
        ///     notification (e.g. WM_CLOSE on Windows, performClose: on OS-X and
        ///     "delete_event" on Linux).
        /// 2.  Application's top-level window receives the close notification and:
        ///     A. Calls CefBrowserHost::CloseBrowser(false).
        ///     B. Cancels the window close.
        /// 3.  JavaScript 'onbeforeunload' handler executes and shows the close
        ///     confirmation dialog (which can be overridden via
        ///     CefJSDialogHandler::OnBeforeUnloadDialog()).
        /// 4.  User approves the close.
        /// 5.  JavaScript 'onunload' handler executes.
        /// 6.  Application's DoClose() handler is called. Application will:
        ///     A. Set a flag to indicate that the next close attempt will be allowed.
        ///     B. Return false.
        /// 7.  CEF sends an OS close notification.
        /// 8.  Application's top-level window receives the OS close notification and
        ///     allows the window to close based on the flag from #6B.
        /// 9.  Browser OS window is destroyed.
        /// 10. Application's CefLifeSpanHandler::OnBeforeClose() handler is called and
        ///     the browser object is destroyed.
        /// 11. Application exits by calling CefQuitMessageLoop() if no other browsers
        ///     exist.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance - check if IsDisposed as it's possible when the browser is disposing</param>
        /// <returns>For default behaviour return false</returns>
        protected virtual bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            //For popups by default are created by CEF and need to be closed via the default behaviour
            //(Sending a WM_CLOSE message)
            if (browser.IsPopup)
            {
                return false;
            }

            //For the main browser NO WM_CLOSE message will be sent.
            return true;
        }

        /// <inheritdoc/>
        void ILifeSpanHandler.OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            OnAfterCreated(chromiumWebBrowser, browser);
        }

        /// <summary>
        /// Called after a new browser is created.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance</param>
        protected virtual void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        /// <inheritdoc/>
        void ILifeSpanHandler.OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            OnBeforeClose(chromiumWebBrowser, browser);
        }

        /// <summary>
        /// Called before a CefBrowser window (either the main browser for <see cref="IWebBrowser"/>, 
        /// or one of its children)
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance</param>
        protected virtual void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        /// <inheritdoc/>
        bool ILifeSpanHandler.OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            return OnBeforePopup(chromiumWebBrowser, browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, browserSettings, ref noJavascriptAccess, out newBrowser);
        }

        /// <summary>
        /// Called before a popup window is created. By default the popup (browser) is created in a new native window.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance that launched this popup.</param>
        /// <param name="frame">The HTML frame that launched this popup.</param>
        /// <param name="targetUrl">The URL of the popup content. (This may be empty/null)</param>
        /// <param name="targetFrameName">The name of the popup. (This may be empty/null)</param>
        /// <param name="targetDisposition">The value indicates where the user intended to
        /// open the popup (e.g. current tab, new tab, etc)</param>
        /// <param name="userGesture">The value will be true if the popup was opened via explicit user gesture
        /// (e.g. clicking a link) or false if the popup opened automatically (e.g. via the DomContentLoaded event).</param>
        /// <param name="popupFeatures"> structure contains additional information about the requested popup window</param>
        /// <param name="windowInfo">window information</param>
        /// <param name="browserSettings">browser settings, defaults to source browsers</param>
        /// <param name="noJavascriptAccess">value indicates whether the new browser window should be scriptable
        /// and in the same process as the source browser.</param>
        /// <param name="newBrowser">
        /// EXPERIMENTAL - Low level this allows for assigning the CefClient instance associated with the new ChromiumWebBrowser instance to the CefClient param of the CefLifeSpanHandler::OnBeforeBrowser method.
        /// This allows for all the handlers, LifeSpanHandler, DisplayHandler, etc to be associated with the CefClient of the new ChromiumWebBrowser instance to be associated with the popup (browser).
        /// WPF/WinForms specific code is still required to host the popup (browser) in the new ChromiumWebBrowser instance.
        /// Set to null for default behaviour. If you return true (cancel popup creation) then his property **MUST** be null, an exception will be thrown otherwise.
        /// </param>
        /// <returns>
        /// By default the popup (browser) is opened in a new native window. If you return true then creation of the popup (browser) is cancelled, no further action will occur.
        /// Otherwise return false to allow creation of the popup (browser). 
        /// </returns>
        /// <remarks>
        /// If you return true and set <paramref name="newBrowser"/> to not null then an exception will be thrown as creation of the popup (browser) was cancelled.
        /// WinForms - To host the popup (browser) in a TAB/Custom Window see https://github.com/cefsharp/CefSharp/wiki/General-Usage#winforms---hosting-popup-using-tab-control for an easy method.
        /// WPF - For an example of hosting the popup (browser) in a custom window see https://github.com/cefsharp/CefSharp/wiki/General-Usage#wpf---hosting-popup-in-new-window-experimental
        /// Same can be applied for hosting the popup in a TAB.
        /// This method is still EXPERIMENTAL and will likely require upstream bug fixes in CEF (https://bitbucket.org/chromiumembedded/cef).
        /// </remarks>
        protected virtual bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            return false;
        }
    }
}
