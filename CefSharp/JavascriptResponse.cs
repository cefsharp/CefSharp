// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Runtime.Serialization;
using CefSharp.Internals;

namespace CefSharp
{
    /// <summary>
    /// Javascript Response
    /// </summary>
    [DataContract]
    [KnownType(typeof(object[]))]
    [KnownType(typeof(JavascriptCallback))]
    [KnownType(typeof(Dictionary<string, object>))]
    public class JavascriptResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Was the javascript executed successfully
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        /// Javascript response
        /// </summary>
        [DataMember]
        public object Result { get; set; }
    }
}
