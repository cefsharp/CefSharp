// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeadlessExperimental
{
    /// <summary>
    /// Encoding options for a screenshot.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ScreenshotParams : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Image compression format (defaults to png).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("format"), IsRequired = (false))]
        public string Format
        {
            get;
            set;
        }

        /// <summary>
        /// Compression quality from range [0..100] (jpeg only).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("quality"), IsRequired = (false))]
        public int? Quality
        {
            get;
            set;
        }
    }
}