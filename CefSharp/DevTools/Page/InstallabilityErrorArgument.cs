// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// InstallabilityErrorArgument
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class InstallabilityErrorArgument : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Argument name (e.g. name:'minimum-icon-size-in-pixels').
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Argument value (e.g. value:'64').
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public string Value
        {
            get;
            set;
        }
    }
}