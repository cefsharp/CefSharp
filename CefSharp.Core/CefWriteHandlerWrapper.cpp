// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefWriteHandlerWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        CefWriteHandlerWrapper::~CefWriteHandlerWrapper()
        {
            //Remove the reference to the stream, must be kept open as it might be reused for subsequent requests (e.g. refresh).
            _stream = nullptr;
        }

        /*size_t StreamAdapter::Read(void* ptr, size_t size, size_t n)
        {
            try
            {
                array<Byte>^ buffer = gcnew array<Byte>(n * size);
                int ret = _stream->Read(buffer, 0, n);
                pin_ptr<Byte> src = &buffer[0];
                memcpy(ptr, static_cast<void*>(src), ret);
                return ret / size;
            }
            catch (Exception^)
            {
                return -1;
            }
        }*/

        size_t CefWriteHandlerWrapper::Write(const void* ptr, size_t size, size_t n)
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

        int CefWriteHandlerWrapper::Seek(int64 offset, int whence)
        {
            System::IO::SeekOrigin seekOrigin;

            if (!_stream->CanSeek)
            {
                return -1;
            }

            switch (whence)
            {
            case SEEK_CUR:
                seekOrigin = System::IO::SeekOrigin::Current;
                break;
            case SEEK_END:
                seekOrigin = System::IO::SeekOrigin::End;
                break;
            case SEEK_SET:
                seekOrigin = System::IO::SeekOrigin::Begin;
                break;
            default:
                return -1;
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

        int64 CefWriteHandlerWrapper::Tell()
        {
            return static_cast<int64>(_stream->Position);
        }

        int CefWriteHandlerWrapper::Flush(){
            _stream->Flush();
            return 0;
        }

        bool CefWriteHandlerWrapper::MayBlock()
        {
            if (_isMemoryStream)
            {
                return false;
            }
            return true;
        }
    }
}