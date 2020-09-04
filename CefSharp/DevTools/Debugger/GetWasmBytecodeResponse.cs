// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// GetWasmBytecodeResponse
    /// </summary>
    public class GetWasmBytecodeResponse
    {
        /// <summary>
        /// Script source.
        /// </summary>
        public string bytecode
        {
            get;
            set;
        }
    }
}