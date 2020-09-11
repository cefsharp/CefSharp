// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.HeadlessExperimental
{
    /// <summary>
    /// BeginFrameResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class BeginFrameResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal bool hasDamage
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the BeginFrame resulted in damage and, thus, a new frame was committed to the
        public bool HasDamage
        {
            get
            {
                return hasDamage;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string screenshotData
        {
            get;
            set;
        }

        /// <summary>
        /// Base64-encoded image data of the screenshot, if one was requested and successfully taken.
        /// </summary>
        public byte[] ScreenshotData
        {
            get
            {
                return Convert(screenshotData);
            }
        }
    }
}