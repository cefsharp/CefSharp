// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// HTTP request data.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Request : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Request URL (without fragment).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Fragment of the requested URL starting with hash, if present.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("urlFragment"), IsRequired = (false))]
        public string UrlFragment
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request method.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("method"), IsRequired = (true))]
        public string Method
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP request headers.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("headers"), IsRequired = (true))]
        public CefSharp.DevTools.Network.Headers Headers
        {
            get;
            set;
        }

        /// <summary>
        /// HTTP POST request data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("postData"), IsRequired = (false))]
        public string PostData
        {
            get;
            set;
        }

        /// <summary>
        /// True when the request has POST data. Note that postData might still be omitted when this flag is true when the data is too long.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("hasPostData"), IsRequired = (false))]
        public bool? HasPostData
        {
            get;
            set;
        }

        /// <summary>
        /// The mixed content type of the request.
        /// </summary>
        public CefSharp.DevTools.Security.MixedContentType? MixedContentType
        {
            get
            {
                return (CefSharp.DevTools.Security.MixedContentType? )(StringToEnum(typeof(CefSharp.DevTools.Security.MixedContentType? ), mixedContentType));
            }

            set
            {
                mixedContentType = (EnumToString(value));
            }
        }

        /// <summary>
        /// The mixed content type of the request.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mixedContentType"), IsRequired = (false))]
        internal string mixedContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Priority of the resource request at the time request is sent.
        /// </summary>
        public CefSharp.DevTools.Network.ResourcePriority InitialPriority
        {
            get
            {
                return (CefSharp.DevTools.Network.ResourcePriority)(StringToEnum(typeof(CefSharp.DevTools.Network.ResourcePriority), initialPriority));
            }

            set
            {
                initialPriority = (EnumToString(value));
            }
        }

        /// <summary>
        /// Priority of the resource request at the time request is sent.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("initialPriority"), IsRequired = (true))]
        internal string initialPriority
        {
            get;
            set;
        }

        /// <summary>
        /// The referrer policy of the request, as defined in https://www.w3.org/TR/referrer-policy/
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("referrerPolicy"), IsRequired = (true))]
        public string ReferrerPolicy
        {
            get;
            set;
        }

        /// <summary>
        /// Whether is loaded via link preload.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isLinkPreload"), IsRequired = (false))]
        public bool? IsLinkPreload
        {
            get;
            set;
        }
    }
}