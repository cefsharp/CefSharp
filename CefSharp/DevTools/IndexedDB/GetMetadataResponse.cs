// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.IndexedDB
{
    /// <summary>
    /// GetMetadataResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetMetadataResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal long entriesCount
        {
            get;
            set;
        }

        /// <summary>
        /// the entries count
        /// </summary>
        public long EntriesCount
        {
            get
            {
                return entriesCount;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal long keyGeneratorValue
        {
            get;
            set;
        }

        /// <summary>
        /// the current value of key generator, to become the next inserted
        public long KeyGeneratorValue
        {
            get
            {
                return keyGeneratorValue;
            }
        }
    }
}