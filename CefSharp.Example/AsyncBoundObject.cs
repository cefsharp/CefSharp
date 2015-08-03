// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading;

namespace CefSharp.Example
{
    public class AsyncBoundObject
    {
        public void Error()
        {
            throw new Exception("This is an exception coming from C#");
        }

        public int Div(int divident, int divisor)
        {
            return divident / divisor;
        }

        public string Hello(string name)
        {
            return "Hello " + name;
        }

        public void DoSomething()
        {
            Thread.Sleep(1000);
        }
    }
}
