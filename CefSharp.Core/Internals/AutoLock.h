#pragma once

#include <Windows.h>

namespace CefSharp
{
  namespace Internals
  {
    private class CriticalSection
    {
    public:
      CriticalSection()
      {
        ::InitializeCriticalSection(&_cs);
      }

      ~CriticalSection()
      {
        ::DeleteCriticalSection(&_cs);
      }

      void Enter()
      {
        ::EnterCriticalSection(&_cs);
      }
      void Leave()
      {
        ::LeaveCriticalSection(&_cs);
      }

    private:
      CriticalSection(const CriticalSection&);
      CriticalSection& operator=(const CriticalSection&);

      CRITICAL_SECTION _cs;
    };

    private class AutoLock
    {
    public:
      AutoLock(CriticalSection& cs) : _cs(cs)
      {
        _cs.Enter();
      }

      ~AutoLock()
      {
        _cs.Leave();
      }

    private:
      AutoLock(const AutoLock&);
      AutoLock& operator=(const AutoLock&);

      CriticalSection& _cs;
    };
  };
};