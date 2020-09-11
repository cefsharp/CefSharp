// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// AXProperty
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AXProperty : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        public CefSharp.DevTools.Accessibility.AXPropertyName Name
        {
            get
            {
                return (CefSharp.DevTools.Accessibility.AXPropertyName)(StringToEnum(typeof(CefSharp.DevTools.Accessibility.AXPropertyName), name));
            }

            set
            {
                name = (EnumToString(value));
            }
        }

        /// <summary>
        /// The name of this property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        internal string name
        {
            get;
            set;
        }

        /// <summary>
        /// The value of this property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public CefSharp.DevTools.Accessibility.AXValue Value
        {
            get;
            set;
        }
    }
}