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
        /// gpu
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
        /// modelName
        /// </summary>
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
        /// modelVersion
        /// </summary>
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
        /// commandLine
        /// </summary>
        public string CommandLine
        {
            get
            {
                return commandLine;
            }
        }
    }
}