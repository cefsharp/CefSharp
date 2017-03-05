// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "TypeConversion.h"
#include "CefWrapper.h"

using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefResponseWrapper : public IResponse, public CefWrapper
        {
            MCefRefPtr<CefResponse> _response;
        internal:
            CefResponseWrapper(CefRefPtr<CefResponse> &response) :
                _response(response)
            {
                
            }

            !CefResponseWrapper()
            {
                _response = nullptr;
            }

            ~CefResponseWrapper()
            {
                this->!CefResponseWrapper();

                _disposed = true;
            }

        public:
            virtual property bool IsReadOnly
            {
                bool get()
                {
                    ThrowIfDisposed();

                    return _response->IsReadOnly();
                }
            }

            virtual property CefErrorCode ErrorCode
            {
                CefErrorCode get()
                {
                    ThrowIfDisposed();

                    return (CefErrorCode)_response->GetError();
                }
                void set(CefErrorCode val)
                {
                    ThrowIfDisposed();

                    _response->SetError((cef_errorcode_t)val);
                }
            }

            virtual property int StatusCode
            {
                int get()
                {
                    ThrowIfDisposed();

                    return _response->GetStatus();
                }
                void set(int val)
                {
                    ThrowIfDisposed();

                    _response->SetStatus(val);
                }
            }

            virtual property String^ StatusText
            {
                String^ get()
                {
                    ThrowIfDisposed();

                    return StringUtils::ToClr(_response->GetStatusText());
                }
                void set(String^ val)
                {
                    ThrowIfDisposed();

                    _response->SetStatusText(StringUtils::ToNative(val));
                }
            }

            virtual property String^ MimeType
            {
                String^ get()
                {
                    ThrowIfDisposed();

                    return StringUtils::ToClr(_response->GetMimeType());
                }
                void set(String^ val)
                {
                    ThrowIfDisposed();

                    _response->SetMimeType(StringUtils::ToNative(val));
                }
            }

            virtual property NameValueCollection^ ResponseHeaders
            {
                NameValueCollection^ get()
                {
                    ThrowIfDisposed();

                    //TODO: Extract this code out as it's duplicated in CefRequestWrapper
                    CefRequest::HeaderMap hm;
                    _response->GetHeaderMap(hm);

                    NameValueCollection^ headers = gcnew NameValueCollection();

                    for (CefRequest::HeaderMap::iterator it = hm.begin(); it != hm.end(); ++it)
                    {
                        String^ name = StringUtils::ToClr(it->first);
                        String^ value = StringUtils::ToClr(it->second);
                        headers->Add(name, value);
                    }

                    return headers;
                }
                void set(NameValueCollection^ headers)
                {
                    ThrowIfDisposed();

                    _response->SetHeaderMap(TypeConversion::ToNative(headers));
                }
            }
        };
    }
}
