// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request_handler.h"
#include "CefWrapper.h"

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
				if (!cert){
					_callback->Select(NULL);
				}

				//Need cert_name from cert
				auto cert_name = cert->SubjectName->Name;

				std::vector<CefRefPtr<CefX509Certificate> >::const_iterator it =
					_certificateList.begin();
				for (; it != _certificateList.end(); ++it) {
					CefString subject((*it)->GetSubject()->GetDisplayName());
					if (subject == StringUtils::ToNative(cert_name)) {
						_callback->Select(*it);
						break;
					}
				}				

				delete this;
			}
		};
	}
}