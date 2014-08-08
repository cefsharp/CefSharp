#include "Stdafx.h"
#pragma once

#include "StatusType.h"

using namespace System;

namespace CefSharp
{
    public ref class StatusMessageEventArgs : EventArgs
    {
        String^ _value;
        StatusType _type;

    public:
        StatusMessageEventArgs(String^ value, StatusType type)
            : _value(value), _type(type) {}

        property String^ Value { String^ get() { return _value; } }
        property StatusType Type { StatusType get() { return _type; } }
    };

    public delegate void StatusMessageEventHandler(Object^ sender, StatusMessageEventArgs^ e);
}