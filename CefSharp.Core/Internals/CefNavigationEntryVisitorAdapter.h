// Copyright © 2016 The CefSharp Authors. All rights reserved.
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
                auto navEntry = TypeConversion::FromNative(entry, current);

                return _handler->Visit(navEntry, current, index, total);
            }

            IMPLEMENT_REFCOUNTING(CefNavigationEntryVisitorAdapter);
        };
    }
}

