// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    /// <summary>
    /// Managed enum for cef_thread_id_t/CefThreadId
    /// </summary>
    public enum CefThreadIds
    {
        // BROWSER PROCESS THREADS -- Only available in the browser process.

        ///
        // The main thread in the browser. This will be the same as the main
        // application thread if CefInitialize() is called with a
        // CefSettings.multi_threaded_message_loop value of false.
        ///
        TID_UI,

        ///
        // Used to interact with the database.
        ///
        TID_DB,

        ///
        // Used to interact with the file system.
        ///
        TID_FILE,

        ///
        // Used for file system operations that block user interactions.
        // Responsiveness of this thread affects users.
        ///
        TID_FILE_USER_BLOCKING,

        ///
        // Used to launch and terminate browser processes.
        ///
        TID_PROCESS_LAUNCHER,

        ///
        // Used to handle slow HTTP cache operations.
        ///
        TID_CACHE,

        ///
        // Used to process IPC and network messages.
        ///
        TID_IO,

        // RENDER PROCESS THREADS -- Only available in the render process.

        ///
        // The main thread in the renderer. Used for all WebKit and V8 interaction.
        ///
        TID_RENDERER,
    }
}
