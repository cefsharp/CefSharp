// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// GetLayoutMetricsResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetLayoutMetricsResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Page.LayoutViewport layoutViewport
        {
            get;
            set;
        }

        /// <summary>
        /// layoutViewport
        /// </summary>
        public CefSharp.DevTools.Page.LayoutViewport LayoutViewport
        {
            get
            {
                return layoutViewport;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.Page.VisualViewport visualViewport
        {
            get;
            set;
        }

        /// <summary>
        /// visualViewport
        /// </summary>
        public CefSharp.DevTools.Page.VisualViewport VisualViewport
        {
            get
            {
                return visualViewport;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.DOM.Rect contentSize
        {
            get;
            set;
        }

        /// <summary>
        /// contentSize
        /// </summary>
        public CefSharp.DevTools.DOM.Rect ContentSize
        {
            get
            {
                return contentSize;
            }
        }
    }
}