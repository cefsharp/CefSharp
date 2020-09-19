// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    /// <summary>
    /// Configuration data for the highlighting of Grid elements.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GridHighlightConfig : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Whether the extension lines from grid cells to the rulers should be shown (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showGridExtensionLines"), IsRequired = (false))]
        public bool? ShowGridExtensionLines
        {
            get;
            set;
        }

        /// <summary>
        /// Show Positive line number labels (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showPositiveLineNumbers"), IsRequired = (false))]
        public bool? ShowPositiveLineNumbers
        {
            get;
            set;
        }

        /// <summary>
        /// Show Negative line number labels (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("showNegativeLineNumbers"), IsRequired = (false))]
        public bool? ShowNegativeLineNumbers
        {
            get;
            set;
        }

        /// <summary>
        /// The grid container border highlight color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("gridBorderColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA GridBorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// The cell border color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cellBorderColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA CellBorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the grid border is dashed (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("gridBorderDash"), IsRequired = (false))]
        public bool? GridBorderDash
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the cell border is dashed (default: false).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("cellBorderDash"), IsRequired = (false))]
        public bool? CellBorderDash
        {
            get;
            set;
        }

        /// <summary>
        /// The row gap highlight fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("rowGapColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA RowGapColor
        {
            get;
            set;
        }

        /// <summary>
        /// The row gap hatching fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("rowHatchColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA RowHatchColor
        {
            get;
            set;
        }

        /// <summary>
        /// The column gap highlight fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnGapColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA ColumnGapColor
        {
            get;
            set;
        }

        /// <summary>
        /// The column gap hatching fill color (default: transparent).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnHatchColor"), IsRequired = (false))]
        public CefSharp.DevTools.DOM.RGBA ColumnHatchColor
        {
            get;
            set;
        }
    }
}