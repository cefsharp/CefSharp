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
        /// hasDamage
        /// </summary>
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
        /// screenshotData
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