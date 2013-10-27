// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Common.h"
#include "JavascriptProxyService.h"
#include "SubprocessCefApp.h"

SubprocessCefApp* SubprocessCefApp::_instance = nullptr;

SubprocessCefApp* SubprocessCefApp::GetInstance()
{
	if (_instance == nullptr)
	{
		_instance = new SubprocessCefApp();
	}

	return _instance;
}

void SubprocessCefApp::OnBrowserCreated(CefRefPtr<CefBrowser> browser)
{
	auto javascriptProxyService = gcnew JavascriptProxyService(browser);
	javascriptProxyService->CreateJavascriptProxyServiceHost();
}
