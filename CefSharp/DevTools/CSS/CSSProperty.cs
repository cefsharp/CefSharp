// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS property declaration data.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CSSProperty : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The property name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The property value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the property has "!important" annotation (implies `false` if absent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("important"), IsRequired = (false))]
        public bool? Important
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the property is implicit (implies `false` if absent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("implicit"), IsRequired = (false))]
        public bool? Implicit
        {
            get;
            set;
        }

        /// <summary>
        /// The full property text as specified in the style.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (false))]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the property is understood by the browser (implies `true` if absent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("parsedOk"), IsRequired = (false))]
        public bool? ParsedOk
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the property is disabled by the user (present for source-based properties only).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("disabled"), IsRequired = (false))]
        public bool? Disabled
        {
            get;
            set;
        }

        /// <summary>
        /// The entire property range in the enclosing style declaration (if available).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("range"), IsRequired = (false))]
        public CefSharp.DevTools.CSS.SourceRange Range
        {
            get;
            set;
        }
    }
}