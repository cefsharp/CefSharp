// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Storage
{
    /// <summary>
    /// Usage for a storage type.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class UsageForType : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        public CefSharp.DevTools.Storage.StorageType StorageType
        {
            get
            {
                return (CefSharp.DevTools.Storage.StorageType)(StringToEnum(typeof(CefSharp.DevTools.Storage.StorageType), storageType));
            }

            set
            {
                storageType = (EnumToString(value));
            }
        }

        /// <summary>
        /// Name of storage type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("storageType"), IsRequired = (true))]
        internal string storageType
        {
            get;
            set;
        }

        /// <summary>
        /// Storage usage (bytes).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("usage"), IsRequired = (true))]
        public long Usage
        {
            get;
            set;
        }
    }
}