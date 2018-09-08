// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request.h"
#include "CefWrapper.h"

using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefRequestWrapper : public IRequest, public CefWrapper
        {
            MCefRefPtr<CefRequest> _wrappedRequest;
            IPostData^ _postData;
        internal:
            CefRequestWrapper(CefRefPtr<CefRequest> &cefRequest) :
                _wrappedRequest(cefRequest), _postData(nullptr)
            {
            }

            !CefRequestWrapper()
            {
                _wrappedRequest = nullptr;
            }

            ~CefRequestWrapper()
            {
                this->!CefRequestWrapper();

                delete _postData;

                _disposed = true;
            }

        public:
            virtual property UrlRequestFlags Flags { UrlRequestFlags get(); void set(UrlRequestFlags flags); }
            virtual property String^ Url { String^ get(); void set(String^ url); }
            virtual property String^ Method { String^ get(); void set(String^ method); }
            virtual property UInt64 Identifier { UInt64 get(); }
            virtual void SetReferrer(String^ referrerUrl, CefSharp::ReferrerPolicy policy);
            virtual property String^ ReferrerUrl { String^ get(); }
            virtual property ResourceType ResourceType { CefSharp::ResourceType get(); }
            virtual property ReferrerPolicy ReferrerPolicy { CefSharp::ReferrerPolicy get(); }
            virtual property NameValueCollection^ Headers { NameValueCollection^ get(); void set(NameValueCollection^ url); }
            virtual property TransitionType TransitionType { CefSharp::TransitionType get(); }
            virtual property IPostData^ PostData { IPostData^ get(); }
            virtual property bool IsReadOnly { bool get(); }
            virtual void InitializePostData();

            operator CefRefPtr<CefRequest>()
            {
                if (this == nullptr)
                {
                    return NULL;
                }
                return _wrappedRequest.get();
            }
        };
    }
}
