// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request.h"

using namespace System::Collections::Specialized;

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefPostDataElementWrapper : public IPostDataElement
        {
            MCefRefPtr<CefPostDataElement> _postDataElement;		
			
        internal:
            CefPostDataElementWrapper(CefRefPtr<CefPostDataElement> postDataElement) :
                _postDataElement(postDataElement)
            {
                
            }

            !CefPostDataElementWrapper()
            {
                _postDataElement = nullptr;
            }

            ~CefPostDataElementWrapper()
            {
                this->!CefPostDataElementWrapper();				
            }

        public:			
		virtual property bool IsReadOnly { bool get(); }
		virtual property String^ File { String^ get(); void set(String^ file); }
		virtual property PostDataElementType Type { PostDataElementType get(); }
		virtual property array<Byte>^ Bytes { array<Byte>^ get(); void set(array<Byte>^ val); }
		virtual void SetToEmpty();            		
        };
    }
}
