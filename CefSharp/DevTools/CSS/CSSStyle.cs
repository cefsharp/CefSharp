// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS style representation.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CSSStyle : CefSharp.DevTools.DevToolsDomainEntityBase
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
        /// CSS properties in the style.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cssProperties"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.CSSProperty> CssProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Computed values for all shorthands found in the style.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("shorthandEntries"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.ShorthandEntry> ShorthandEntries
        {
            get;
            set;
        }

        /// <summary>
        /// Style declaration text (if available).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cssText"), IsRequired = (false))]
        public string CssText
        {
            get;
            set;
        }

        /// <summary>
        /// Style declaration range in the enclosing stylesheet (if available).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("range"), IsRequired = (false))]
        public CefSharp.DevTools.CSS.SourceRange Range
        {
            get;
            set;
        }
    }
}