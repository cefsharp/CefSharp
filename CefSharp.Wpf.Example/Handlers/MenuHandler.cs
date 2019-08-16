// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;

namespace CefSharp.Wpf.Example.Handlers
{
    public class MenuHandler : IContextMenuHandler
    {
        void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            if (model.Count > 0)
            {
                model.AddSeparator();
            }

            model.AddItem((CefMenuCommand)26501, "Show DevTools");
            model.AddItem((CefMenuCommand)26502, "Close DevTools");


            //To disable context mode then clear
            // model.Clear();
        }

        bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
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

        void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            webBrowser.Dispatcher.Invoke(() =>
            {
                webBrowser.ContextMenu = null;
            });
        }

        bool IContextMenuHandler.RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            //NOTE: Return false to use the built in Context menu - in WPF this requires you integrate into your existing message loop, read the General Usage Guide for more details
            //https://github.com/cefsharp/CefSharp/wiki/General-Usage#multithreadedmessageloop
            //return false;

            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            //IMenuModel is only valid in the context of this method, so need to read the values before invoking on the UI thread
            var menuItems = GetMenuItems(model).ToList();

            webBrowser.Dispatcher.Invoke(() =>
            {
                var menu = new ContextMenu
                {
                    IsOpen = true
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
                    if (item.Item2 == CefMenuCommand.NotFound && string.IsNullOrWhiteSpace(item.Item1))
                    {
                        menu.Items.Add(new Separator());
                        continue;
                    }

                    menu.Items.Add(new MenuItem
                    {
                        Header = item.Item1.Replace("&", "_"),
                        IsEnabled = item.Item3,
                        Command = new RelayCommand(() =>
                        {
                            //BUG: CEF currently not executing callbacks correctly so we manually map the commands below
                            //see https://github.com/cefsharp/CefSharp/issues/1767
                            //The following line worked in previous versions, it doesn't now, so custom EXAMPLE below
                            //callback.Continue(item.Item2, CefEventFlags.None);

                            //NOTE: Note all menu item options below have been tested, you can work out the rest
                            switch (item.Item2)
                            {
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
                                case CefMenuCommand.Undo:
                                {
                                    browser.FocusedFrame.Undo();
                                    break;
                                }
                                case CefMenuCommand.StopLoad:
                                {
                                    browser.StopLoad();
                                    break;
                                }
                                case CefMenuCommand.SelectAll:
                                {
                                    browser.FocusedFrame.SelectAll();
                                    break;
                                }
                                case CefMenuCommand.Redo:
                                {
                                    browser.FocusedFrame.Redo();
                                    break;
                                }
                                case CefMenuCommand.Find:
                                {
                                    browser.GetHost().Find(0, parameters.SelectionText, true, false, false);
                                    break;
                                }
                                case CefMenuCommand.AddToDictionary:
                                {
                                    browser.GetHost().AddWordToDictionary(parameters.MisspelledWord);
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
                                case (CefMenuCommand)26501:
                                {
                                    browser.GetHost().ShowDevTools();
                                    break;
                                }
                                case (CefMenuCommand)26502:
                                {
                                    browser.GetHost().CloseDevTools();
                                    break;
                                }
                            }
                        }, keepTargetAlive: true)
                    });
                }
                webBrowser.ContextMenu = menu;
            });

            return true;
        }

        private static IEnumerable<Tuple<string, CefMenuCommand, bool>> GetMenuItems(IMenuModel model)
        {
            for (var i = 0; i < model.Count; i++)
            {
                var header = model.GetLabelAt(i);
                var commandId = model.GetCommandIdAt(i);
                var isEnabled = model.IsEnabledAt(i);
                yield return new Tuple<string, CefMenuCommand, bool>(header, commandId, isEnabled);
            }
        }
    }
}
