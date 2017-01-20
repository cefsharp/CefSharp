﻿// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Example.RequestEventHandler
{
    public class OnProtocolExecutionEventArgs : BaseRequestEventArgs
    {
        public OnProtocolExecutionEventArgs(IWebBrowser browserControl, IBrowser browser, string url) : base(browserControl, browser)
        {
            Url = url;
        }

        public string Url { get; }

        /// <summary>
        ///     Set to true to attempt execution via the registered OS protocol handler, if any. Otherwise set to false.
        /// </summary>
        public bool AttemptExecution { get; set; } = false;
    }
}
