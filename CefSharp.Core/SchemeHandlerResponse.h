// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "SchemeHandlerWrapper.h"
#include "MCefRefPtr.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    class SchemeHandlerWrapper;

    public ref class SchemeHandlerResponseWrapper : public SchemeHandlerResponse
    {
    internal:
        MCefRefPtr<SchemeHandlerWrapper> _schemeHandlerWrapper; 
        
        void OnRequestCompleted(Task^ previous );

    public:
        SchemeHandlerResponseWrapper(CefRefPtr<SchemeHandlerWrapper> schemeHandlerWrapper) :
            _schemeHandlerWrapper(schemeHandlerWrapper)
        {
        };

        virtual void DoDispose(bool isdisposing) override
        {
            _schemeHandlerWrapper = nullptr;

            SchemeHandlerResponse::DoDispose(isdisposing);
        }
    };
};
