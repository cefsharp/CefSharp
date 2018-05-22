// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace CefSharp.Example
{
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

        public JsSerializableStruct ReturnObject(string name)
        {
            return new JsSerializableStruct
            {
                Value = name
            };
        }

        public JsSerializableClass ReturnClass(string name)
        {
            return new JsSerializableClass
            {
                Value = name
            };
        }

        public JsSerializableStruct[] ReturnStructArray(string name)
        {
            return new[] 
            {
                new JsSerializableStruct { Value = name + "Item1" },
                new JsSerializableStruct { Value = name + "Item2" }
            };
        }

        public JsSerializableClass[] ReturnClassesArray(string name)
        {
            return new[]
            {
                new JsSerializableClass { Value = name + "Item1" },
                new JsSerializableClass { Value = name + "Item2" }
            };
        }

        public string[] EchoArray(string[] arg) 
        {
            return arg;
        }

        public int[] EchoValueTypeArray(int[] arg) 
        {
            return arg;
        }

        public int[][] EchoMultidimensionalArray(int[][] arg) 
        {
            return arg;
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

        public Dictionary<string, int> MethodReturnsDictionary1()
        {
            return new Dictionary<string, int>()
            {
                {"five", 5},
                {"ten", 10}
            };
        }

        public Dictionary<string, object> MethodReturnsDictionary2()
        {
            return new Dictionary<string, object>()
            {
                {"onepointfive", 1.5},
                {"five", 5},
                {"ten", "ten"},
                {"twotwo", new Int32[]{2, 2} }
            };
        }

        public Dictionary<string, IDictionary> MethodReturnsDictionary3()
        {
            return new Dictionary<string, IDictionary>()
            {
                {"data", MethodReturnsDictionary2()}
            };
        }
    }
}
