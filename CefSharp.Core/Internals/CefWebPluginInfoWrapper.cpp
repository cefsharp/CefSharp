// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefWebPluginInfoWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        String^ CefWebPluginInfoWrapper::Description::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetDescription());
        }

        String^ CefWebPluginInfoWrapper::Name::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetName());
        }

        String^ CefWebPluginInfoWrapper::Path::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetPath());
        }

        String^ CefWebPluginInfoWrapper::Version::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetVersion());
        }
    }
}
