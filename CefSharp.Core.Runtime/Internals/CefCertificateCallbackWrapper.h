// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request_handler.h"
#include "CefWrapper.h"

using namespace System::Security::Cryptography::X509Certificates;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefCertificateCallbackWrapper : public ISelectClientCertificateCallback, public CefWrapper
        {
        private:
            MCefRefPtr<CefSelectClientCertificateCallback> _callback;
            // Owned copy of the certificates Chromium offered, not a reference to the caller's list.
            // That list belongs to CEF for the duration of ClientAdapter::OnSelectClientCertificate,
            // and this wrapper deliberately outlives that call - CEF allows Select to be called
            // "either in this method or at a later time" - so a reference would dangle the moment
            // the handler returns and a deferred Select would read freed memory.
            // A ref class cannot hold a std::vector by value, hence the pointer. Copying the vector
            // copies the reference-counted CefX509Certificate pointers, and those references are
            // what keep the certificates themselves alive.
            CefRequestHandler::X509CertificateList* _certificateList;

        public:
            CefCertificateCallbackWrapper(CefRefPtr<CefSelectClientCertificateCallback>& callback, const CefRequestHandler::X509CertificateList& certificates)
                : _callback(callback), _certificateList(new CefRequestHandler::X509CertificateList(certificates))
            {

            }

            !CefCertificateCallbackWrapper()
            {
                _callback = nullptr;

                delete _certificateList;
                _certificateList = nullptr;
            }

            ~CefCertificateCallbackWrapper()
            {
                this->!CefCertificateCallbackWrapper();

                _disposed = true;
            }

            virtual void Select(X509Certificate2^ cert)
            {
                ThrowIfDisposed();

                if (cert == nullptr)
                {
                    _callback->Select(nullptr);
                }
                else
                {
                    auto certThumbprint = cert->Thumbprint;

                    std::vector<CefRefPtr<CefX509Certificate>>::const_iterator it =
                        _certificateList->begin();
                    for (; it != _certificateList->end(); ++it)
                    {
                        auto bytes((*it)->GetDEREncoded());
                        auto byteSize = bytes->GetSize();

                        auto bufferByte = gcnew cli::array<Byte>(byteSize);
                        pin_ptr<Byte> src = &bufferByte[0]; // pin pointer to first element in arr

                        bytes->GetData(static_cast<void*>(src), byteSize, 0);
                        auto newcert = gcnew System::Security::Cryptography::X509Certificates::X509Certificate2(bufferByte);
                        auto thumbprintStr = newcert->Thumbprint;

                        if (String::Equals(certThumbprint, thumbprintStr, StringComparison::OrdinalIgnoreCase))
                        {
                            _callback->Select(*it);
                            break;
                        }
                    }
                }

                delete this;
            }
        };
    }
}