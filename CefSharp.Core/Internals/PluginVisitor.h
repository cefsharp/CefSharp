// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include/cef_web_plugin.h"

using namespace System::Threading::Tasks;

namespace CefSharp
{
    private class PluginVisitor : public CefWebPluginInfoVisitor
    {
    private:
        gcroot<TaskCompletionSource<List<Plugin>^>^> _taskCompletionSource;
        gcroot<List<Plugin>^> _list;

    public:
        PluginVisitor()
        {
            _list = gcnew List<Plugin>();
            _taskCompletionSource = gcnew TaskCompletionSource<List<Plugin>^>();

            //NOTE: Use fully qualified name as TaskExtensions is ambiguious
            CefSharp::Internals::TaskExtensions::WithTimeout<List<Plugin>^>(_taskCompletionSource, TimeSpan::FromMilliseconds(2000));
        }

        ~PluginVisitor()
        {
            _list = nullptr;
            _taskCompletionSource = nullptr;
        }

        Task<List<Plugin>^>^ GetTask()
        {
            return _taskCompletionSource->Task;
        }

        virtual bool Visit(CefRefPtr<CefWebPluginInfo> info, int count, int total) OVERRIDE
        {
            Plugin plugin;
            plugin.Name = StringUtils::ToClr(info->GetName());
            plugin.Description = StringUtils::ToClr(info->GetDescription());
            plugin.Path = StringUtils::ToClr(info->GetPath());
            plugin.Version = StringUtils::ToClr(info->GetVersion());

            _list->Add(plugin);

            if(count == (total - 1))
            {
                _taskCompletionSource->SetResult(_list);
            }

            return true;
        }

        IMPLEMENT_REFCOUNTING(PluginVisitor);
    };
}