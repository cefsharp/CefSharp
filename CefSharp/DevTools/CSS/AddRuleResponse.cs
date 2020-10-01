// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// AddRuleResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AddRuleResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.CSS.CSSRule rule
        {
            get;
            set;
        }

        /// <summary>
        /// rule
        /// </summary>
        public CefSharp.DevTools.CSS.CSSRule Rule
        {
            get
            {
                return rule;
            }
        }
    }
}