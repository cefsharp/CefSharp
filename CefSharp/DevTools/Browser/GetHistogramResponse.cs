// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// GetHistogramResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetHistogramResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Browser.Histogram histogram
        {
            get;
            set;
        }

        /// <summary>
        /// histogram
        /// </summary>
        public CefSharp.DevTools.Browser.Histogram Histogram
        {
            get
            {
                return histogram;
            }
        }
    }
}