// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeapProfiler
{
    /// <summary>
    /// GetHeapObjectIdResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetHeapObjectIdResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string heapSnapshotObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// heapSnapshotObjectId
        /// </summary>
        public string HeapSnapshotObjectId
        {
            get
            {
                return heapSnapshotObjectId;
            }
        }
    }
}