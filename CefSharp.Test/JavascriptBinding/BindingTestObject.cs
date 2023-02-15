// Copyright © 2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Test.JavascriptBinding
{
    internal class BindingTestObject
    {
        public int EchoMethodCallCount { get; private set; }
        public string Echo(string arg)
        {
            EchoMethodCallCount++;

            return arg;
        }
    }
}
