// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    namespace Internals
    {
        public interface struct IBrowserAdapterNative : IBrowserAdapter
        {
            virtual Task<JavascriptResponse^>^ EvaluateScriptAsync(const CefRefPtr<CefFrame>& frame, String^ script, Nullable<TimeSpan> timeout);
        };
    }
}
