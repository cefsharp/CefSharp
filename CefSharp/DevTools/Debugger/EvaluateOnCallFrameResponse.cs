// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// EvaluateOnCallFrameResponse
    /// </summary>
    public class EvaluateOnCallFrameResponse
    {
        /// <summary>
        /// Object wrapper for the evaluation result.
        /// </summary>
        public Runtime.RemoteObject result
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details.
        /// </summary>
        public Runtime.ExceptionDetails exceptionDetails
        {
            get;
            set;
        }
    }
}