#include "Stdafx.h"
#pragma once

using namespace System;

namespace CefSharp
{
    public ref class LoadCompletedEventArgs : EventArgs
    {
        String^ _url;
        bool _isMainFrame;

    public:
        LoadCompletedEventArgs(String^ url, bool isMainFrame)
            : _url(url), _isMainFrame(isMainFrame) {}

        property String^ Url { String^ get() { return _url; } }
        property bool IsMainFrame { bool get() { return _isMainFrame; } }
    };

    public delegate void LoadCompletedEventHandler(Object^ sender, LoadCompletedEventArgs^ url);
}