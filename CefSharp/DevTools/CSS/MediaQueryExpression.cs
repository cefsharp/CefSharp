// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    /// <summary>
    /// Media query expression descriptor.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class MediaQueryExpression : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Media query expression value.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("value"), IsRequired = (true))]
        public long Value
        {
            get;
            set;
        }

        /// <summary>
        /// Media query expression units.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("unit"), IsRequired = (true))]
        public string Unit
        {
            get;
            set;
        }

        /// <summary>
        /// Media query expression feature.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("feature"), IsRequired = (true))]
        public string Feature
        {
            get;
            set;
        }

        /// <summary>
        /// The associated range of the value text in the enclosing stylesheet (if available).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("valueRange"), IsRequired = (false))]
        public CefSharp.DevTools.CSS.SourceRange ValueRange
        {
            get;
            set;
        }

        /// <summary>
        /// Computed length of media query expression (if applicable).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("computedLength"), IsRequired = (false))]
        public long? ComputedLength
        {
            get;
            set;
        }
    }
}