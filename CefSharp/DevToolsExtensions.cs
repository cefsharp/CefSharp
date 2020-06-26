// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.Web;

namespace CefSharp
{
    /// <summary>
    /// Extensions for accessing DevTools through <see cref="IBrowserHost"/>
    /// </summary>
    public static class DevToolsExtensions
    {
        /// <summary>
        /// Execute a method call over the DevTools protocol. This is a more structured
        /// version of SendDevToolsMessage.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="paramsAsJson"/> dictionary contents.
        /// See the SendDevToolsMessage documentation for additional usage information.
        /// </summary>
        /// <param name="messageId">is an incremental number that uniquely identifies the message (pass 0 to have the next number assigned
        /// automatically based on previous values)</param>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a <see cref="JsonString"/>,
        /// which may be empty.</param>
        /// <returns>return the assigned message Id if called on the CEF UI thread and the message was
        /// successfully submitted for validation, otherwise 0</returns>
        public static int ExecuteDevToolsMethod(this IBrowserHost browserHost, int messageId, string method, JsonString parameters)
        {
            WebBrowserExtensions.ThrowExceptionIfBrowserHostNull(browserHost);

            var json = parameters == null ? null : parameters.Json;

            return browserHost.ExecuteDevToolsMethod(messageId, method, json);
        }
    }
}
