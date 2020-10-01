// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Cookie parameter object
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CookieParam : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Cookie name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// The request-URI to associate with the setting of the cookie. This value can affect the
        /// default domain and path values of the created cookie.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (false))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie domain.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("domain"), IsRequired = (false))]
        public string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie path.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("path"), IsRequired = (false))]
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is secure.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("secure"), IsRequired = (false))]
        public bool? Secure
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is http-only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("httpOnly"), IsRequired = (false))]
        public bool? HttpOnly
        {
            get;
            set;
        }

        public CefSharp.DevTools.Network.CookieSameSite? SameSite
        {
            get
            {
                return (CefSharp.DevTools.Network.CookieSameSite? )(StringToEnum(typeof(CefSharp.DevTools.Network.CookieSameSite? ), sameSite));
            }

            set
            {
                sameSite = (EnumToString(value));
            }
        }

        /// <summary>
        /// Cookie SameSite type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sameSite"), IsRequired = (false))]
        internal string sameSite
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie expiration date, session cookie if not set
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("expires"), IsRequired = (false))]
        public long? Expires
        {
            get;
            set;
        }

        public CefSharp.DevTools.Network.CookiePriority? Priority
        {
            get
            {
                return (CefSharp.DevTools.Network.CookiePriority? )(StringToEnum(typeof(CefSharp.DevTools.Network.CookiePriority? ), priority));
            }

            set
            {
                priority = (EnumToString(value));
            }
        }

        /// <summary>
        /// Cookie Priority.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("priority"), IsRequired = (false))]
        internal string priority
        {
            get;
            set;
        }
    }
}