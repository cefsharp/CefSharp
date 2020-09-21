// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Stack entry for runtime errors and assertions.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class CallFrame : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// JavaScript function name.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("functionName"), IsRequired = (true))]
        public string FunctionName
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script id.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptId"), IsRequired = (true))]
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script name or url.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (true))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script line number (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (true))]
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript script column number (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnNumber"), IsRequired = (true))]
        public int ColumnNumber
        {
            get;
            set;
        }
    }
}