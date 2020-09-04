// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// BreakLocation
    /// </summary>
    public class BreakLocation
    {
        /// <summary>
        /// Script identifier as reported in the `Debugger.scriptParsed`.
        /// </summary>
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the script (0-based).
        /// </summary>
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Column number in the script (0-based).
        /// </summary>
        public int? ColumnNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get;
            set;
        }
    }
}