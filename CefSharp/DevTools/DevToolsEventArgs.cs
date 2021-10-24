// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTools Event EventAargs
    /// </summary>
    public class DevToolsEventArgs : DevToolsDomainEventArgsBase
    {
        /// <summary>
        /// Event Name
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// Event paramaters as Json string
        /// </summary>
        public string ParametersAsJsonString { get; private set; }

        public DevToolsEventArgs(string eventName, string paramsAsJsonString)
        {
            EventName = eventName;
            ParametersAsJsonString = paramsAsJsonString;
        }
    }
}
