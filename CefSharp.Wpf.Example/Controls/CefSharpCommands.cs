// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System.Windows.Input;

namespace CefSharp.Wpf.Example.Controls
{
    public static class CefSharpCommands
    {
        public static RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(CefSharpCommands));
        public static RoutedUICommand OpenTabCommand = new RoutedUICommand("OpenTabCommand", "OpenTabCommand", typeof(CefSharpCommands));
        public static RoutedUICommand PrintTabToPdfCommand = new RoutedUICommand("PrintTabToPdfCommand", "PrintTabToPdfCommand", typeof(CefSharpCommands));
        public static RoutedUICommand CustomCommand = new RoutedUICommand("CustomCommand", "CustomCommand", typeof(CefSharpCommands));
    }
}
