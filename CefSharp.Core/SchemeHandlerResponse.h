// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "ResourceHandlerWrapper.h"
#include "Internals/MCefRefPtr.h"

using namespace System;
using namespace System::Collections::Specialized;
using namespace System::IO;

namespace CefSharp
{
    class SchemeHandlerWrapper;

    public ref class SchemeHandlerResponse : IResourceHandlerResponse
    {
    internal:
        MCefRefPtr<ResourceHandlerWrapper> _resourceHandlerWrapper;

    public:
        /// <summary>
        /// A Stream with the response data. If the request didn't return any response, leave this property as null.
        /// </summary>
        virtual property Stream^ ResponseStream;

        virtual property String^ MimeType;
        virtual property NameValueCollection^ ResponseHeaders;

        /// <summary>
        /// The status code of the response. Unless set, the default value used is 200
        /// (corresponding to HTTP status OK).
        /// </summary>
        virtual property int StatusCode;

        /// <summary>
        /// The length of the response contents. Defaults to -1, which means unknown length
        /// and causes CefSharp to read the response stream in pieces. Thus, setting a length
        /// is optional but allows for more optimal response reading.
        /// </summary>
        virtual property int ContentLength;

        /// <summary>
        /// URL to redirect to (leave empty to not redirect).
        /// </summary>
        virtual property String^ RedirectUrl;

        /// <summary>
        /// Set to true to close the response stream once it has been read. The default value
        /// is false in order to preserve the old CefSharp behavior of not closing the stream.
        /// </summary>
        virtual property bool CloseStream;

        SchemeHandlerResponse(ResourceHandlerWrapper* resourceHandlerWrapper)
        {
            ContentLength = -1;
            _resourceHandlerWrapper = resourceHandlerWrapper;
        }

        ~SchemeHandlerResponse()
        {
            _resourceHandlerWrapper = nullptr;
        }

        virtual void ProcessRequestCallback()
        {
            _resourceHandlerWrapper->ProcessRequestCallback(this);
        }
    };
};
