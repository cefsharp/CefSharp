#include "stdafx.h"
#pragma once

#include "include/cef_download_handler.h"
#include "IDownload.h"

namespace CefSharp
{
    class DownloadAdapter : public CefDownloadHandler
    {
        gcroot<IDownload^> _download;

    public:
        DownloadAdapter(IDownload^ download) : _download(download) { }

        virtual bool ReceivedData(void* data, int data_size);
        virtual void Complete();

        IMPLEMENT_REFCOUNTING(DownloadAdapter);
    };
}