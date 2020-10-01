// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Information about the Frame hierarchy.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class FrameTree : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Frame information for this tree item.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("frame"), IsRequired = (true))]
        public CefSharp.DevTools.Page.Frame Frame
        {
            get;
            set;
        }

        /// <summary>
        /// Child frames.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("childFrames"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Page.FrameTree> ChildFrames
        {
            get;
            set;
        }
    }
}