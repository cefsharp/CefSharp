// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    /// <summary>
    /// Configuration data for the highlighting of page elements.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class HighlightConfig : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Whether the node info tooltip should be shown (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showInfo"), IsRequired = (false))]
        public bool? ShowInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the node styles in the tooltip (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showStyles"), IsRequired = (false))]
        public bool? ShowStyles
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the rulers should be shown (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showRulers"), IsRequired = (false))]
        public bool? ShowRulers
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the a11y info should be shown (default: true).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showAccessibilityInfo"), IsRequired = (false))]
        public bool? ShowAccessibilityInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the extension lines from node to the rulers should be shown (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showExtensionLines"), IsRequired = (false))]
        public bool? ShowExtensionLines
        {
            get;
            set;
        }

        /// <summary>
        /// The content box highlight fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contentColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA ContentColor
        {
            get;
            set;
        }

        /// <summary>
        /// The padding highlight fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("paddingColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA PaddingColor
        {
            get;
            set;
        }

        /// <summary>
        /// The border highlight fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("borderColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA BorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// The margin highlight fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("marginColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA MarginColor
        {
            get;
            set;
        }

        /// <summary>
        /// The event target element highlight fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("eventTargetColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA EventTargetColor
        {
            get;
            set;
        }

        /// <summary>
        /// The shape outside fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("shapeColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA ShapeColor
        {
            get;
            set;
        }

        /// <summary>
        /// The shape margin fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("shapeMarginColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA ShapeMarginColor
        {
            get;
            set;
        }

        /// <summary>
        /// The grid layout color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cssGridColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA CssGridColor
        {
            get;
            set;
        }

        /// <summary>
        /// The color format used to format color styles (default: hex).
        /// </summary>
        public CefSharp.DevTools.Overlay.ColorFormat? ColorFormat
        {
            get
            {
                return (CefSharp.DevTools.Overlay.ColorFormat? )(StringToEnum(typeof(CefSharp.DevTools.Overlay.ColorFormat? ), colorFormat));
            }

            set
            {
                colorFormat = (EnumToString(value));
            }
        }

        /// <summary>
        /// The color format used to format color styles (default: hex).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("colorFormat"), IsRequired = (false))]
        internal string colorFormat
        {
            get;
            set;
        }

        /// <summary>
        /// The grid layout highlight configuration (default: all transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("gridHighlightConfig"), IsRequired = (false))]
        public CefSharp.DevTools.Overlay.GridHighlightConfig GridHighlightConfig
        {
            get;
            set;
        }
    }
}