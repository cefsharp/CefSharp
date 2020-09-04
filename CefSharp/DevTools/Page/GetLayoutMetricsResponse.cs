// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetLayoutMetricsResponse
    /// </summary>
    public class GetLayoutMetricsResponse
    {
        /// <summary>
        /// Metrics relating to the layout viewport.
        /// </summary>
        public LayoutViewport layoutViewport
        {
            get;
            set;
        }

        /// <summary>
        /// Metrics relating to the visual viewport.
        /// </summary>
        public VisualViewport visualViewport
        {
            get;
            set;
        }

        /// <summary>
        /// Size of scrollable area.
        /// </summary>
        public DOM.Rect contentSize
        {
            get;
            set;
        }
    }
}