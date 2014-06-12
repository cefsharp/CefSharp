// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;
using namespace System::IO;
using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Internals
    {
        enum class ResponseAction
        {
            Continue,
            Cancel,
            Redirect,
            Respond
        };

        private ref class RequestResponse : IRequestResponse
        {
            IRequest^ _request;
            Stream^ _responseStream;
            String^ _mimeType;
            String^ _redirectUrl;
            ResponseAction _action;
            String^ _statusText;
            int _statusCode;
            NameValueCollection^ _responseHeaders;

        internal:
            RequestResponse(IRequest^ request) :
                _action(ResponseAction::Continue),
                _request(request) 
            {
            }

            property Stream^ ResponseStream { Stream^ get() { return _responseStream; } }
            property String^ MimeType { String^ get() { return _mimeType; } }
            property String^ StatusText { String^ get() { return _statusText; } }
            property int StatusCode { int get() { return _statusCode; } }
            property NameValueCollection^ ResponseHeaders { NameValueCollection^ get() { return _responseHeaders; } }
            property String^ RedirectUrl { String^ get() { return _redirectUrl; } }
            property ResponseAction Action { ResponseAction get() { return _action; } }

        public:
            virtual void Cancel();
            virtual property IRequest^ Request { IRequest^ get() { return _request; } }
            virtual void Redirect(String^ url);
            virtual void RespondWith(Stream^ stream, String^ mimeType);
            virtual void RespondWith(Stream^ stream, String^ mimeType, String^ statusText, int statusCode, NameValueCollection^ responseHeaders);
        };
    }
}