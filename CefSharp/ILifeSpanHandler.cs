// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface ILifeSpanHandler
    {
        /// <summary>
        /// Called before a popup window is created.
        /// </summary>
        /// <param name="browser">The IWebBrowser control this request is for.</param>
        /// <param name="sourceUrl">The URL of the HTML frame that launched this popup.</param>
        /// <param name="targetUrl">The URL of the popup content. (This may be empty/null)</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
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
        bool OnBeforePopup(IWebBrowser browser, string sourceUrl, string targetUrl, ref int x, ref int y, ref int width, ref int height);

        /// <summary>
        /// Called before a CefBrowser window (either the main browser for IWebBrowser, 
        /// or one of its children)
        /// </summary>
        /// <param name="browser"></param>
        void OnBeforeClose(IWebBrowser browser);
    }
}
