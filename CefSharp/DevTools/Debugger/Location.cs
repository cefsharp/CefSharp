// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Debugger
{
    /// <summary>
    /// Location in the source code.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class Location : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Script identifier as reported in the `Debugger.scriptParsed`.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptId"), IsRequired = (true))]
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the script (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (true))]
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Column number in the script (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnNumber"), IsRequired = (false))]
        public int? ColumnNumber
        {
            get;
            set;
        }
    }
}