// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#ifndef CEFSHARP_CORE_CEFREFCOUNTEDMANAGED_H
#define CEFSHARP_CORE_CEFREFCOUNTEDMANAGED_H
#pragma once

#include <winnt.h>

#include "include\base\cef_macros.h"

class CefRefCountManaged
{
public:
    CefRefCountManaged() : ref_count_(0)
    {
    }

    ///
    // Increment the reference count.
    ///
    void AddRef() const
    {
        InterlockedIncrement(&ref_count_);
    }

    ///
    // Decrement the reference count. Returns true if the reference count is 0.
    ///
    bool Release() const
    {
        LONG res = InterlockedDecrement(&ref_count_);

        return res == 0;
    }

    ///
    // Returns true if the reference count is 1.
    ///
    bool HasOneRef() const
    {
        LONG res = InterlockedCompareExchange(&ref_count_, 0, 0);

        return res == 1;
    }

    ///
    // Returns true if the reference count is at least 1.
    ///
    bool HasAtLeastOneRef() const
    {
        LONG res = InterlockedCompareExchange(&ref_count_, 0, 0);

        return res > 0;
    }

private:
    mutable volatile LONG ref_count_;
    DISALLOW_COPY_AND_ASSIGN(CefRefCountManaged);
};


///
// Macro that provides a reference counting implementation for classes extending
// CefBase.
///
#define IMPLEMENT_REFCOUNTINGM(ClassName)                            \
 public:                                                             \
  void AddRef() const override { ref_count_.AddRef(); }              \
  bool Release() const override {                                    \
    if (ref_count_.Release()) {                                      \
      delete static_cast<const ClassName*>(this);                    \
      return true;                                                   \
    }                                                                \
    return false;                                                    \
  }                                                                  \
  bool HasOneRef() const override { return ref_count_.HasOneRef(); } \
  bool HasAtLeastOneRef() const override {                           \
    return ref_count_.HasAtLeastOneRef();                            \
  }                                                                  \
                                                                     \
 private:                                                            \
  CefRefCountManaged ref_count_

#endif  // CEFSHARP_CORE_CEFREFCOUNTEDMANAGED_H
