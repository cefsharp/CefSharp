// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Accessibility
{
    /// <summary>
    /// A single source for a computed AX property.
    /// </summary>
    public class AXValueSource : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// What type of source this is.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public CefSharp.DevTools.Accessibility.AXValueSourceType Type
        {
            get;
            set;
        }

        /// <summary>
        /// The value of this property source.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValue Value
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the relevant attribute, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("attribute"), IsRequired = (false))]
        public string Attribute
        {
            get;
            set;
        }

        /// <summary>
        /// The value of the relevant attribute, if any.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("attributeValue"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValue AttributeValue
        {
            get;
            set;
        }

        /// <summary>
        /// Whether this source is superseded by a higher priority source.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("superseded"), IsRequired = (false))]
        public bool? Superseded
        {
            get;
            set;
        }

        /// <summary>
        /// The native markup source for this value, e.g. a <label> element.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nativeSource"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValueNativeSourceType? NativeSource
        {
            get;
            set;
        }

        /// <summary>
        /// The value, such as a node or node list, of the native source.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("nativeSourceValue"), IsRequired = (false))]
        public CefSharp.DevTools.Accessibility.AXValue NativeSourceValue
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the value for this property is invalid.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("invalid"), IsRequired = (false))]
        public bool? Invalid
        {
            get;
            set;
        }

        /// <summary>
        /// Reason for the value being invalid, if it is.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("invalidReason"), IsRequired = (false))]
        public string InvalidReason
        {
            get;
            set;
        }
    }
}