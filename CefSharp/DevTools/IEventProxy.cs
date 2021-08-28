// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

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
        /// <param name="stream">json Stream</param>
        void Raise(object sender, string eventName, Stream stream);
    }
}
