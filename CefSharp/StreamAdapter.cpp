#include "Stdafx.h"
#include "StreamAdapter.h"

namespace CefSharp
{
    void StreamAdapter::_Close(StreamAdapter* const _this)
    {
        _this->_stream->Close();
    }

    StreamAdapter::~StreamAdapter()
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_Close, this);
        } else {
            _Close(this);
        }
    }

    size_t StreamAdapter::Read(void* ptr, size_t size, size_t n)
    {
        AutoLock lock_scope(this);

        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_Read, this, ptr, size, n);
        } else {
            return _Read(this, ptr, size, n);
        }
    }

    size_t StreamAdapter::_Read(StreamAdapter* const _this, void* ptr, size_t size, size_t n)
    {
        try {
            array<Byte>^ buffer = gcnew array<Byte>(n * size);
            int ret = _this->_stream->Read(buffer, 0, n);
            pin_ptr<Byte> src = &buffer[0];
            memcpy(ptr, static_cast<void*>(src), ret);
            return ret / size;
        }
        catch(Exception^)
        {
            return -1;
        }
    }

    int StreamAdapter::_Seek(StreamAdapter* const _this, int64 offset, int whence)
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
            _this->_stream->Seek(offset, seekOrigin);
        }
        catch (Exception^)
        {
            return -1;
        }

        return 0;
    }

    int StreamAdapter::Seek(int64 offset, int whence)
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_Seek, this, offset, whence);
        } else {
            return _Seek(this, offset, whence);
        }
    }

    int64 StreamAdapter::_Tell(StreamAdapter* const _this)
    {
        return static_cast<int64>(_this->_stream->Position);
    }

    int64 StreamAdapter::Tell()
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_Tell, this);
        } else {
            return _Tell(this);
        }
    }

    int StreamAdapter::_Eof(StreamAdapter* const _this)
    {
        return _this->_stream->Length == _this->_stream->Position;
    }

    int StreamAdapter::Eof()
    {
        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_Eof, this);
        } else {
            return _Eof(this);
        }
    }
}