// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// InstallabilityErrorArgument
    /// </summary>
    public class InstallabilityErrorArgument
    {
        /// <summary>
        /// Argument name (e.g. name:'minimum-icon-size-in-pixels').
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Argument value (e.g. value:'64').
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}