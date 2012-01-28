#include "stdafx.h"

#pragma once

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