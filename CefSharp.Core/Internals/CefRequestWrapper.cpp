// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefRequestWrapper.h"

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

        String^ CefRequestWrapper::Body::get()
        {
            CefPostData::ElementVector ev;

            CefRefPtr<CefPostData> data = _wrappedRequest->GetPostData();

            if (data.get() != nullptr)
            {
                data.get()->GetElements(ev);

                for (CefPostData::ElementVector::iterator it = ev.begin(); it != ev.end(); ++it)
                {
                    CefPostDataElement *el = it->get();

                    if (el->GetType() == PDE_TYPE_BYTES)
                    {
                        size_t count = el->GetBytesCount();
                        char* bytes = new char[count];

                        el->GetBytes(count, bytes);

                        return gcnew String(bytes, 0, count);
                    }
                    else if (el->GetType() == PDE_TYPE_FILE)
                    {
                        return StringUtils::ToClr(el->GetFile());
                    }
                }
            }

            return nullptr;
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
    }
}
