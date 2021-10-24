// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_extension.h"

#include "Internals\TypeConversion.h"
#include "CefValueWrapper.h"
#include "CefWrapper.h"

using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefExtensionWrapper : public IExtension, public CefWrapper
        {
            MCefRefPtr<CefExtension> _extension;

        internal:
            CefExtensionWrapper(CefRefPtr<CefExtension> &extension) :
                _extension(extension)
            {

            }

            !CefExtensionWrapper()
            {
                _extension = nullptr;
            }

            ~CefExtensionWrapper()
            {
                this->!CefExtensionWrapper();

                _disposed = true;
            }

        public:
            virtual property String^ Identifier { String^ get(); }
            virtual property String^ Path { String^ get(); }
            virtual property IDictionary<String^, IValue^>^ Manifest { IDictionary<String^, IValue^>^ get(); }
            virtual bool IsSame(IExtension^ that);
            virtual property IRequestContext^ LoaderContext { IRequestContext^ get(); }
            virtual property bool IsLoaded { bool get(); }
            virtual void Unload();            
        };
    }
}
