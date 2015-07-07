// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "Internals/TypeConversion.h"

using namespace System;
using namespace System::Collections::Specialized;
using namespace CefSharp;

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefPostDataWrapper : public IPostData
        {
            MCefRefPtr<CefPostData> _postData;
        internal:
            CefPostDataWrapper(CefRefPtr<CefPostData> &postData) :
                _postData(postData)
            {
                
            }

            !CefPostDataWrapper()
            {
                _postData = nullptr;
            }

            ~CefPostDataWrapper()
            {
                this->!CefPostDataWrapper();
            }

        public:
            virtual property bool IsReadOnly
            {
                bool get()
                {
                    return _postData->IsReadOnly();
                }
            }

            virtual property IList<IPostDataElement^>^ Elements
            {
                IList<IPostDataElement^>^ get()
                {
                    return nullptr;
                }
            }

            virtual bool AddElement(IPostDataElement^ element)
            {
                return false;
            }

            virtual bool RemoveElement(IPostDataElement^ element)
            {
                return false;
            }

            virtual void RemoveElements()
            {
                
            }
        };
    }
}
