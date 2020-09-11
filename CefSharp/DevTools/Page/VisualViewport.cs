// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Visual viewport position, dimensions, and scale.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class VisualViewport : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Horizontal offset relative to the layout viewport (CSS pixels).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("offsetX"), IsRequired = (true))]
        public long OffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Vertical offset relative to the layout viewport (CSS pixels).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("offsetY"), IsRequired = (true))]
        public long OffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Horizontal offset relative to the document (CSS pixels).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pageX"), IsRequired = (true))]
        public long PageX
        {
            get;
            set;
        }

        /// <summary>
        /// Vertical offset relative to the document (CSS pixels).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pageY"), IsRequired = (true))]
        public long PageY
        {
            get;
            set;
        }

        /// <summary>
        /// Width (CSS pixels), excludes scrollbar if present.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("clientWidth"), IsRequired = (true))]
        public long ClientWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Height (CSS pixels), excludes scrollbar if present.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("clientHeight"), IsRequired = (true))]
        public long ClientHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Scale relative to the ideal viewport (size at width=device-width).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scale"), IsRequired = (true))]
        public long Scale
        {
            get;
            set;
        }

        /// <summary>
        /// Page zoom factor (CSS to device independent pixels ratio).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("zoom"), IsRequired = (false))]
        public long? Zoom
        {
            get;
            set;
        }
    }
}