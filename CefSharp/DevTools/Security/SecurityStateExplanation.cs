// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// An explanation of an factor contributing to the security state.
    /// </summary>
    public class SecurityStateExplanation : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Security state representing the severity of the factor being explained.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("securityState"), IsRequired = (true))]
        public string SecurityState
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

        /// <summary>
        /// The type of mixed content described by the explanation.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("mixedContentType"), IsRequired = (true))]
        public string MixedContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Page certificate.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("certificate"), IsRequired = (true))]
        public string Certificate
        {
            get;
            set;
        }

        /// <summary>
        /// Recommendations to fix any issues.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("recommendations"), IsRequired = (false))]
        public string Recommendations
        {
            get;
            set;
        }
    }
}