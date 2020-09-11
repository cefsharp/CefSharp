// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// Describes a single graphics processor (GPU).
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GPUDevice : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// PCI ID of the GPU vendor, if available; 0 otherwise.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("vendorId"), IsRequired = (true))]
        public long VendorId
        {
            get;
            set;
        }

        /// <summary>
        /// PCI ID of the GPU device, if available; 0 otherwise.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("deviceId"), IsRequired = (true))]
        public long DeviceId
        {
            get;
            set;
        }

        /// <summary>
        /// Sub sys ID of the GPU, only available on Windows.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("subSysId"), IsRequired = (false))]
        public long? SubSysId
        {
            get;
            set;
        }

        /// <summary>
        /// Revision of the GPU, only available on Windows.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("revision"), IsRequired = (false))]
        public long? Revision
        {
            get;
            set;
        }

        /// <summary>
        /// String description of the GPU vendor, if the PCI ID is not available.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("vendorString"), IsRequired = (true))]
        public string VendorString
        {
            get;
            set;
        }

        /// <summary>
        /// String description of the GPU device, if the PCI ID is not available.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("deviceString"), IsRequired = (true))]
        public string DeviceString
        {
            get;
            set;
        }

        /// <summary>
        /// String description of the GPU driver vendor.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("driverVendor"), IsRequired = (true))]
        public string DriverVendor
        {
            get;
            set;
        }

        /// <summary>
        /// String description of the GPU driver version.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("driverVersion"), IsRequired = (true))]
        public string DriverVersion
        {
            get;
            set;
        }
    }
}