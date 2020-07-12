// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "Include\cef_urlrequest.h"

namespace CefSharp
{
    namespace Internals
    {
        /// Interface that should be implemented by the CefUrlRequest client.
        /// The methods of this class will be called on the same thread that created
        /// the request unless otherwise documented. 
        public class CefUrlRequestClientAdapter : public CefURLRequestClient
        {
        private:
            gcroot<IUrlRequestClient^> _client;

        public:
            CefUrlRequestClientAdapter(IUrlRequestClient^ client)
            {
                _client = client;
            }

            ~CefUrlRequestClientAdapter()
            {
                delete _client;
                _client = nullptr;
            }

            // Notifies the client that the request has completed. Use the
            // CefURLRequest::GetRequestStatus method to determine if the request was
            // successful or not.
            ///
            /*--cef()--*/
            virtual void OnRequestComplete(CefRefPtr<CefURLRequest> request) OVERRIDE;

            ///
            // Notifies the client of upload progress. |current| denotes the number of
            // bytes sent so far and |total| is the total size of uploading data (or -1 if
            // chunked upload is enabled). This method will only be called if the
            // UR_FLAG_REPORT_UPLOAD_PROGRESS flag is set on the request.
            ///
            /*--cef()--*/
            virtual void OnUploadProgress(CefRefPtr<CefURLRequest> request,
                int64 current,
                int64 total) OVERRIDE;

            ///ref 
            // Notifies the client of download progress. |current| denotes the number of
            // bytes received up to the call and |total| is the expected total size of the
            // response (or -1 if not determined).
            ///
            /*--cef()--*/
            virtual void OnDownloadProgress(CefRefPtr<CefURLRequest> request,
                int64 current,
                int64 total) OVERRIDE;

            ///
            // Called when some part of the response is read. |data| contains the current
            // bytes received since the last call. This method will not be called if the
            // UR_FLAG_NO_DOWNLOAD_DATA flag is set on the request.
            ///
            /*--cef()--*/
            virtual void OnDownloadData(CefRefPtr<CefURLRequest> request,
                const void* data,
                size_t data_length) OVERRIDE;

            ///
            // Called on the IO thread when the browser needs credentials from the user.
            // |isProxy| indicates whether the host is a proxy server. |host| contains the
            // hostname and |port| contains the port number. Return true to continue the
            // request and call CefAuthCallback::Continue() when the authentication
            // information is available. If the request has an associated browser/frame
            // then returning false will result in a call to GetAuthCredentials on the
            // CefRequestHandler associated with that browser, if any. Otherwise,
            // returning false will cancel the request immediately. This method will only
            // be called for requests initiated from the browser process.
            ///
            /*--cef(optional_param=realm)--*/
            virtual bool GetAuthCredentials(bool isProxy,
                const CefString& host,
                int port,
                const CefString& realm,
                const CefString& scheme,
                CefRefPtr<CefAuthCallback> callback) OVERRIDE;

            IMPLEMENT_REFCOUNTING(CefUrlRequestClientAdapter);
        };
    }
}
