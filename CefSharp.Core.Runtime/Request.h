// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request.h"
#include "Internals\CefWrapper.h"

using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Core
    {
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class Request : public IRequest, public CefWrapper
        {
            MCefRefPtr<CefRequest> _request;
            IPostData^ _postData;
        internal:
            Request(CefRefPtr<CefRequest> &cefRequest) :
                _request(cefRequest), _postData(nullptr)
            {
            }

            !Request()
            {
                _request = nullptr;
            }

            ~Request()
            {
                this->!Request();

                delete _postData;

                _disposed = true;
            }

            operator CefRefPtr<CefRequest>()
            {
                if (this == nullptr)
                {
                    return NULL;
                }
                return _request.get();
            }

            void ThrowIfReadOnly()
            {
                if (_request->IsReadOnly())
                {
                    throw gcnew NotSupportedException("IRequest is read-only and cannot be modified. Check IRequest.IsReadOnly to guard against this exception.");
                }
            }

        public:
            Request()
            {
                _request = CefRequest::Create();
            }

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
            virtual property IPostData^ PostData { IPostData^ get(); void set(IPostData^ postData);  }
            virtual property bool IsReadOnly { bool get(); }
            virtual void InitializePostData();

            virtual String^ GetHeaderByName(String^ name);
            virtual void SetHeaderByName(String^ name, String^ value, bool overwrite);
        };
    }
}
