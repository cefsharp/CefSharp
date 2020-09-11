// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// If `debuggerId` is set stack trace comes from another debugger and can be resolved there. This
    /// allows to track cross-debugger calls. See `Runtime.StackTrace` and `Debugger.paused` for usages.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class StackTraceId : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Id
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("id"), IsRequired = (true))]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// DebuggerId
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("debuggerId"), IsRequired = (false))]
        public string DebuggerId
        {
            get;
            set;
        }
    }
}