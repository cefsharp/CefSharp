// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// GlobalLexicalScopeNamesResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GlobalLexicalScopeNamesResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] names
        {
            get;
            set;
        }

        /// <summary>
        /// names
        /// </summary>
        public string[] Names
        {
            get
            {
                return names;
            }
        }
    }
}