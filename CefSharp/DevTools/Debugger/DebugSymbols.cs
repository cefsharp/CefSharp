// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Debug symbols available for a wasm script.
    /// </summary>
    public class DebugSymbols
    {
        /// <summary>
        /// Type of the debug symbols.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the external symbol source.
        /// </summary>
        public string ExternalURL
        {
            get;
            set;
        }
    }
}