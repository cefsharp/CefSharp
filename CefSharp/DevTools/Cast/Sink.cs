// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Cast
{
    /// <summary>
    /// Sink
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Sink : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Name
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("name"), IsRequired = (true))]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Id
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Text describing the current session. Present only if there is an active
        /// session on the sink.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("session"), IsRequired = (false))]
        public string Session
        {
            get;
            set;
        }
    }
}