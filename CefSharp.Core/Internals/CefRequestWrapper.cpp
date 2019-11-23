// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefRequestWrapper.h"
#include "PostData.h"

using namespace System::Text;

namespace CefSharp
{
    namespace Internals
    {
        UrlRequestFlags CefRequestWrapper::Flags::get()
        {
            ThrowIfDisposed();

            return (UrlRequestFlags)_request->GetFlags();
        }

        void CefRequestWrapper::Flags::set(UrlRequestFlags flags)
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetFlags((int)flags);
        }

        String^ CefRequestWrapper::Url::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetURL());
        }

        void CefRequestWrapper::Url::set(String^ url)
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

        String^ CefRequestWrapper::Method::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetMethod());
        }

        void CefRequestWrapper::Method::set(String^ method)
        {
            if (method == nullptr)
            {
                throw gcnew System::ArgumentException("cannot be null", "method");
            }

            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetMethod(StringUtils::ToNative(method));
        }

        UInt64 CefRequestWrapper::Identifier::get()
        {
            ThrowIfDisposed();

            return _request->GetIdentifier();
        }

        void CefRequestWrapper::SetReferrer(String^ referrerUrl, CefSharp::ReferrerPolicy policy)
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetReferrer(StringUtils::ToNative(referrerUrl), (cef_referrer_policy_t)policy);
        }

        String^ CefRequestWrapper::ReferrerUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetReferrerURL());
        }

        CefSharp::ResourceType CefRequestWrapper::ResourceType::get()
        {
            ThrowIfDisposed();

            return (CefSharp::ResourceType)_request->GetResourceType();
        }

        CefSharp::ReferrerPolicy CefRequestWrapper::ReferrerPolicy::get()
        {
            ThrowIfDisposed();

            return (CefSharp::ReferrerPolicy)_request->GetReferrerPolicy();
        }

        NameValueCollection^ CefRequestWrapper::Headers::get()
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

        void CefRequestWrapper::Headers::set(NameValueCollection^ headers)
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

        TransitionType CefRequestWrapper::TransitionType::get()
        {
            ThrowIfDisposed();

            return (CefSharp::TransitionType) _request->GetTransitionType();
        }

        IPostData^ CefRequestWrapper::PostData::get()
        {
            ThrowIfDisposed();

            if (_postData == nullptr)
            {
                auto postData = _request->GetPostData();
                if (postData.get())
                {
                    _postData = gcnew CefSharp::PostData(postData);
                }
            }
            return _postData;
        }

        bool CefRequestWrapper::IsReadOnly::get()
        {
            ThrowIfDisposed();

            return _request->IsReadOnly();
        }

        void CefRequestWrapper::InitializePostData()
        {
            ThrowIfDisposed();

            ThrowIfReadOnly();

            _request->SetPostData(CefPostData::Create());
        }

        String^ CefRequestWrapper::GetHeaderByName(String^ name)
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_request->GetHeaderByName(StringUtils::ToNative(name)));
        }

        void CefRequestWrapper::SetHeaderByName(String^ name, String^ value, bool overwrite)
        {
            ThrowIfDisposed();
            ThrowIfReadOnly();

            _request->SetHeaderByName(StringUtils::ToNative(name), StringUtils::ToNative(value), overwrite);
        }
    }
}
