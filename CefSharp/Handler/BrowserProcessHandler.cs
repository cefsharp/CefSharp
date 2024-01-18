// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

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

        /// <inheritdoc/>
        bool IBrowserProcessHandler.OnAlreadyRunningAppRelaunch(IReadOnlyDictionary<string, string> commandLine, string currentDirectory)
        {
            return OnAlreadyRunningAppRelaunch(commandLine, currentDirectory);
        }

        /// <summary>
        /// Implement this method to provide app-specific behavior when an already
        /// running app is relaunched with the same CefSettings.RootCachePath value.
        /// For example, activate an existing app window or create a new app window.
        /// 
        /// To avoid cache corruption only a single app instance is allowed to run for
        /// a given CefSettings.RootCachePath value. On relaunch the app checks a
        /// process singleton lock and then forwards the new launch arguments to the
        /// already running app process before exiting early. Client apps should
        /// therefore check the Cef.Initialize() return value for early exit before
        /// proceeding.
        ///
        /// It's important to note that this method will be called on a CEF UI thread,
        /// which by default is not the same as your application UI thread.
        ///
        /// </summary>
        /// <param name="commandLine">Command line arugments/switches</param>
        /// <param name="currentDirectory">current directory (optional)</param>
        /// <returns>
        /// Return true if the relaunch is handled or false for default relaunch
        /// behavior. Default behavior will create a new default styled Chrome window.
        /// </returns>
        /// <remarks>
        /// The <paramref name="commandLine"/> dictionary may contain keys that have empty string values
        /// (arugments).
        /// </remarks>
        protected virtual bool OnAlreadyRunningAppRelaunch(IReadOnlyDictionary<string, string> commandLine, string currentDirectory)
        {
            return false;
        }

        /// <summary>
        /// IsDisposed
        /// </summary>
        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        /// <summary>
        /// Disposes of the resources
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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
