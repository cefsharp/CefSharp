// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Database
{
    /// <summary>
    /// Database error.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Error : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Error message.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("message"), IsRequired = (true))]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Error code.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("code"), IsRequired = (true))]
        public int Code
        {
            get;
            set;
        }
    }
}