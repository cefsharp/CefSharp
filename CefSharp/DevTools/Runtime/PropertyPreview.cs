// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// PropertyPreview
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PropertyPreview : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Property name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Object type. Accessor means that the property itself is an accessor property.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// User-friendly property value string.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (false))]
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Nested value preview.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("valuePreview"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.ObjectPreview ValuePreview
        {
            get;
            set;
        }

        /// <summary>
        /// Object subtype hint. Specified for `object` type values only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("subtype"), IsRequired = (false))]
        public string Subtype
        {
            get;
            set;
        }
    }
}