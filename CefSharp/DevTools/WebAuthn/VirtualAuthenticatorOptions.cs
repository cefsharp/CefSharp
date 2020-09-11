// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAuthn
{
    /// <summary>
    /// VirtualAuthenticatorOptions
    /// </summary>
    public class VirtualAuthenticatorOptions : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Protocol
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("protocol"), IsRequired = (true))]
        public CefSharp.DevTools.WebAuthn.AuthenticatorProtocol Protocol
        {
            get;
            set;
        }

        /// <summary>
        /// Transport
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("transport"), IsRequired = (true))]
        public CefSharp.DevTools.WebAuthn.AuthenticatorTransport Transport
        {
            get;
            set;
        }

        /// <summary>
        /// Defaults to false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("hasResidentKey"), IsRequired = (false))]
        public bool? HasResidentKey
        {
            get;
            set;
        }

        /// <summary>
        /// Defaults to false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("hasUserVerification"), IsRequired = (false))]
        public bool? HasUserVerification
        {
            get;
            set;
        }

        /// <summary>
        /// If set to true, tests of user presence will succeed immediately.
        /// Otherwise, they will not be resolved. Defaults to true.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("automaticPresenceSimulation"), IsRequired = (false))]
        public bool? AutomaticPresenceSimulation
        {
            get;
            set;
        }

        /// <summary>
        /// Sets whether User Verification succeeds or fails for an authenticator.
        /// Defaults to false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isUserVerified"), IsRequired = (false))]
        public bool? IsUserVerified
        {
            get;
            set;
        }
    }
}