// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Call frames for assertions or error messages.
    /// </summary>
    public class StackTrace : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// String label of this stack trace. For async traces this may be a name of the function that
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("description"), IsRequired = (false))]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript function name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("callFrames"), IsRequired = (true))]
        public System.Collections.Generic.IList<CallFrame> CallFrames
        {
            get;
            set;
        }

        /// <summary>
        /// Asynchronous JavaScript stack trace that preceded this stack, if available.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("parent"), IsRequired = (false))]
        public StackTrace Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Asynchronous JavaScript stack trace that preceded this stack, if available.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("parentId"), IsRequired = (false))]
        public StackTraceId ParentId
        {
            get;
            set;
        }
    }
}