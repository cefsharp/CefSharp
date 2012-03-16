// Copyright (c) 2008 Marshall A. Greenblatt. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//    * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//    * Neither the name of Google Inc. nor the name Chromium Embedded
// Framework nor the names of its contributors may be used to endorse
// or promote products derived from this software without specific prior
// written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


#ifndef CEF_INCLUDE_INTERNAL_CEF_WIN_H_
#define CEF_INCLUDE_INTERNAL_CEF_WIN_H_
#pragma once

#if defined(OS_WIN)
#include <windows.h>
#include "include/internal/cef_types_win.h"
#include "include/internal/cef_types_wrappers.h"

///
// Atomic increment and decrement.
///
#define CefAtomicIncrement(p) InterlockedIncrement(p)
#define CefAtomicDecrement(p) InterlockedDecrement(p)

///
// Critical section wrapper.
///
class CefCriticalSection {
 public:
  CefCriticalSection() {
    memset(&m_sec, 0, sizeof(CRITICAL_SECTION));
    InitializeCriticalSection(&m_sec);
  }
  virtual ~CefCriticalSection() {
    DeleteCriticalSection(&m_sec);
  }
  void Lock() {
    EnterCriticalSection(&m_sec);
  }
  void Unlock() {
    LeaveCriticalSection(&m_sec);
  }

  CRITICAL_SECTION m_sec;
};

///
// Handle types.
///
#define CefWindowHandle cef_window_handle_t
#define CefCursorHandle cef_cursor_handle_t


struct CefWindowInfoTraits {
  typedef cef_window_info_t struct_type;

  static inline void init(struct_type* s) {}

  static inline void clear(struct_type* s) {
    cef_string_clear(&s->m_windowName);
  }

  static inline void set(const struct_type* src, struct_type* target,
      bool copy) {
    target->m_dwExStyle = src->m_dwExStyle;
    cef_string_set(src->m_windowName.str, src->m_windowName.length,
        &target->m_windowName, copy);
    target->m_dwStyle = src->m_dwStyle;
    target->m_x = src->m_x;
    target->m_y = src->m_y;
    target->m_nWidth = src->m_nWidth;
    target->m_nHeight = src->m_nHeight;
    target->m_hWndParent = src->m_hWndParent;
    target->m_hMenu = src->m_hMenu;
    target->m_bWindowRenderingDisabled = src->m_bWindowRenderingDisabled;
    target->m_bTransparentPainting = src->m_bTransparentPainting;
    target->m_hWnd = src->m_hWnd;
  }
};

///
// Class representing window information.
///
class CefWindowInfo : public CefStructBase<CefWindowInfoTraits> {
 public:
  typedef CefStructBase<CefWindowInfoTraits> parent;

  CefWindowInfo() : parent() {}
  explicit CefWindowInfo(const cef_window_info_t& r) : parent(r) {}
  explicit CefWindowInfo(const CefWindowInfo& r) : parent(r) {}

  void SetAsChild(HWND hWndParent, RECT windowRect) {
    m_dwStyle = WS_CHILD | WS_CLIPCHILDREN | WS_CLIPSIBLINGS | WS_TABSTOP |
                WS_VISIBLE;
    m_hWndParent = hWndParent;
    m_x = windowRect.left;
    m_y = windowRect.top;
    m_nWidth = windowRect.right - windowRect.left;
    m_nHeight = windowRect.bottom - windowRect.top;
  }

  void SetAsPopup(HWND hWndParent, const CefString& windowName) {
    m_dwStyle = WS_OVERLAPPEDWINDOW | WS_CLIPCHILDREN | WS_CLIPSIBLINGS |
                WS_VISIBLE;
    m_hWndParent = hWndParent;
    m_x = CW_USEDEFAULT;
    m_y = CW_USEDEFAULT;
    m_nWidth = CW_USEDEFAULT;
    m_nHeight = CW_USEDEFAULT;

    cef_string_copy(windowName.c_str(), windowName.length(), &m_windowName);
  }

  void SetAsOffScreen(HWND hWndParent) {
    m_bWindowRenderingDisabled = TRUE;
    m_hWndParent = hWndParent;
  }

  void SetTransparentPainting(BOOL transparentPainting) {
    m_bTransparentPainting = transparentPainting;
  }
};


struct CefPrintInfoTraits {
  typedef cef_print_info_t struct_type;

  static inline void init(struct_type* s) {}
  static inline void clear(struct_type* s) {}

  static inline void set(const struct_type* src, struct_type* target,
      bool copy) {
    target->m_hDC = src->m_hDC;
    target->m_Rect = src->m_Rect;
    target->m_Scale = src->m_Scale;
  }
};

///
// Class representing print context information.
///
typedef CefStructBase<CefPrintInfoTraits> CefPrintInfo;

#endif  // OS_WIN

#endif  // CEF_INCLUDE_INTERNAL_CEF_WIN_H_
