// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CefSharp.Example
{
    public struct JsObject 
    {
        public string Value;
    }

    public class AsyncBoundObject
    {
        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public void Error()
        {
            throw new Exception("This is an exception coming from C#");
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public int Div(int divident, int divisor)
        {
            return divident / divisor;
        }

        public string Hello(string name)
        {
            return "Hello " + name;
        }

        public string DoSomething()
        {
            Thread.Sleep(1000);

            return "Waited for 1000ms before returning";
        }

        public JsObject ReturnObject(string name)
        {
            return new JsObject
            {
                Value = name
            };
        }

        public JsObject[] ObjectArray(string name)
        {
            return new[] 
            {
                new JsObject() { Value = "Item1" },
                new JsObject() { Value = "Item2" }
            };
        }

        public string DynamiObjectList(IList<dynamic> objects)
        {
            var builder = new StringBuilder();

            foreach(var browser in objects)
            {
                builder.Append("Browser(Name:" + browser.Name + ";Engine:" + browser.Engine.Name + ");");
            }

            return builder.ToString();
        }
    }
}
