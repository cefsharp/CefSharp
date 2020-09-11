// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Network
{
    /// <summary>
    /// Cookie object
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Cookie : CefSharp.DevTools.DevToolsDomainEntityBase
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
        /// Cookie domain.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("domain"), IsRequired = (true))]
        public string Domain
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie path.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("path"), IsRequired = (true))]
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie expiration date as the number of seconds since the UNIX epoch.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("expires"), IsRequired = (true))]
        public long Expires
        {
            get;
            set;
        }

        /// <summary>
        /// Cookie size.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("size"), IsRequired = (true))]
        public int Size
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is http-only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("httpOnly"), IsRequired = (true))]
        public bool HttpOnly
        {
            get;
            set;
        }

        /// <summary>
        /// True if cookie is secure.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("secure"), IsRequired = (true))]
        public bool Secure
        {
            get;
            set;
        }

        /// <summary>
        /// True in case of session cookie.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("session"), IsRequired = (true))]
        public bool Session
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

        public CefSharp.DevTools.Network.CookiePriority Priority
        {
            get
            {
                return (CefSharp.DevTools.Network.CookiePriority)(StringToEnum(typeof(CefSharp.DevTools.Network.CookiePriority), priority));
            }

            set
            {
                priority = (EnumToString(value));
            }
        }

        /// <summary>
        /// Cookie Priority
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("priority"), IsRequired = (true))]
        internal string priority
        {
            get;
            set;
        }
    }
}