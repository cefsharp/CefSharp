// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// A structure holding an RGBA color.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RGBA : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The red component, in the [0-255] range.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("r"), IsRequired = (true))]
        public int R
        {
            get;
            set;
        }

        /// <summary>
        /// The green component, in the [0-255] range.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("g"), IsRequired = (true))]
        public int G
        {
            get;
            set;
        }

        /// <summary>
        /// The blue component, in the [0-255] range.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("b"), IsRequired = (true))]
        public int B
        {
            get;
            set;
        }

        /// <summary>
        /// The alpha component, in the [0-1] range (default: 1).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("a"), IsRequired = (false))]
        public long? A
        {
            get;
            set;
        }
    }
}