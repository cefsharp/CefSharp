// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Location in the source code.
    /// </summary>
    public class ScriptPosition : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (true))]
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnNumber"), IsRequired = (true))]
        public int ColumnNumber
        {
            get;
            set;
        }
    }
}