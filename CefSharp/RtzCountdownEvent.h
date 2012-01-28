#include "stdafx.h"

#pragma once

using namespace System;
using namespace System::Threading;

namespace CefSharp
{

    ///<summary>Return-To-Zero latch, signals whenever count is zero</summary>
    [System::Diagnostics::DebuggerDisplayAttribute("Current Count={CurrentCount}")]
    public ref class RtzCountdownEvent sealed
    {
        initonly ManualResetEvent^ _event;
        int _currentCount;
        bool _disposed;

    public:

        RtzCountdownEvent() :
            _event(gcnew ManualResetEvent(true)),
            _currentCount(0),
            _disposed(false) {}

        !RtzCountdownEvent()
        {
            if(_disposed)
            {
                _disposed = true;
                //_event->Dispose();
            }
        }

        ~RtzCountdownEvent()
        {
            this->!RtzCountdownEvent();
        }


        property int CurrentCount
        {
            int get() { return _currentCount; }
        }

        void AddCount(int count);
        void AddCount();
        bool Signal(int count);
        bool Signal();
        void Reset();
        void Wait();
        void Wait(int timeout);
    };

}