// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "SchemeHandlerResponse.h"

namespace CefSharp
{
    void SchemeHandlerResponse::OnRequestCompleted()
    {
        _schemeHandlerWrapper->ProcessRequestCallback(this);
    }
}