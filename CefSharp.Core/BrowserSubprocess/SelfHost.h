// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        /// <summary>
        /// SelfHost can be used to self host the BrowserSubProcess in your
        /// existing application (preferred approach for .Net Core).
        /// </summary>
        public ref class SelfHost
        {
        public:
            /// <summary>
            /// This function should be called from the application entry point function (typically Program.Main)
            /// to execute a secondary process e.g. gpu, plugin, renderer, utility
            /// This overload is primarily intended to be used for .Net Core.
            /// - Pass in command line args
            /// - To support High DPI Displays you should call  Cef.EnableHighDPISupport before any other processing
            /// or add the relevant entries to your app.manifest
            /// </summary>
            /// <param name="args">command line args</param>
            /// <returns>
            /// If called for the browser process (identified by no "type" command-line value) it will return immediately
            /// with a value of -1. If called for a recognized secondary process it will block until the process should exit
            /// and then return the process exit code.
            /// </returns
            static int Main(array<String^>^ args);
            
        };
    }
}
