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
        /// backgroundColors
        /// </summary>
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
        /// computedFontSize
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
        /// computedFontWeight
        /// </summary>
        public string ComputedFontWeight
        {
            get
            {
                return computedFontWeight;
            }
        }
    }
}