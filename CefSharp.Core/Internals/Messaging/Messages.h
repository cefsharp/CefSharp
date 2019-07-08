// Copyright © 2015 The CefSharp Authors. All rights reserved.
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
            //Message to invoke a stored js function
            const CefString kJavascriptCallbackRequest = "JavascriptCallbackRequest";
            //Message to dereference a stored js function
            const CefString kJavascriptCallbackDestroyRequest = "JavascriptCallbackDestroyRequest";
            //Message containing the result of a given js function call
            const CefString kJavascriptCallbackResponse = "JavascriptCallbackDoneResponse";
            //Message containing a request JSB root objects
            const CefString kJavascriptRootObjectRequest = "JavascriptRootObjectRequest";
            //Message containing the response for the JSB root objects
            const CefString kJavascriptRootObjectResponse = "JavascriptRootObjectResponse";
            //Message from the render process to request a method invocation on a bound object
            const CefString kJavascriptAsyncMethodCallRequest = "JavascriptAsyncMethodCallRequest";
            //Message from the browser process containing the result of a bound method invocation
            const CefString kJavascriptAsyncMethodCallResponse = "JavascriptAsyncMethodCallResponse";
            //Message that signals a new V8Context has been created
            const CefString kOnContextCreatedRequest = "OnContextCreated";
            //Message that signals a new V8Context has been released
            const CefString kOnContextReleasedRequest = "OnContextReleased";
            // Message from the render process that an element (or nothing) has
            // gotten focus. This message is only sent if specified as an
            // optional message via command line argument when the subprocess is
            // created.
            const CefString kOnFocusedNodeChanged = "OnFocusedNodeChanged";
            //Message that signals an uncaught exception has occurred
            const CefString kOnUncaughtException = "OnUncaughtException";
            //Message containing a request/notification that JSB objects have been bound
            const CefString kJavascriptObjectsBoundInJavascript = "JavascriptObjectsBoundInJavascript";

            //Message containing the CefSharp.PostMessage request
            const CefString kJavascriptMessageReceived = "JavascriptMessageReceived";
        }
    }
}
