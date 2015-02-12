#include "Stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public ref class LoadStartedEventArgs : EventArgs
    {
        String^ _url;
        bool _isMainFrame;

    public:
        LoadStartedEventArgs(String^ url, bool isMainFrame)
            : _url(url), _isMainFrame(isMainFrame) {}

        property String^ Url { String^ get() { return _url; } }
        property bool IsMainFrame { bool get() { return _isMainFrame; } }
    };

    public delegate void LoadStartedEventHandler(Object^ sender, LoadStartedEventArgs^ url);
}