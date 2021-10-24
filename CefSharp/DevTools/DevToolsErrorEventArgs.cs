// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevToolsErrorEventArgs - Raised when an exception occurs when
    /// attempting to raise <see cref="IDevToolsClient.DevToolsEvent"/>
    /// </summary>
    public class DevToolsErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Event Name
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// Json
        /// </summary>
        public string Json { get; private set; }

        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// DevToolsErrorEventArgs
        /// </summary>
        /// <param name="eventName">Event Name</param>
        /// <param name="json">json</param>
        /// <param name="ex">Exception</param>
        public DevToolsErrorEventArgs(string eventName, string json, Exception ex)
        {
            EventName = eventName;
            Json = json;
            Exception = ex;
        }
    }
}
