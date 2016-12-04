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
		private ref class CefCertCallbackWrapper : public ISelectCertCallback, public CefWrapper
		{
		private:
			MCefRefPtr<CefSelectClientCertificateCallback> _callback;

		public:
			CefCertCallbackWrapper(CefRefPtr<CefSelectClientCertificateCallback> &callback)
				: _callback(callback)
			{

			}

			!CefCertCallbackWrapper()
			{
				_callback = NULL;
			}

			~CefCertCallbackWrapper()
			{
				this->!CefCertCallbackWrapper();

				_disposed = true;
			}

			virtual void Select(System::Security::Cryptography::X509Certificates::X509Certificate^ cert)
			{
				ThrowIfDisposed();
				_callback->Select((CefX509Certificate) cert);

				delete this;
			}
		};
	}
}