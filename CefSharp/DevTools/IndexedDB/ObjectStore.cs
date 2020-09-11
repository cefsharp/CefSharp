// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// Object store.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ObjectStore : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Object store name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Object store key path.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("keyPath"), IsRequired = (true))]
        public CefSharp.DevTools.IndexedDB.KeyPath KeyPath
        {
            get;
            set;
        }

        /// <summary>
        /// If true, object store has auto increment flag set.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("autoIncrement"), IsRequired = (true))]
        public bool AutoIncrement
        {
            get;
            set;
        }

        /// <summary>
        /// Indexes in this object store.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("indexes"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.IndexedDB.ObjectStoreIndex> Indexes
        {
            get;
            set;
        }
    }
}