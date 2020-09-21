// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Layout viewport position and dimensions.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class LayoutViewport : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Horizontal offset relative to the document (CSS pixels).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pageX"), IsRequired = (true))]
        public int PageX
        {
            get;
            set;
        }

        /// <summary>
        /// Vertical offset relative to the document (CSS pixels).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pageY"), IsRequired = (true))]
        public int PageY
        {
            get;
            set;
        }

        /// <summary>
        /// Width (CSS pixels), excludes scrollbar if present.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("clientWidth"), IsRequired = (true))]
        public int ClientWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Height (CSS pixels), excludes scrollbar if present.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("clientHeight"), IsRequired = (true))]
        public int ClientHeight
        {
            get;
            set;
        }
    }
}