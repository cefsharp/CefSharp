// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IJsDialogHandler
    {
        bool OnJSAlert(IWebBrowser browser, string url, string message);
        bool OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval);
        bool OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result);
        
        /// <summary>
        /// When leaving the page a Javascript dialog is displayed asking for user confirmation.
        /// Returning True allows you to implement a custom dialog or programatically handle.
        /// To cancel the unload return True and set allowUnload to False.
        /// </summary>
        /// <param name="browser">the browser object</param>
        /// <param name="message">message (optional)</param>
        /// <param name="isReload">indicates a page reload</param>
        /// <param name="allowUnload">True to allow unload, otherwise False</param>
        /// <returns>Return false to use the default dialog implementation otherwise return true to handle</returns>
        bool OnJSBeforeUnload(IWebBrowser browser, string message, bool isReload, out bool allowUnload);
    }
}
