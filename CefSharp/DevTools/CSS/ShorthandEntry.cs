// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// ShorthandEntry
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ShorthandEntry : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Shorthand name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Shorthand value.
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
    }
}