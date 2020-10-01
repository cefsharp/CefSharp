// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CreateStyleSheetResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CreateStyleSheetResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string styleSheetId
        {
            get;
            set;
        }

        /// <summary>
        /// styleSheetId
        /// </summary>
        public string StyleSheetId
        {
            get
            {
                return styleSheetId;
            }
        }
    }
}