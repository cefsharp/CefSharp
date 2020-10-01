// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Fetch
{
    /// <summary>
    /// Response HTTP header entry
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class HeaderEntry : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Name
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Value
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public string Value
        {
            get;
            set;
        }
    }
}