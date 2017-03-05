// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
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
            const CefRequestHandler::X509CertificateList& _certificateList;

        public:
            CefCertificateCallbackWrapper(CefRefPtr<CefSelectClientCertificateCallback>& callback, const CefRequestHandler::X509CertificateList& certificates)
                : _callback(callback), _certificateList(certificates)
            {

            }

            !CefCertificateCallbackWrapper()
            {
                _callback = NULL;
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
                    _callback->Select(NULL);
                }
                else
                {
                    auto certSerial = cert->SerialNumber;

                    std::vector<CefRefPtr<CefX509Certificate>>::const_iterator it =
                        _certificateList.begin();
                    for (; it != _certificateList.end(); ++it) 
                    {

                        // TODO: Need to make this logic of comparing serial number of the certificate (*it)
                        // with the selected certificate returned by the user selection (cert).
                        // Better and more performant way would be to read the serial number from
                        // (*it) and convert it into System::String, so that it can directly compared
                        // with certSerial. This is how I tried it but the Encoding class garbled up
                        // the string when converting it from CefRefPtr<CefBinaryValue> to System::String
                        // Try to find a better way to do this
                        //
                        //auto serialNum((*it)->GetSerialNumber());
                        //auto byteSize = serialNum->GetSize();
                        //auto bufferByte = gcnew cli::array<Byte>(byteSize);
                        //pin_ptr<Byte> src = &bufferByte[0]; // pin pointer to first element in arr
                        //serialNum->GetData(static_cast<void*>(src), byteSize, 0);
                        //UTF8Encoding^ encoding = gcnew UTF8Encoding;
                        //auto serialStr(encoding->GetString(bufferByte));

                        auto bytes((*it)->GetDEREncoded());
                        auto byteSize = bytes->GetSize();

                        auto bufferByte = gcnew cli::array<Byte>(byteSize);
                        pin_ptr<Byte> src = &bufferByte[0]; // pin pointer to first element in arr

                        bytes->GetData(static_cast<void*>(src), byteSize, 0);
                        auto newcert = gcnew System::Security::Cryptography::X509Certificates::X509Certificate2(bufferByte);
                        auto serialStr = newcert->SerialNumber;

                        if (certSerial == serialStr)
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