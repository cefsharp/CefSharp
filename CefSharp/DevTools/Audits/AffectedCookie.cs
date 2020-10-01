// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Audits
{
    /// <summary>
    /// Information about a cookie that is affected by an inspector issue.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AffectedCookie : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The following three properties uniquely identify a cookie
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Path
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("path"), IsRequired = (true))]
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Domain
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("domain"), IsRequired = (true))]
        public string Domain
        {
            get;
            set;
        }
    }
}