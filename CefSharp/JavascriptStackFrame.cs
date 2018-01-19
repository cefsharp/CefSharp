// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Runtime.Serialization;

namespace CefSharp
{
    [DataContract]
    public class JavascriptStackFrame
    {
        [DataMember]
        public string FunctionName { get; set; }

        [DataMember]
        public int LineNumber { get; set; }

        [DataMember]
        public string SourceName { get; set; }
    }
}
