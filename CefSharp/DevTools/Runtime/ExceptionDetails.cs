// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Runtime
{
    /// <summary>
    /// Detailed information about exception (or error) that was thrown during script compilation or
    /// execution.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class ExceptionDetails : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Exception id.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("exceptionId"), IsRequired = (true))]
        public int ExceptionId
        {
            get;
            set;
        }

        /// <summary>
        /// Exception text, which should be used together with exception object when available.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (true))]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Line number of the exception location (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (true))]
        public int LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Column number of the exception location (0-based).
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("columnNumber"), IsRequired = (true))]
        public int ColumnNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Script ID of the exception location.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("scriptId"), IsRequired = (false))]
        public string ScriptId
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the exception location, to be used when the script was not reported.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (false))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript stack trace if available.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("stackTrace"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.StackTrace StackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Exception object if available.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("exception"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.RemoteObject Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the context where exception happened.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("executionContextId"), IsRequired = (false))]
        public int? ExecutionContextId
        {
            get;
            set;
        }
    }
}