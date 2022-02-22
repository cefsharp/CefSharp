// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;

namespace CefSharp.Wpf.Handler
{
    /// <summary>
    /// Implementation of <see cref="IContextMenuHandler"/> that uses a <see cref="ContextMenu"/>
    /// to display the context menu.
    /// </summary>
    public class ContextMenuHandler : CefSharp.Handler.ContextMenuHandler
    {
        /// <summary>
        /// Open DevTools <see cref="CefMenuCommand"/> Id
        /// </summary>
        public const int CefMenuCommandShowDevToolsId = 28440;
        /// <summary>
        /// Close DevTools <see cref="CefMenuCommand"/> Id
        /// </summary>
        public const int CefMenuCommandCloseDevToolsId = 28441;

        private readonly bool addDevtoolsMenuItems;

        public ContextMenuHandler(bool addDevtoolsMenuItems = false)
        {
            this.addDevtoolsMenuItems = addDevtoolsMenuItems;
        }

        /// <inheritdoc/>
        protected override void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            if (addDevtoolsMenuItems)
            {
                if (model.Count > 0)
                {
                    model.AddSeparator();
                }

                model.AddItem((CefMenuCommand)CefMenuCommandShowDevToolsId, "Show DevTools (Inspect)");
                model.AddItem((CefMenuCommand)CefMenuCommandCloseDevToolsId, "Close DevTools");
            }
        }

        /// <inheritdoc/>
        protected override void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            webBrowser.UiThreadRunAsync(() =>
            {
                webBrowser.ContextMenu = null;
            });
        }


        /// <inheritdoc/>
        protected override bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            //IMenuModel is only valid in the context of this method, so need to read the values before invoking on the UI thread
            var menuItems = GetMenuItems(model);
            var dictionarySuggestions = parameters.DictionarySuggestions;
            var xCoord = parameters.XCoord;
            var yCoord = parameters.YCoord;
            var misspelledWord = parameters.MisspelledWord;
            var selectionText = parameters.SelectionText;

            webBrowser.UiThreadRunAsync(() =>
            {
                var menu = new ContextMenu
                {
                    IsOpen = true,
                    Placement = PlacementMode.Mouse
                };

                RoutedEventHandler handler = null;

                handler = (s, e) =>
                {
                    menu.Closed -= handler;

                    //If the callback has been disposed then it's already been executed
                    //so don't call Cancel
                    if (!callback.IsDisposed)
                    {
                        callback.Cancel();
                    }
                };

                menu.Closed += handler;

                foreach (var item in menuItems)
                {
                    if (item.IsSeperator)
                    {
                        menu.Items.Add(new Separator());

                        continue;
                    }

                    if (item.CommandId == CefMenuCommand.NotFound)
                    {
                        continue;
                    }                    

                    var menuItem = new MenuItem
                    {
                        Header = item.Label.Replace("&", "_"),
                        IsEnabled = item.IsEnabled,
                        IsChecked = item.IsChecked.GetValueOrDefault(),
                        IsCheckable = item.IsChecked.HasValue,
                        Command = new DelegateCommand(() =>
                        {
                            //BUG: CEF currently not executing callbacks correctly so we manually map the commands below
                            //see https://github.com/cefsharp/CefSharp/issues/1767
                            //The following line worked in previous versions, it doesn't now, so custom handling below
                            //callback.Continue(item.Item2, CefEventFlags.None);
                            ExecuteCommand(browser, new ContextMenuExecuteModel(item.CommandId, dictionarySuggestions, xCoord, yCoord, selectionText, misspelledWord));
                        }),
                    };

                    //TODO: Make this recursive and remove duplicate code
                    if(item.SubMenus != null && item.SubMenus.Count > 0)
                    {
                        foreach(var subItem in item.SubMenus)
                        {
                            if (subItem.CommandId == CefMenuCommand.NotFound)
                            {
                                continue;
                            }

                            if (subItem.IsSeperator)
                            {
                                menu.Items.Add(new Separator());

                                continue;
                            }

                            var subMenuItem = new MenuItem
                            {
                                Header = subItem.Label.Replace("&", "_"),
                                IsEnabled = subItem.IsEnabled,
                                IsChecked = subItem.IsChecked.GetValueOrDefault(),
                                IsCheckable = subItem.IsChecked.HasValue,
                                Command = new DelegateCommand(() =>
                                {
                                    //BUG: CEF currently not executing callbacks correctly so we manually map the commands below
                                    //see https://github.com/cefsharp/CefSharp/issues/1767
                                    //The following line worked in previous versions, it doesn't now, so custom handling below
                                    //callback.Continue(item.Item2, CefEventFlags.None);
                                    ExecuteCommand(browser, new ContextMenuExecuteModel(subItem.CommandId, dictionarySuggestions, xCoord, yCoord, selectionText, misspelledWord));
                                }),
                            };

                            menuItem.Items.Add(subMenuItem);
                        }
                    }

                    menu.Items.Add(menuItem);
                }
                webBrowser.ContextMenu = menu;
            });

            return true;
        }

        protected virtual void ExecuteCommand(IBrowser browser, ContextMenuExecuteModel model)
        {
            // If the user chose a replacement word for a misspelling, replace it here.
            if (model.MenuCommand >= CefMenuCommand.SpellCheckSuggestion0 &&
                model.MenuCommand <= CefMenuCommand.SpellCheckSuggestion4)
            {
                int sugestionIndex = ((int)model.MenuCommand) - (int)CefMenuCommand.SpellCheckSuggestion0;
                if (sugestionIndex < model.DictionarySuggestions.Count)
                {
                    var suggestion = model.DictionarySuggestions[sugestionIndex];
                    browser.ReplaceMisspelling(suggestion);
                }

                return;
            }

            switch (model.MenuCommand)
            {
                // Navigation.
                case CefMenuCommand.Back:
                {
                    browser.GoBack();
                    break;
                }
                case CefMenuCommand.Forward:
                {
                    browser.GoForward();
                    break;
                }
                case CefMenuCommand.Reload:
                {
                    browser.Reload();
                    break;
                }
                case CefMenuCommand.ReloadNoCache:
                {
                    browser.Reload(ignoreCache: true);
                    break;
                }
                case CefMenuCommand.StopLoad:
                {
                    browser.StopLoad();
                    break;
                }

                //Editing
                case CefMenuCommand.Undo:
                {
                    browser.FocusedFrame.Undo();
                    break;
                }
                case CefMenuCommand.Redo:
                {
                    browser.FocusedFrame.Redo();
                    break;
                }
                case CefMenuCommand.Cut:
                {
                    browser.FocusedFrame.Cut();
                    break;
                }
                case CefMenuCommand.Copy:
                {
                    browser.FocusedFrame.Copy();
                    break;
                }
                case CefMenuCommand.Paste:
                {
                    browser.FocusedFrame.Paste();
                    break;
                }
                case CefMenuCommand.Delete:
                {
                    browser.FocusedFrame.Delete();
                    break;
                }
                case CefMenuCommand.SelectAll:
                {
                    browser.FocusedFrame.SelectAll();
                    break;
                }

                // Miscellaneous.
                case CefMenuCommand.Print:
                {
                    browser.GetHost().Print();
                    break;
                }
                case CefMenuCommand.ViewSource:
                {
                    browser.FocusedFrame.ViewSource();
                    break;
                }
                case CefMenuCommand.Find:
                {
                    browser.GetHost().Find(model.SelectionText, true, false, false);
                    break;
                }

                // Spell checking.
                case CefMenuCommand.AddToDictionary:
                {
                    browser.GetHost().AddWordToDictionary(model.MisspelledWord);
                    break;
                }

                case (CefMenuCommand)CefMenuCommandShowDevToolsId:
                {
                    browser.GetHost().ShowDevTools(inspectElementAtX: model.XCoord, inspectElementAtY: model.YCoord);
                    break;
                }
                case (CefMenuCommand)CefMenuCommandCloseDevToolsId:
                {
                    browser.GetHost().CloseDevTools();
                    break;
                }
            }
        }

        private static IList<MenuModel> GetMenuItems(IMenuModel model)
        {
            var menuItems = new List<MenuModel>();

            for (var i = 0; i < model.Count; i++)
            {
                var type = model.GetTypeAt(i);
                bool? isChecked = null;

                if(type == MenuItemType.Check)
                {
                    isChecked = model.IsCheckedAt(i);
                }

                var subItems = model.GetSubMenuAt(i);

                var subMenus = subItems == null ? null : GetMenuItems(subItems);

                var menuItem = new MenuModel
                {
                    Label = model.GetLabelAt(i),
                    CommandId = model.GetCommandIdAt(i),
                    IsEnabled = model.IsEnabledAt(i),
                    Type = type,
                    IsSeperator = type == MenuItemType.Separator,
                    IsChecked = isChecked,
                    SubMenus = subMenus
                };

                menuItems.Add(menuItem);
            }

            return menuItems;
        }

        //TODO: One class per file
        internal class MenuModel
        {
            internal string Label { get; set; }
            internal CefMenuCommand CommandId { get; set; }
            internal bool IsEnabled { get; set; }
            internal bool IsSeperator { get; set; }
            internal bool? IsChecked { get; set; }
            internal MenuItemType Type { get; set; }

            internal IList<MenuModel> SubMenus { get; set; }
        }
    }
}
