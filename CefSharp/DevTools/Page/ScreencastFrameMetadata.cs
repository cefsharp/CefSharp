// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Screencast frame metadata.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ScreencastFrameMetadata : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Top offset in DIP.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("offsetTop"), IsRequired = (true))]
        public long OffsetTop
        {
            get;
            set;
        }

        /// <summary>
        /// Page scale factor.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("pageScaleFactor"), IsRequired = (true))]
        public long PageScaleFactor
        {
            get;
            set;
        }

        /// <summary>
        /// Device screen width in DIP.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("deviceWidth"), IsRequired = (true))]
        public long DeviceWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Device screen height in DIP.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("deviceHeight"), IsRequired = (true))]
        public long DeviceHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Position of horizontal scroll in CSS pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollOffsetX"), IsRequired = (true))]
        public long ScrollOffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Position of vertical scroll in CSS pixels.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scrollOffsetY"), IsRequired = (true))]
        public long ScrollOffsetY
        {
            get;
            set;
        }

        /// <summary>
        /// Frame swap timestamp.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("timestamp"), IsRequired = (false))]
        public long? Timestamp
        {
            get;
            set;
        }
    }
}