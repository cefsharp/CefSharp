// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Schema
{
    /// <summary>
    /// Description of the protocol domain.
    /// </summary>
    public class Domain : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Domain name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Domain version.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("version"), IsRequired = (true))]
        public string Version
        {
            get;
            set;
        }
    }
}