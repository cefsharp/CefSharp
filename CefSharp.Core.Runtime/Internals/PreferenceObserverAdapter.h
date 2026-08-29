// Copyright © 2026 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "include/cef_preference.h"
#include <gcroot.h>

#include "StringUtils.h"

using namespace CefSharp::Callback;

namespace CefSharp
{
    namespace Internals
    {
        private class PreferenceObserverAdapter : public CefPreferenceObserver
        {
        private:
            gcroot<IPreferenceObserver^> _handler;

        public:
            PreferenceObserverAdapter(IPreferenceObserver^ handler)
            {
                _handler = handler;
            }

            ~PreferenceObserverAdapter()
            {
                delete _handler;
                _handler = nullptr;
            }

            virtual void OnPreferenceChanged(const CefString& name) override
            {
                _handler->OnPreferenceChanged(StringUtils::ToClr(name));
            }

            IMPLEMENT_REFCOUNTINGM(PreferenceObserverAdapter);
        };
    }
}
