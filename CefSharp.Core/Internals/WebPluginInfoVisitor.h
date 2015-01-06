// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_web_plugin.h"


namespace CefSharp
{
    private class WebPluginInfoVisitor : public CefWebPluginInfoVisitor
    {
    private:
        gcroot<IWebPluginInfoVisitor^> _visitor;

    public:
        WebPluginInfoVisitor(IWebPluginInfoVisitor^ visitor) :
            _visitor(visitor)
        {
        }

        ~WebPluginInfoVisitor()
        {
            _visitor = nullptr;
        }
        virtual bool Visit(CefRefPtr<CefWebPluginInfo> info, int count, int totale) OVERRIDE;

        IMPLEMENT_REFCOUNTING(WebPluginInfoVisitor);
    };
}
