// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// CSS keyframe rule representation.
    /// </summary>
    public class CSSKeyframeRule : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The css style sheet identifier (absent for user agent stylesheet and user-specified
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("styleSheetId"), IsRequired = (false))]
        public string StyleSheetId
        {
            get;
            set;
        }

        /// <summary>
        /// Parent stylesheet's origin.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("origin"), IsRequired = (true))]
        public CefSharp.DevTools.CSS.StyleSheetOrigin Origin
        {
            get;
            set;
        }

        /// <summary>
        /// Associated key text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("keyText"), IsRequired = (true))]
        public CefSharp.DevTools.CSS.Value KeyText
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
    }
}