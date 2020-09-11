// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// NavigateResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class NavigateResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal string frameId
        {
            get;
            set;
        }

        /// <summary>
        /// Frame id that has navigated (or failed to navigate)
        /// </summary>
        public string FrameId
        {
            get
            {
                return frameId;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string loaderId
        {
            get;
            set;
        }

        /// <summary>
        /// Loader identifier.
        /// </summary>
        public string LoaderId
        {
            get
            {
                return loaderId;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string errorText
        {
            get;
            set;
        }

        /// <summary>
        /// User friendly error message, present if and only if navigation has failed.
        /// </summary>
        public string ErrorText
        {
            get
            {
                return errorText;
            }
        }
    }
}