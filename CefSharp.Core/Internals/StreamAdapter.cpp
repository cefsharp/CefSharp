// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "StreamAdapter.h"

namespace CefSharp
{
    namespace Internals
    {
        StreamAdapter::~StreamAdapter()
        {
            _stream->Close();
            _stream = nullptr;
        }

        size_t StreamAdapter::Read(void* ptr, size_t size, size_t n)
        {
            AutoLock lock_scope(_syncRoot);

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
        }

        int StreamAdapter::Seek(int64 offset, int whence)
        {
            System::IO::SeekOrigin seekOrigin;

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

        int64 StreamAdapter::Tell()
        {
            return static_cast<int64>(_stream->Position);
        }

        int StreamAdapter::Eof()
        {
            return _stream->Length == _stream->Position;
        }

        bool StreamAdapter::MayBlock()
        {
            return true;
        }
    }
}