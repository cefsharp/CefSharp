#include "stdafx.h"

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    class StreamAdapter : public CefThreadSafeBase<CefReadHandler>
    {
        gcroot<Stream^> _stream;

    public:
        StreamAdapter(Stream^ stream) : _stream(stream) { }

        virtual size_t Read(void* ptr, size_t size, size_t n)
        {
            Lock();
            
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
            finally 
            {
                Unlock();
            }

        }
        
        virtual int Seek(long offset, int whence)
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
            } catch (Exception^)
            {
                return -1;
            }

            return 0;
        };
        
        virtual long Tell()
        {
            return static_cast<long>(_stream->Position);
        };

        virtual int Eof()
        {
            return _stream->Length == _stream->Position;
        };
    };
}