// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Log
{
    /// <summary>
    /// Violation configuration setting.
    /// </summary>
    public class ViolationSetting
    {
        /// <summary>
        /// Violation type.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Time threshold to trigger upon.
        /// </summary>
        public long Threshold
        {
            get;
            set;
        }
    }
}