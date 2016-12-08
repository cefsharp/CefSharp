// Copyright � 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request_handler.h"
#include "CefWrapper.h"

using namespace System::Text;
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

			virtual void Select(System::Security::Cryptography::X509Certificates::X509Certificate2^ cert)
			{
				ThrowIfDisposed();
				if (cert == nullptr){
					_callback->Select(NULL);
				}
				else
				{
					auto certSerial = cert->SerialNumber;

					std::vector<CefRefPtr<CefX509Certificate> >::const_iterator it =
						_certificateList.begin();
					for (; it != _certificateList.end(); ++it) 
					{

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