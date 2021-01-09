// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_v8.h"
#include "RegisterBoundObjectRegistry.h"
#include "..\CefSharp.Core.Runtime\Internals\Messaging\Messages.h"
#include "..\CefSharp.Core.Runtime\Internals\Serialization\Primitives.h"

using namespace System;
using namespace CefSharp::Internals::Messaging;
using namespace CefSharp::Internals::Serialization;

namespace CefSharp
{
    const CefString kIsObjectCached = CefString("IsObjectCached");
    const CefString kIsObjectCachedCamelCase = CefString("isObjectCached");
    const CefString kRemoveObjectFromCache = CefString("RemoveObjectFromCache");
    const CefString kRemoveObjectFromCacheCamelCase = CefString("removeObjectFromCache");
    const CefString kDeleteBoundObject = CefString("DeleteBoundObject");
    const CefString kDeleteBoundObjectCamelCase = CefString("deleteBoundObject");

    private class RegisterBoundObjectHandler : public CefV8Handler
    {
    private:
        gcroot<Dictionary<String^, JavascriptObject^>^> _javascriptObjects;

    public:
        RegisterBoundObjectHandler(Dictionary<String^, JavascriptObject^>^ javascriptObjects)
        {
            _javascriptObjects = javascriptObjects;
        }

        bool Execute(const CefString& name, CefRefPtr<CefV8Value> object, const CefV8ValueList& arguments, CefRefPtr<CefV8Value>& retval, CefString& exception) OVERRIDE
        {
            auto context = CefV8Context::GetCurrentContext();
            if (context.get())
            {
                if (context.get() && context->Enter())
                {
                    try
                    {
                        if (name == kIsObjectCached || name == kIsObjectCachedCamelCase)
                        {
                            if (arguments.size() == 0 || arguments.size() > 1)
                            {
                                //TODO: Improve error message
                                exception = "Must specify the name of a single bound object to check the cache for";

                                return true;
                            }

                            auto objectName = arguments[0]->GetStringValue();
                            auto managedObjectName = StringUtils::ToClr(objectName);

                            //Check to see if the object name is within the cache
                            retval = CefV8Value::CreateBool(_javascriptObjects->ContainsKey(managedObjectName));
                        }
                        else if (name == kRemoveObjectFromCache || name == kRemoveObjectFromCacheCamelCase)
                        {
                            if (arguments.size() == 0 || arguments.size() > 1)
                            {
                                //TODO: Improve error message
                                exception = "Must specify the name of a single bound object to remove from cache";

                                return true;
                            }

                            auto objectName = arguments[0]->GetStringValue();
                            auto managedObjectName = StringUtils::ToClr(objectName);

                            if (_javascriptObjects->ContainsKey(managedObjectName))
                            {
                                //Remove object from cache
                                retval = CefV8Value::CreateBool(_javascriptObjects->Remove(managedObjectName));
                            }
                            else
                            {
                                retval = CefV8Value::CreateBool(false);
                            }
                        }
                        //TODO: Better name for this function
                        else if (name == kDeleteBoundObject || name == kDeleteBoundObjectCamelCase)
                        {
                            if (arguments.size() == 0 || arguments.size() > 1)
                            {
                                //TODO: Improve error message
                                exception = "Must specify the name of a bound object to unbind, one object at a time.";

                                return true;
                            }

                            auto objectName = arguments[0]->GetStringValue();

                            auto global = context->GetGlobal();

                            auto success = global->DeleteValue(objectName);

                            retval = CefV8Value::CreateBool(success);
                        }
                    }
                    finally
                    {
                        context->Exit();
                    }
                }
                else
                {
                    exception = "Unable to Enter Context";
                }
            }
            else
            {
                exception = "Unable to get current context";
            }


            return true;
        }

        IMPLEMENT_REFCOUNTING(RegisterBoundObjectHandler);
    };
}

