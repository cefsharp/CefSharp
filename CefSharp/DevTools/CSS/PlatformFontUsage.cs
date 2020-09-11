// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// Information about amount of glyphs that were rendered with given font.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class PlatformFontUsage : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Font's family name reported by platform.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("familyName"), IsRequired = (true))]
        public string FamilyName
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates if the font was downloaded or resolved locally.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("isCustomFont"), IsRequired = (true))]
        public bool IsCustomFont
        {
            get;
            set;
        }

        /// <summary>
        /// Amount of glyphs that were rendered with this font.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("glyphCount"), IsRequired = (true))]
        public long GlyphCount
        {
            get;
            set;
        }
    }
}