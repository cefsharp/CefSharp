// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Default font sizes.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class FontSizes : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Default standard font size.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("standard"), IsRequired = (false))]
        public int? Standard
        {
            get;
            set;
        }

        /// <summary>
        /// Default fixed font size.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fixed"), IsRequired = (false))]
        public int? Fixed
        {
            get;
            set;
        }
    }
}