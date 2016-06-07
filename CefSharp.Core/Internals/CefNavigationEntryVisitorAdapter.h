// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_browser.h"

namespace CefSharp
{
    namespace Internals
    {
        private class CefNavigationEntryVisitorAdapter : public CefNavigationEntryVisitor
        {
        private:
            gcroot<INavigationEntryVisitor^> _handler;

        public:
            CefNavigationEntryVisitorAdapter(INavigationEntryVisitor^ handler)
            {
                _handler = handler;
            }

            ~CefNavigationEntryVisitorAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            bool Visit(CefRefPtr<CefNavigationEntry> entry,
                bool current,
                int index,
                int total) OVERRIDE
            {
                NavigationEntry navEntry;

                if (entry->IsValid())
                {
                    auto time = entry->GetCompletionTime();
                    DateTime completionTime = CefSharp::Internals::CefTimeUtils::ConvertCefTimeToDateTime(time.GetDoubleT());
                    navEntry = NavigationEntry(current, completionTime, StringUtils::ToClr(entry->GetDisplayURL()), entry->GetHttpStatusCode(), StringUtils::ToClr(entry->GetOriginalURL()), StringUtils::ToClr(entry->GetTitle()), (TransitionType)entry->GetTransitionType(), StringUtils::ToClr(entry->GetURL()), entry->HasPostData(), true);
                }
                else
                {
                    //Invalid nav entry
                    navEntry = NavigationEntry(current, DateTime::MinValue, nullptr, -1, nullptr, nullptr, (TransitionType)-1, nullptr, false, false);
                }

                return _handler->Visit(navEntry, current, index, total);
            }

            IMPLEMENT_REFCOUNTING(CefNavigationEntryVisitorAdapter);
        };
    }
}

