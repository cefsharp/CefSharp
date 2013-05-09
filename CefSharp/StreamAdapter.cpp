#include "Stdafx.h"
#include "StreamAdapter.h"

namespace CefSharp
{
    StreamAdapter::~StreamAdapter()
    {
        _stream->Close();
    }

    size_t StreamAdapter::Read(void* ptr, size_t size, size_t n)
    {
        AutoLock lock_scope(this);

        try {
            array<Byte>^ buffer = gcnew array<Byte>(n * size);
            int ret = _stream->Read(buffer, 0, n);
            pin_ptr<Byte> src = &buffer[0];
            memcpy(ptr, static_cast<void*>(src), ret);
            return ret / size;
        }
        catch(Exception^)
        {
            return -1;
        }
    }

    int StreamAdapter::Seek(int64 offset, int whence)
    {
        SeekOrigin seekOrigin;
        switch(whence)
        {
        case SEEK_CUR:
            seekOrigin = SeekOrigin::Current;
            break;
        case SEEK_END:
            seekOrigin = SeekOrigin::End;
            break;
        case SEEK_SET:
            seekOrigin = SeekOrigin::Begin;
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
}