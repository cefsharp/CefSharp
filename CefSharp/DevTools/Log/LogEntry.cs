// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Log
{
    /// <summary>
    /// Log entry.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Log entry source.
        /// </summary>
        public string Source
        {
            get;
            set;
        }

        /// <summary>
        /// Log entry severity.
        /// </summary>
        public string Level
        {
            get;
            set;
        }

        /// <summary>
        /// Logged text.
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Timestamp when this entry was added.
        /// </summary>
        public long Timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// URL of the resource if known.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Line number in the resource.
        /// </summary>
        public int? LineNumber
        {
            get;
            set;
        }

        /// <summary>
        /// JavaScript stack trace.
        /// </summary>
        public Runtime.StackTrace StackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the network request associated with this entry.
        /// </summary>
        public string NetworkRequestId
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of the worker associated with this entry.
        /// </summary>
        public string WorkerId
        {
            get;
            set;
        }

        /// <summary>
        /// Call arguments.
        /// </summary>
        public System.Collections.Generic.IList<Runtime.RemoteObject> Args
        {
            get;
            set;
        }
    }
}