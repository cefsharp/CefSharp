// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Handler
{
    /// <summary>
    /// Inherit from this class to handle events related to JavaScript dialogs.
    /// The methods of this class will be called on the CEF UI thread. 
    /// </summary>
    public class JsDialogHandler : IJsDialogHandler
    {
        /// <summary>
        /// Called to run a JavaScript dialog.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="originUrl">originating url</param>
        /// <param name="dialogType">Dialog Type</param>
        /// <param name="messageText">Message Text</param>
        /// <param name="defaultPromptText">value will be specified for prompt dialogs only</param>
        /// <param name="callback">Callback can be executed inline or in an async fashion</param>
        /// <param name="suppressMessage">Set suppressMessage to true and return false to suppress the message (suppressing messages is preferable to immediately executing the callback as this is used to detect presumably malicious behavior like spamming alert messages in onbeforeunload). Set suppressMessage to false and return false to use the default implementation (the default implementation will show one modal dialog at a time and suppress any additional dialog requests until the displayed dialog is dismissed).</param>
        /// <returns>Return true if the application will use a custom dialog or if the callback has been executed immediately. Custom dialogs may be either modal or modeless. If a custom dialog is used the application must execute |callback| once the custom dialog is dismissed.</returns>
        bool IJsDialogHandler.OnJSDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            return OnJSDialog(chromiumWebBrowser, browser, originUrl, dialogType, messageText, defaultPromptText, callback, ref suppressMessage);
        }

        /// <summary>
        /// Called to run a JavaScript dialog.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="originUrl">originating url</param>
        /// <param name="dialogType">Dialog Type</param>
        /// <param name="messageText">Message Text</param>
        /// <param name="defaultPromptText">value will be specified for prompt dialogs only</param>
        /// <param name="callback">Callback can be executed inline or in an async fashion</param>
        /// <param name="suppressMessage">Set suppressMessage to true and return false to suppress the message (suppressing messages is preferable to immediately executing the callback as this is used to detect presumably malicious behavior like spamming alert messages in onbeforeunload). Set suppressMessage to false and return false to use the default implementation (the default implementation will show one modal dialog at a time and suppress any additional dialog requests until the displayed dialog is dismissed).</param>
        /// <returns>Return true if the application will use a custom dialog or if the callback has been executed immediately. Custom dialogs may be either modal or modeless. If a custom dialog is used the application must execute |callback| once the custom dialog is dismissed.</returns>
        protected virtual bool OnJSDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            return false;
        }

        /// <summary>
        /// Called to run a dialog asking the user if they want to leave a page. Return false to use the default dialog implementation.
        /// Return true if the application will use a custom dialog or if the callback has been executed immediately.
        /// Custom dialogs may be either modal or modeless. If a custom dialog is used the application must execute <paramref name="callback"/>
        /// once the custom dialog is dismissed.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="messageText">message text (optional)</param>
        /// <param name="isReload">indicates a page reload</param>
        /// <param name="callback">Callback can be executed inline or in an async fashion</param>
        /// <returns>Return false to use the default dialog implementation otherwise return true to handle with your own custom implementation.</returns>
        bool IJsDialogHandler.OnBeforeUnloadDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string messageText, bool isReload, IJsDialogCallback callback)
        {
            return OnBeforeUnloadDialog(chromiumWebBrowser, browser, messageText, isReload, callback);
        }

        /// <summary>
        /// Called to run a dialog asking the user if they want to leave a page. Return false to use the default dialog implementation.
        /// Return true if the application will use a custom dialog or if the callback has been executed immediately.
        /// Custom dialogs may be either modal or modeless. If a custom dialog is used the application must execute <paramref name="callback"/>
        /// once the custom dialog is dismissed.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="messageText">message text (optional)</param>
        /// <param name="isReload">indicates a page reload</param>
        /// <param name="callback">Callback can be executed inline or in an async fashion</param>
        /// <returns>Return false to use the default dialog implementation otherwise return true to handle with your own custom implementation.</returns>
        protected virtual bool OnBeforeUnloadDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string messageText, bool isReload, IJsDialogCallback callback)
        {
            return false;
        }

        /// <summary>
        /// Called to cancel any pending dialogs and reset any saved dialog state. Will
        /// be called due to events like page navigation irregardless of whether any
        /// dialogs are currently pending.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        void IJsDialogHandler.OnResetDialogState(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            OnResetDialogState(chromiumWebBrowser, browser);
        }

        /// <summary>
        /// Called to cancel any pending dialogs and reset any saved dialog state. Will
        /// be called due to events like page navigation irregardless of whether any
        /// dialogs are currently pending.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        protected virtual void OnResetDialogState(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        /// <summary>
        /// Called when the default implementation dialog is closed.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        void IJsDialogHandler.OnDialogClosed(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            OnDialogClosed(chromiumWebBrowser, browser);
        }

        /// <summary>
        /// Called when the default implementation dialog is closed.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        protected virtual void OnDialogClosed(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }
    }
}
