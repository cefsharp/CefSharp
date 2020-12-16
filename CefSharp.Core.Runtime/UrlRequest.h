// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_urlrequest.h"

#include "Request.h"
#include "RequestContext.h"
#include "Internals\CefUrlRequestClientAdapter.h"
#include "Internals\CefWrapper.h"

namespace CefSharp
{
    namespace Core
    {
        // Class used to make a URL request. URL requests are not associated with
        // a browser instance so no CefClient callbacks will be executed.
        // URL requests can be created on any valid CEF thread in either the browser
        // or render process. Once created the methods of the URL request object must
        // be accessed on the same thread that created it. 
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class UrlRequest : public IUrlRequest, public CefWrapper
        {
        private:
            MCefRefPtr<CefURLRequest> _urlRequest;
        internal:
            UrlRequest(CefRefPtr<CefURLRequest> &urlRequest)
                : _urlRequest(urlRequest)
            {
            }

            !UrlRequest()
            {
                _urlRequest = NULL;
            }

            ~UrlRequest()
            {
                this->!UrlRequest();
            }

        public:
            UrlRequest(IRequest^ request, IUrlRequestClient^ urlRequestClient)
                : UrlRequest(request, urlRequestClient, nullptr)
            {
            }

            UrlRequest(IRequest^ request, IUrlRequestClient^ urlRequestClient, IRequestContext^ requestContext)
            {
                if (request == nullptr)
                {
                    throw gcnew ArgumentNullException("request");
                }
                if (urlRequestClient == nullptr)
                {
                    throw gcnew ArgumentNullException("urlRequestClient");
                }

                _urlRequest = CefURLRequest::Create((Request^)request, new CefUrlRequestClientAdapter(urlRequestClient), (RequestContext^)requestContext);
            }

            ///
            // Returns true if the response body was served from the cache. This includes
            // responses for which revalidation was required.
            ///
            /*--cef()--*/
            virtual property bool ResponseWasCached
            {
                bool get();
            }

            ///
            // Returns the response, or NULL if no response information is available.
            // Response information will only be available after the upload has completed.
            // The returned object is read-only and should not be modified.
            ///
            /*--cef()--*/
            virtual property IResponse^ Response
            {
                IResponse^ get();
            }

            ///
            // Returns the request status.
            ///
            /*--cef(default_retval=UR_UNKNOWN)--*/
            virtual property UrlRequestStatus RequestStatus
            {
                UrlRequestStatus get();
            }
        };
    }
}
