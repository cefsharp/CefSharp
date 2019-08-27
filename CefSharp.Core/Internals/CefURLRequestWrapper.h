#pragma once

#include "Stdafx.h"

#include "include\cef_urlrequest.h"
#include "include\cef_v8.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        ///
        // Class used to make a URL request. URL requests are not associated with
        // a browser instance so no CefClient callbacks will be executed.
        // URL requests can be created on any valid CEF thread in either the browser
        // or render process. Once created the methods of the URL request object must
        // be accessed on the same thread that created it. 
        ///
        /*--cef(source=library)--*/
        ref class CefURLRequestWrapper : public IURLRequest, public CefWrapper
        {
        private:
            MCefRefPtr<CefURLRequest> _urlRequest;
        internal:
            CefURLRequestWrapper::CefURLRequestWrapper(CefRefPtr<CefURLRequest> &urlRequest)
                : _urlRequest(urlRequest)
            {
            }

            !CefURLRequestWrapper()
            {
                _urlRequest = NULL;
            }

            ~CefURLRequestWrapper()
            {
                this->!CefURLRequestWrapper();
            }

        public:
            ///
            // Returns true if the response body was served from the cache. This includes
            // responses for which revalidation was required.
            ///
            /*--cef()--*/
            virtual bool ResponseWasCached();
            
            ///
            // Returns the response, or NULL if no response information is available.
            // Response information will only be available after the upload has completed.
            // The returned object is read-only and should not be modified.
            ///
            /*--cef()--*/
            virtual IResponse^ GetResponse();

            ///
            // Returns the request status.
            ///
            /*--cef(default_retval=UR_UNKNOWN)--*/
            virtual UrlRequestStatus GetRequestStatus();
        };
    }
}
