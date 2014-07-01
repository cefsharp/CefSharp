// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    public class ScriptException : Exception
    {
        public ScriptException()
        {
        }

        public ScriptException(string message)
            : base(message)
        {
        }
    };
}
