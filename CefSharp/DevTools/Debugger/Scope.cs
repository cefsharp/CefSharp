// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Scope description.
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Scope type.
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Object representing the scope. For `global` and `with` scopes it represents the actual
        public Runtime.RemoteObject Object
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code where scope starts
        /// </summary>
        public Location StartLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Location in the source code where scope ends
        /// </summary>
        public Location EndLocation
        {
            get;
            set;
        }
    }
}