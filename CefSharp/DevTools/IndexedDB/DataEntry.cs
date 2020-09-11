// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// Data entry.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class DataEntry : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Key object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("key"), IsRequired = (true))]
        public CefSharp.DevTools.Runtime.RemoteObject Key
        {
            get;
            set;
        }

        /// <summary>
        /// Primary key object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("primaryKey"), IsRequired = (true))]
        public CefSharp.DevTools.Runtime.RemoteObject PrimaryKey
        {
            get;
            set;
        }

        /// <summary>
        /// Value object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public CefSharp.DevTools.Runtime.RemoteObject Value
        {
            get;
            set;
        }
    }
}