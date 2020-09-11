// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// GetBrowserCommandLineResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetBrowserCommandLineResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] arguments
        {
            get;
            set;
        }

        /// <summary>
        /// arguments
        /// </summary>
        public string[] Arguments
        {
            get
            {
                return arguments;
            }
        }
    }
}