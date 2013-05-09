#include "Stdafx.h"
#pragma once

namespace CefSharp
{
    public interface class IDownloadHandler
    {
    public:
        bool ReceivedData(array<Byte>^ data);
        void Complete();
    };
}