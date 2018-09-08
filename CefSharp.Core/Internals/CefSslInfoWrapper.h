// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "TypeConversion.h"
#include "CefWrapper.h"

#include "include\cef_ssl_info.h"

using namespace System::Security::Cryptography::X509Certificates;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefSslInfoWrapper : public ISslInfo, public CefWrapper
        {
        private:
            MCefRefPtr<CefSSLInfo> _sslInfo;

        public:
            CefSslInfoWrapper(CefRefPtr<CefSSLInfo> &sslInfo)
                : _sslInfo(sslInfo)
            {

            }

            !CefSslInfoWrapper()
            {
                _sslInfo = NULL;
            }

            ~CefSslInfoWrapper()
            {
                this->!CefSslInfoWrapper();

                _disposed = true;
            }

            virtual property CertStatus CertStatus
            {
                CefSharp::CertStatus get()
                {
                    ThrowIfDisposed();

                    return (CefSharp::CertStatus)_sslInfo->GetCertStatus();
                }
            }

            virtual property X509Certificate2^ X509Certificate
            {
                X509Certificate2^ get()
                {
                    ThrowIfDisposed();

                    auto certificate = _sslInfo->GetX509Certificate();
                    if (certificate.get())
                    {
                        auto derEncodedCertificate = certificate->GetDEREncoded();
                        auto byteCount = derEncodedCertificate->GetSize();
                        if (byteCount == 0)
                        {
                            return nullptr;
                        }

                        auto bytes = gcnew cli::array<Byte>(byteCount);
                        pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

                        derEncodedCertificate->GetData(static_cast<void*>(src), byteCount, 0);

                        return gcnew X509Certificate2(bytes);
                    }

                    return nullptr;
                }
            }
        };
    }
}