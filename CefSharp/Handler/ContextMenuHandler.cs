// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Handler
{
    /// Inherit from this class to handle context menu events. 
    /// </summary>
    public class ContextMenuHandler : IContextMenuHandler
    {
        /// <inheritdoc/>
        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                                IMenuModel model)
        {
            OnBeforeContextMenu(chromiumWebBrowser, browser, frame, parameters, model);
        }

        /// <summary>
        /// Called before a context menu is displayed. The model can be cleared to show no context menu or
        /// modified to show a custom menu.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="parameters">provides information about the context menu state</param>
        /// <param name="model">initially contains the default context menu</param>
        protected virtual void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                                IMenuModel model)
        {

        }

        /// <inheritdoc/>
        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                                  CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return OnContextMenuCommand(chromiumWebBrowser, browser, frame, parameters, commandId, eventFlags);
        }

        /// <summary>
        /// Called to execute a command selected from the context menu. See
        /// cef_menu_id_t for the command ids that have default implementations. All
        /// user-defined command ids should be between MENU_ID_USER_FIRST and
        /// MENU_ID_USER_LAST.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="parameters">will have the same values as what was passed to</param>
        /// <param name="commandId">menu command id</param>
        /// <param name="eventFlags">event flags</param>
        /// <returns>Return true if the command was handled or false for the default implementation.</returns>
        protected virtual bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                                  CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        /// <inheritdoc/>
        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            OnContextMenuDismissed(chromiumWebBrowser, browser, frame);
        }

        /// <summary>
        /// Called when the context menu is dismissed irregardless of whether the menu
        /// was empty or a command was selected.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        protected virtual void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {

        }

        /// <inheritdoc/>
        bool IContextMenuHandler.RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return RunContextMenu(chromiumWebBrowser, browser, frame, parameters, model, callback);
        }

        /// <summary>
        /// Called to allow custom display of the context menu.
        /// For custom display return true and execute callback either synchronously or asynchronously with the selected command Id.
        /// For default display return false. Do not keep references to parameters or model outside of this callback. 
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="parameters">provides information about the context menu state</param>
        /// <param name="model">contains the context menu model resulting from OnBeforeContextMenu</param>
        /// <param name="callback">the callback to execute for custom display</param>
        /// <returns>For custom display return true and execute callback either synchronously or asynchronously with the selected command ID.</returns>
        protected virtual bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
