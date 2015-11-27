﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
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
        String^ CefRequestWrapper::Url::get()
        {
            return StringUtils::ToClr(_wrappedRequest->GetURL());
        }

        void CefRequestWrapper::Url::set(String^ url)
        {
            if (url == nullptr)
            {
                throw gcnew System::ArgumentException("cannot be null", "url");
            }

            CefString str = StringUtils::ToNative(url);
            _wrappedRequest->SetURL(str);
        }

        String^ CefRequestWrapper::Method::get()
        {
            return StringUtils::ToClr(_wrappedRequest->GetMethod());
        }

	void CefRequestWrapper::Method::set(String^ method)
	{
	    if (method == nullptr)
	    {
		throw gcnew System::ArgumentException("cannot be null", "method");
	    }

	    _wrappedRequest->SetMethod(StringUtils::ToNative(method));
	}

        NameValueCollection^ CefRequestWrapper::Headers::get()
        {
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
            return (CefSharp::TransitionType) _wrappedRequest->GetTransitionType();
        }

        IPostData^ CefRequestWrapper::PostData::get()
        {
            if (_postData == nullptr)
            {
                _postData = gcnew CefPostDataWrapper(_wrappedRequest->GetPostData());
            }
            return _postData;
        }

	void CefRequestWrapper::PostData::set(IPostData^ postData)
	{
	    _postData = postData;
	}

	IPostData^ CefRequestWrapper::CreatePostData()
	{
	    return gcnew CefPostDataWrapper(CefPostData::Create());
	}
    }
}
