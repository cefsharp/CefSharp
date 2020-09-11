// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// An explanation of an factor contributing to the security state.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class SecurityStateExplanation : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        public CefSharp.DevTools.Security.SecurityState SecurityState
        {
            get
            {
                return (CefSharp.DevTools.Security.SecurityState)(StringToEnum(typeof(CefSharp.DevTools.Security.SecurityState), securityState));
            }

            set
            {
                securityState = (EnumToString(value));
            }
        }

        /// <summary>
        /// Security state representing the severity of the factor being explained.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("securityState"), IsRequired = (true))]
        internal string securityState
        {
            get;
            set;
        }

        /// <summary>
        /// Title describing the type of factor.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("title"), IsRequired = (true))]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Short phrase describing the type of factor.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("summary"), IsRequired = (true))]
        public string Summary
        {
            get;
            set;
        }

        /// <summary>
        /// Full text explanation of the factor.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("description"), IsRequired = (true))]
        public string Description
        {
            get;
            set;
        }

        public CefSharp.DevTools.Security.MixedContentType MixedContentType
        {
            get
            {
                return (CefSharp.DevTools.Security.MixedContentType)(StringToEnum(typeof(CefSharp.DevTools.Security.MixedContentType), mixedContentType));
            }

            set
            {
                mixedContentType = (EnumToString(value));
            }
        }

        /// <summary>
        /// The type of mixed content described by the explanation.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mixedContentType"), IsRequired = (true))]
        internal string mixedContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Page certificate.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificate"), IsRequired = (true))]
        public string[] Certificate
        {
            get;
            set;
        }

        /// <summary>
        /// Recommendations to fix any issues.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("recommendations"), IsRequired = (false))]
        public string[] Recommendations
        {
            get;
            set;
        }
    }
}