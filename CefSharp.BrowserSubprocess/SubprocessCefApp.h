// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include <list>
#include "include/cef_app.h"

private class SubprocessCefApp : public CefApp,
                                 public CefRenderProcessHandler
{
    static SubprocessCefApp* _instance;
    std::list<CefRefPtr<CefBrowser>> _browsers;

public:

    static SubprocessCefApp* GetInstance()
    {
        if (_instance == nullptr)
        {
            _instance = new SubprocessCefApp();
        }

        return _instance;
    }

    virtual DECL CefRefPtr<CefRenderProcessHandler> GetRenderProcessHandler() OVERRIDE
    {
        return this;
    }

    virtual DECL void OnBrowserCreated(CefRefPtr<CefBrowser> browser) OVERRIDE
    {
        _browsers.push_back(browser);
    }

    virtual DECL void OnBrowserDestroyed(CefRefPtr<CefBrowser> browser) OVERRIDE
    {
        _browsers.remove(browser);
    }

    virtual DECL CefRefPtr<CefBrowser> GetBrowserById(int browser_id)
    {
        // FIXME: Doesn't seem to work. Our list contains a browser w/ ID == 0, and the parameter we get is 1. How come?
        // We could experiment with OnContextCreated() to see if it would work better...
		for (auto browser = _browsers.begin();
			 browser != _browsers.end();
			 browser++)
        {
            if ((*browser)->GetIdentifier() == browser_id)
            {
                return *browser;
            }
        }
        
        return nullptr;
    }

    IMPLEMENT_REFCOUNTING(SubprocessCefApp);
};
