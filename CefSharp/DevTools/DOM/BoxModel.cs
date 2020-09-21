// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.DOM
{
    /// <summary>
    /// Box model.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class BoxModel : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Content box
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("content"), IsRequired = (true))]
        public long[] Content
        {
            get;
            set;
        }

        /// <summary>
        /// Padding box
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("padding"), IsRequired = (true))]
        public long[] Padding
        {
            get;
            set;
        }

        /// <summary>
        /// Border box
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("border"), IsRequired = (true))]
        public long[] Border
        {
            get;
            set;
        }

        /// <summary>
        /// Margin box
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("margin"), IsRequired = (true))]
        public long[] Margin
        {
            get;
            set;
        }

        /// <summary>
        /// Node width
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("width"), IsRequired = (true))]
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Node height
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("height"), IsRequired = (true))]
        public int Height
        {
            get;
            set;
        }

        /// <summary>
        /// Shape outside coordinates
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("shapeOutside"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.ShapeOutsideInfo ShapeOutside
        {
            get;
            set;
        }
    }
}