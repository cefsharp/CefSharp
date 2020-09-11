// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// Describes a supported image decoding profile with its associated minimum and
    public class ImageDecodeAcceleratorCapability : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Image coded, e.g. Jpeg.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("imageType"), IsRequired = (true))]
        public CefSharp.DevTools.SystemInfo.ImageType ImageType
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum supported dimensions of the image in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("maxDimensions"), IsRequired = (true))]
        public CefSharp.DevTools.SystemInfo.Size MaxDimensions
        {
            get;
            set;
        }

        /// <summary>
        /// Minimum supported dimensions of the image in pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("minDimensions"), IsRequired = (true))]
        public CefSharp.DevTools.SystemInfo.Size MinDimensions
        {
            get;
            set;
        }

        /// <summary>
        /// Optional array of supported subsampling formats, e.g. 4:2:0, if known.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("subsamplings"), IsRequired = (true))]
        public CefSharp.DevTools.SystemInfo.SubsamplingFormat[] Subsamplings
        {
            get;
            set;
        }
    }
}