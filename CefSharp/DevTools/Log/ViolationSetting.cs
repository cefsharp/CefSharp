// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Log
{
    /// <summary>
    /// Violation configuration setting.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ViolationSetting : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Violation type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Time threshold to trigger upon.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("threshold"), IsRequired = (true))]
        public long Threshold
        {
            get;
            set;
        }
    }
}