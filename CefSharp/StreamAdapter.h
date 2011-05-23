#include "stdafx.h"
#pragma once

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    class StreamAdapter : public CefReadHandler
    {
        gcroot<Stream^> _stream;

    public:
        virtual ~StreamAdapter();
        StreamAdapter(Stream^ stream) : _stream(stream) { }

        virtual size_t Read(void* ptr, size_t size, size_t n);       
        virtual int Seek(long offset, int whence);
        virtual long Tell();
        virtual int Eof();

        IMPLEMENT_LOCKING(StreamAdapter);
    };
}