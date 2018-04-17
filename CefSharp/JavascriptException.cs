// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Runtime.Serialization;

namespace CefSharp
{
    public class JavascriptException
    {
        public string Message { get; set; }

        public JavascriptStackFrame[] StackTrace { get; set; }
    }
}
