// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <intrin.h>

#ifdef _M_ARM64
extern "C" unsigned __int64 __cdecl _CountOneBits64(unsigned __int64);
#pragma intrinsic(_CountOneBits64)
#endif

#ifdef EXPORT
#define DECL __declspec(dllexport)
#else
#define DECL __declspec(dllimport)
#endif

#include <vector>

#include <include/cef_base.h>

#include ".\..\CefSharp.Core.Runtime\Internals\CefRefCountManaged.h"
#include ".\..\CefSharp.Core.Runtime\Internals\MCefRefPtr.h"
#include ".\..\CefSharp.Core.Runtime\Internals\StringUtils.h"
#include "vcclr_local.h"

using namespace System;
using namespace CefSharp;
using namespace CefSharp::Internals;
