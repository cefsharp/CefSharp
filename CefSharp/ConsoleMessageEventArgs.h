// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the ConsoleMessage event handler set up in IWebBrowser.
    /// </summary>
    public ref class ConsoleMessageEventArgs : EventArgs
    {
    private:
        String^ _message;
        String^ _source;
        int _line;

    internal:
        ConsoleMessageEventArgs(String^ message, String^ source, int line)
            : _message(message), _source(source), _line(line) {}

    public:
        /// <summary>
        /// The message text of the console message.
        /// </summary>
        property String^ Message { String^ get() { return _message; } }

        /// <summary>
        /// The source of the console message.
        /// </summary>
        property String^ Source { String^ get() { return _source; } }

        /// <summary>
        /// The line number that generated the console message.
        /// </summary>
        property int Line { int get() { return _line; } }
    };

    /// <summary>
    /// A delegate type used to listen to ConsoleMessage events.
    /// </summary>
    public delegate void ConsoleMessageEventHandler(Object^ sender, ConsoleMessageEventArgs^ e);
}