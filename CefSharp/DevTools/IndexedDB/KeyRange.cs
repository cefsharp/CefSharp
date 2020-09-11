// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// Key range.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class KeyRange : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Lower bound.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lower"), IsRequired = (false))]
        public CefSharp.DevTools.IndexedDB.Key Lower
        {
            get;
            set;
        }

        /// <summary>
        /// Upper bound.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("upper"), IsRequired = (false))]
        public CefSharp.DevTools.IndexedDB.Key Upper
        {
            get;
            set;
        }

        /// <summary>
        /// If true lower bound is open.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lowerOpen"), IsRequired = (true))]
        public bool LowerOpen
        {
            get;
            set;
        }

        /// <summary>
        /// If true upper bound is open.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("upperOpen"), IsRequired = (true))]
        public bool UpperOpen
        {
            get;
            set;
        }
    }
}