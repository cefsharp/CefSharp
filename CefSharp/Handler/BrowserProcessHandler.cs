// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Handler
{
    /// <summary>
    /// Inherit from this class to handle events related to browser process callbacks.
    /// The methods of this class will be called on the CEF UI thread unless otherwise indicated. . 
    /// </summary>
    public class BrowserProcessHandler : IBrowserProcessHandler
    {
        private bool isDisposed;

        /// <inheritdoc/>
        void IBrowserProcessHandler.OnContextInitialized()
        {
            OnContextInitialized();
        }

        /// <summary>
        /// Called on the CEF UI thread immediately after the CEF context has been initialized.
        /// You can now access the Global RequestContext through Cef.GetGlobalRequestContext() - this is the
        /// first place you can set Preferences (e.g. proxy settings, spell check dictionaries).
        /// </summary>
        protected virtual void OnContextInitialized()
        {

        }

        /// <inheritdoc/>
        void IBrowserProcessHandler.OnScheduleMessagePumpWork(long delay)
        {
            OnScheduleMessagePumpWork(delay);
        }

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
        protected virtual void OnScheduleMessagePumpWork(long delay)
        {

        }

        /// <summary>
        /// IsDisposed
        /// </summary>
        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        protected virtual void Dispose(bool disposing)
        {
            isDisposed = true;
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
