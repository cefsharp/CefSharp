// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    /// <summary>
    /// Configuration for dual screen hinge
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class HingeConfig : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// A rectangle represent hinge
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("rect"), IsRequired = (true))]
        public CefSharp.DevTools.DOM.Rect Rect
        {
            get;
            set;
        }

        /// <summary>
        /// The content box highlight fill color (default: a dark color).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA ContentColor
        {
            get;
            set;
        }

        /// <summary>
        /// The content box highlight outline color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("outlineColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA OutlineColor
        {
            get;
            set;
        }
    }
}