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


#ifndef _CEF_WIN_H
#define _CEF_WIN_H

#ifdef _WIN32
#include <windows.h>
#include "cef_types_win.h"

// Atomic increment and decrement.
#define CefAtomicIncrement(p) InterlockedIncrement(p)
#define CefAtomicDecrement(p) InterlockedDecrement(p)

// Critical section wrapper.
class CefCriticalSection
{
public:
  CefCriticalSection()
  {
    memset(&m_sec, 0, sizeof(CRITICAL_SECTION));
    InitializeCriticalSection(&m_sec);
  }
  virtual ~CefCriticalSection()
  {
    DeleteCriticalSection(&m_sec);
  }
  void Lock()
  {
    EnterCriticalSection(&m_sec);
  }
  void Unlock()
  {
    LeaveCriticalSection(&m_sec);
  }

  CRITICAL_SECTION m_sec;
};

// Class representing window information.
class CefWindowInfo : public cef_window_info_t
{
public:
  CefWindowInfo()
  {
    Init();
  }
  virtual ~CefWindowInfo()
  {
    Reset();
  }

  CefWindowInfo(const CefWindowInfo& r)
  {
    Init();
    *this = r;
  }
  CefWindowInfo(const cef_window_info_t& r)
  {
    Init();
    *this = r;
  }

  void Reset()
  {
    cef_string_clear(&m_windowName);
    Init();
  }

  void Attach(const cef_window_info_t& r)
  {
    Reset();
    *static_cast<cef_window_info_t*>(this) = r;
  }

  void Detach()
  {
    Init();
  }

  CefWindowInfo& operator=(const CefWindowInfo& r)
  {
    return operator=(static_cast<const cef_window_info_t&>(r));
  }
  CefWindowInfo& operator=(const cef_window_info_t& r)
  {
    m_dwExStyle = r.m_dwExStyle;
    cef_string_copy(r.m_windowName.str, r.m_windowName.length, &m_windowName);
    m_dwStyle = r.m_dwStyle;
    m_x = r.m_x;
    m_y = r.m_y;
    m_nWidth = r.m_nWidth;
    m_nHeight = r.m_nHeight;
    m_hWndParent = r.m_hWndParent;
    m_hMenu = r.m_hMenu;
    m_hWnd = r.m_hWnd;
    return *this;
  }

  void SetAsChild(HWND hWndParent, RECT windowRect)
  {
    m_dwStyle = WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN
        | WS_CLIPSIBLINGS | WS_TABSTOP;
    m_hWndParent = hWndParent;
    m_x = windowRect.left;
    m_y = windowRect.top;
    m_nWidth = windowRect.right - windowRect.left;
    m_nHeight = windowRect.bottom - windowRect.top;
  }

  void SetAsPopup(HWND hWndParent, const CefString& windowName)
  {
    m_dwStyle = WS_OVERLAPPEDWINDOW | WS_CLIPCHILDREN | WS_CLIPSIBLINGS;
    m_hWndParent = hWndParent;
    m_x = CW_USEDEFAULT;
    m_y = CW_USEDEFAULT;
    m_nWidth = CW_USEDEFAULT;
    m_nHeight = CW_USEDEFAULT;

    cef_string_copy(windowName.c_str(), windowName.length(), &m_windowName);
  }

protected:
  void Init()
  {
    memset(static_cast<cef_window_info_t*>(this), 0, sizeof(cef_window_info_t));
  }
};

// Class representing print context information.
class CefPrintInfo : public cef_print_info_t
{
public:
  CefPrintInfo()
  {
    Init();
  }
  virtual ~CefPrintInfo()
  {
  }

  CefPrintInfo(const CefPrintInfo& r)
  {
    Init();
    *this = r;
  }
  CefPrintInfo(const cef_print_info_t& r)
  {
    Init();
    *this = r;
  }

  void Init()
  {
    m_hDC = NULL;
    m_Rect.left = m_Rect.right = m_Rect.top = m_Rect.bottom = 0;
    m_Scale = 0;
  }

  CefPrintInfo& operator=(const CefPrintInfo& r)
  {
    return operator=(static_cast<const cef_print_info_t&>(r));
  }
  CefPrintInfo& operator=(const cef_print_info_t& r)
  {
    m_hDC = r.m_hDC;
    m_Rect.left = r.m_Rect.left;
    m_Rect.right = r.m_Rect.right;
    m_Rect.top = r.m_Rect.top;
    m_Rect.bottom = r.m_Rect.bottom;
    m_Scale = r.m_Scale;
    return *this;
  }
};

// Window handle.
#define CefWindowHandle cef_window_handle_t
#endif // _WIN32

#endif // _CEF_WIN_H
