// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// GetBackgroundColorsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetBackgroundColorsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string[] backgroundColors
        {
            get;
            set;
        }

        /// <summary>
        /// The range of background colors behind this element, if it contains any visible text. If no
        public string[] BackgroundColors
        {
            get
            {
                return backgroundColors;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string computedFontSize
        {
            get;
            set;
        }

        /// <summary>
        /// The computed font size for this node, as a CSS computed value string (e.g. '12px').
        /// </summary>
        public string ComputedFontSize
        {
            get
            {
                return computedFontSize;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string computedFontWeight
        {
            get;
            set;
        }

        /// <summary>
        /// The computed font weight for this node, as a CSS computed value string (e.g. 'normal' or
        public string ComputedFontWeight
        {
            get
            {
                return computedFontWeight;
            }
        }
    }
}