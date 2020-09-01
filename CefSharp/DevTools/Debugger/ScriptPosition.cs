// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Location in the source code.
    /// </summary>
    public class ScriptPosition
    {
        /// <summary>
        /// 
        /// </summary>
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ColumnNumber
        {
            get;
            set;
        }
    }
}