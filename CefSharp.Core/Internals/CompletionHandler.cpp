// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

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