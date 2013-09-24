// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Program.h"
#include "CefRenderProcess.h"
#include "JavascriptProxyService.h"

int APIENTRY wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
{
    //CreateJavascriptProxyServiceHost();
    return ExecuteCefRenderProcess(hInstance);
}
