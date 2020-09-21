// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS rule representation.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CSSRule : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The css style sheet identifier (absent for user agent stylesheet and user-specified
        /// stylesheet rules) this rule came from.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styleSheetId"), IsRequired = (false))]
        public string StyleSheetId
        {
            get;
            set;
        }

        /// <summary>
        /// Rule selector data.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("selectorList"), IsRequired = (true))]
        public CefSharp.DevTools.CSS.SelectorList SelectorList
        {
            get;
            set;
        }

        public CefSharp.DevTools.CSS.StyleSheetOrigin Origin
        {
            get
            {
                return (CefSharp.DevTools.CSS.StyleSheetOrigin)(StringToEnum(typeof(CefSharp.DevTools.CSS.StyleSheetOrigin), origin));
            }

            set
            {
                origin = (EnumToString(value));
            }
        }

        /// <summary>
        /// Parent stylesheet's origin.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("origin"), IsRequired = (true))]
        internal string origin
        {
            get;
            set;
        }

        /// <summary>
        /// Associated style declaration.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("style"), IsRequired = (true))]
        public CefSharp.DevTools.CSS.CSSStyle Style
        {
            get;
            set;
        }

        /// <summary>
        /// Media list array (for rules involving media queries). The array enumerates media queries
        /// starting with the innermost one, going outwards.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("media"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSMedia> Media
        {
            get;
            set;
        }
    }
}