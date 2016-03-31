// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "TypeConversion.h"
#include "CefWrapper.h"

#include "include\cef_ssl_info.h"

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

            public:
                /*virtual property SslCertPrincipal^ Subject
                {
                    SslCertPrincipal^ get()
                    {
                        auto certPrincipal = gcnew SslCertPrincipal();

                        auto _sslInfo->GetSubject();

                        auto elementCount = _postData->GetElementCount();
                        if (elementCount == 0)
                        {
                            return gcnew ReadOnlyCollection<IPostDataElement^>(elements);
                        }
                        CefPostData::ElementVector ev;

                        _postData->GetElements(ev);

                        for (CefPostData::ElementVector::iterator it = ev.begin(); it != ev.end(); ++it)
                        {
                            CefPostDataElement *el = it->get();

                            elements->Add(gcnew CefPostDataElementWrapper(el));
                        }

                        return gcnew ReadOnlyCollection<IPostDataElement^>(elements);;
                    }
                }*/

                /// <summary>
                /// Returns the subject of the X.509 certificate. For HTTPS server
                /// certificates this represents the web server.  The common name of the
                /// subject should match the host name of the web server.
                /// </summary>
                /*SslCertPrincipal Subject { get; }*/

                /// <summary>
                /// Returns the issuer of the X.509 certificate.
                /// </summary>
                /*SslCertPrincipal Issuer { get; }*/

                /// <summary>
                /// Returns the DER encoded serial number for the X.509 certificate. The value
                /// possibly includes a leading 00 byte.
                /// </summary>
                virtual property cli::array<Byte>^ SerialNumber
                {
                    cli::array<Byte>^ get()
                    {
                        ThrowIfDisposed();

                        auto serialNumber = _sslInfo->GetSerialNumber();
                        auto byteCount = serialNumber->GetSize();
                        if (byteCount == 0)
                        {
                            return nullptr;
                        }

                        auto bytes = gcnew cli::array<Byte>(byteCount);
                        pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

                        serialNumber->GetData(static_cast<void*>(src), byteCount, 0);

                        return bytes;
                    }
                }
  
                /// <summary>
                /// Returns the date before which the X.509 certificate is invalid.
                /// will return null if no date was specified.
                /// </summary>
                virtual property Nullable<DateTime> ValidStart
                {
                    Nullable<DateTime> get()
                    {
                        ThrowIfDisposed();

                        return TypeConversion::FromNative(_sslInfo->GetValidStart());
                    }
                }

                /// <summary>
                /// Returns the date after which the X.509 certificate is invalid.
                /// will return null if no date was specified.
                /// </summary>
                virtual property Nullable<DateTime> ValidExpiry
                {
                    Nullable<DateTime> get()
                    {
                        ThrowIfDisposed();

                        return TypeConversion::FromNative(_sslInfo->GetValidExpiry());
                    }
                }

                /// <summary>
                /// Returns the DER encoded data for the X.509 certificate.
                /// </summary>
                virtual property cli::array<Byte>^ DerEncoded
                {
                    cli::array<Byte>^ get()
                    {
                        ThrowIfDisposed();

                        auto serialNumber = _sslInfo->GetDEREncoded();
                        auto byteCount = serialNumber->GetSize();
                        if (byteCount == 0)
                        {
                            return nullptr;
                        }

                        auto bytes = gcnew cli::array<Byte>(byteCount);
                        pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

                        serialNumber->GetData(static_cast<void*>(src), byteCount, 0);

                        return bytes;
                    }
                }

                /// <summary>
                /// Returns the PEM encoded data for the X.509 certificate.
                /// </summary>
                virtual property cli::array<Byte>^ PemEncoded
                {
                    cli::array<Byte>^ get()
                    {
                        ThrowIfDisposed();

                        auto serialNumber = _sslInfo->GetPEMEncoded();
                        auto byteCount = serialNumber->GetSize();
                        if (byteCount == 0)
                        {
                            return nullptr;
                        }

                        auto bytes = gcnew cli::array<Byte>(byteCount);
                        pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

                        serialNumber->GetData(static_cast<void*>(src), byteCount, 0);

                        return bytes;
                    }
                }
        };
    }
}