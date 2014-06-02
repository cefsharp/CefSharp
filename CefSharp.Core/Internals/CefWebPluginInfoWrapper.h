// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "MCefRefPtr.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        ref class CefWebPluginInfoWrapper : public IWebPluginInfo
        {
            MCefRefPtr<CefWebPluginInfo> _wrappedInfo;

        internal:
            CefWebPluginInfoWrapper(CefRefPtr<CefWebPluginInfo> cefInfo) : _wrappedInfo(cefInfo) {}

        public:
            virtual property String^ Description { String^ get(); }
            virtual property String^ Name { String^ get(); }
            virtual property String^ Path { String^ get(); }
            virtual property String^ Version { String^ get(); }
        };
    }
}
