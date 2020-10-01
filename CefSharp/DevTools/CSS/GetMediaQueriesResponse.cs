// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// GetMediaQueriesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetMediaQueriesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSMedia> medias
        {
            get;
            set;
        }

        /// <summary>
        /// medias
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSMedia> Medias
        {
            get
            {
                return medias;
            }
        }
    }
}