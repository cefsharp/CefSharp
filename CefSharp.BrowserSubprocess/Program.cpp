// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "include/cef_app.h"
#include "JavascriptProxy.h"

using namespace CefSharp::Internals::JavascriptBinding;
using namespace CefSharp::BrowserSubprocess;
using namespace System;
using namespace System::ServiceModel;
using namespace System::Threading;

static void threadEntryPoint();

int APIENTRY wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
{
    auto thread = gcnew Thread(gcnew ThreadStart(threadEntryPoint));
    thread->Start();

    CefMainArgs main_args(hInstance);
    CefRefPtr<CefApp> app;
    return CefExecuteProcess(main_args, app);
}

void threadEntryPoint()
{
    auto uris = gcnew array<Uri^>(1);
    uris[0] = gcnew Uri(JavascriptProxy::BaseAddress);

    auto host = gcnew ServiceHost(JavascriptProxy::typeid, uris);
    host->AddServiceEndpoint(
        IJavascriptProxy::typeid,
        gcnew NetNamedPipeBinding(),
        JavascriptProxy::ServiceName
    );

    host->Open();
}
