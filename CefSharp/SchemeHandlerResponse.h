// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "SchemeHandlerWrapper.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;

namespace CefSharp
{
    class SchemeHandlerWrapper;

    public ref class SchemeHandlerResponse
    {
    internal:
        CefRefPtr<SchemeHandlerWrapper>* _schemeHandlerWrapper;
        void OnRequestCompleted();
        
    public:
        /// <summary>
        /// A Stream with the response data. If the request didn't return any response, leave this property as null.
        /// </summary>
        property Stream^ ResponseStream;

        property String^ MimeType;
        property IDictionary<String^, String^>^ ResponseHeaders;

        /// <summary>
        /// The status code of the response. Unless set, the default value used is 200
        /// (corresponding to HTTP status OK).
        /// </summary>
        property int StatusCode;

        /// <summary>
        /// URL to redirect to (leave empty to not redirect).
        /// </summary>
        property String^ RedirectUrl;

        SchemeHandlerResponse(SchemeHandlerWrapper* schemeHandlerWrapper)
        {
            _schemeHandlerWrapper = new CefRefPtr<SchemeHandlerWrapper>(schemeHandlerWrapper);
        }

        void ReleaseSchemeHandlerWrapper()
        {
            delete _schemeHandlerWrapper;
        }
    };
};
