// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// GetHistogramsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetHistogramsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Browser.Histogram> histograms
        {
            get;
            set;
        }

        /// <summary>
        /// histograms
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Browser.Histogram> Histograms
        {
            get
            {
                return histograms;
            }
        }
    }
}