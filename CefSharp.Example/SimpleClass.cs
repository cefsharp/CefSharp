// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
namespace CefSharp.Example
{
    public class SimpleClass
    {
        public IJavascriptCallback Callback { get; set; }
        public string TestString { get; set; }

        public IList<SimpleSubClass> SubClasses { get; set; }
    }
}
