// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Request pattern for interception.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RequestPattern : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Wildcards ('*' -> zero or more, '?' -> exactly one) are allowed. Escape character is
        /// backslash. Omitting is equivalent to "*".
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("urlPattern"), IsRequired = (false))]
        public string UrlPattern
        {
            get;
            set;
        }

        public CefSharp.DevTools.Network.ResourceType? ResourceType
        {
            get
            {
                return (CefSharp.DevTools.Network.ResourceType? )(StringToEnum(typeof(CefSharp.DevTools.Network.ResourceType? ), resourceType));
            }

            set
            {
                resourceType = (EnumToString(value));
            }
        }

        /// <summary>
        /// If set, only requests for matching resource types will be intercepted.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("resourceType"), IsRequired = (false))]
        internal string resourceType
        {
            get;
            set;
        }

        public CefSharp.DevTools.Network.InterceptionStage? InterceptionStage
        {
            get
            {
                return (CefSharp.DevTools.Network.InterceptionStage? )(StringToEnum(typeof(CefSharp.DevTools.Network.InterceptionStage? ), interceptionStage));
            }

            set
            {
                interceptionStage = (EnumToString(value));
            }
        }

        /// <summary>
        /// Stage at wich to begin intercepting requests. Default is Request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("interceptionStage"), IsRequired = (false))]
        internal string interceptionStage
        {
            get;
            set;
        }
    }
}