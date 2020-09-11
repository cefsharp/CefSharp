// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Log
{
    /// <summary>
    /// Log entry.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute]
    public class LogEntry : CefSharp.DevTools.DevToolsDomainEntityBase
    {
        /// <summary>
        /// Log entry source.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("source"), IsRequired = (true))]
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Log entry severity.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("level"), IsRequired = (true))]
        public string Level
        {
            get;
            set;
        }

        /// <summary>
        /// Logged text.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("text"), IsRequired = (true))]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Timestamp when this entry was added.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("timestamp"), IsRequired = (true))]
        public long Timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the resource if known.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("url"), IsRequired = (false))]
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the resource.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("lineNumber"), IsRequired = (false))]
        public int? LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript stack trace.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("stackTrace"), IsRequired = (false))]
        public CefSharp.DevTools.Runtime.StackTrace StackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the network request associated with this entry.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("networkRequestId"), IsRequired = (false))]
        public string NetworkRequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the worker associated with this entry.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("workerId"), IsRequired = (false))]
        public string WorkerId
        {
            get;
            set;
        }

        /// <summary>
        /// Call arguments.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute(Name = ("args"), IsRequired = (false))]
        public System.Collections.Generic.IList<CefSharp.DevTools.Runtime.RemoteObject> Args
        {
            get;
            set;
        }
    }
}