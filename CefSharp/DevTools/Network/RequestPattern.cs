// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Request pattern for interception.
    /// </summary>
    public class RequestPattern : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Wildcards ('*' -> zero or more, '?' -> exactly one) are allowed. Escape character is
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("urlPattern"), IsRequired = (false))]
        public string UrlPattern
        {
            get;
            set;
        }

        /// <summary>
        /// If set, only requests for matching resource types will be intercepted.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("resourceType"), IsRequired = (false))]
        public string ResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// Stage at wich to begin intercepting requests. Default is Request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("interceptionStage"), IsRequired = (false))]
        public string InterceptionStage
        {
            get;
            set;
        }
    }
}