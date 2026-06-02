// Copyright © 2026 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefIdMappers.h"
#include "include\cef_id_mappers.h"
#include <msclr/marshal_cppstd.h>

using namespace msclr::interop;

namespace CefSharp
{
    namespace Core
    {
        int CefIdMappers::CefIdForCommandIdName(String^ name)
        {
            if (String::IsNullOrEmpty(name))
            {
                return -1;
            }

            std::string nativeString = marshal_as<std::string>(name);

            return cef_id_for_command_id_name(nativeString.c_str());
        }
    };
}
