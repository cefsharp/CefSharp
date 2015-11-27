// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "Internals\CefPostDataWrapper.h"

using namespace System::Text;

namespace CefSharp
{
	namespace Internals
	{
		bool CefPostDataWrapper::IsReadOnly::get()
		{
			return _postData->IsReadOnly();
		}

		IList<IPostDataElement^>^ CefPostDataWrapper::Elements::get()
		{			
			auto elements = gcnew List<IPostDataElement^>();

			auto elementCount = _postData->GetElementCount();
			if (elementCount == 0)
			{				
				return nullptr;
			}
			CefPostData::ElementVector ev;

			_postData->GetElements(ev);

			for (CefPostData::ElementVector::iterator it = ev.begin(); it != ev.end(); ++it)
			{
				CefPostDataElement *el = it->get();

				elements->Add(gcnew CefPostDataElementWrapper(el));
			}
			
			return gcnew ReadOnlyCollection<IPostDataElement^>(elements);
		}

		bool CefPostDataWrapper::AddElement(String^ key, String^ value)
		{			
			if (key->IsNullOrEmpty(key) || value->IsNullOrEmpty(value))
			{
				throw gcnew System::ArgumentException("cannot be null or empty.", key->IsNullOrEmpty(key) ? "key" : "value");
			}
			
			String^ rdatastr = key + "=" + value;	
			array<Byte>^ byteData = System::Text::Encoding::UTF8->GetBytes(rdatastr);
			
			if (_elements->Count > 0)
			{									
				auto cefPDElement = gcnew CefPostDataElementWrapper(CefPostDataElement::Create());
				cefPDElement->Bytes = byteData;
				_elements->Add(cefPDElement);
			}			
			else
			{
				List<CefPostDataElementWrapper^>^ elementList = gcnew List<CefPostDataElementWrapper^>;
				
				auto cefPDElement = gcnew CefPostDataElementWrapper(CefPostDataElement::Create());
				cefPDElement->Bytes = byteData;
				elementList->Add(cefPDElement);
				_elements = elementList;
			}
			return true;
		}

		//bool CefPostDataWrapper::AddElement(String^ fileName)
		//{
		//	if (fileName->IsNullOrEmpty(fileName))
		//	{
		//		throw gcnew System::ArgumentException("cannot be null or empty.", "fileName");
		//	}			

		//	if (_elements->Count > 0)
		//	{
		//		auto cefPDElement = gcnew CefPostDataElementWrapper(CefPostDataElement::Create());
		//		cefPDElement->File = fileName;
		//		_elements->Add(cefPDElement);
		//	}
		//	else
		//	{
		//		List<CefPostDataElementWrapper^>^ elementList = gcnew List<CefPostDataElementWrapper^>;

		//		auto cefPDElement = gcnew CefPostDataElementWrapper(CefPostDataElement::Create());
		//		cefPDElement->File = fileName;
		//		elementList->Add(cefPDElement);
		//		_elements = elementList;
		//	}
		//	return true;
		//}

		void CefPostDataWrapper::RemoveElements()
		{
			_elements->Clear();
		}
	}
}
