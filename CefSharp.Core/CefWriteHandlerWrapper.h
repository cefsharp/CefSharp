// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    namespace Internals
    {
        private class CefWriteHandlerWrapper : public CefWriteHandler
        {
            gcroot<Stream^> _stream;
            bool _isMemoryStream;

        public:
            virtual ~CefWriteHandlerWrapper();
            CefWriteHandlerWrapper(Stream^ stream) : _stream(stream)
            {
                //Reset stream position
                stream->Position = 0;
                _isMemoryStream = (dynamic_cast<MemoryStream^>(stream)) != nullptr;
            }

            virtual size_t Write(const void* ptr, size_t size, size_t n) OVERRIDE;
            virtual int Seek(int64 offset, int whence) OVERRIDE;
            virtual int64 Tell() OVERRIDE;
            virtual int Flush() OVERRIDE;
            virtual bool MayBlock() OVERRIDE;

            IMPLEMENT_REFCOUNTING(CefWriteHandlerWrapper);
        };
    }
}
