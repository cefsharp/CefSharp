#pragma once

#ifdef EXPORT
  #define DECL __declspec(dllexport)
#else
  #define DECL __declspec(dllimport)
#endif

#include <vcclr.h>
#include "cef.h"
#include "MCefRefPtr.h"
#include "Utils.h"