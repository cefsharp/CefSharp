// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Media
{
    /// <summary>
    /// Corresponds to kMediaError
    /// </summary>
    public class PlayerError : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Type
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("type"), IsRequired = (true))]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// When this switches to using media::Status instead of PipelineStatus
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("errorCode"), IsRequired = (true))]
        public string ErrorCode
        {
            get;
            set;
        }
    }
}