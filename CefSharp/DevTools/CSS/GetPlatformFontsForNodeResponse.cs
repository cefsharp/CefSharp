// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// GetPlatformFontsForNodeResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetPlatformFontsForNodeResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal System.Collections.Generic.IList<CefSharp.DevTools.CSS.PlatformFontUsage> fonts
        {
            get;
            set;
        }

        /// <summary>
        /// fonts
        /// </summary>
        public System.Collections.Generic.IList<CefSharp.DevTools.CSS.PlatformFontUsage> Fonts
        {
            get
            {
                return fonts;
            }
        }
    }
}