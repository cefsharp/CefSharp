// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Fetch
{
    /// <summary>
    /// Response to an AuthChallenge.
    /// </summary>
    public class AuthChallengeResponse : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The decision on what to do in response to the authorization challenge.  Default means
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("response"), IsRequired = (true))]
        public string Response
        {
            get;
            set;
        }

        /// <summary>
        /// The username to provide, possibly empty. Should only be set if response is
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("username"), IsRequired = (false))]
        public string Username
        {
            get;
            set;
        }

        /// <summary>
        /// The password to provide, possibly empty. Should only be set if response is
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("password"), IsRequired = (false))]
        public string Password
        {
            get;
            set;
        }
    }
}