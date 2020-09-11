// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Protocol object for AudioListner
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AudioListener : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// ListenerId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("listenerId"), IsRequired = (true))]
        public string ListenerId
        {
            get;
            set;
        }

        /// <summary>
        /// ContextId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contextId"), IsRequired = (true))]
        public string ContextId
        {
            get;
            set;
        }
    }
}