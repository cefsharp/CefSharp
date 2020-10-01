// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// AddScriptToEvaluateOnNewDocumentResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AddScriptToEvaluateOnNewDocumentResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string identifier
        {
            get;
            set;
        }

        /// <summary>
        /// identifier
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }
    }
}