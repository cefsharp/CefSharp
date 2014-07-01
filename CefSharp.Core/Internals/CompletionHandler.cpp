// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CompletionHandler.h"

using namespace System::Net;

namespace CefSharp
{
    void CompletionHandler::OnComplete()
    {
        _handler->OnComplete();
    }
}