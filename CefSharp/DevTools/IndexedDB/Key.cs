// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// Key.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Key : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Key type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Number value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("number"), IsRequired = (false))]
        public long? Number
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
        /// Date value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("date"), IsRequired = (false))]
        public long? Date
        {
            get;
            set;
        }

        /// <summary>
        /// Array value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("array"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.IndexedDB.Key> Array
        {
            get;
            set;
        }
    }
}