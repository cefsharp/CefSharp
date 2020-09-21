// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Security
{
    /// <summary>
    /// Information about insecure content on the page.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class InsecureContentStatus : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Always false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ranMixedContent"), IsRequired = (true))]
        public bool RanMixedContent
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("displayedMixedContent"), IsRequired = (true))]
        public bool DisplayedMixedContent
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("containedMixedForm"), IsRequired = (true))]
        public bool ContainedMixedForm
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ranContentWithCertErrors"), IsRequired = (true))]
        public bool RanContentWithCertErrors
        {
            get;
            set;
        }

        /// <summary>
        /// Always false.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("displayedContentWithCertErrors"), IsRequired = (true))]
        public bool DisplayedContentWithCertErrors
        {
            get;
            set;
        }

        public CefSharp.DevTools.Security.SecurityState RanInsecureContentStyle
        {
            get
            {
                return (CefSharp.DevTools.Security.SecurityState)(StringToEnum(typeof(CefSharp.DevTools.Security.SecurityState), ranInsecureContentStyle));
            }

            set
            {
                ranInsecureContentStyle = (EnumToString(value));
            }
        }

        /// <summary>
        /// Always set to unknown.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("ranInsecureContentStyle"), IsRequired = (true))]
        internal string ranInsecureContentStyle
        {
            get;
            set;
        }

        public CefSharp.DevTools.Security.SecurityState DisplayedInsecureContentStyle
        {
            get
            {
                return (CefSharp.DevTools.Security.SecurityState)(StringToEnum(typeof(CefSharp.DevTools.Security.SecurityState), displayedInsecureContentStyle));
            }

            set
            {
                displayedInsecureContentStyle = (EnumToString(value));
            }
        }

        /// <summary>
        /// Always set to unknown.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("displayedInsecureContentStyle"), IsRequired = (true))]
        internal string displayedInsecureContentStyle
        {
            get;
            set;
        }
    }
}