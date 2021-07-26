// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.IO;

namespace CefSharp.DevTools
{
    // Helper class for disposing event handler registrations
    internal sealed class DevToolsEventHandlerRegistration : IRegistration
    {
        private readonly string _eventName;
        private ConcurrentDictionary<string, EventHandler<Stream>> _eventHandlers;
        private EventHandler<Stream> _eventHandler;

        public DevToolsEventHandlerRegistration(string eventName, EventHandler<Stream> eventHandler, ConcurrentDictionary<string, EventHandler<Stream>> eventHandlers)
        {
            _eventName = eventName;
            _eventHandler = eventHandler;
            _eventHandlers = eventHandlers;
        }

        public void Dispose()
        {
            var eventHandler = _eventHandler;
            var eventHandlers = _eventHandlers;

            _eventHandler = null;
            _eventHandlers = null;

            if (eventHandler != null && eventHandlers != null)
            {
                EventHandler<Stream> eventRegistration;
                if (eventHandlers.TryGetValue(_eventName, out eventRegistration))
                {
                    eventRegistration -= eventHandler;
                    if (eventRegistration == null)
                    {
                        eventHandlers.TryRemove(_eventName, out _);
                    }
                }
            }
        }
    }
}
