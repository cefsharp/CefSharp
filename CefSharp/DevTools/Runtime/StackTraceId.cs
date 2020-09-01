// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// If `debuggerId` is set stack trace comes from another debugger and can be resolved there. This
    public class StackTraceId
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DebuggerId
        {
            get;
            set;
        }
    }
}