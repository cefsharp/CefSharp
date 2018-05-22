// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefRequestWrapper.h"
#include "CefPostDataWrapper.h"

using namespace System::Text;

namespace CefSharp
{
    namespace Internals
    {
        UrlRequestFlags CefRequestWrapper::Flags::get()
        {
            ThrowIfDisposed();

            return (UrlRequestFlags)_wrappedRequest->GetFlags();
        }

        void CefRequestWrapper::Flags::set(UrlRequestFlags flags)
        {
            ThrowIfDisposed();

            _wrappedRequest->SetFlags((int)flags);
        }

        String^ CefRequestWrapper::Url::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedRequest->GetURL());
        }

        void CefRequestWrapper::Url::set(String^ url)
        {
            if (url == nullptr)
            {
                throw gcnew System::ArgumentException("cannot be null", "url");
            }

            ThrowIfDisposed();

            CefString str = StringUtils::ToNative(url);
            _wrappedRequest->SetURL(str);
        }

        String^ CefRequestWrapper::Method::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedRequest->GetMethod());
        }

        void CefRequestWrapper::Method::set(String^ method)
        {
            if (method == nullptr)
            {
                throw gcnew System::ArgumentException("cannot be null", "method");
            }

            ThrowIfDisposed();

            _wrappedRequest->SetMethod(StringUtils::ToNative(method));
        }

        UInt64 CefRequestWrapper::Identifier::get()
        {
            ThrowIfDisposed();

            return _wrappedRequest->GetIdentifier();
        }

        void CefRequestWrapper::SetReferrer(String^ referrerUrl, CefSharp::ReferrerPolicy policy)
        {
            ThrowIfDisposed();

            _wrappedRequest->SetReferrer(StringUtils::ToNative(referrerUrl), (cef_referrer_policy_t)policy);
        }

        String^ CefRequestWrapper::ReferrerUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedRequest->GetReferrerURL());
        }

        CefSharp::ResourceType CefRequestWrapper::ResourceType::get()
        {
            ThrowIfDisposed();

            return (CefSharp::ResourceType)_wrappedRequest->GetResourceType();
        }

        CefSharp::ReferrerPolicy CefRequestWrapper::ReferrerPolicy::get()
        {
            ThrowIfDisposed();

            return (CefSharp::ReferrerPolicy)_wrappedRequest->GetReferrerPolicy();
        }

        NameValueCollection^ CefRequestWrapper::Headers::get()
        {
            ThrowIfDisposed();

            CefRequest::HeaderMap hm;
            _wrappedRequest->GetHeaderMap(hm);

            NameValueCollection^ headers = gcnew NameValueCollection();

            for (CefRequest::HeaderMap::iterator it = hm.begin(); it != hm.end(); ++it)
            {
                String^ name = StringUtils::ToClr(it->first);
                String^ value = StringUtils::ToClr(it->second);
                headers->Add(name, value);
            }

            return headers;
        }

        void CefRequestWrapper::Headers::set(NameValueCollection^ headers)
        {
            ThrowIfDisposed();

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

            _wrappedRequest->SetHeaderMap(hm);
        }

        TransitionType CefRequestWrapper::TransitionType::get()
        {
            ThrowIfDisposed();

            return (CefSharp::TransitionType) _wrappedRequest->GetTransitionType();
        }

        IPostData^ CefRequestWrapper::PostData::get()
        {
            ThrowIfDisposed();

            if (_postData == nullptr)
            {
                auto postData = _wrappedRequest->GetPostData();
                if (postData.get())
                {
                    _postData = gcnew CefPostDataWrapper(postData);
                }
            }
            return _postData;
        }

        bool CefRequestWrapper::IsReadOnly::get()
        {
            ThrowIfDisposed();

            return _wrappedRequest->IsReadOnly();
        }

        void CefRequestWrapper::InitializePostData()
        {
            ThrowIfDisposed();

            _wrappedRequest->SetPostData(CefPostData::Create());
        }
    }
}
