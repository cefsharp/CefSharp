// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// GetStyleSheetTextResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetStyleSheetTextResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string text
        {
            get;
            set;
        }

        /// <summary>
        /// text
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
        }
    }
}