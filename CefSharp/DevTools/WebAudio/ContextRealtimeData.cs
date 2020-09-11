// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.WebAudio
{
    /// <summary>
    /// Fields in AudioContext that change in real-time.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ContextRealtimeData : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The current context time in second in BaseAudioContext.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("currentTime"), IsRequired = (true))]
        public long CurrentTime
        {
            get;
            set;
        }

        /// <summary>
        /// The time spent on rendering graph divided by render qunatum duration,
        /// and multiplied by 100. 100 means the audio renderer reached the full
        /// capacity and glitch may occur.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("renderCapacity"), IsRequired = (true))]
        public long RenderCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// A running mean of callback interval.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("callbackIntervalMean"), IsRequired = (true))]
        public long CallbackIntervalMean
        {
            get;
            set;
        }

        /// <summary>
        /// A running variance of callback interval.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("callbackIntervalVariance"), IsRequired = (true))]
        public long CallbackIntervalVariance
        {
            get;
            set;
        }
    }
}