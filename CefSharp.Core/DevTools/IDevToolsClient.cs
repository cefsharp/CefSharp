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
    public interface IDevToolsClient : IDisposable
    {
        /// <summary>
        /// Will be called on receipt of a DevTools protocol event. Events by default are disabled and need to be
        /// enabled on a per domain basis, e.g. Sending Network.enable (or calling <see cref="Network.NetworkClient.EnableAsync(int?, int?, int?)"/>)
        /// to enable network related events.
        /// </summary>
        event EventHandler<DevToolsEventArgs> DevToolsEvent;

        /// <summary>
        /// Will be called when an error occurs when attempting to raise <see cref="DevToolsEvent"/>
        /// </summary>
        event EventHandler<DevToolsErrorEventArgs> DevToolsEventError;

        /// <summary>
        /// Add event handler for a DevTools protocol event. Events by default are disabled and need to be
        /// enabled on a per domain basis, e.g. Sending Network.enable (or calling <see cref="Network.NetworkClient.EnableAsync(int?, int?, int?)"/>)
        /// to enable network related events.
        /// </summary>
        /// <typeparam name="T">The event args type to which the event will be deserialized to.</typeparam>
        /// <param name="eventName">is the event name to listen to</param>
        /// <param name="eventHandler">event handler to call when the event occurs</param>
        void AddEventHandler<T>(string eventName, EventHandler<T> eventHandler) where T : EventArgs;

        /// <summary>
        /// Remove event handler for a DevTools protocol event.
        /// </summary>
        /// <typeparam name="T">The event args type to which the event will be deserialized to.</typeparam>
        /// <param name="eventName">is the event name to listen to</param>
        /// <param name="eventHandler">event handler to call when the event occurs</param>
        /// <returns>
        /// Returns false if all handlers for the <paramref name="eventName"/> have been removed,
        /// otherwise returns true if there are still handlers registered.
        /// </returns>
        bool RemoveEventHandler<T>(string eventName, EventHandler<T> eventHandler) where T : EventArgs;

        /// <summary>
        /// Execute a method call over the DevTools protocol. This method can be called on any thread.
        /// See the DevTools protocol documentation at https://chromedevtools.github.io/devtools-protocol/ for details
        /// of supported methods and the expected <paramref name="parameters"/> dictionary contents.
        /// </summary>
        /// <typeparam name="T">The type to which the method result will be deserialzed to.</typeparam>
        /// <param name="method">is the method name</param>
        /// <param name="parameters">are the method parameters represented as a dictionary,
        /// which may be empty.</param>
        /// <returns>return a Task that can be awaited to obtain the method result</returns>
        Task<T> ExecuteDevToolsMethodAsync<T>(string method, IDictionary<string, object> parameters = null) where T : DevToolsDomainResponseBase;
    }
}
