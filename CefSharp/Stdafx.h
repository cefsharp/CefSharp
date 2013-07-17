#pragma once

#ifdef EXPORT
  #define DECL __declspec(dllexport)
#else
  #define DECL __declspec(dllimport)
#endif

#include "vcclr_local.h"

#include "include/cef_base.h"
#include "MCefRefPtr.h"
#include "Internals/StringUtils.h"
#include "TypeUtil.h"