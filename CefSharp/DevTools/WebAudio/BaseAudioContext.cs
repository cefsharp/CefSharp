// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Protocol object for BaseAudioContext
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class BaseAudioContext : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// ContextId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contextId"), IsRequired = (true))]
        public string ContextId
        {
            get;
            set;
        }

        public CefSharp.DevTools.WebAudio.ContextType ContextType
        {
            get
            {
                return (CefSharp.DevTools.WebAudio.ContextType)(StringToEnum(typeof(CefSharp.DevTools.WebAudio.ContextType), contextType));
            }

            set
            {
                contextType = (EnumToString(value));
            }
        }

        /// <summary>
        /// ContextType
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contextType"), IsRequired = (true))]
        internal string contextType
        {
            get;
            set;
        }

        public CefSharp.DevTools.WebAudio.ContextState ContextState
        {
            get
            {
                return (CefSharp.DevTools.WebAudio.ContextState)(StringToEnum(typeof(CefSharp.DevTools.WebAudio.ContextState), contextState));
            }

            set
            {
                contextState = (EnumToString(value));
            }
        }

        /// <summary>
        /// ContextState
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("contextState"), IsRequired = (true))]
        internal string contextState
        {
            get;
            set;
        }

        /// <summary>
        /// RealtimeData
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("realtimeData"), IsRequired = (false))]
        public CefSharp.DevTools.WebAudio.ContextRealtimeData RealtimeData
        {
            get;
            set;
        }

        /// <summary>
        /// Platform-dependent callback buffer size.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("callbackBufferSize"), IsRequired = (true))]
        public long CallbackBufferSize
        {
            get;
            set;
        }

        /// <summary>
        /// Number of output channels supported by audio hardware in use.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("maxOutputChannelCount"), IsRequired = (true))]
        public long MaxOutputChannelCount
        {
            get;
            set;
        }

        /// <summary>
        /// Context sample rate.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("sampleRate"), IsRequired = (true))]
        public long SampleRate
        {
            get;
            set;
        }
    }
}