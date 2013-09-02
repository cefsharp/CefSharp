// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptProxy.h"

using namespace CefSharp::Internals::JavascriptBinding;
using namespace CefSharp::BrowserSubprocess;
using namespace System;
using namespace System::ServiceModel;
using namespace System::ServiceModel::Description;
using namespace System::Threading;

static void JavascriptProxyServiceEntryPoint();
static void AddDebugBehavior(ServiceHost^ host);

void CreateJavascriptProxyServiceHost()
{
    auto thread = gcnew Thread(gcnew ThreadStart(JavascriptProxyServiceEntryPoint));
    thread->Start();
}

void JavascriptProxyServiceEntryPoint()
{
    auto uris = gcnew array<Uri^>(1);
    uris[0] = gcnew Uri(JavascriptProxy::BaseAddress);

    auto host = gcnew ServiceHost(JavascriptProxy::typeid, uris);
    AddDebugBehavior(host);

    host->AddServiceEndpoint(
        IJavascriptProxy::typeid,
        gcnew NetNamedPipeBinding(),
        JavascriptProxy::ServiceName
    );

    host->Open();
}

void AddDebugBehavior(ServiceHost^ host)
{
    auto serviceDebugBehavior = host->Description->Behaviors->Find<ServiceDebugBehavior^>();

    if (serviceDebugBehavior == nullptr)
    {    
        serviceDebugBehavior = gcnew ServiceDebugBehavior();
        serviceDebugBehavior->IncludeExceptionDetailInFaults = true;
        host->Description->Behaviors->Add(serviceDebugBehavior);
    }
    else
    {
        serviceDebugBehavior->IncludeExceptionDetailInFaults = true;
    }
}
