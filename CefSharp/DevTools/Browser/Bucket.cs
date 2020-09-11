// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// Chrome histogram bucket.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Bucket : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Minimum value (inclusive).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("low"), IsRequired = (true))]
        public int Low
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum value (exclusive).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("high"), IsRequired = (true))]
        public int High
        {
            get;
            set;
        }

        /// <summary>
        /// Number of samples.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("count"), IsRequired = (true))]
        public int Count
        {
            get;
            set;
        }
    }
}