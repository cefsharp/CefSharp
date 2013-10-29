// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Common.h"
#include "JavascriptProxyService.h"
#include "SubprocessCefApp.h"

SubprocessCefApp* SubprocessCefApp::_instance = nullptr;
CefRefPtr<CefBrowser> SubprocessCefApp::_browser = nullptr;

SubprocessCefApp* SubprocessCefApp::GetInstance()
{
	if (_instance == nullptr)
	{
		_instance = new SubprocessCefApp();
	}

	return _instance;
}

CefRefPtr<CefBrowser> SubprocessCefApp::GetCefBrowser()
{
	return _browser;
}

void SubprocessCefApp::OnBrowserCreated(CefRefPtr<CefBrowser> browser)
{
	_browser = browser;

	// FIXME: deallocate this sometime.
	auto javascriptProxyService = new JavascriptProxyService(browser->GetIdentifier());
	javascriptProxyService->CreateJavascriptProxyServiceHost();
}
