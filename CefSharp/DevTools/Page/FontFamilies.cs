// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Generic font families collection.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class FontFamilies : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The standard font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("standard"), IsRequired = (false))]
        public string Standard
        {
            get;
            set;
        }

        /// <summary>
        /// The fixed font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fixed"), IsRequired = (false))]
        public string Fixed
        {
            get;
            set;
        }

        /// <summary>
        /// The serif font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("serif"), IsRequired = (false))]
        public string Serif
        {
            get;
            set;
        }

        /// <summary>
        /// The sansSerif font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sansSerif"), IsRequired = (false))]
        public string SansSerif
        {
            get;
            set;
        }

        /// <summary>
        /// The cursive font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cursive"), IsRequired = (false))]
        public string Cursive
        {
            get;
            set;
        }

        /// <summary>
        /// The fantasy font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fantasy"), IsRequired = (false))]
        public string Fantasy
        {
            get;
            set;
        }

        /// <summary>
        /// The pictograph font-family.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pictograph"), IsRequired = (false))]
        public string Pictograph
        {
            get;
            set;
        }
    }
}