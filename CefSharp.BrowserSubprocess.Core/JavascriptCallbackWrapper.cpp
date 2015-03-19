// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "stdafx.h"
#include "TypeUtils.h"
#include "JavascriptCallbackWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        JavascriptCallbackWrapper::~JavascriptCallbackWrapper()
        {
            value = nullptr;
            context = nullptr;
        }

        JavascriptResponse^ JavascriptCallbackWrapper::Execute(array<Object^>^ parms)
        {
            JavascriptResponse^ response = nullptr;
            if (context->Enter())
            {
                response = gcnew JavascriptResponse();
                try
                {
                    CefV8ValueList args;
                    for each (auto parm in parms)
                    {
                        auto cefParm = TypeUtils::ConvertToCef(parm, nullptr);
                        args.push_back(cefParm);
                    }

                    auto retval = value->ExecuteFunctionWithContext(context.get(), nullptr, args);
                    response->Success = retval != nullptr;
                    if (response->Success)
                    {
                        response->Result = TypeUtils::ConvertFromCef(retval);
                    }
                    else
                    {
                        auto exception = value->GetException();
                        if (exception.get())
                        {
                            response->Message = StringUtils::ToClr(exception->GetMessage());
                        }
                    }
                }
                finally
                {
                    context->Exit();
                }
            }
            return response;
        }
    }
}
