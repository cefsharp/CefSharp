// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// CSS Shape Outside details.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ShapeOutsideInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Shape bounds
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("bounds"), IsRequired = (true))]
        public long[] Bounds
        {
            get;
            set;
        }

        /// <summary>
        /// Shape coordinate details
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("shape"), IsRequired = (true))]
        public object[] Shape
        {
            get;
            set;
        }

        /// <summary>
        /// Margin shape bounds
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("marginShape"), IsRequired = (true))]
        public object[] MarginShape
        {
            get;
            set;
        }
    }
}