// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.WinForms.Example.Handlers
{
    internal class MenuHandler : IContextMenuHandler
    {
        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            //To disable the menu then call clear
            // model.Clear();

            //Removing existing menu item
            bool removed = model.Remove((int)CefMenuCommand.ViewSource); // Remove "View Source" option

            //adding new menu item
            model.AddItem((int)CefMenuCommand.Reload, "Reload");
        }

        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, int commandId, CefEventFlags eventFlags)
        {
            if (commandId == (int)CefMenuCommand.Reload)
            {
                browser.Reload();
            }
            return false;
        }

        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }
    }
}