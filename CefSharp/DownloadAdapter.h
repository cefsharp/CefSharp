#include "stdafx.h"
#pragma once

#include "include/cef_download_handler.h"

namespace CefSharp
{
    class DownloadAdapter : public CefDownloadHandler
    {
    public:
        virtual bool ReceivedData(void* data, int data_size);
        virtual void Complete();

        IMPLEMENT_REFCOUNTING(DownloadAdapter);
    };
}