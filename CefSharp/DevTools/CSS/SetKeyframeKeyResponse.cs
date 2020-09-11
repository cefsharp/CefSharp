// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// SetKeyframeKeyResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SetKeyframeKeyResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.CSS.Value keyText
        {
            get;
            set;
        }

        /// <summary>
        /// keyText
        /// </summary>
        public CefSharp.DevTools.CSS.Value KeyText
        {
            get
            {
                return keyText;
            }
        }
    }
}