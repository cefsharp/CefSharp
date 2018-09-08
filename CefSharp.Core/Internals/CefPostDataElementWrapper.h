// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request.h"
#include "Internals\TypeConversion.h"
#include "CefWrapper.h"

using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefPostDataElementWrapper : public IPostDataElement, public CefWrapper
        {
            MCefRefPtr<CefPostDataElement> _postDataElement;
        internal:
            CefPostDataElementWrapper(CefRefPtr<CefPostDataElement> postDataElement) :
                _postDataElement(postDataElement)
            {

            }

            !CefPostDataElementWrapper()
            {
                _postDataElement = nullptr;
            }

            ~CefPostDataElementWrapper()
            {
                this->!CefPostDataElementWrapper();

                _disposed = true;
            }

        public:
            virtual property bool IsReadOnly
            {
                bool get()
                {
                    ThrowIfDisposed();

                    return _postDataElement->IsReadOnly();
                }
            }

            virtual property String^ File
            {
                String^ get()
                {
                    ThrowIfDisposed();

                    return StringUtils::ToClr(_postDataElement->GetFile());
                }
                void set(String^ val)
                {
                    ThrowIfDisposed();

                    _postDataElement->SetToFile(StringUtils::ToNative(val));
                }
            }

            virtual void SetToEmpty()
            {
                ThrowIfDisposed();

                _postDataElement->SetToEmpty();
            }

            virtual property PostDataElementType Type
            {
                PostDataElementType get()
                {
                    ThrowIfDisposed();

                    return (PostDataElementType)_postDataElement->GetType();
                }
            }

            virtual property cli::array<Byte>^ Bytes
            {
                cli::array<Byte>^ get()
                {
                    ThrowIfDisposed();

                    auto byteCount = _postDataElement->GetBytesCount();
                    if (byteCount == 0)
                    {
                        return nullptr;
                    }

                    auto bytes = gcnew cli::array<Byte>(byteCount);
                    pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

                    _postDataElement->GetBytes(byteCount, static_cast<void*>(src));

                    return bytes;
                }
                void set(cli::array<Byte>^ val)
                {
                    ThrowIfDisposed();

                    pin_ptr<Byte> src = &val[0];
                    _postDataElement->SetToBytes(val->Length, static_cast<void*>(src));
                }
            }

            operator CefRefPtr<CefPostDataElement>()
            {
                if (this == nullptr)
                {
                    return NULL;
                }
                return _postDataElement.get();
            }
        };
    }
}
