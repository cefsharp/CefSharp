// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// GetContentQuadsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetContentQuadsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal long[] quads
        {
            get;
            set;
        }

        /// <summary>
        /// quads
        /// </summary>
        public long[] Quads
        {
            get
            {
                return quads;
            }
        }
    }
}