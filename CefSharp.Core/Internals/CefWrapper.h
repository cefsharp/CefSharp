// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
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
                    auto stackFrame = gcnew StackFrame(1);
                    auto callingMethodName = stackFrame->GetMethod()->Name;

                    throw gcnew ObjectDisposedException(gcnew String(L"This instance has been disposed! Method:" + callingMethodName));
                }
            }
        
        public:        
            CefWrapper() : _disposed(false)
            {
            
            };

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
