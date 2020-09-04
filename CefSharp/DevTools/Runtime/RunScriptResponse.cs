// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// RunScriptResponse
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class RunScriptResponse
    {
        [System.Runtime.Serialization.DataMemberAttribute]
        internal RemoteObject result
        {
            get;
            set;
        }

        /// <summary>
        /// Run result.
        /// </summary>
        public RemoteObject Result
        {
            get
            {
                return result;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        internal ExceptionDetails exceptionDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details.
        /// </summary>
        public ExceptionDetails ExceptionDetails
        {
            get
            {
                return exceptionDetails;
            }
        }
    }
}