// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    [DataContract]
    public class JavascriptRootObject
    {
        [DataMember]
        public List<JavascriptObject> MemberObjects { get; set; }

        public JavascriptRootObject()
        {
            MemberObjects = new List<JavascriptObject>();
        }
    }
}
