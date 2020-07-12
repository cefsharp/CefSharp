// Copyright © 2018 The CefSharp Authors. All rights reserved.
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
            CefWriteHandlerWrapper(Stream^ stream) : _stream(stream)
            {
                //Reset stream position
                stream->Position = 0;
                _isMemoryStream = stream->GetType() == MemoryStream::typeid;
            }

            ~CefWriteHandlerWrapper()
            {
                //Remove the reference to the stream, must be kept open
                _stream = nullptr;
            }

            virtual size_t Write(const void* ptr, size_t size, size_t n) OVERRIDE
            {
                try
                {
                    array<Byte>^ buffer = gcnew array<Byte>(n * size);
                    pin_ptr<Byte> src = &buffer[0];
                    memcpy(static_cast<void*>(src), ptr, n);

                    _stream->Write(buffer, 0, n);
                    return n / size;
                }
                catch (Exception^)
                {
                    return -1;
                }
            }

            virtual int Seek(int64 offset, int whence) OVERRIDE
            {
                System::IO::SeekOrigin seekOrigin;

                if (!_stream->CanSeek)
                {
                    return -1;
                }

                switch (whence)
                {
                    case SEEK_CUR:
                    {
                        seekOrigin = System::IO::SeekOrigin::Current;
                        break;
                    }
                    case SEEK_END:
                    {
                        seekOrigin = System::IO::SeekOrigin::End;
                        break;
                    }
                    case SEEK_SET:
                    {
                        seekOrigin = System::IO::SeekOrigin::Begin;
                        break;
                    }
                    default:
                    {
                        return -1;
                    }
                }

                try
                {
                    _stream->Seek(offset, seekOrigin);
                }
                catch (Exception^)
                {
                    return -1;
                }

                return 0;
            }

            virtual int64 Tell() OVERRIDE
            {
                return static_cast<int64>(_stream->Position);
            }

            virtual int Flush() OVERRIDE
            {
                _stream->Flush();
                return 0;
            }

            virtual bool MayBlock() OVERRIDE
            {
                return !_isMemoryStream;
            }

            IMPLEMENT_REFCOUNTING(CefWriteHandlerWrapper);
        };
    }
}
