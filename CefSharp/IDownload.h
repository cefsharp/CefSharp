#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public interface class IDownload
    {
    public:
        bool ReceivedData(array<Byte>^ data);
        void Complete();
    };
}