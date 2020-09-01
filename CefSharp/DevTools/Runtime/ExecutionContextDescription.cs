// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Description of an isolated world.
    /// </summary>
    public class ExecutionContextDescription
    {
        /// <summary>
        /// Unique id of the execution context. It can be used to specify in which execution context
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Execution context origin.
        /// </summary>
        public string Origin
        {
            get;
            set;
        }

        /// <summary>
        /// Human readable name describing given context.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Embedder-specific auxiliary data.
        /// </summary>
        public object AuxData
        {
            get;
            set;
        }
    }
}