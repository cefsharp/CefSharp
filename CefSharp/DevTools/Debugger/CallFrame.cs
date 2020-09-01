// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// JavaScript call frame. Array of call frames form the call stack.
    /// </summary>
    public class CallFrame
    {
        /// <summary>
        /// Call frame identifier. This identifier is only valid while the virtual machine is paused.
        /// </summary>
        public string CallFrameId
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the JavaScript function called on this call frame.
        /// </summary>
        public string FunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code.
        /// </summary>
        public Location FunctionLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code.
        /// </summary>
        public Location Location
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script name or url.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Scope chain for this call frame.
        /// </summary>
        public System.Collections.Generic.IList<Scope> ScopeChain
        {
            get;
            set;
        }

        /// <summary>
        /// `this` object for this call frame.
        /// </summary>
        public Runtime.RemoteObject This
        {
            get;
            set;
        }

        /// <summary>
        /// The value being returned, if the function is at return point.
        /// </summary>
        public Runtime.RemoteObject ReturnValue
        {
            get;
            set;
        }
    }
}