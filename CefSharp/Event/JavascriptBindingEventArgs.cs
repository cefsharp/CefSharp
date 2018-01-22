// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp;
using System;
using System.Collections.Generic;

namespace CefSharp.Event
{
    public class JavascriptBindingEventArgs : EventArgs
    {
        public IJavascriptObjectRepository ObjectRepository { get; private set; }
        public string ObjectName { get; private set; }

        public JavascriptBindingEventArgs(IJavascriptObjectRepository objectRepository, string name)
        {
            ObjectRepository = objectRepository;
            ObjectName = name;
        }
    }
}
