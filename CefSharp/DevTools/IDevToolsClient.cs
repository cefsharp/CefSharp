// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CefSharp.DevTools
{
    /// <summary>
    /// DevTools Client
    /// </summary>
    public interface IDevToolsClient
    {
        /// <summary>
        /// Will be called on receipt of a DevTools protocol event. Events by default are disabled and need to be
        /// enabled on a per domain basis, e.g. Sending Network.enable (or calling <see cref="Network.NetworkClient.EnableAsync(int?, int?, int?)"/>)
        /// to enable network related events.
        /// </summary>
        event EventHandler<DevToolsEventArgs> DevToolsEvent;

        /// <summary>
        /// Registers an event handler for a DevTools protocol event. Events by default are disabled and need to be
        /// enabled on a per domain basis, e.g. Sending Network.enable (or calling <see cref="Network.NetworkClient.EnableAsync(int?, int?, int?)"/>)
        /// to enable network related events.
        /// </summary>
        /// <typeparam name="T">The event args type to which the event will be deserialized to.</typeparam>
        /// <param name="eventName">is the event name to listen to</param>
        /// <param name="eventHandler">event handler to call when the event occurs</param>
        /// <returns>return an IRegistration that can be used to unregister the event handler.</returns>
        IRegistration RegisterEventHandler<T>(string eventName, EventHandler<T> eventHandler) where T : DevToolsDomainEventArgsBase;

        /// <summary>
        /// Execute a method call over the DevTools protocol. This method can be called on any thread.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="parameters"/> dictionary contents.
        /// </summary>
        /// <typeparam name="T">The type into which the result will be deserialzed.</typeparam>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a dictionary,
        /// which may be empty.</param>
        /// <returns>return a Task that can be awaited to obtain the method result</returns>
        Task<T> ExecuteDevToolsMethodAsync<T>(string method, IDictionary<string, object> parameters = null) where T : DevToolsDomainResponseBase;
    }
}
