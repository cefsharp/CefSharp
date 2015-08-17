// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    public interface IContextMenuHandler
    {
        /// <summary>
        /// Called before a context menu is displayed. |params| provides information
        /// about the context menu state. |model| initially contains the default
        /// context menu. The |model| can be cleared to show no context menu or
        /// modified to show a custom menu.
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="parameters"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                                IMenuModel model);

        /// <summary>
        /// Called to execute a command selected from the context menu. Return true if
        /// the command was handled or false for the default implementation. See
        /// cef_menu_id_t for the command ids that have default implementations. All
        /// user-defined command ids should be between MENU_ID_USER_FIRST and
        /// MENU_ID_USER_LAST. |params| will have the same values as what was passed to
        /// OnBeforeContextMenu().
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <param name="parameters"></param>
        /// <param name="commandId"></param>
        /// <param name="eventFlags"></param>
        /// <returns></returns>
        bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters,
                                  int commandId, CefEventFlags eventFlags);

        /// <summary>
        /// Called when the context menu is dismissed irregardless of whether the menu
        /// was empty or a command was selected.
        /// </summary>
        /// <param name="browserControl">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="frame">The frame the request is coming from</param>
        void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame);
    }
}
