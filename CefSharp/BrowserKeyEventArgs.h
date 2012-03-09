#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public ref class BrowserKeyEventArgs : EventArgs
    {

		int _type; 
		int _code; 
		int _modifiers; 
		bool _isSystemKey;

    public:
        BrowserKeyEventArgs(int type, int code, int modifiers, bool isSystemKey)
            : _type(type), _code(code), _modifiers(modifiers), _isSystemKey(isSystemKey) {}


        property int Type { int get() { return _type; } }
		property int Code { int get() { return _code; } }
		property int Modifiers { int get() { return _modifiers; } }
		property bool IsSystemKey { bool get() { return _isSystemKey; } }

    };

    public delegate void KeyEventHandler(Object^ sender, BrowserKeyEventArgs^ e);
}