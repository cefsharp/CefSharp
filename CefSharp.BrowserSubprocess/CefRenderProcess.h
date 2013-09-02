// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "include/cef_app.h"

int ExecuteCefRenderProcess(HINSTANCE hInstance)
{
    CefMainArgs main_args(hInstance);
    CefRefPtr<CefApp> app;
    return CefExecuteProcess(main_args, app);
}
