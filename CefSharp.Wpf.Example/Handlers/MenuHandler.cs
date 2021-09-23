// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using CefSharp.Wpf.Handler;

namespace CefSharp.Wpf.Example.Handlers
{
    public class MenuHandler : CefSharp.Wpf.Handler.ContextMenuHandler
    {
        public MenuHandler(bool addDevtoolsMenuItems = false) : base(addDevtoolsMenuItems)
        {
        }

        protected override void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            if (model.Count > 0)
            {
                model.AddSeparator();
            }

            //For this menu handler 26501 and 26502 are used by the Show/Close DevTools commands
            model.AddItem((CefMenuCommand)26503, "Do Something");

            //To disable context mode then clear
            // model.Clear();
        }

        protected override bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            if (commandId == (CefMenuCommand)26501)
            {
                browser.GetHost().ShowDevTools();
                return true;
            }
            if (commandId == (CefMenuCommand)26502)
            {
                browser.GetHost().CloseDevTools();
                return true;
            }

            return false;
        }

        protected override void ExecuteCommand(IBrowser browser, ContextMenuExecuteModel model)
        {
            //Custom item
            if (model.MenuCommand == (CefMenuCommand)26503)
            {
                Console.WriteLine("Custom menu used");
            }
            else
            {
                base.ExecuteCommand(browser, model);
            }
        }
    }
}
