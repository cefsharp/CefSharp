// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// Security state information about the page.
    /// </summary>
    public class VisibleSecurityState
    {
        /// <summary>
        /// The security level of the page.
        /// </summary>
        public string SecurityState
        {
            get;
            set;
        }

        /// <summary>
        /// Security state details about the page certificate.
        /// </summary>
        public CertificateSecurityState CertificateSecurityState
        {
            get;
            set;
        }

        /// <summary>
        /// The type of Safety Tip triggered on the page. Note that this field will be set even if the Safety Tip UI was not actually shown.
        /// </summary>
        public SafetyTipInfo SafetyTipInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Array of security state issues ids.
        /// </summary>
        public string SecurityStateIssueIds
        {
            get;
            set;
        }
    }
}