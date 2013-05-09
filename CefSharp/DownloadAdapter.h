#include "Stdafx.h"
#pragma once

#include "include/cef_download_handler.h"
#include "IDownloadHandler.h"

namespace CefSharp
{
    class DownloadAdapter : public CefDownloadHandler
    {
        gcroot<IDownloadHandler^> _handler;

    public:
        DownloadAdapter(IDownloadHandler^ handler) : _handler(handler) { }

        virtual bool ReceivedData(void* data, int data_size);
        virtual void Complete();

        IMPLEMENT_REFCOUNTING(DownloadAdapter);
    };
}