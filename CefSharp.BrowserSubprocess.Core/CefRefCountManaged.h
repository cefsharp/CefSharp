// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

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
        
    }

    ///
    // Decrement the reference count. Returns true if the reference count is 0.
    ///
    bool Release() const
    {
        return false;
    }

    ///
    // Returns true if the reference count is 1.
    ///
    bool HasOneRef() const
    {
        return false;
    }

    ///
    // Returns true if the reference count is at least 1.
    ///
    bool HasAtLeastOneRef() const
    {
        return false;
    }

private:
    int ref_count_;
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
