// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// Rectangle.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Rect : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("x"), IsRequired = (true))]
        public long X
        {
            get;
            set;
        }

        /// <summary>
        /// Y coordinate
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("y"), IsRequired = (true))]
        public long Y
        {
            get;
            set;
        }

        /// <summary>
        /// Rectangle width
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("width"), IsRequired = (true))]
        public long Width
        {
            get;
            set;
        }

        /// <summary>
        /// Rectangle height
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("height"), IsRequired = (true))]
        public long Height
        {
            get;
            set;
        }
    }
}