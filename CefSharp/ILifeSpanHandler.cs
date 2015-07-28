// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface ILifeSpanHandler
    {
        /// <summary>
        /// Called before a popup window is created.
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control this request is for.</param>
        /// <param name="browser">The browser instance that launched this popup.</param>
        /// <param name="frame">The HTML frame that launched this popup.</param>
        /// <param name="targetUrl">The URL of the popup content. (This may be empty/null)</param>
        /// <param name="targetFrameName">The name of the popup. (This may be empty/null)</param>
        /// <param name="targetDisposition">The value indicates where the user intended to
        /// open the popup (e.g. current tab, new tab, etc)</param>
        /// <param name="userGesture">The value will be true if the popup was opened via explicit user gesture
        /// (e.g. clicking a link) or false if the popup opened automatically (e.g. via the DomContentLoaded event).</param>
        /// <param name="windowInfo">window information</param>
        /// <param name="noJavascriptAccess">value indicates whether the new browser window should be scriptable
        /// and in the same process as the source browser.</param>
        /// <param name="newBrowser">A newly created browser that will host the popup</param>
        /// <returns>To cancel creation of the popup window return true otherwise return false.</returns>
        /// <remarks>
        /// CEF documentation:
        /// 
        /// Called on the IO thread before a new popup window is created. The |browser|
        /// and |frame| parameters represent the source of the popup request. The
        /// |target_url| and |target_frame_name| values may be empty if none were
        /// specified with the request. The |popupFeatures| structure contains
        /// information about the requested popup window. To allow creation of the
        /// popup window optionally modify |windowInfo|, |client|, |settings| and
        /// |no_javascript_access| and return false. To cancel creation of the popup
        /// window return true. The |client| and |settings| values will default to the
        /// source browser's values. The |no_javascript_access| value indicates whether
        /// the new browser window should be scriptable and in the same process as the
        /// source browser.
        /// </remarks>
        bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser);

        /// <summary>
        /// Called after a new browser is created.
        /// </summary>
        /// <param name="browser"></param>
        void OnAfterCreated(IWebBrowser browser);

        /// <summary>
        /// Called before a CefBrowser window (either the main browser for <see cref="IWebBrowser"/>, 
        /// or one of its children)
        /// </summary>
        /// <param name="browserControl">The <see cref="IWebBrowser"/> control that is realted to the window is closing.</param>
        /// <param name="browser">The browser instance</param>
        void OnBeforeClose(IWebBrowser browserControl, IBrowser browser);
    }
}
