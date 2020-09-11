// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.LayerTree
{
    /// <summary>
    /// SnapshotCommandLogResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SnapshotCommandLogResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<object> commandLog
        {
            get;
            set;
        }

        /// <summary>
        /// commandLog
        /// </summary>
        public System.Collections.Generic.IList<object> CommandLog
        {
            get
            {
                return commandLog;
            }
        }
    }
}