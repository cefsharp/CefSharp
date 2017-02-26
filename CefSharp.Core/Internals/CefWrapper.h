// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System::Diagnostics;

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefWrapper abstract
        {
        protected:
            bool _disposed;

            void ThrowIfDisposed()
            {
                if (_disposed)
                {
                    auto type = GetType();

                    throw gcnew ObjectDisposedException(gcnew String(L"This instance of " + type->GetInterfaces()[0]->FullName + " been disposed!"));
                }
            }
        
        internal:
            CefWrapper() : _disposed(false)
            {
            
            };

        public:
            virtual property bool IsDisposed
            {
                bool get()
                {
                    return _disposed;
                }
            }
        };
    }
}
