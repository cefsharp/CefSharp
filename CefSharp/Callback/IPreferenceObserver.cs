// Copyright © 2026 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Callback
{
    /// <summary>
    /// Implemented by the client to observe preference changes and registered via
    /// <see cref="IRequestContext.AddPreferenceObserver"/>. The methods of this class will
    /// be called on the browser process UI thread.
    /// </summary>
    public interface IPreferenceObserver : IDisposable
    {
        /// <summary>
        /// Called when a preference has changed. The new value can be retrieved using
        /// <see cref="IRequestContext.GetPreference"/>.
        /// </summary>
        /// <param name="name">preference key</param>
        void OnPreferenceChanged(string name);
    }
}
