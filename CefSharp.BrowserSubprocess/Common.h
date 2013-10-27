// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

// Not really used so much by all our classes, but it's imperative that this file gets included early enough to avoid this issue:
// http://stackoverflow.com/questions/4000663/issue-in-compiling-with-marshal-h-error-c2872-iserviceprovider-ambiguous
#include <Windows.h>

#define DECL __declspec(dllexport)
