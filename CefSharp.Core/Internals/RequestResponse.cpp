// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "RequestResponse.h"

namespace CefSharp
{
    namespace Internals
    {
        void RequestResponse::Cancel()
        {
            _action = ResponseAction::Cancel;
        }

        void RequestResponse::Redirect(String^ url)
        {
            _redirectUrl = url;
            _action = ResponseAction::Redirect;
        }

        void RequestResponse::RespondWith(Stream^ stream, String^ mimeType)
        {
            RespondWith(stream, mimeType, "OK", 200, nullptr);
        }

        void RequestResponse::RespondWith(Stream^ stream, String^ mimeType, String^ statusText, int statusCode, NameValueCollection^ responseHeaders)
        {
            if (String::IsNullOrEmpty(mimeType))
            {
                throw gcnew ArgumentException("must provide a mime type", "mimeType");
            }

            if (stream == nullptr)
            {
                throw gcnew ArgumentNullException("stream");
            }

            _responseStream = stream;
            _mimeType = mimeType;
            _statusText = statusText;
            _statusCode = statusCode;
            _responseHeaders = responseHeaders;

            _action = ResponseAction::Respond;
        }
    }
}