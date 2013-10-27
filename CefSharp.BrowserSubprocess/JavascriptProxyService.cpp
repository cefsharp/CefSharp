// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include <msclr/marshal.h>
#include <sstream>
#include "Common.h"
#include "JavascriptProxyService.h"

using namespace CefSharp::Internals::JavascriptBinding;
using namespace CefSharp::BrowserSubprocess;
using namespace System;
using namespace System::ServiceModel;
using namespace System::ServiceModel::Description;
using namespace System::Threading;

JavascriptProxyService::JavascriptProxyService(CefRefPtr<CefBrowser> browser)
{
	_browser = &browser;
	_browserIdentifier = browser->GetIdentifier();
}

void JavascriptProxyService::CreateJavascriptProxyServiceHost()
{
	auto thread = gcnew Thread(gcnew ThreadStart(this, &JavascriptProxyService::JavascriptProxyServiceEntryPoint));
	thread->Start();
}

void JavascriptProxyService::JavascriptProxyServiceEntryPoint()
{
	auto uris = gcnew array<Uri^>(1);
	uris[0] = gcnew Uri(JavascriptProxy::BaseAddress);

	auto host = gcnew ServiceHost(JavascriptProxy::typeid, uris);
	AddDebugBehavior(host);

	// TODO: Include the name of the "parent process" here also, and perhaps then also clean out the debug stuff...
	auto serviceName = JavascriptProxy::ServiceName + "_" + _browserIdentifier;
	auto log = "Setting up JavascriptProxy listener at " + serviceName;	
	pin_ptr<const wchar_t> serviceNameUnmanagedStr = PtrToStringChars(log);
	OutputDebugString(serviceNameUnmanagedStr);

	host->AddServiceEndpoint(
		IJavascriptProxy::typeid,
		gcnew NetNamedPipeBinding(),
		serviceName
	);

	host->Open();
}

void JavascriptProxyService::AddDebugBehavior(ServiceHost^ host)
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
