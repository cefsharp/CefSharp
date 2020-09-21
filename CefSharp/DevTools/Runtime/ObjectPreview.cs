// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Object containing abbreviated remote object value.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ObjectPreview : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Object type.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Object subtype hint. Specified for `object` type values only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("subtype"), IsRequired = (false))]
        public string Subtype
        {
            get;
            set;
        }

        /// <summary>
        /// String representation of the object.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("description"), IsRequired = (false))]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// True iff some of the properties or entries of the original object did not fit.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("overflow"), IsRequired = (true))]
        public bool Overflow
        {
            get;
            set;
        }

        /// <summary>
        /// List of the properties.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("properties"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Runtime.PropertyPreview> Properties
        {
            get;
            set;
        }

        /// <summary>
        /// List of the entries. Specified for `map` and `set` subtype values only.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("entries"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Runtime.EntryPreview> Entries
        {
            get;
            set;
        }
    }
}