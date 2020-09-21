// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// Provides information about the GPU(s) on the system.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GPUInfo : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// The graphics devices on the system. Element 0 is the primary GPU.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("devices"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.SystemInfo.GPUDevice> Devices
        {
            get;
            set;
        }

        /// <summary>
        /// An optional dictionary of additional GPU related attributes.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("auxAttributes"), IsRequired = (false))]
        public object AuxAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// An optional dictionary of graphics features and their status.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("featureStatus"), IsRequired = (false))]
        public object FeatureStatus
        {
            get;
            set;
        }

        /// <summary>
        /// An optional array of GPU driver bug workarounds.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("driverBugWorkarounds"), IsRequired = (true))]
        public string[] DriverBugWorkarounds
        {
            get;
            set;
        }

        /// <summary>
        /// Supported accelerated video decoding capabilities.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("videoDecoding"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.SystemInfo.VideoDecodeAcceleratorCapability> VideoDecoding
        {
            get;
            set;
        }

        /// <summary>
        /// Supported accelerated video encoding capabilities.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("videoEncoding"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.SystemInfo.VideoEncodeAcceleratorCapability> VideoEncoding
        {
            get;
            set;
        }

        /// <summary>
        /// Supported accelerated image decoding capabilities.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("imageDecoding"), IsRequired = (true))]
        public System.Collections.Generic.IList<CefSharp.DevTools.SystemInfo.ImageDecodeAcceleratorCapability> ImageDecoding
        {
            get;
            set;
        }
    }
}