// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.LayerTree
{
    /// <summary>
    /// MakeSnapshotResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class MakeSnapshotResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string snapshotId
        {
            get;
            set;
        }

        /// <summary>
        /// snapshotId
        /// </summary>
        public string SnapshotId
        {
            get
            {
                return snapshotId;
            }
        }
    }
}