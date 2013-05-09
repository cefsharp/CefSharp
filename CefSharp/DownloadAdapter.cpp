#include "Stdafx.h"
#include "DownloadAdapter.h"

using namespace System::Runtime::InteropServices;

namespace CefSharp
{
    bool DownloadAdapter::ReceivedData(void* data, int data_size)
    {
        array<Byte>^ bytes = gcnew array<Byte>(data_size);
        Marshal::Copy(IntPtr(data), bytes, 0, data_size);

        return _handler->ReceivedData(bytes);
    }

    void DownloadAdapter::Complete()
    {
        _handler->Complete();
    }
}