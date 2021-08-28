// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

//Originally based on https://github.com/CefNet/CefNet.DevTools.Protocol/blob/0a124720474a469b5cef03839418f5e1debaf2f0/CefNet.DevTools.Protocol/Internal/EventProxy.T.cs

using System;
using System.IO;

namespace CefSharp.DevTools
{
    /// <summary>
    /// Generic Typed Event Proxy
    /// </summary>
    /// <typeparam name="T">Event Args Type</typeparam>
    internal class EventProxy<T> : IEventProxy
    {
        private event EventHandler<T> handlers;
        private Func<string, Stream, T> convert;

        public EventProxy(Func<string, Stream, T> convert)
        {
            this.convert = convert;
        }

        public void AddHandler(EventHandler<T> handler)
        {
            handlers += handler;
        }

        /// <summary>
        /// Remove the event handler
        /// </summary>
        /// <param name="handler">event handler to remove</param>
        /// <returns>returns true if the last event handler for this proxy was removed.</returns>
        public bool RemoveHandler(EventHandler<T> handler)
        {
            handlers -= handler;

            return handlers == null;
        }

        public void Raise(object sender, string eventName, Stream stream)
        {
            stream.Position = 0;

            var args = convert(eventName, stream);

            handlers?.Invoke(sender, args);
        }

        public void Dispose()
        {
            handlers = null;
        }
    }
}
