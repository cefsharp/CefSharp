// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_request.h"

#include "Internals\TypeConversion.h"
#include "CefPostDataElementWrapper.h"
#include "CefWrapper.h"

using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefPostDataWrapper : public IPostData, public CefWrapper
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
                    ThrowIfDisposed();

                    return _postData->IsReadOnly();
                }
            }

            virtual property IList<IPostDataElement^>^ Elements
            {
                IList<IPostDataElement^>^ get()
                {
                    ThrowIfDisposed();

                    auto elements = gcnew List<IPostDataElement^>();

                    auto elementCount = _postData.get() ? _postData->GetElementCount() : 0;
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
            }

            virtual bool AddElement(IPostDataElement^ element)
            {
                ThrowIfDisposed();

                return false;
            }

            virtual bool RemoveElement(IPostDataElement^ element)
            {
                ThrowIfDisposed();

                return false;
            }

            virtual void RemoveElements()
            {
                ThrowIfDisposed();

                _postData->RemoveElements();
            }
        };
    }
}
