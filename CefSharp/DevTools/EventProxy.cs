// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

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

        public EventProxy(EventHandler<T> handler, Func<string, Stream, T> convert)
        {
            handlers += handler;
            this.convert = convert;
        }

        public void AddHandler(EventHandler<T> handler)
        {
            handlers += handler;
        }

        public void RemoveHandler(EventHandler<T> handler)
        {
            handlers -= handler;
        }

        public void Raise(object sender, string eventName, Stream stream)
        {
            var args = convert(eventName, stream);

            handlers?.Invoke(sender, args);
        }

        public void Dispose()
        {
            handlers = null;
        }
    }
}
