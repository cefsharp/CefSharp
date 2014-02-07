#include "stdafx.h"
#pragma once

#include "include/cef_client.h"

using namespace System;

class AppDomainSafeCefBase : public CefBase
{
public:
    AppDomainSafeCefBase()
    {
        _appDomainId = AppDomain::CurrentDomain->Id;
    }
    virtual ~AppDomainSafeCefBase()
    {
    }

    DWORD inline GetAppDomainId() {
        return _appDomainId;
    }

    bool inline IsCrossDomainCallRequired() {
        return AppDomain::CurrentDomain->Id != GetAppDomainId();
    }

private:
    DWORD _appDomainId;
};

#define IMPLEMENT_SAFE_REFCOUNTING(ClassName)                                \
  public:                                                                    \
    int AddRef() { return refct_.AddRef(); }                                 \
    int Release() {                                                          \
      int retval = refct_.Release();                                         \
      if (retval == 0)                                                       \
        if (IsCrossDomainCallRequired()) {                                   \
            msclr::call_in_appdomain(GetAppDomainId(), &_Release, this);     \
        } else {                                                             \
            _Release(this);                                                  \
        }                                                                    \
      return retval;                                                         \
    }                                                                        \
    int GetRefCt() { return refct_.GetRefCt(); }                             \
  private:                                                                   \
    CefRefCount refct_;                                                      \
    static void _Release(ClassName* const _this){                            \
        delete _this;                                                        \
    }