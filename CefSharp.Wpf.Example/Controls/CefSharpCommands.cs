﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System.Windows.Input;

namespace CefSharp.Wpf.Example.Controls
{
    public static class CefSharpCommands
    {
        public static RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(CefSharpCommands));
        public static RoutedUICommand OpenTabBindingTest = new RoutedUICommand("OpenTabBindingTest", "OpenTabBindingTest", typeof(CefSharpCommands));
        public static RoutedUICommand OpenTabPlugins = new RoutedUICommand("OpenTabPlugins", "OpenTabPlugins", typeof(CefSharpCommands));
        public static RoutedUICommand OpenPopupTest = new RoutedUICommand("OpenPopupTest", "OpenPopupTest", typeof(CefSharpCommands));
    }
}
