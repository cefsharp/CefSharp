// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Media
{
    /// <summary>
    /// Corresponds to kMediaError
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
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
        /// we can remove "errorCode" and replace it with the fields from
        /// a Status instance. This also seems like a duplicate of the error
        /// level enum - there is a todo bug to have that level removed and
        /// use this instead. (crbug.com/1068454)
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("errorCode"), IsRequired = (true))]
        public string ErrorCode
        {
            get;
            set;
        }
    }
}