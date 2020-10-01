// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetInstallabilityErrorsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetInstallabilityErrorsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.Page.InstallabilityError> installabilityErrors
        {
            get;
            set;
        }

        /// <summary>
        /// installabilityErrors
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.Page.InstallabilityError> InstallabilityErrors
        {
            get
            {
                return installabilityErrors;
            }
        }
    }
}