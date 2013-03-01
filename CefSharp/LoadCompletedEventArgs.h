#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public ref class LoadCompletedEventArgs : EventArgs
    {
        String^ _url;

    public:
        LoadCompletedEventArgs(String^ url)
            : _url(url) {}

        property String^ Url { String^ get() { return _url; } }
    };

    public delegate void LoadCompletedEventHandler(Object^ sender, LoadCompletedEventArgs^ url);
}