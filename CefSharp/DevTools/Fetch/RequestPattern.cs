// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Fetch
{
    /// <summary>
    /// RequestPattern
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

        /// <summary>
        /// If set, only requests for matching resource types will be intercepted.
        /// </summary>
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

        /// <summary>
        /// Stage at wich to begin intercepting requests. Default is Request.
        /// </summary>
        public CefSharp.DevTools.Fetch.RequestStage? RequestStage
        {
            get
            {
                return (CefSharp.DevTools.Fetch.RequestStage? )(StringToEnum(typeof(CefSharp.DevTools.Fetch.RequestStage? ), requestStage));
            }

            set
            {
                requestStage = (EnumToString(value));
            }
        }

        /// <summary>
        /// Stage at wich to begin intercepting requests. Default is Request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("requestStage"), IsRequired = (false))]
        internal string requestStage
        {
            get;
            set;
        }
    }
}