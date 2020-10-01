// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Navigation history entry.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class NavigationEntry : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Unique id of the navigation history entry.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the navigation history entry.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// URL that the user typed in the url bar.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("userTypedURL"), IsRequired = (true))]
        public string UserTypedURL
        {
            get;
            set;
        }

        /// <summary>
        /// Title of the navigation history entry.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("title"), IsRequired = (true))]
        public string Title
        {
            get;
            set;
        }

        public CefSharp.DevTools.Page.TransitionType TransitionType
        {
            get
            {
                return (CefSharp.DevTools.Page.TransitionType)(StringToEnum(typeof(CefSharp.DevTools.Page.TransitionType), transitionType));
            }

            set
            {
                transitionType = (EnumToString(value));
            }
        }

        /// <summary>
        /// Transition type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("transitionType"), IsRequired = (true))]
        internal string transitionType
        {
            get;
            set;
        }
    }
}