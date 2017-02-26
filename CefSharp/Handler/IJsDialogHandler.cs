// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to JavaScript dialogs.
    /// The methods of this class will be called on the CEF UI thread. 
    /// </summary>
    public interface IJsDialogHandler
    {
        /// <summary>
        /// Called to run a JavaScript dialog.
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="originUrl">originating url</param>
        /// <param name="dialogType">Dialog Type</param>
        /// <param name="messageText">Message Text</param>
        /// <param name="defaultPromptText">value will be specified for prompt dialogs only</param>
        /// <param name="callback">Callback can be executed inline or in an async fashion</param>
        /// <param name="suppressMessage">Set suppressMessage to true and return false to suppress the message (suppressing messages is preferable to immediately executing the callback as this is used to detect presumably malicious behavior like spamming alert messages in onbeforeunload). Set suppressMessage to false and return false to use the default implementation (the default implementation will show one modal dialog at a time and suppress any additional dialog requests until the displayed dialog is dismissed).</param>
        /// <returns>Return true if the application will use a custom dialog or if the callback has been executed immediately. Custom dialogs may be either modal or modeless. If a custom dialog is used the application must execute |callback| once the custom dialog is dismissed.</returns>
        bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage);
        
        /// <summary>
        /// When leaving the page a Javascript dialog is displayed asking for user confirmation.
        /// Returning True allows you to implement a custom dialog or programatically handle.
        /// To cancel the unload return True and set allowUnload to False.
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="message">message (optional)</param>
        /// <param name="isReload">indicates a page reload</param>
        /// <param name="callback">Callback can be executed inline or in an async fashion</param>
        /// <returns>Return false to use the default dialog implementation otherwise return true to handle</returns>
        bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback);

        /// <summary>
        /// Called to cancel any pending dialogs and reset any saved dialog state. Will
        /// be called due to events like page navigation irregardless of whether any
        /// dialogs are currently pending.
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        void OnResetDialogState(IWebBrowser browserControl, IBrowser browser);

        /// <summary>
        /// Called when the default implementation dialog is closed.
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        void OnDialogClosed(IWebBrowser browserControl, IBrowser browser);
    }
}
