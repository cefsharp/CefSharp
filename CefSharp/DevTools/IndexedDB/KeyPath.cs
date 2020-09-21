// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// Key path.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class KeyPath : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Key path type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// String value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("string"), IsRequired = (false))]
        public string String
        {
            get;
            set;
        }

        /// <summary>
        /// Array value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("array"), IsRequired = (false))]
        public string[] Array
        {
            get;
            set;
        }
    }
}