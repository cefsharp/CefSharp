#include "stdafx.h"

#include "RtzCountdownEvent.h"

namespace CefSharp
{
    void RtzCountdownEvent::AddCount(int count)
    {
        int num;

        if(count <= 0)
        {
            throw gcnew ArgumentOutOfRangeException("count");
        }

        if(_disposed)
        {
            throw gcnew ObjectDisposedException("RtzCountdownEvent");
        }

    addLoop:
        num = _currentCount;

        if(Interlocked::CompareExchange(_currentCount, num + count, num) != num)
        {
            Thread::SpinWait(1);
            goto addLoop;
        }

        if(num == count)
        {
            _event->Reset();
        }
    }


    bool RtzCountdownEvent::Signal(int count)
    {
        int num;

        if(count <= 0)
        {
            throw gcnew ArgumentOutOfRangeException("count");
        }

        if(_disposed)
        {
            throw gcnew ObjectDisposedException("RtzCountdownEvent");
        }

    signalLoop:
        num = _currentCount;
        int newCount = Math::Max(0, num - count);

        if(Interlocked::CompareExchange(_currentCount, newCount, num) != num)
        {
            Thread::SpinWait(1);
            goto signalLoop;
        }

        if(num <= count)
        {
            _event->Set();
            return true;
        }

        return false;
    }

    void RtzCountdownEvent::AddCount()
    {
        AddCount(1);
    }

    bool RtzCountdownEvent::Signal()
    {
        return Signal(1);
    }

    void RtzCountdownEvent::Reset()
    {
        if(_disposed)
        {
            throw gcnew ObjectDisposedException("RtzCountdownEvent");
        }

        _currentCount = 0;
        _event->Reset();
    }

    void RtzCountdownEvent::Wait()
    {
        _event->WaitOne();
    }

    void RtzCountdownEvent::Wait(int timeout)
    {
        _event->WaitOne(timeout);
    }
};