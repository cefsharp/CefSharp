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


#ifndef _CEF_PLUGIN_H
#define _CEF_PLUGIN_H

#include "cef_string.h"
#include <vector>
#include "third_party/npapi/bindings/npapi.h"
#include "third_party/npapi/bindings/nphostapi.h"

// Netscape plugins are normally built at separate DLLs that are loaded by the
// browser when needed.  This interface supports the creation of plugins that
// are an embedded component of the application.  Embedded plugins built using
// this interface use the same Netscape Plugin API as DLL-based plugins.
// See https://developer.mozilla.org/En/Gecko_Plugin_API_Reference for complete
// documentation on how to use the Netscape Plugin API.

// This structure describes a mime type entry for a plugin.
struct CefPluginMimeType {
  // The actual mime type.
  CefString mime_type;

  // A list of all the file extensions for this mime type.
  std::vector<CefString> file_extensions;

  // Description of the mime type.
  CefString description;
};

// This structure provides attribute information and entry point functions for
// a plugin.
struct CefPluginInfo {
  // The unique name that identifies the plugin.
  CefString unique_name;

  // The friendly display name of the plugin.
  CefString display_name;

  // The version string of the plugin.
  CefString version;

  // A description of the plugin.
  CefString description;

  // A list of all the mime types that this plugin supports.
  std::vector<CefPluginMimeType> mime_types;

  // Entry point function pointers.
#if !defined(OS_POSIX) || defined(OS_MACOSX)
  NP_GetEntryPointsFunc np_getentrypoints;
#endif
  NP_InitializeFunc np_initialize;
  NP_ShutdownFunc np_shutdown;
};

// Register a plugin with the system.
bool CefRegisterPlugin(const struct CefPluginInfo& plugin_info);

#endif // _CEF_PLUGIN_H
