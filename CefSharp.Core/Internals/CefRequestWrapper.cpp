// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefRequestWrapper.h"

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

                        // Attempt to honour the charset specified by the request's Content-Type header.
                        String^ charset = this->CharSet;
                        if (charset != nullptr)
                        {
                            Encoding^ encoding;
                            try
                            {
                                encoding = Encoding::GetEncoding(charset);
                            }
                            catch (ArgumentException^)
                            {
                                encoding = nullptr;
                            }
                            if (encoding != nullptr)
                            {
                                return gcnew String(bytes, 0, count, encoding);
                            }
                        }

                        // Revert to using the system's default code page.
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

        /// <summary>
        /// Extracts the charset argument from the content-type header.
        /// The charset is optional, so a nullptr may be returned.
        /// For example, given a Content-Type header "application/json; charset=UTF-8",
        /// this function will return "UTF-8".
        /// </summary>
        String^ CefRequestWrapper::CharSet::get()
        {
            // Extract the Content-Type header value.
            auto headers = this->Headers;
            
            String^ contentType = nullptr;
            for each(String^ key in headers)
            {
                if (key->Equals("content-type", System::StringComparison::InvariantCultureIgnoreCase))
                {
                    for each(String^ element in headers->GetValues(key))
                    {
                        contentType = element;
                        break;
                    }
                    break;
                }
            }

            if (contentType == nullptr)
            {
                return nullptr;
            }

            // Look for charset after the mime-type.
            const int semiColonIndex = contentType->IndexOf(";");
            if (semiColonIndex == -1)
            {
                return nullptr;
            }

            String^ charsetArgument = contentType->Substring(semiColonIndex + 1)->Trim();
            const int equalsIndex = charsetArgument->IndexOf("=");
            if (equalsIndex == -1)
            {
                return nullptr;
            }

            String^ argumentName = charsetArgument->Substring(0, equalsIndex)->Trim();
            if (!argumentName->Equals("charset", System::StringComparison::InvariantCultureIgnoreCase))
            {
                return nullptr;
            }

            String^ charset = charsetArgument->Substring(equalsIndex + 1)->Trim();
            // Remove redundant characters (e.g. "UTF-8"; -> UTF-8)
            charset = charset->TrimStart(' ', '"');
            charset = charset->TrimEnd(' ', '"', ';');

            return charset;
        }
    }
}
