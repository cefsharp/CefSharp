// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// CompileScriptResponse
    /// </summary>
    public class CompileScriptResponse
    {
        /// <summary>
        /// Id of the script.
        /// </summary>
        public string scriptId
        {
            get;
            set;
        }

        /// <summary>
        /// Exception details.
        /// </summary>
        public ExceptionDetails exceptionDetails
        {
            get;
            set;
        }
    }
}