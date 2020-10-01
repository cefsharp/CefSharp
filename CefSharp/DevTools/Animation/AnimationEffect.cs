// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Animation
{
    /// <summary>
    /// AnimationEffect instance
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class AnimationEffect : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// `AnimationEffect`'s delay.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("delay"), IsRequired = (true))]
        public long Delay
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s end delay.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("endDelay"), IsRequired = (true))]
        public long EndDelay
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s iteration start.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("iterationStart"), IsRequired = (true))]
        public long IterationStart
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s iterations.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("iterations"), IsRequired = (true))]
        public long Iterations
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s iteration duration.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("duration"), IsRequired = (true))]
        public long Duration
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s playback direction.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("direction"), IsRequired = (true))]
        public string Direction
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s fill mode.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("fill"), IsRequired = (true))]
        public string Fill
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s target node.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("backendNodeId"), IsRequired = (false))]
        public int? BackendNodeId
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s keyframes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("keyframesRule"), IsRequired = (false))]
        public CefSharp.DevTools.Animation.KeyframesRule KeyframesRule
        {
            get;
            set;
        }

        /// <summary>
        /// `AnimationEffect`'s timing function.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("easing"), IsRequired = (true))]
        public string Easing
        {
            get;
            set;
        }
    }
}