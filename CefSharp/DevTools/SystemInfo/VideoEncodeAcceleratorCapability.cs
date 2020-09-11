// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// Describes a supported video encoding profile with its associated maximum
    /// resolution and maximum framerate.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class VideoEncodeAcceleratorCapability : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Video codec profile that is supported, e.g H264 Main.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("profile"), IsRequired = (true))]
        public string Profile
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum video dimensions in pixels supported for this |profile|.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("maxResolution"), IsRequired = (true))]
        public CefSharp.DevTools.SystemInfo.Size MaxResolution
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum encoding framerate in frames per second supported for this
        /// |profile|, as fraction's numerator and denominator, e.g. 24/1 fps,
        /// 24000/1001 fps, etc.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("maxFramerateNumerator"), IsRequired = (true))]
        public int MaxFramerateNumerator
        {
            get;
            set;
        }

        /// <summary>
        /// MaxFramerateDenominator
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("maxFramerateDenominator"), IsRequired = (true))]
        public int MaxFramerateDenominator
        {
            get;
            set;
        }
    }
}