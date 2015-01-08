// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        ref class CefWebPluginInfoWrapper : public IWebPluginInfo
        {
        internal:
            CefWebPluginInfoWrapper(CefRefPtr<CefWebPluginInfo> webPluginInfo)
            {
                Description = StringUtils::ToClr(webPluginInfo->GetDescription());
                Name = StringUtils::ToClr(webPluginInfo->GetName());
                Path = StringUtils::ToClr(webPluginInfo->GetPath());
                Version = StringUtils::ToClr(webPluginInfo->GetVersion());
            }

        public:
            virtual property String^ Description;
            virtual property String^ Name;
            virtual property String^ Path;
            virtual property String^ Version;
        };
    }
}
