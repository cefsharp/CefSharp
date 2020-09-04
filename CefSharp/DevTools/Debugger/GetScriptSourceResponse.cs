// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// GetScriptSourceResponse
    /// </summary>
    public class GetScriptSourceResponse
    {
        /// <summary>
        /// Script source (empty in case of Wasm bytecode).
        /// </summary>
        public string scriptSource
        {
            get;
            set;
        }

        /// <summary>
        /// Wasm bytecode.
        /// </summary>
        public string bytecode
        {
            get;
            set;
        }
    }
}