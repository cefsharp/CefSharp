// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_web_plugin.h"

namespace CefSharp
{
    private class PluginVisitor : public CefWebPluginInfoVisitor
    {
    private:
        gcroot<IWebPluginInfoVisitor^> _visitor;

    public:
        PluginVisitor(IWebPluginInfoVisitor^ visitor) : _visitor(visitor)
        {

        }

        ~PluginVisitor()
        {
            delete _visitor;
            _visitor = nullptr;
        }

        virtual bool Visit(CefRefPtr<CefWebPluginInfo> info, int count, int total) OVERRIDE
        {
            auto plugin = gcnew WebPluginInfo(StringUtils::ToClr(info->GetName()),
                StringUtils::ToClr(info->GetDescription()),
                StringUtils::ToClr(info->GetPath()),
                StringUtils::ToClr(info->GetVersion()));

            return _visitor->Visit(plugin, count, total);
        }

        IMPLEMENT_REFCOUNTING(PluginVisitor);
    };
}