// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

// Originally Based on https://github.com/CefNet/CefNet.DevTools.Protocol/blob/0a124720474a469b5cef03839418f5e1debaf2f0/CefNet.DevTools.Protocol/IEventProxy.cs

using System;
using System.IO;
using System.Threading;

namespace CefSharp.DevTools
{
    /// <summary>
    /// Event Proxy
    /// </summary>
    internal interface IEventProxy : IDisposable
    {
        /// <summary>
        /// Raise Event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="eventName">event name</param>
        /// <param name="stream">Stream containing JSON</param>
        /// <param name="syncContext">SynchronizationContext</param>
        void Raise(object sender, string eventName, Stream stream, SynchronizationContext syncContext);
    }
}
