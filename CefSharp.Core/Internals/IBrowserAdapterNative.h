// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

namespace CefSharp
{
    namespace Internals
    {
        // Have the C++/CLI portion of the interface here, since we
        // can't express the frame parameter type from C#.
        public interface struct IBrowserAdapterNative : IBrowserAdapter
        {
            virtual Task<JavascriptResponse^>^ EvaluateScriptAsync(const CefRefPtr<CefFrame>& frame, String^ script, Nullable<TimeSpan> timeout);
        };
    }
}
