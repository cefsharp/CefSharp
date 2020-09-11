// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// Properties of a web font: https://www.w3.org/TR/2008/REC-CSS2-20080411/fonts.html#font-descriptions
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class FontFace : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fontFamily"), IsRequired = (true))]
        public string FontFamily
        {
            get;
            set;
        }

        /// <summary>
        /// The font-style.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fontStyle"), IsRequired = (true))]
        public string FontStyle
        {
            get;
            set;
        }

        /// <summary>
        /// The font-variant.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fontVariant"), IsRequired = (true))]
        public string FontVariant
        {
            get;
            set;
        }

        /// <summary>
        /// The font-weight.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fontWeight"), IsRequired = (true))]
        public string FontWeight
        {
            get;
            set;
        }

        /// <summary>
        /// The font-stretch.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fontStretch"), IsRequired = (true))]
        public string FontStretch
        {
            get;
            set;
        }

        /// <summary>
        /// The unicode-range.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("unicodeRange"), IsRequired = (true))]
        public string UnicodeRange
        {
            get;
            set;
        }

        /// <summary>
        /// The src.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("src"), IsRequired = (true))]
        public string Src
        {
            get;
            set;
        }

        /// <summary>
        /// The resolved platform font family
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("platformFontFamily"), IsRequired = (true))]
        public string PlatformFontFamily
        {
            get;
            set;
        }
    }
}