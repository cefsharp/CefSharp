// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Runtime.Serialization;

namespace CefSharp.DevTools
{
    /// <summary>
    /// Error Message parsed from JSON
    /// e.g. {"code":-32601,"message":"'Browser.getWindowForTarget' wasn't found"}
    /// </summary>
    public class DevToolsDomainErrorResponse
    {
        /// <summary>
        /// Message Id
        /// </summary>
        [IgnoreDataMember]
        public int MessageId { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        [DataMember(Name = "code", IsRequired = true)]
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        [DataMember(Name = "message", IsRequired = true)]
        public string Message
        {
            get;
            set;
        }
    }
}
