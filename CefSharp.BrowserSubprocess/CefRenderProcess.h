// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_app.h"
#include "SubprocessCefApp.h"

int ExecuteCefRenderProcess(HINSTANCE hInstance)
{
    CefMainArgs main_args(hInstance);
    CefRefPtr<SubprocessCefApp> subprocessCefAppPtr = SubprocessCefApp::GetInstance();
    return CefExecuteProcess(main_args, (CefRefPtr<CefApp>) subprocessCefAppPtr);
}
