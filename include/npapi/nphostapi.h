// Copyright (c) 2010 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

#ifndef _NPHOSTAPI_H_
#define _NPHOSTAPI_H_

#include "port.h"
#include "npapi.h"
#include "npapi_extensions.h"
#include "npfunctions.h"
#include "npruntime.h"

#ifdef __cplusplus
extern "C" {
#endif

//
// NPAPI library entry points
//
#if defined(OS_POSIX) && !defined(OS_MACOSX)
typedef NPError (API_CALL * NP_InitializeFunc)(NPNetscapeFuncs* pNFuncs,
                                               NPPluginFuncs* pPFuncs);
#else
typedef NPError (API_CALL * NP_InitializeFunc)(NPNetscapeFuncs* pFuncs);
typedef NPError (API_CALL * NP_GetEntryPointsFunc)(NPPluginFuncs* pFuncs);
#endif
typedef NPError (API_CALL * NP_ShutdownFunc)(void);

#ifdef __cplusplus
} // extern "C"
#endif

#endif // _NPHOSTAPI_H_
