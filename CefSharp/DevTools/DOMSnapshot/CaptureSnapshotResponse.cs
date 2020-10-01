// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOMSnapshot
{
    /// <summary>
    /// CaptureSnapshotResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CaptureSnapshotResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.DOMSnapshot.DocumentSnapshot> documents
        {
            get;
            set;
        }

        /// <summary>
        /// documents
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.DOMSnapshot.DocumentSnapshot> Documents
        {
            get
            {
                return documents;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] strings
        {
            get;
            set;
        }

        /// <summary>
        /// strings
        /// </summary>
        public string[] Strings
        {
            get
            {
                return strings;
            }
        }
    }
}