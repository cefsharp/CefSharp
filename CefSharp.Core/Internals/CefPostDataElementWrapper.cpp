// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "Internals\CefPostDataElementWrapper.h"

using namespace System::Text;

namespace CefSharp
{
	namespace Internals
	{
		bool CefPostDataElementWrapper::IsReadOnly::get()
		{
			return _postDataElement->IsReadOnly();
		}

		String^ CefPostDataElementWrapper::File::get()
		{
			return StringUtils::ToClr(_postDataElement->GetFile());
		}

		void CefPostDataElementWrapper::File::set(String^ file)
		{
			if (file == nullptr)
			{
				throw gcnew System::ArgumentException("cannot be null", "file");
			}

			CefString fileStr = StringUtils::ToNative(file);
			_postDataElement->SetToFile(fileStr);
		}

		void CefPostDataElementWrapper::SetToEmpty()
		{
			_postDataElement->SetToEmpty();
		}

		PostDataElementType CefPostDataElementWrapper::Type::get()
		{
			return (PostDataElementType)_postDataElement->GetType();
		}	

		array<Byte>^ CefPostDataElementWrapper::Bytes::get()
		{
			auto byteCount = _postDataElement->GetBytesCount();
			if (byteCount == 0)
			{
				return nullptr;
			}

			auto bytes = gcnew array<Byte>(byteCount);
			pin_ptr<Byte> src = &bytes[0]; // pin pointer to first element in arr

			_postDataElement->GetBytes(byteCount, static_cast<void*>(src));

			return bytes;
		}

		void CefPostDataElementWrapper::Bytes::set(array<Byte>^ val)
		{
			pin_ptr<Byte> src = &val[0];
			_postDataElement->SetToBytes(val->Length, static_cast<void*>(src));
		}
	}
}
