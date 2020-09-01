// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Location range within one script.
    /// </summary>
    public class LocationRange
    {
        /// <summary>
        /// 
        /// </summary>
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptPosition Start
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptPosition End
        {
            get;
            set;
        }
    }
}