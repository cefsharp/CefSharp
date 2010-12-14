#include "stdafx.h"

#include "RtzCountdownEvent.h"

namespace CefSharp
{

void WriteLine(String^ arg)
{
    // Console::WriteLine(arg);
}

void WriteLine(String^ format, Object^ arg0, Object^ arg1)
{
    // Console::WriteLine(format, arg0, arg1);
}

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

    //SpinWait^ wait = gcnew SpinWait();

addLoop:
    num = _currentCount;

    if(Interlocked::CompareExchange(_currentCount, num + count, num) != num)
    {
        //wait->SpinOnce();
        Thread::SpinWait(1);
        goto addLoop;
    }

    if(num == count)
    {
        _event->Reset();
    }

    WriteLine("RtzCountdownEvent AddCount {0} -> {1}", num, num + count);
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

    //SpinWait^ wait = gcnew SpinWait();

signalLoop:
    num = _currentCount;
    int newCount = Math::Max(0, num - count);

    if(Interlocked::CompareExchange(_currentCount, newCount, num) != num)
    {
        //wait->SpinOnce();
        Thread::SpinWait(1);
        goto signalLoop;
    }

    if(num <= count)
    {
        _event->Set();
        WriteLine("RtzCountdownEvent Signal {0} -> {1} ***", num, newCount);
        return true;
    }

    WriteLine("RtzCountdownEvent Signal {0} -> {1}", num, newCount);

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
    WriteLine("RtzCountdownEvent Reset");
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