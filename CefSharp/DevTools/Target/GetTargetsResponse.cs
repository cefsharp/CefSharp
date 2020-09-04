// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Target
{
    /// <summary>
    /// GetTargetsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetTargetsResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<TargetInfo> targetInfos
        {
            get;
            set;
        }

        /// <summary>
        /// The list of targets.
        /// </summary>
        public System.Collections.Generic.IList<TargetInfo> TargetInfos
        {
            get
            {
                return targetInfos;
            }
        }
    }
}