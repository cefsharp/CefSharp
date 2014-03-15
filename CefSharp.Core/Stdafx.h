// Copyright � 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#ifdef EXPORT
  #define DECL __declspec(dllexport)
#else
  #define DECL __declspec(dllimport)
#endif

#include <include/cef_base.h>

#include "Cef.h"
#include "Internals/MCefRefPtr.h"
#include "Internals/StringUtils.h"
#include "TypeUtils.h"
#include "vcclr_local.h"
