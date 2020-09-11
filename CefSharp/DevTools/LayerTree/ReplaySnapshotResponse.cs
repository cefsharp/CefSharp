// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.LayerTree
{
    /// <summary>
    /// ReplaySnapshotResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ReplaySnapshotResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string dataURL
        {
            get;
            set;
        }

        /// <summary>
        /// dataURL
        /// </summary>
        public string DataURL
        {
            get
            {
                return dataURL;
            }
        }
    }
}