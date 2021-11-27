// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
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
            base.OnBeforeContextMenu(chromiumWebBrowser, browser, frame, parameters, model);

            Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            if (model.Count > 0)
            {
                model.AddSeparator();
            }

            //For this menu handler 28440 and 28441 are used by the Show/Close DevTools commands
            model.AddItem((CefMenuCommand)26501, "Do Something");

            //To disable context menu then clear
            // model.Clear();
        }

        protected override void ExecuteCommand(IBrowser browser, ContextMenuExecuteModel model)
        {
            //Custom item
            if (model.MenuCommand == (CefMenuCommand)26501)
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
