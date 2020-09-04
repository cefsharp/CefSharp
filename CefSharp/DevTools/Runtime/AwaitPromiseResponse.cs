// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// AwaitPromiseResponse
    /// </summary>
    public class AwaitPromiseResponse
    {
        /// <summary>
        /// Promise result. Will contain rejected value if promise was rejected.
        /// </summary>
        public RemoteObject result
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details if stack strace is available.
        /// </summary>
        public ExceptionDetails exceptionDetails
        {
            get;
            set;
        }
    }
}