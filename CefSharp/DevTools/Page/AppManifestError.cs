// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Error while paring app manifest.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AppManifestError : CefSharp.DevTools.DevToolsDomainEntityBase
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
        /// If criticial, this is a non-recoverable parse error.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("critical"), IsRequired = (true))]
        public int Critical
        {
            get;
            set;
        }

        /// <summary>
        /// Error line.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("line"), IsRequired = (true))]
        public int Line
        {
            get;
            set;
        }

        /// <summary>
        /// Error column.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("column"), IsRequired = (true))]
        public int Column
        {
            get;
            set;
        }
    }
}