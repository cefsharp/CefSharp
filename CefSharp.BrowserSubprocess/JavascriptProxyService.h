// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "JavascriptProxy.h"
#include "include/cef_browser.h"

class JavascriptProxyService
{
	CefRefPtr<CefBrowser>* _browser;
	int _browserIdentifier;

public:
	JavascriptProxyService(int browserIdentifier);

	void CreateJavascriptProxyServiceHost();

private:

	void JavascriptProxyServiceEntryPoint();
	void AddDebugBehavior(System::ServiceModel::ServiceHost^ host);
	void EvaluateScriptHelper(String^ script);
};