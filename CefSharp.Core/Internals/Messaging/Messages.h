// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_base.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace Messaging
        {
            //contains process message names for all handled messages

            //Message containing a script to be evaluated
            const CefString kEvaluateJavascriptRequest = "EvaluateJavascriptRequest";
            //Message containing the result for a given evaluation
            const CefString kEvaluateJavascriptResponse = "EvaluateJavascriptDoneResponse";
        }
    }
}