// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// SetStyleSheetTextResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetStyleSheetTextResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string sourceMapURL
        {
            get;
            set;
        }

        /// <summary>
        /// sourceMapURL
        /// </summary>
        public string SourceMapURL
        {
            get
            {
                return sourceMapURL;
            }
        }
    }
}