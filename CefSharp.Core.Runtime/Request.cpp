// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Request.h"
#include "PostData.h"

using namespace System::Text;

namespace CefSharp
{
    namespace Core
    {
        UrlRequestFlags Request::Flags::get()
        {
            ThrowIfDisposed();

            return (UrlRequestFlags)_request->GetFlags();
        }

        void Request::Flags::set(UrlRequestFlags flags)
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetFlags((int)flags);
        }

        String^ Request::Url::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetURL());
        }

        void Request::Url::set(String^ url)
        {
            if (url == nullptr)
            {
                throw gcnew System::ArgumentException("cannot be null", "url");
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            CefString str = StringUtils::ToNative(url);
            _request->SetURL(str);
        }

        String^ Request::Method::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetMethod());
        }

        void Request::Method::set(String^ method)
        {
            if (method == nullptr)
            {
                throw gcnew System::ArgumentException("cannot be null", "method");
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetMethod(StringUtils::ToNative(method));
        }

        UInt64 Request::Identifier::get()
        {
            ThrowIfDisposed();

            return _request->GetIdentifier();
        }

        void Request::SetReferrer(String^ referrerUrl, CefSharp::ReferrerPolicy policy)
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetReferrer(StringUtils::ToNative(referrerUrl), (cef_referrer_policy_t)policy);
        }

        String^ Request::ReferrerUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetReferrerURL());
        }

        CefSharp::ResourceType Request::ResourceType::get()
        {
            ThrowIfDisposed();

            return (CefSharp::ResourceType)_request->GetResourceType();
        }

        CefSharp::ReferrerPolicy Request::ReferrerPolicy::get()
        {
            ThrowIfDisposed();

            return (CefSharp::ReferrerPolicy)_request->GetReferrerPolicy();
        }

        NameValueCollection^ Request::Headers::get()
        {
            ThrowIfDisposed();

            CefRequest::HeaderMap hm;
            _request->GetHeaderMap(hm);

            auto headers = gcnew HeaderNameValueCollection();

            for (CefRequest::HeaderMap::iterator it = hm.begin(); it != hm.end(); ++it)
            {
                String^ name = StringUtils::ToClr(it->first);
                String^ value = StringUtils::ToClr(it->second);
                headers->Add(name, value);
            }

            if (_request->IsReadOnly())
            {
                headers->SetReadOnly();
            }

            return headers;
        }

        void Request::Headers::set(NameValueCollection^ headers)
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            CefRequest::HeaderMap hm;

            for each(String^ key in headers)
            {
                CefString name = StringUtils::ToNative(key);
                for each(String^ element in headers->GetValues(key))
                {
                    CefString value = StringUtils::ToNative(element);
                    hm.insert(std::make_pair(name, value));
                }
            }

            _request->SetHeaderMap(hm);
        }

        TransitionType Request::TransitionType::get()
        {
            ThrowIfDisposed();

            return (CefSharp::TransitionType) _request->GetTransitionType();
        }

        IPostData^ Request::PostData::get()
        {
            ThrowIfDisposed();

            if (_postData == nullptr)
            {
                auto postData = _request->GetPostData();
                if (postData.get())
                {
                    _postData = gcnew CefSharp::Core::PostData(postData);
                }
            }
            return _postData;
        }

        void Request::PostData::set(IPostData^ postData)
        {
            ThrowIfDisposed();

            ThrowIfReadOnly();

            _request->SetPostData((CefSharp::Core::PostData^)postData->UnWrap());
        }

        bool Request::IsReadOnly::get()
        {
            ThrowIfDisposed();

            return _request->IsReadOnly();
        }

        void Request::InitializePostData()
        {
            ThrowIfDisposed();

            ThrowIfReadOnly();

            _request->SetPostData(CefPostData::Create());
        }

        String^ Request::GetHeaderByName(String^ name)
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetHeaderByName(StringUtils::ToNative(name)));
        }

        void Request::SetHeaderByName(String^ name, String^ value, bool overwrite)
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetHeaderByName(StringUtils::ToNative(name), StringUtils::ToNative(value), overwrite);
        }
    }
}
