﻿// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class OnPluginCrashedEventArgs : BaseRequestEventArgs
    {
        public OnPluginCrashedEventArgs(IWebBrowser browserControl, IBrowser browser, string pluginPath) : base(browserControl, browser)
        {
            PluginPath = pluginPath;
        }

        public string PluginPath { get; private set; }
    }
}
