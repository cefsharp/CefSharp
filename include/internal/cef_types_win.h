// Copyright (c) 2009 Marshall A. Greenblatt. All rights reserved.
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


#ifndef CEF_INCLUDE_INTERNAL_CEF_TYPES_WIN_H_
#define CEF_INCLUDE_INTERNAL_CEF_TYPES_WIN_H_
#pragma once

#include "include/internal/cef_build.h"

#if defined(OS_WIN)
#include <windows.h>
#include "include/internal/cef_string.h"

#ifdef __cplusplus
extern "C" {
#endif

// Handle types.
#define cef_cursor_handle_t HCURSOR
#define cef_event_handle_t MSG*
#define cef_window_handle_t HWND

///
// Structure representing CefExecuteProcess arguments.
///
typedef struct _cef_main_args_t {
  HINSTANCE instance;
} cef_main_args_t;

///
// Structure representing window information.
///
typedef struct _cef_window_info_t {
  // Standard parameters required by CreateWindowEx()
  DWORD ex_style;
  cef_string_t window_name;
  DWORD style;
  int x;
  int y;
  int width;
  int height;
  cef_window_handle_t parent_window;
  HMENU menu;

  // If window rendering is disabled no browser window will be created. Set
  // |parent_window| to be used for identifying monitor info
  // (MonitorFromWindow). If |parent_window| is not provided the main screen
  // monitor will be used.
  BOOL window_rendering_disabled;

  // Set to true to enable transparent painting.
  // If window rendering is disabled and |transparent_painting| is set to true
  // WebKit rendering will draw on a transparent background (RGBA=0x00000000).
  // When this value is false the background will be white and opaque.
  BOOL transparent_painting;

  // Handle for the new browser window.
  cef_window_handle_t window;
} cef_window_info_t;

#ifdef __cplusplus
}
#endif

#endif  // OS_WIN

#endif  // CEF_INCLUDE_INTERNAL_CEF_TYPES_WIN_H_
