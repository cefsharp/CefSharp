// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Common.h"
#include "include/cef_app.h"

private class SubprocessCefApp : public CefApp,
                                 public CefRenderProcessHandler
{
    static SubprocessCefApp* _instance;

public:

	static SubprocessCefApp* GetInstance();

    virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE
    {
        return this;
    }

	virtual DECL void OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE;

    virtual DECL void OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE
    {
		// FIXME: Why do we never get called?
		OutputDebugString(L"OnBrowserDestroyed called");
    }

    IMPLEMENT_REFCOUNTING(SubprocessCefApp);
};
