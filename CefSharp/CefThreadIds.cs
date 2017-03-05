// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
        /// The CEF UI thread in the browser. In CefSharp this is ALWAYS
        /// separate from the application's main thread (and thus the main 
        /// WinForm UI thread).
        /// </summary>
        TID_UI,

        /// <summary>
        /// Used to interact with the database.
        /// </summary>
        TID_DB,

        /// <summary>
        /// Used to interact with the file system.
        /// </summary>
        TID_FILE,

        /// <summary>
        /// Used for file system operations that block user interactions.
        /// Responsiveness of this thread affects users.
        /// </summary>
        TID_FILE_USER_BLOCKING,

        /// <summary>
        /// Used to launch and terminate browser processes.
        /// </summary>
        TID_PROCESS_LAUNCHER,

        /// <summary>
        /// Used to handle slow HTTP cache operations.
        /// </summary>
        TID_CACHE,

        /// <summary>
        /// Used to process IPC and network messages.
        /// </summary>
        TID_IO,

        // RENDER PROCESS THREADS -- Only available in the render process.

        /// <summary>
        /// The main thread in the renderer. Used for all WebKit and V8 interaction.
        /// </summary>
        TID_RENDERER,
    }
}
