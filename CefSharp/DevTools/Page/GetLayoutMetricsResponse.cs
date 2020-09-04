// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetLayoutMetricsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetLayoutMetricsResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal LayoutViewport layoutViewport
        {
            get;
            set;
        }

        /// <summary>
        /// Metrics relating to the layout viewport.
        /// </summary>
        public LayoutViewport LayoutViewport
        {
            get
            {
                return layoutViewport;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal VisualViewport visualViewport
        {
            get;
            set;
        }

        /// <summary>
        /// Metrics relating to the visual viewport.
        /// </summary>
        public VisualViewport VisualViewport
        {
            get
            {
                return visualViewport;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal DOM.Rect contentSize
        {
            get;
            set;
        }

        /// <summary>
        /// Size of scrollable area.
        /// </summary>
        public DOM.Rect ContentSize
        {
            get
            {
                return contentSize;
            }
        }
    }
}