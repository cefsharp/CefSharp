#include "Stdafx.h"
#pragma once

using namespace System;
using namespace System::IO;

namespace CefSharp
{
    class StreamAdapter : public CefReadHandler, public AppDomainSafeCefBase
    {
        gcroot<Stream^> _stream;

    private:
        static size_t _Read(StreamAdapter* const _this, void* ptr, size_t size, size_t n);
        static void _Close(StreamAdapter* const _this);
        static int _Seek(StreamAdapter* const _this, int64 offset, int whence);
        static int64 _Tell(StreamAdapter* const _this);
        static int _Eof(StreamAdapter* const _this);

    public:
        virtual ~StreamAdapter();
        StreamAdapter(Stream^ stream) : _stream(stream) { }

        virtual size_t Read(void* ptr, size_t size, size_t n);
        virtual int Seek(int64 offset, int whence);
        virtual int64 Tell();
        virtual int Eof();

        IMPLEMENT_LOCKING(StreamAdapter);
        IMPLEMENT_SAFE_REFCOUNTING(StreamAdapter);
    };
}