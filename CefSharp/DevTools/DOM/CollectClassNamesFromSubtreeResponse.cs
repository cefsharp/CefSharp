// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// CollectClassNamesFromSubtreeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CollectClassNamesFromSubtreeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] classNames
        {
            get;
            set;
        }

        /// <summary>
        /// classNames
        /// </summary>
        public string[] ClassNames
        {
            get
            {
                return classNames;
            }
        }
    }
}