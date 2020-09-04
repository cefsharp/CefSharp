// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// EvaluateOnCallFrameResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class EvaluateOnCallFrameResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal Runtime.RemoteObject result
        {
            get;
            set;
        }

        /// <summary>
        /// Object wrapper for the evaluation result.
        /// </summary>
        public Runtime.RemoteObject Result
        {
            get
            {
                return result;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal Runtime.ExceptionDetails exceptionDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details.
        /// </summary>
        public Runtime.ExceptionDetails ExceptionDetails
        {
            get
            {
                return exceptionDetails;
            }
        }
    }
}