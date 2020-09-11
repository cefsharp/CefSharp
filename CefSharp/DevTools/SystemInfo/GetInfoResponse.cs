// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.SystemInfo
{
    /// <summary>
    /// GetInfoResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class GetInfoResponse : CefSharp.DevTools.DevToolsDomainResponseBase
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal CefSharp.DevTools.SystemInfo.GPUInfo gpu
        {
            get;
            set;
        }

        /// <summary>
        /// Information about the GPUs on the system.
        /// </summary>
        public CefSharp.DevTools.SystemInfo.GPUInfo Gpu
        {
            get
            {
                return gpu;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string modelName
        {
            get;
            set;
        }

        /// <summary>
        /// A platform-dependent description of the model of the machine. On Mac OS, this is, for
        public string ModelName
        {
            get
            {
                return modelName;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string modelVersion
        {
            get;
            set;
        }

        /// <summary>
        /// A platform-dependent description of the version of the machine. On Mac OS, this is, for
        public string ModelVersion
        {
            get
            {
                return modelVersion;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal string commandLine
        {
            get;
            set;
        }

        /// <summary>
        /// The command line string used to launch the browser. Will be the empty string if not
        public string CommandLine
        {
            get
            {
                return commandLine;
            }
        }
    }
}