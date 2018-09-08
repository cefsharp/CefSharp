// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Implement this interface to handle events related to browser process callbacks.
    /// The methods of this class will be called on the CEF UI thread unless otherwise indicated. . 
    /// </summary>
    public interface IBrowserProcessHandler : IDisposable
    {
        /// <summary>
        /// Called on the CEF UI thread immediately after the CEF context has been initialized.
        /// You can now access the Global RequestContext through Cef.GetGlobalRequestContext() - this is the
        /// first place you can set Preferences (e.g. proxy settings, spell check dictionaries).
        /// </summary>
        void OnContextInitialized();

        /// <summary>
        /// Called from any thread when work has been scheduled for the browser process
        /// main (UI) thread. This callback is used in combination with CefSettings.
        /// ExternalMessagePump and Cef.DoMessageLoopWork() in cases where the CEF
        /// message loop must be integrated into an existing application message loop
        /// (see additional comments and warnings on Cef.DoMessageLoopWork). This
        /// callback should schedule a Cef.DoMessageLoopWork() call to happen on the
        /// main (UI) thread. 
        /// </summary>
        /// <param name="delay">is the requested delay in milliseconds. If
        /// delay is less than or equal to 0 then the call should happen reasonably soon. If
        /// delay is greater than 0 then the call should be scheduled to happen after the
        /// specified delay and any currently pending scheduled call should be
        /// cancelled.</param>
        void OnScheduleMessagePumpWork(long delay);
    }
}
