// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;

namespace CefSharp
{
    public ref class ConsoleMessageEventArgs : EventArgs
    {
        String^ _message;
        String^ _source;
        int _line;

    public:
        ConsoleMessageEventArgs(String^ message, String^ source, int line)
            : _message(message), _source(source), _line(line) {}

        property String^ Message { String^ get() { return _message; } }
        property String^ Source { String^ get() { return _source; } }
        property int Line { int get() { return _line; } }
    };

    public delegate void ConsoleMessageEventHandler(Object^ sender, ConsoleMessageEventArgs^ e);
}