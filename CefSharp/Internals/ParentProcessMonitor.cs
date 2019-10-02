// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CefSharp.Internals
{
    /// <summary>
    /// Monitor the parent process and exit if the parent process closes
    /// before the subprocess. This class is used by the CefSharp.BrowserSubprocess to
    /// self terminate if the parent dies without notifying it to exit.
    /// See https://github.com/cefsharp/CefSharp/issues/2359 for more information.
    /// </summary>
    public static class ParentProcessMonitor
    {
        /// <summary>
        /// Starts a long running task (spawns new thread) used to monitor the parent process
        /// and calls <see cref="Process.Kill"/> if the parent exits unexpectedly (usually result of a crash).
        /// </summary>
        /// <param name="parentProcessId">process Id of the parent application</param>
        public static void StartMonitorTask(int parentProcessId)
        {
            Task.Factory.StartNew(() => AwaitParentProcessExit(parentProcessId), TaskCreationOptions.LongRunning);
        }

        private static async void AwaitParentProcessExit(int parentProcessId)
        {
            try
            {
                var parentProcess = Process.GetProcessById(parentProcessId);
                parentProcess.WaitForExit();
            }
            catch (Exception e)
            {
                //main process probably died already
                Debug.WriteLine(e);
            }

            await Task.Delay(1000); //wait a bit before exiting

            Debug.WriteLine("BrowserSubprocess shutting down forcibly.");

            Process.GetCurrentProcess().Kill();
        }
    }
}
