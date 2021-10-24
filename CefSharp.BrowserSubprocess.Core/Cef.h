// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "include\cef_v8.h"

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        /// <summary>
        /// Global CEF methods are exposed through this class. e.g. CefRegisterExtension maps to Cef.RegisterExtension
        /// Only methods relevant to the Render Process are included in this class.
        /// CEF API Doc https://magpcss.org/ceforum/apidocs3/projects/(default)/(_globals).html
        /// This class cannot be inherited.
        /// </summary>
        public ref class Cef sealed
        {
        public:
            /// <summary>
            /// Register a new V8 extension with the specified JavaScript extension code.
            /// </summary>
            /// <param name="name">name</param>
            /// <param name="javascriptCode">JavaScript code</param>
            static void RegisterExtension(String^ name, String^ javascriptCode)
            {
                CefRegisterExtension(StringUtils::ToNative(name), StringUtils::ToNative(javascriptCode), nullptr);
            }
        };
    }
}
