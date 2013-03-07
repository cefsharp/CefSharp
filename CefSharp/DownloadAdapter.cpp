#include "stdafx.h"
#include "DownloadAdapter.h"

namespace CefSharp
{
    bool DownloadAdapter::ReceivedData(void* data, int data_size)
    {
        return true;
    }

    void DownloadAdapter::Complete()
    {

    }
}