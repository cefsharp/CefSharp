// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request.h"

#include "CefWrapper.h"
#include "CefPostDataElementWrapper.h"

using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;

namespace CefSharp
{
    namespace Internals
    {
		public ref class CefPostDataWrapper : public IPostData, public CefWrapper
        {
            MCefRefPtr<CefPostData> _postData;
			List<CefPostDataElementWrapper^>^ _elements;
        internal:
            CefPostDataWrapper(CefRefPtr<CefPostData> &postData) :
                _postData(postData)
            {
		_elements = gcnew List < CefPostDataElementWrapper^ > ;
            }

            !CefPostDataWrapper()
            {
                _postData = nullptr;
            }

            ~CefPostDataWrapper()
            {
                this->!CefPostDataWrapper();
		delete _elements;
                _disposed = true;
            }

        public:
		virtual property bool IsReadOnly { bool get(); }
		virtual property IList<IPostDataElement^>^ Elements { IList<IPostDataElement^>^ get(); }
		virtual bool AddElement(String^ key, String^ value);
		//virtual bool AddElement(String^ fileName);
		virtual void RemoveElements();            
        };
    }
}
