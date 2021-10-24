// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp
{
    /// <summary>
    /// Managed enum for cef_thread_id_t/CefThreadId
    /// </summary>
    public enum CefThreadIds
    {
        // BROWSER PROCESS THREADS -- Only available in the browser process.

        /// <summary>
        /// The CEF UI thread in the browser. In CefSharp this is by default
        /// different from your main applications UI Thread
        /// (e.g. WPF/WinForms UI Threads). Only when MultiThreadedMessageLoop
        /// is false will this be the same as your app UI Thread.
        /// </summary>
        TID_UI,

        /// <summary>
        /// Used for blocking tasks (e.g. file system access) where the user won't
        /// notice if the task takes an arbitrarily long time to complete. All tasks
        /// posted after <see cref="IBrowserProcessHandler.OnContextInitialized"/>
        /// and before Cef.Shutdown() are guaranteed to run.
        /// </summary>
        TID_FILE_BACKGROUND,

        /// <summary>
        /// Used for blocking tasks (e.g. file system access) that affect UI or
        /// responsiveness of future user interactions. Do not use if an immediate
        /// response to a user interaction is expected. All tasks posted after
        /// <see cref="IBrowserProcessHandler.OnContextInitialized"/> and before Cef.Shutdown()
        /// are guaranteed to run.
        /// Examples:
        /// - Updating the UI to reflect progress on a long task.
        /// - Loading data that might be shown in the UI after a future user
        ///   interaction.
        /// </summary>
        TID_FILE_USER_VISIBLE,

        /// <summary>
        /// Used for blocking tasks (e.g. file system access) that affect UI
        /// immediately after a user interaction. All tasks posted after
        /// <see cref="IBrowserProcessHandler.OnContextInitialized"/> and before Cef.Shutdown()
        /// are guaranteed to run.
        /// Example: Generating data shown in the UI immediately after a click.
        /// </summary>
        TID_FILE_USER_BLOCKING,

        /// <summary>
        /// Used to launch and terminate browser processes.
        /// </summary>
        TID_PROCESS_LAUNCHER,

        /// <summary>
        /// Used to process IPC and network messages. Do not perform blocking tasks on
        /// this thread. All tasks posted after <see cref="IBrowserProcessHandler.OnContextInitialized"/>
        /// and before Cef.Shutdown() are guaranteed to run.
        /// </summary>
        TID_IO,

        // RENDER PROCESS THREADS -- Only available in the render process.

        /// <summary>
        /// The main thread in the renderer. Used for all WebKit and V8 interaction.
        /// Tasks may be posted to this thread after
        /// CefRenderProcessHandler::OnWebKitInitialized but are not guaranteed to
        /// run before sub-process termination (sub-processes may be killed at any time
        /// without warning).
        /// </summary>
        TID_RENDERER,
    }
}
